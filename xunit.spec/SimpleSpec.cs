using System.Threading.Tasks;
using Bogus;

namespace Xunit.Spec
{
    /// <summary>
    /// A simple specification without a subject or mocking container.
    /// </summary>
    public abstract class SimpleSpec : IAsyncLifetime
    {
        /// <summary>
        /// A non-generic faker instance for convenience.
        /// </summary>
        protected readonly Faker Faker = new Faker();

        /// <summary>
        /// Arranges the specification.
        /// </summary>
        protected virtual void Arrange()
        {
        }

        /// <summary>
        /// Performs the specification action.
        /// </summary>
        protected virtual void Act()
        {
        }

        /// <summary>
        /// Cleans up the specification.
        /// </summary>
        protected virtual void CleanUp()
        {
        }

        /// <summary>
        /// Called immediately after the class has been created, before it is used.
        /// </summary>
        /// <returns></returns>
        public Task InitializeAsync()
        {
            Arrange();
            Act();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called when an object is no longer needed. Called just before <see cref="M:System.IDisposable.Dispose" />
        /// if the class also implements that.
        /// </summary>
        /// <returns></returns>
        public Task DisposeAsync()
        {
            CleanUp();
            return Task.CompletedTask;
        }
    }
}
