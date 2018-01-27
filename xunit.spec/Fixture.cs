using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Core;
using Autofac.Extras.Moq;

namespace xunit.spec
{
    /// <inheritdoc />
    /// <summary>
    /// A test fixture used in conjunction with a specification.
    /// Transient specifications have a new instance per test.
    /// Non-transient specifications share a fixture between tests.
    /// </summary>
    /// <seealso cref="T:System.IDisposable" />
    internal class Fixture : IDisposable
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private AutoMock _mock;
        private object _subject;
        private bool _initialized;
        private Action _cleanUp;
        
        public object Result { get; private set; }

        public Exception Exception { get; private set; }

        public async Task ThrowIfNotInitializedAsync()
        {
            try
            {
                await _semaphore.WaitAsync().ConfigureAwait(false);
                if (!_initialized)
                {
                    throw new InvalidOperationException("Must initialize the fixture");
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task SetupAsync<TSubject, TResult>(Func<AutoMock, Task> arrange,
                                                          Func<TSubject, Task<TResult>> act,
                                                          Func<Parameter[]> getParameters,
                                                          Func<bool> expectThrow,
                                                          Action cleanUp)
        {
            try
            {
                await _semaphore.WaitAsync().ConfigureAwait(false);
                
                if (!_initialized)
                {
                    _cleanUp = cleanUp;
                    _mock = AutoMock.GetStrict();
                    await arrange(_mock).ConfigureAwait(false);
                    var subject = _mock.Create<TSubject>(getParameters());
                    _subject = subject;

                    try
                    {
                        Result = await act(subject).ConfigureAwait(false);
                        if (expectThrow())
                        {
                            throw new DidNotThrowException(Result);
                        }
                    }
                    catch (DidNotThrowException)
                    {
                        throw;
                    }
                    catch (Exception e)
                    {
                        if (!expectThrow())
                        {
                            throw;
                        }

                        Exception = e;
                    }
                    
                    _initialized = true;
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void Dispose()
        {
            _cleanUp?.Invoke();
            _mock?.Dispose();
            (Result as IDisposable)?.Dispose();
            (_subject as IDisposable)?.Dispose();
            _semaphore.Dispose();
        }

        private class DidNotThrowException : Exception
        {
            public DidNotThrowException(object result) : base($"Expected to throw but did not. Subject returned {result ?? "<null>"}")
            {
            }
        }
    }
}