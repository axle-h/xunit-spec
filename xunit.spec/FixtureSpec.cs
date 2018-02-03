using System.Diagnostics;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Xunit.Spec.Base;

namespace Xunit.Spec
{
    /// <inheritdoc />
    /// <summary>
    /// A specification with fixture based test data (shared between tests) and whose action does not return a result.
    /// A fixture test will only run the act and arrange steps once per class and then re-use the test fixture for each test in a new instance of the test class.
    /// This means that it would be silly to define any instance variables or properties as they'd only be available for the very first unit test run.
    /// Instead we must use the provided <see cref="FixtureSpecBase{TSubject, TResult}.Put{TData}"/>
    /// and <see cref="FixtureSpecBase{TSubject, TResult}.Get{TData}"/> methods to save data to the fixture itself.
    /// 
    /// Fixture tests are fiddly and a special case that should only be used when the act or arrange steps are very slow e.g. have some heavy IO.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    public abstract class FixtureSpec<TSubject> : FixtureSpecBase<TSubject, object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FixtureSpec{TSubject}"/> class.
        /// </summary>
        /// <param name="fixture"></param>
        protected FixtureSpec(Fixture fixture) : base(fixture)
        {
        }

        /// <summary>
        /// Performs the specification action.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns></returns>
        protected abstract Task ActAsync(TSubject subject);

        internal sealed override async Task<object> ActInternalAsync(TSubject subject)
        {
            await ActAsync(subject);
            return null;
        }
    }

    /// <inheritdoc cref="SpecBase{TSubject,TResult}" />
    /// <summary>
    /// A synchronous specification with fixture based test data (shared between tests) and whose action does not return a result.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    public abstract class SyncFixtureSpec<TSubject> : FixtureSpec<TSubject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FixtureSpec{TSubject}"/> class.
        /// </summary>
        /// <param name="fixture"></param>
        protected SyncFixtureSpec(Fixture fixture) : base(fixture)
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
        protected abstract void Act(TSubject subject);

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
        protected sealed override Task ActAsync(TSubject subject)
        {
            Act(subject);
            return Task.CompletedTask;
        }
    }
}
