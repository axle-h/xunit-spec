using Autofac;
using FluentAssertions;

namespace Xunit.Spec
{
    /// <inheritdoc />
    /// <summary>
    /// A test specification configured for testing a registrations in an <see cref="T:Autofac.IContainer" />.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TConcreteService">The type of the concrete service.</typeparam>
    /// <seealso cref="T:xunit.spec.BasicSpec" />
    /// <seealso cref="T:System.IDisposable" />
    public abstract class WiringSpec<TService, TConcreteService> : SimpleSpec
        where TConcreteService : TService
    {
        private IContainer _container;
        private TService _service;

        /// <summary>
        /// Loads application registrations into the specified container builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected abstract void Load(ContainerBuilder builder);

        /// <inheritdoc />
        /// <summary>
        /// Arranges the specification.
        /// </summary>
        protected override void Arrange()
        {
            var builder = new ContainerBuilder();
            Load(builder);
            _container = builder.Build();
        }

        /// <inheritdoc />
        /// <summary>
        /// Cleans up the specification.
        /// </summary>
        protected override void CleanUp() => _container?.Dispose();

        /// <inheritdoc />
        /// <summary>
        /// Performs the specification action.
        /// </summary>
        protected override void Act() => _service = _container.Resolve<TService>();

        /// <summary>
        /// Determines whether any service was resolved.
        /// </summary>
        [Fact] public void It_should_resolve_some_service() => _service.Should().NotBeNull();

        /// <summary>
        /// Determines whether the correct service was resolved.
        /// </summary>
        [Fact] public void It_should_resolve_correct_service() => _service.Should().BeOfType<TConcreteService>();
    }
}
