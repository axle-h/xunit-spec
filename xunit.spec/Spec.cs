﻿using System.Diagnostics;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Xunit.Spec.Base;

namespace Xunit.Spec
{
    /// <inheritdoc />
    /// <summary>
    /// A specification with transient test data (not shared between tests) and whose action does not return a result.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    public abstract class Spec<TSubject> : TransientSpecBase<TSubject, object>
    {
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

    /// <inheritdoc />
    /// <summary>
    /// A synchronous specification with transient test data (not shared between tests) and whose action does not return a result.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
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