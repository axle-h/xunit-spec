using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bogus.DataSets;

namespace xunit.spec
{
    /// <summary>
    /// Extensions for <see cref="Faker"/> datasets.
    /// </summary>
    public static class FakeExtensions
    {
        /// <summary>
        /// Generates a collection of fakes using the specified faker.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="f">The faker.</param>
        /// <returns></returns>
        public static ICollection<TModel> GenerateSome<TModel>(this Faker<TModel> f) where TModel : class => f.Generate(8).ToArray();

        /// <summary>
        /// Get a date?time offset in the past between refDate and years past that date.
        /// </summary>
        /// <param name="f">The faker.</param>
        /// <param name="yearsToGoBack">Years to go back from refDate. Default is 1 year.</param>
        /// <param name="refDate">The date to start calculations. Default is now.</param>
        /// <returns></returns>
        public static DateTimeOffset PastOffset(this Date f, int yearsToGoBack = 1, DateTime? refDate = null) =>
            new DateTimeOffset(f.Past(yearsToGoBack, refDate).ToUniversalTime().Ticks, TimeSpan.Zero);

        /// <summary>
        /// Get a random date/time offset within the last few days since now.
        /// </summary>
        /// <param name="f">The faker.</param>
        /// <param name="days">Number of days to go back.</param>
        /// <returns></returns>
        public static DateTimeOffset RecentOffset(this Date f, int days = 1) => new DateTimeOffset(f.Recent(days).ToUniversalTime().Ticks, TimeSpan.Zero);
    }
}
