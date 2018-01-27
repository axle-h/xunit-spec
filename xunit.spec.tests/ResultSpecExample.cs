using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace xunit.spec.Tests
{
    [TestClass]
    public class ResultSpecExample : ResultSpec<object, object>
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


        [TestMethod] public void It_should_run_a_test_transiently() => Assert(SpecState.Testing, SpecState.Cleaning);

        [TestMethod]
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