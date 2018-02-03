using System;
using System.Diagnostics;
using Autofac.Extras.Moq;
using Microsoft.Extensions.Logging;
using Moq;

namespace Xunit.Spec.Util
{
    /// <summary>
    /// Extensions for the <see cref="AutoMock"/> container.
    /// </summary>
    public static class AutoMockExtensions
    {
        /// <summary>
        /// Sets up the Microsoft logging framework in the specified auto mock contianer.
        /// Will log all messages to the console.
        /// </summary>
        /// <param name="mock">The mock.</param>
        /// <returns></returns>
        public static AutoMock WithLogging(this AutoMock mock)
        {
            var logger = new UnitTestLogger();
            mock.Provide<ILogger>(logger);
            var factory = mock.Mock<ILoggerFactory>();
            factory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(logger);
            factory.Setup(x => x.Dispose());

            return mock;
        }

        private class UnitTestLogger : ILogger, IDisposable
        {
            private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
            
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
                => Console.WriteLine($"[{_stopwatch.Elapsed}] [{logLevel}] {formatter(state, exception)}");

            public bool IsEnabled(LogLevel logLevel) => true;

            public IDisposable BeginScope<TState>(TState state) => new Scope();

            private class Scope : IDisposable
            {
                public void Dispose()
                {
                }
            }

            public void Dispose() => _stopwatch.Stop();
        }
    }
}
