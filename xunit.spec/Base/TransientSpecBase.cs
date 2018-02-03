namespace Xunit.Spec.Base
{
    /// <inheritdoc />
    /// <summary>
    /// Base class for specifications that create a new fixture instance per unit test.
    /// This is not intended to be implemented by unit tests.
    /// Asynchronous tests should use <see cref="Spec{TSubject}"/> or <see cref="ResultSpec{TSubject,TResult}"/>.
    /// Synchronous tests should use <see cref="SyncSpec{TSubject}"/> or <see cref="SyncResultSpec{TSubject,TResult}"/>.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public abstract class TransientSpecBase<TSubject, TResult> : SpecBase<TSubject, TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransientSpecBase{TSubject, TResult}" /> class.
        /// </summary>
        protected TransientSpecBase() : base(new Fixture(), true)
        {
        }
    }
}