using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using FluentAssertions;
using Xunit;

namespace Xunit.Spec.Tests
{
    public class When_running_a_result_spec : ResultSpec<object, object>
    {
        private SpecState _state = SpecState.Arranging;
        private object _result;
        
        protected override Task ArrangeAsync(AutoMock mock)
        {
            Assert(SpecState.Arranging, SpecState.Acting);
            return Task.CompletedTask;
        }

        protected override Task<object> ActAsync(object subject)
        {
            Assert(SpecState.Acting, SpecState.Testing);
            _result = new object();
            return Task.FromResult(_result);
        }

        protected override void CleanUp() => Assert(SpecState.Cleaning, SpecState.Finished);


        [Fact] public void It_should_run_a_test_transiently() => Assert(SpecState.Testing, SpecState.Cleaning);

        [Fact]
        public void It_should_have_correct_result_transiently()
        {
            Assert(SpecState.Testing, SpecState.Cleaning);
            Result.Should().BeSameAs(_result);
        }

        private void Assert(SpecState expected, SpecState next)
        {
            Console.WriteLine(expected.ToString());
            _state.Should().Be(expected);
            _state = next;
        }
    }
}