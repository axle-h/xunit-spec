using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace xunit.spec.Tests
{
    [TestClass]
    public class SimpleSpecExample : SimpleSpec
    {
        private SpecState _state = SpecState.Arranging;

        protected override void Arrange() => Assert(SpecState.Arranging, SpecState.Acting);

        protected override void Act() => Assert(SpecState.Acting, SpecState.Testing);

        protected override void CleanUp() => Assert(SpecState.Cleaning, SpecState.Finished);

        [TestMethod] public void It_should_run_a_test_transiently() => Assert(SpecState.Testing, SpecState.Cleaning);

        private void Assert(SpecState expected, SpecState next)
        {
            Console.WriteLine(expected.ToString());
            _state.Should().Be(expected);
            _state = next;
        }
    }
}
