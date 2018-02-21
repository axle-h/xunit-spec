using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Autofac.Core;
using Autofac.Extras.Moq;
using AutoFixture;
using Bogus;

namespace Xunit.Spec.Base
{
    /// <inheritdoc />
    /// <summary>
    /// Base class for all specifications.
    /// This is not intended to be implemented by unit tests.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <seealso cref="T:Xunit.IAsyncLifetime" />
    public abstract class SpecBase<TSubject, TResult> : IAsyncLifetime
    {
        private readonly List<Parameter> _parameters = new List<Parameter>();
        private readonly bool _disposeFixture;
        private bool _shouldThrow;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecBase{TSubject, TResult}" /> class.
        /// </summary>
        /// <param name="specFixture">The fixture.</param>
        /// <param name="disposeFixture">if set to <c>true</c> [dispose fixture].</param>
        protected SpecBase(SpecFixture specFixture, bool disposeFixture)
        {
            SpecFixture = specFixture;
            _disposeFixture = disposeFixture;
        }

        /// <summary>
        /// Gets the test fixture.
        /// </summary>
        protected SpecFixture SpecFixture { get; }

        /// <summary>
        /// An auto fixture instance for convenience.
        /// Customizations should be done in the arrange step.
        /// </summary>
        protected Fixture Fixture { get; } = new Fixture();

        /// <summary>
        /// A bogus faker instance for convenience.
        /// </summary>
        protected Faker Faker { get; } = new Faker();

        /// <summary>
        /// Gets the result from the fixture.
        /// </summary>
        /// <value>
        /// The result from the fixture.
        /// </value>
        protected TResult Result => (TResult) SpecFixture.Result;

        /// <summary>
        /// Gets the exception of the specified type that was thown by the act step of this specification.
        /// </summary>
        /// <value>
        /// The exception of the specified type that was thrown by the act step of this specification
        /// </value>
        protected TException Exception<TException>() where TException : Exception => (TException) SpecFixture.Exception;

        /// <summary>
        /// Gets the exception that was thrown by the act step of this specification.
        /// </summary>
        /// <value>
        /// The exception that was thrown by the act step of this specification
        /// </value>
        protected Exception Exception() => SpecFixture.Exception;

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
        /// Optional clean up method that runs after tests.
        /// </summary>
        protected virtual void CleanUp()
        {
        }
        
        /// <summary>
        /// Performs the specification action.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        internal abstract Task<TResult> ActInternalAsync(TSubject subject);

        /// <summary>
        /// Called immediately after the class has been created, before it is used.
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAsync()
        {
            await SpecFixture.SetupAsync<TSubject, TResult>(ArrangeAsync,
                                                         ActInternalAsync,
                                                         () => _parameters.ToArray(),
                                                         () => _shouldThrow,
                                                         CleanUp);
        }

        /// <summary>
        /// Called when an object is no longer needed. Called just before <see cref="M:System.IDisposable.Dispose" />
        /// if the class also implements that.
        /// </summary>
        /// <returns></returns>
        public Task DisposeAsync()
        {
            if (_disposeFixture)
            {
                SpecFixture.Dispose();
            }
            return Task.CompletedTask;
        }
    }
}