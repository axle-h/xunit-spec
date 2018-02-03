using System;
using FluentAssertions;
using Xunit;

namespace Xunit.Spec.Tests
{
    public class When_running_a_simple_spec : SimpleSpec
    {
        private SpecState _state = SpecState.Arranging;

        protected override void Arrange() => Assert(SpecState.Arranging, SpecState.Acting);

        protected override void Act() => Assert(SpecState.Acting, SpecState.Testing);

        protected override void CleanUp() => Assert(SpecState.Cleaning, SpecState.Finished);

        [Fact] public void It_should_run_a_test_transiently() => Assert(SpecState.Testing, SpecState.Cleaning);

        private void Assert(SpecState expected, SpecState next)
        {
            Console.WriteLine(expected.ToString());
            _state.Should().Be(expected);
            _state = next;
        }
    }
}
