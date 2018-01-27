using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Autofac.Core;
using Autofac.Extras.Moq;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace xunit.spec
{
    /// <inheritdoc />
    /// <summary>
    /// Base class for all specifications.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <seealso cref="T:Xunit.IAsyncLifetime" />
    public abstract class SpecBase<TSubject, TResult> : IDisposable
    {
        private readonly List<Parameter> _parameters = new List<Parameter>();
        private bool _shouldThrow;
        
        internal Fixture Fixture { get; private set; }

        /// <summary>
        /// A non-generic faker instance for convenience.
        /// </summary>
        protected readonly Faker Faker = new Faker();
        
        /// <summary>
        /// Gets the result from the fixture.
        /// </summary>
        /// <value>
        /// The result from the fixture.
        /// </value>
        protected TResult Result => (TResult) Fixture.Result;

        /// <summary>
        /// Gets the exception of the specified type that was thown by the act step of this specification.
        /// </summary>
        /// <value>
        /// The exception of the specified type that was thown by the act step of this specification
        /// </value>
        protected TException Exception<TException>() where TException : Exception => (TException) Fixture.Exception;

        /// <summary>
        /// Gets the exception that was thown by the act step of this specification.
        /// </summary>
        /// <value>
        /// The exception that was thown by the act step of this specification
        /// </value>
        protected Exception Exception() => Fixture.Exception;

        /// <summary>
        /// Adds an assertion that an exception should be thrown when this specification is run.
        /// You can also add custom assertions on the content and type of the exception.
        /// You should call this method only once in your arrange step.
        /// </summary>
        protected void ShouldThrow()
        {
            if (_shouldThrow)
            {
                throw new InvalidOperationException("you have already defined an exception assertion for this specification");
            }

            _shouldThrow = true;
        }

        /// <summary>
        /// Provides parameters which is used when resolving the subject from the container.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        protected void WithParameters(params Parameter[] parameters) => _parameters.AddRange(parameters);

        /// <summary>
        /// Arranges the specification.
        /// </summary>
        /// <param name="mock">The auto mock repository/container.</param>
        /// <returns></returns>
        protected abstract Task ArrangeAsync(AutoMock mock);

        /// <summary>
        /// Performs the specification action.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        internal abstract Task<TResult> ActInternalAsync(TSubject subject);

        /// <summary>
        /// Optional clean up method that runs after tests.
        /// </summary>
        protected virtual void CleanUp()
        {
        }
        
        /// <summary>
        /// Initializes the specification.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            Fixture = new Fixture();
            Fixture.SetupAsync<TSubject, TResult>(ArrangeAsync,
                                                  ActInternalAsync,
                                                  () => _parameters.ToArray(),
                                                  () => _shouldThrow,
                                                  CleanUp).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() => Fixture?.Dispose();
    }
}