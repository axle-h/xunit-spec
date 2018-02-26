using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;

namespace Xunit.Spec.Tests.BlogExample
{
    public class BreakfastServiceTests
    {
        private static readonly Fixture Fixture = new Fixture();
        
        [Fact]
        public async Task TestMakingAYummyBaconSandwich()
        {
            var owner = Fixture.Create<string>();
            var bacon = Fixture.Create<Bacon>();

            var baconRepository = Mock.Of<IBaconRepository>();
            Mock.Get(baconRepository)
                .Setup(x => x.TakeBaconOfPreferredTypeAsync(bacon.IsSmoked))
                .ReturnsAsync(bacon)
                .Verifiable();

            var service = new BreakfastService(baconRepository);
            var result = await service.MakeMeABaconSandwichAsync(bacon.IsSmoked, owner);

            result.Should().NotBeNull();
            result.IsYummy.Should().BeTrue();
            result.Owner.Should().Be(owner);
        }

        [Fact]
        public async Task TestMakingANotSoYummyBaconSandwich()
        {
            var owner = Fixture.Create<string>();
            var bacon = Fixture.Create<Bacon>();

            var baconRepository = Mock.Of<IBaconRepository>();
            Mock.Get(baconRepository)
                .Setup(x => x.TakeBaconOfPreferredTypeAsync(!bacon.IsSmoked))
                .ReturnsAsync(bacon)
                .Verifiable();

            var service = new BreakfastService(baconRepository);
            var result = await service.MakeMeABaconSandwichAsync(!bacon.IsSmoked, owner);

            result.Should().NotBeNull();
            result.IsYummy.Should().BeFalse();
            result.Owner.Should().Be(owner);
        }

        [Fact]
        public async Task TestMakingABaconSandwichWithoutBacon()
        {
            var owner = Fixture.Create<string>();
            var preferSmoked = Fixture.Create<bool>();

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