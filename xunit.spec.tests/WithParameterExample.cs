using Autofac;
using Autofac.Extras.Moq;
using FluentAssertions;
using Moq;

namespace Xunit.Spec.Tests
{
    public class BakedBeans
    {
        public BakedBeans(ITinCan can, ITomatoSauce sauce, ITomatoSauce extraSauce)
        {
            Can = can;
            Sauce = sauce;
            ExtraSauce = extraSauce;
        }

        public ITinCan Can { get; }

        public ITomatoSauce Sauce { get; }

        public ITomatoSauce ExtraSauce { get; }
    }

    public interface ITomatoSauce
    {
    }

    public interface ITinCan
    {
    }
    
    public class When_running_spec_with_named_parameters : SyncResultSpec<BakedBeans, (ITomatoSauce sauce, ITomatoSauce extraSauce, ITinCan can)>
    {
        private ITomatoSauce _sauce;
        private ITomatoSauce _extraSauce;

        protected override void Arrange(AutoMock mock)
        {
            _sauce = new Mock<ITomatoSauce>().Object;
            _extraSauce = new Mock<ITomatoSauce>().Object;

            WithParameters(new NamedParameter("sauce", _sauce), new NamedParameter("extraSauce", _extraSauce));
        }

        protected override (ITomatoSauce sauce, ITomatoSauce extraSauce, ITinCan can) Act(BakedBeans subject) => (subject.Sauce, subject.ExtraSauce, subject.Can);

        [Fact] public void It_should_inject_some_sauce() => Result.sauce.Should().NotBeNull().And.BeSameAs(_sauce).And.NotBeSameAs(_extraSauce);

        /// <summary>
        /// We inject a difference instances of <see cref="ITomatoSauce"/> for each sauces. We have extra sauce. :-)
        /// </summary>
        [Fact] public void It_should_inject_some_extra_sauce() => Result.extraSauce.Should().NotBeNull().And.BeSameAs(_extraSauce).And.NotBeSameAs(_sauce);
        
        [Fact] public void It_should_inject_a_tin_can() => Result.can.Should().NotBeNull();
    }
    
    public class When_running_spec_without_named_parameters : SyncResultSpec<BakedBeans, (ITomatoSauce sauce, ITomatoSauce extraSauce, ITinCan can)>
    {
        private ITomatoSauce _sauce;

        protected override void Arrange(AutoMock mock)
        {
            _sauce = mock.Mock<ITomatoSauce>().Object;
        }

        protected override (ITomatoSauce sauce, ITomatoSauce extraSauce, ITinCan can) Act(BakedBeans subject) => (subject.Sauce, subject.ExtraSauce, subject.Can);

        [Fact] public void It_should_inject_some_sauce() => Result.sauce.Should().NotBeNull().And.BeSameAs(_sauce);

        /// <summary>
        /// We inject the same instance of <see cref="ITomatoSauce"/> for both sauces. There is no extra sauce. :-(
        /// </summary>
        [Fact] public void There_should_be_no_extra_sauce() => Result.extraSauce.Should().NotBeNull().And.BeSameAs(_sauce);

        [Fact] public void It_should_inject_a_tin_can() => Result.can.Should().NotBeNull();
    }
}
