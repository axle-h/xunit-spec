using System.Diagnostics;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Xunit.Spec.Base;

namespace Xunit.Spec
{
    /// <summary>
    /// A specification with fixture based test data (shared between tests) and whose action returns a result.
    /// A fixture test will only run the act and arrange steps once per class and then re-use the test fixture for each test in a new instance of the test class.
    /// This means that it would be silly to define any instance variables or properties as they'd only be available for the very first unit test run.
    /// Instead we must use the provided <see cref="FixtureSpecBase{TSubject, TResult}.Put{TData}"/>
    /// and <see cref="FixtureSpecBase{TSubject, TResult}.Get{TData}"/> methods to save data to the fixture itself.
    /// 
    /// Fixture tests are fiddly and a special case that should only be used when the act or arrange steps are very slow e.g. have some heavy IO.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <seealso cref="SpecBase{TSubject,TResult}" />
    /// <seealso cref="SpecFixture" />
    /// <inheritdoc cref="SpecBase{TSubject,TResult}" />
    public abstract class FixtureResultSpec<TSubject, TResult> : FixtureSpecBase<TSubject, TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FixtureSpec{TSubject}"/> class.
        /// </summary>
        /// <param name="specFixture"></param>
        protected FixtureResultSpec(SpecFixture specFixture) : base(specFixture)
        {
        }

        /// <summary>
        /// Performs the specification action.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns></returns>
        protected abstract Task<TResult> ActAsync(TSubject subject);

        internal sealed override async Task<TResult> ActInternalAsync(TSubject subject) => await ActAsync(subject);
    }

    /// <summary>
    /// A synchronous specification with fixture based test data (shared between tests) and whose action returns a result.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <inheritdoc cref="SpecBase{TSubject,TResult}" />
    public abstract class SyncFixtureResultSpec<TSubject, TResult> : FixtureResultSpec<TSubject, TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FixtureSpec{TSubject}"/> class.
        /// </summary>
        /// <param name="specFixture"></param>
        protected SyncFixtureResultSpec(SpecFixture specFixture) : base(specFixture)
        {
        }

        /// <summary>
        /// Arranges the specification.
        /// </summary>
        /// <param name="mock">The auto mock repository/container.</param>
        protected abstract void Arrange(AutoMock mock);

        /// <summary>
        /// Performs the specification action.
        /// </summary>
        /// <param name="subject">The subject.</param>
        protected abstract TResult Act(TSubject subject);

        /// <inheritdoc />
        /// <summary>
        /// Arranges the specification.
        /// </summary>
        /// <param name="mock">The auto mock repository/container.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        protected sealed override Task ArrangeAsync(AutoMock mock)
        {
            Arrange(mock);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        /// <summary>
        /// Performs the specification action.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        protected sealed override Task<TResult> ActAsync(TSubject subject) => Task.FromResult(Act(subject));
    }
}
