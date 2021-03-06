﻿using System.Diagnostics;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Xunit.Spec.Base;

namespace Xunit.Spec
{
    /// <inheritdoc />
    /// <summary>
    ///  A specification with a transient lifetime (not shared between tests) and whose action returns a result of type <see cref="!:TResult" />.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public abstract class ResultSpec<TSubject, TResult> : TransientSpecBase<TSubject, TResult>
    {
        /// <summary>
        /// Performs the specification action.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns></returns>
        protected abstract Task<TResult> ActAsync(TSubject subject);

        internal sealed override async Task<TResult> ActInternalAsync(TSubject subject) => await ActAsync(subject);
    }

    /// <inheritdoc />
    /// <summary>
    /// A synchronous specification with a transient lifetime (not shared between tests) and whose action returns a result of type <see cref="!:TResult" />.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public abstract class SyncResultSpec<TSubject, TResult> : ResultSpec<TSubject, TResult>
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