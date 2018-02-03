using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;

namespace Xunit.Spec.Tests
{
    public abstract class SubSpecExample : Spec<object>
    {
        protected override Task ArrangeAsync(AutoMock mock) => Task.CompletedTask;

        protected override Task ActAsync(object subject) => Task.CompletedTask;

        [Fact(Skip = "This test is intended to fail")] public void This_test_should_fail() => throw new ArgumentException("IT WORKED");
    }
    
    public class When_inheriting_from_sub_class_and_having_our_own_tests : SubSpecExample
    {
        [Fact(Skip = "This test is intended to fail")] public void This_test_should_also_fail() => throw new ArgumentException("IT WORKED");
    }
    
    public class When_inheriting_from_sub_class : SubSpecExample
    {
    }
}
