using System;
using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using Xunit.Spec.Automapper;

namespace Xunit.Spec.Tests
{
    public class PocoSource
    {
        public string String { get; set; }

        public int Int { get; set; }
    }

    public class PocoDestination
    {
        public string String { get; set; }

        public int Int { get; set; }
    }

    public class PocoProfile : Profile
    {
        public PocoProfile()
        {
            CreateMap<PocoSource, PocoDestination>();
        }
    }
    
    public class When_mapping_from_PocoSource_to_PocoDestination : MappingSpec<PocoSource, PocoDestination>
    {
        protected override ICollection<Type> ProfileTypes { get; } = new[] { typeof(PocoProfile) };

        protected override PocoSource GenerateSource() => new PocoSource
                                                          {
                                                              Int = Faker.Random.Int(),
                                                              String = Faker.Company.Bs()
                                                          };

        [Fact] public void It_should_map_String() => Destination.String.Should().Be(Source.String);

        [Fact] public void It_should_map_Int() => Destination.Int.Should().Be(Source.Int);
    }
}
