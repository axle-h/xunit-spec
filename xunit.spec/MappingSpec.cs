using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace xunit.spec
{
    /// <inheritdoc />
    /// <summary>
    /// A test specification pre-configured to test a <see cref="T:AutoMapper.Mapper" />.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <seealso cref="T:xunit.spec.SyncTransientSimpleSpec`1" />
    [TestCategory("Mapping")]
    public abstract class MappingSpec<TSource, TDestination> : SyncResultSpec<Mapper, TDestination>
    {
        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        protected TSource Source { get; private set; }

        /// <summary>
        /// Gets the destination.
        /// </summary>
        /// <value>
        /// The destination.
        /// </value>
        protected TDestination Destination => Result;
        
        /// <summary>
        /// Gets a collection of types whose assemblies contain profiles.
        /// </summary>
        /// <value>
        /// A collection of types whose assemblies contain profiles.
        /// </value>
        protected abstract ICollection<Type> ProfileTypes { get; }

        /// <summary>
        /// Generates the mapping source.
        /// </summary>
        /// <returns></returns>
        protected abstract TSource GenerateSource();

        /// <summary>
        /// Optionally generates the mapping destination.
        /// </summary>
        /// <returns></returns>
        protected virtual TDestination GenerateDestination() => throw new NotImplementedException(nameof(GenerateDestination));

        /// <inheritdoc />
        /// <summary>
        /// Performs the specification action.
        /// </summary>
        /// <param name="subject">The subject.</param>
        protected sealed override TDestination Act(Mapper subject)
        {
            var mapper = (IMapper) subject;
            
            try
            {
                // Try to generate the destination.
                var destination = GenerateDestination();
                mapper.Map(Source, destination);
                return destination;
            }
            catch (NotImplementedException nse)
            {
                if (nse.Message != nameof(GenerateDestination))
                {
                    throw;
                }

                return mapper.Map<TDestination>(Source);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Arranges the specification.
        /// </summary>
        /// <param name="mock">The auto mock repository/container.</param>
        protected override void Arrange(AutoMock mock)
        {
            mock.Provide<IConfigurationProvider>(new MapperConfiguration(x => x.AddProfiles(ProfileTypes)));
            Source = GenerateSource();
        }
    }
}
