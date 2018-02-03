using System.Threading.Tasks;
using Autofac.Extras.Moq;
using FluentAssertions;
using Xunit.Abstractions;
using Xunit.Spec.Base;

namespace Xunit.Spec.Tests
{
    /// <summary>
    /// This demonstrates running a fixture spec.
    /// A fixture spec uses a class fixture i.e. shares a single <see cref="Fixture"/> instance between all tests in the class.
    /// Since xunit will create a new instance of the test class per test we cannot use any instance variables safely.
    /// Instead we must use the context 
    /// </summary>
    /// <seealso cref="FixtureSpec{TSubject}" />
    public class When_running_a_fixture_spec : FixtureSpec<object>
    {
        private readonly ITestOutputHelper _output;

        public When_running_a_fixture_spec(Fixture fixture, ITestOutputHelper output) : base(fixture)
        {
            _output = output;
        }

        private StateData Context
        {
            get => Get<StateData>();
            set => Put(value);
        }

        private class StateData
        {
            public int TestsRun { get; set; }

            public SpecState State { get; set; }
        }

        protected override Task ArrangeAsync(AutoMock mock)
        {
            Context = new StateData
            {
                TestsRun = 0,
                State = SpecState.Initializing
            };
            _output.WriteLine(nameof(ArrangeAsync));
            AssertStateChange(SpecState.Initializing, SpecState.Arranging);
            return Task.CompletedTask;
        }

        protected override Task ActAsync(object subject)
        {
            _output.WriteLine(nameof(ActAsync));
            AssertStateChange(SpecState.Arranging, SpecState.Acting);
            return Task.CompletedTask;
        }

        protected override void CleanUp()
        {
            AssertStateChange(SpecState.Testing, SpecState.Cleaning);
            Context.TestsRun.Should().Be(5); // 2 normal tests and 3 data driven tests.
        }

        [Fact] public void It_should_run_a_test_with_shared_fixture() => AssertFixtureTest();

        [Fact] public void It_should_run_another_test_with_shared_fixture() => AssertFixtureTest();

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void It_should_run_data_driven_tests_with_shared_fixture(int n)
        {
            _output.WriteLine($"{nameof(It_should_run_data_driven_tests_with_shared_fixture)}({n})");
            AssertFixtureTest();
        }

        private void AssertFixtureTest()
        {
            if (Context.State == SpecState.Acting)
            {
                _output.WriteLine("First unit test on shared fixture");
                Context.State = SpecState.Testing;
            }

            Context.State.Should().Be(SpecState.Testing);
            Context.TestsRun++;
        }

        private void AssertStateChange(SpecState expected, SpecState next)
        {
            Context.State.Should().Be(expected);
            Context.State = next;
        }
    }
}
