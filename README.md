[![Build status](https://ci.appveyor.com/api/projects/status/l4cahcbsvm7wcjnk/branch/master?svg=true)](https://ci.appveyor.com/project/axle-h/xunit-spec/branch/master)
[![NuGet](https://img.shields.io/nuget/v/xunit.spec.svg)](https://www.nuget.org/packages/xunit.spec/)

# xunit.spec

Specification structured testing for xunit.

## Specification structured testing

The whole idea behind specification based testing is to define your tests in terms of the behaviours of a single subject. In a microservice architecture, a subject is most likely to be a service, repository or controller but could also be an automapper profile or the DI container. Behaviours are most likely to be defined by the result of calling a method on the subject. We can best think of these behaviours in a hierarchical flow:

* **When** we use a service to do an action
  * **And** the data has feature A
    * **Then** it should not return null
    * **And** it should do something specific to feature A
    * **And** it should not do something specific to feature B
  * **And** the data has feature B
    * **Then** it should not return null
    * **And** it should do something specific to feature B
    * **And** it should not do something specific to feature A
  * **And** there is no data
    * **Then** it should return null
    * **And** it should not throw (implied)

## Example

Specification structured testing is all about maintaining explicit, readable test structure whilst providing richer messaging.
xunit.spec consists of base classes for deriving specifications depending on whether your actions are asynchronous and whether your service method returns a result. For example, we can create our own coarse specification to inherit from using a `ResultSpec<TSubject, TResult>`, which is asynchronous and whose act step returns a result:

```C#
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
```

So we now have the top level part of the behaviour tree; we have all the code necessary to setup and call the service with toggles present to send us down each behaviour, all in a concise class. Fine behaviours can be defined by inheritance:

```C#
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
```

## Blog

Please check out my [blog post](https://ax-h.com/software/development/testing/2018/02/26/specification-structured-testing.html), which is supplemental to this library.