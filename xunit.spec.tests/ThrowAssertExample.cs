using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace xunit.spec.Tests
{
    public class NoBaconException : Exception
    {
    }

    public class BaconButty
    {
    }

    public class BaconService
    {
        private readonly ILogger _logger;

        public BaconService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BaconService>();
        }

        public Task<BaconButty> BaconButtyPlzAsync()
        {
            _logger.LogInformation("Hello world");
            throw new NoBaconException();
        }
    }

    [TestClass]
    public class ThrowAssertExample : ResultSpec<BaconService, BaconButty>
    {
        protected override Task ArrangeAsync(AutoMock mock)
        {
            mock.WithLogging();
            ShouldThrow();
            return Task.CompletedTask;
        }

        protected override Task<BaconButty> ActAsync(BaconService subject) => subject.BaconButtyPlzAsync();

        [TestMethod] public void The_exception_should_be_a_NoBaconException() => Exception().Should().BeOfType<NoBaconException>();
    }
}
