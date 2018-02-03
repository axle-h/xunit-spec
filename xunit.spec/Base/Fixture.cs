using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Core;
using Autofac.Extras.Moq;

namespace Xunit.Spec.Base
{
    /// <inheritdoc />
    /// <summary>
    /// A test fixture used in conjunction with a specification.
    /// Transient specifications have a new instance per test.
    /// Non-transient specifications share a fixture between tests.
    /// </summary>
    /// <seealso cref="T:System.IDisposable" />
    public class Fixture : IDisposable
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly ConcurrentDictionary<string, object> _store;
        private AutoMock _mock;
        private object _subject;
        private bool _initialized;
        private Action _cleanUp;

        /// <summary>
        /// Initializes a new instance of the <see cref="Fixture"/> class.
        /// </summary>
        public Fixture()
        {
            _store = new ConcurrentDictionary<string, object>();
        }

        internal object Result { get; private set; }

        internal Exception Exception { get; private set; }

        internal bool TryGetData(string key, out object value) => _store.TryGetValue(key, out value);

        internal void AddData(string key, object data) => _store.AddOrUpdate(key, data, (s, o) => data);

        internal async Task ThrowIfNotInitializedAsync()
        {
            try
            {
                await _semaphore.WaitAsync();
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

        internal async Task SetupAsync<TSubject, TResult>(Func<AutoMock, Task> arrange,
                                                          Func<TSubject, Task<TResult>> act,
                                                          Func<Parameter[]> getParameters,
                                                          Func<bool> expectThrow,
                                                          Action cleanUp)
        {
            try
            {
                await _semaphore.WaitAsync();
                
                if (!_initialized)
                {
                    _cleanUp = cleanUp;
                    _mock = AutoMock.GetStrict();
                    await arrange(_mock);
                    var subject = _mock.Create<TSubject>(getParameters());
                    _subject = subject;

                    try
                    {
                        Result = await act(subject);
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
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