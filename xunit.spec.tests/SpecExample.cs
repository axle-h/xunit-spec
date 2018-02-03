using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using FluentAssertions;

namespace Xunit.Spec.Tests
{
    public class When_running_a_spec : Spec<object>
    {
        private SpecState _state = SpecState.Arranging;
        
        protected override Task ArrangeAsync(AutoMock mock)
        {
            Assert(SpecState.Arranging, SpecState.Acting);
            return Task.CompletedTask;
        }

        protected override Task ActAsync(object subject)
        {
            Assert(SpecState.Acting, SpecState.Testing);
            return Task.CompletedTask;
        }

        protected override void CleanUp() => Assert(SpecState.Cleaning, SpecState.Finished);

        [Fact] public void It_should_run_a_test_transiently() => AssertTransientTest();

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void It_should_run_a_data_driven_test_transiently(int n) => AssertTransientTest();

        private void AssertTransientTest() => Assert(SpecState.Testing, SpecState.Cleaning);

        private void Assert(SpecState expected, SpecState next)
        {
            Console.WriteLine(expected.ToString());
            _state.Should().Be(expected);
            _state = next;
        }
    }
}
