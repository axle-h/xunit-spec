using System.Threading.Tasks;
using Autofac.Extras.Moq;
using FluentAssertions;
using Moq;

namespace Xunit.Spec.Tests.BlogExample
{
    public abstract class BaconService_BaconSandwich_Spec : ResultSpec<BreakfastService, BaconSandwich>
    {
        private bool _preferSmoked;
        protected string Owner;

        protected virtual bool PreferredBaconAvailable { get; } = true;

        protected virtual bool AnyBaconAvailable { get; } = true;

        protected override Task ArrangeAsync(AutoMock mock)
        {
            Owner = Faker.Name.FullName();
            _preferSmoked = Faker.Random.Bool();

            var bacon = AnyBaconAvailable ? new Bacon { IsSmoked = PreferredBaconAvailable ? _preferSmoked : !_preferSmoked } : null;
            mock.Mock<IBaconRepository>()
                .Setup(x => x.TakeBaconOfPreferredTypeAsync(_preferSmoked))
                .ReturnsAsync(bacon)
                .Verifiable();

            return Task.CompletedTask;
        }

        protected override Task<BaconSandwich> ActAsync(BreakfastService subject) => subject.MakeMeABaconSandwichAsync(_preferSmoked, Owner);
    }

    public abstract class Successfully_making_a_bacon_sandwich : BaconService_BaconSandwich_Spec
    {
        [Fact] public void It_should_not_return_null() => Result.Should().NotBeNull();

        [Fact] public void It_should_belong_to_the_correct_person() => Result.Owner.Should().Be(Owner);
    }

    public class When_successfully_making_a_yummy_bacon_sandwich : Successfully_making_a_bacon_sandwich
    {
        [Fact] public void It_should_be_yummy() => Result.IsYummy.Should().BeTrue();
    }

    public class When_successfully_making_a_not_so_yummy_bacon_sandwich : Successfully_making_a_bacon_sandwich
    {
        protected override bool PreferredBaconAvailable { get; } = false;

        [Fact] public void It_should_not_be_yummy() => Result.IsYummy.Should().BeFalse();
    }

    public class When_attempting_to_make_a_bacon_sandwich_without_bacon : BaconService_BaconSandwich_Spec
    {
        protected override bool AnyBaconAvailable { get; } = false;

        [Fact] public void It_should_return_null() => Result.Should().BeNull();
    }
}
