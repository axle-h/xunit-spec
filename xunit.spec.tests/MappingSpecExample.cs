using System;
using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace xunit.spec.Tests
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

    [TestClass]
    public class MappingSpecExample : MappingSpec<PocoSource, PocoDestination>
    {
        protected override ICollection<Type> ProfileTypes { get; } = new[] { typeof(MappingSpecExample) };

        protected override PocoSource GenerateSource() => new PocoSource
                                                          {
                                                              Int = Faker.Random.Int(),
                                                              String = Faker.Company.Bs()
                                                          };

        [TestMethod] public void It_should_map_String() => Destination.String.Should().Be(Source.String);

        [TestMethod] public void It_should_map_Int() => Destination.Int.Should().Be(Source.Int);
    }
}
