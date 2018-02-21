using System.Threading.Tasks;
using AutoFixture;
using Bogus;

namespace Xunit.Spec
{
    /// <summary>
    /// A simple specification without a subject or mocking container.
    /// </summary>
    public abstract class SimpleSpec : IAsyncLifetime
    {
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
