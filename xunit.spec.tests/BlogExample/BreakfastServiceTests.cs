using System;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;

namespace Xunit.Spec.Tests.BlogExample
{
    public class BreakfastServiceTests
    {
        private static readonly Faker Faker = new Faker();

        [Fact]
        public async Task TestMakingAYummyBaconSandwich()
        {
            var owner = Faker.Name.FullName();
            var preferSmoked = Faker.Random.Bool();
            var bacon = new Bacon { IsSmoked = preferSmoked };

            var baconRepository = Mock.Of<IBaconRepository>();
            Mock.Get(baconRepository)
                .Setup(x => x.TakeBaconOfPreferredTypeAsync(preferSmoked))
                .ReturnsAsync(bacon)
                .Verifiable();

            var service = new BreakfastService(baconRepository);
            var result = await service.MakeMeABaconSandwichAsync(preferSmoked, owner);

            result.Should().NotBeNull();
            result.Owner.Should().Be(owner);
            result.IsYummy.Should().BeTrue();
        }

        [Fact]
        public async Task TestMakingANotSoYummyBaconSandwich()
        {
            var owner = Faker.Name.FullName();
            var preferSmoked = Faker.Random.Bool();
            var bacon = new Bacon { IsSmoked = !preferSmoked };

            var baconRepository = Mock.Of<IBaconRepository>();
            Mock.Get(baconRepository)
                .Setup(x => x.TakeBaconOfPreferredTypeAsync(preferSmoked))
                .ReturnsAsync(bacon)
                .Verifiable();

            var service = new BreakfastService(baconRepository);
            var result = await service.MakeMeABaconSandwichAsync(preferSmoked, owner);

            result.Should().NotBeNull();
            result.Owner.Should().Be(owner);
            result.IsYummy.Should().BeFalse();
        }

        [Fact]
        public async Task TestMakingABaconSandwichWithoutBacon()
        {
            var owner = Faker.Name.FullName();
            var preferSmoked = Faker.Random.Bool();

            var baconRepository = Mock.Of<IBaconRepository>();
            Mock.Get(baconRepository)
                .Setup(x => x.TakeBaconOfPreferredTypeAsync(preferSmoked))
                .ReturnsAsync(null as Bacon)
                .Verifiable();

            var service = new BreakfastService(baconRepository);
            var result = await service.MakeMeABaconSandwichAsync(preferSmoked, owner);

            result.Should().BeNull();
        }
    }
}