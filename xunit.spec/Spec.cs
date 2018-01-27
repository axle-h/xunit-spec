using System.Diagnostics;
using System.Threading.Tasks;
using Autofac.Extras.Moq;

namespace xunit.spec
{
    /// <inheritdoc />
    /// <summary>
    /// A specification with transient test data and whose action does not return a result.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    /// <seealso cref="T:xunit.spec.SpecWithoutResult`1" />
    public abstract class Spec<TSubject> : SpecBase<TSubject, object>
    {
        /// <summary>
        /// Performs the specification action.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns></returns>
        protected abstract Task ActAsync(TSubject subject);

        internal override async Task<object> ActInternalAsync(TSubject subject)
        {
            await ActAsync(subject).ConfigureAwait(false);
            return null;
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// A synchronous specification with transient test data and whose action does not return a result.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    /// <seealso cref="T:xunit.spec.SimpleTransientSpec`1" />
    public abstract class SyncSpec<TSubject> : Spec<TSubject>
    {
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