using Autofac;
using Autofac.Extras.Moq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace xunit.spec.Tests
{
    public class BranstonBeans
    {
        public BranstonBeans(ITinCan can, ITomatoSauce sauce, ITomatoSauce extraSauceBecauseWhyNot)
        {
            Can = can;
            Sauce = sauce;
            ExtraSauceBecauseWhyNot = extraSauceBecauseWhyNot;
        }

        public ITinCan Can { get; }

        public ITomatoSauce Sauce { get; }

        public ITomatoSauce ExtraSauceBecauseWhyNot { get; }
    }

    public interface ITomatoSauce
    {
    }

    public interface ITinCan
    {
    }

    [TestClass]
    public class WithParameterExample : SyncResultSpec<BranstonBeans, (ITomatoSauce sauce, ITomatoSauce extraSauce, ITinCan can)>
    {
        private ITomatoSauce _sauce;
        private ITomatoSauce _extraSauce;

        protected override void Arrange(AutoMock mock)
        {
            _sauce = new Mock<ITomatoSauce>().Object;
            _extraSauce = new Mock<ITomatoSauce>().Object;

            WithParameters(new NamedParameter("sauce", _sauce), new NamedParameter("extraSauceBecauseWhyNot", _extraSauce));
        }

        protected override (ITomatoSauce sauce, ITomatoSauce extraSauce, ITinCan can) Act(BranstonBeans subject) => (subject.Sauce, subject.ExtraSauceBecauseWhyNot, subject.Can);

        [TestMethod] public void It_should_inject_some_sauce() => Result.sauce.Should().NotBeNull().And.BeSameAs(_sauce).And.NotBeSameAs(_extraSauce);

        /// <summary>
        /// We inject a difference instances of <see cref="ITomatoSauce"/> for each sauces. We have extra sauce. :-)
        /// </summary>
        [TestMethod] public void It_should_inject_some_extra_sauce() => Result.extraSauce.Should().NotBeNull().And.BeSameAs(_extraSauce).And.NotBeSameAs(_sauce);
        
        [TestMethod] public void It_should_inject_a_tin_can() => Result.can.Should().NotBeNull();
    }

    [TestClass]
    public class WithoutParameterExample : SyncResultSpec<BranstonBeans, (ITomatoSauce sauce, ITomatoSauce extraSauce, ITinCan can)>
    {
        private ITomatoSauce _sauce;

        protected override void Arrange(AutoMock mock)
        {
            _sauce = mock.Mock<ITomatoSauce>().Object;
        }

        protected override (ITomatoSauce sauce, ITomatoSauce extraSauce, ITinCan can) Act(BranstonBeans subject) => (subject.Sauce, subject.ExtraSauceBecauseWhyNot, subject.Can);

        [TestMethod] public void It_should_inject_some_sauce() => Result.sauce.Should().NotBeNull().And.BeSameAs(_sauce);

        /// <summary>
        /// We inject the same instance of <see cref="ITomatoSauce"/> for both sauces. There is no extra sauce. :-(
        /// </summary>
        [TestMethod] public void There_should_be_no_extra_sauce() => Result.extraSauce.Should().NotBeNull().And.BeSameAs(_sauce);

        [TestMethod] public void It_should_inject_a_tin_can() => Result.can.Should().NotBeNull();
    }
}
