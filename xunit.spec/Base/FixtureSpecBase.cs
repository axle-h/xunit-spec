using System;

namespace Xunit.Spec.Base
{
    /// <inheritdoc cref="SpecBase{TSubject,TResult}" />
    /// <summary>
    /// Base class for specifications that share a class fixture between unit tests.
    /// This base class defines extra methods for accessing test fixture data.
    /// This is not intended to be implemented by unit tests.
    /// Asynchronous tests should use <see cref="FixtureSpec{TSubject}"/> or <see cref="FixtureResultSpec{TSubject,TResult}"/>.
    /// Synchronous tests should use <see cref="SyncFixtureSpec{TSubject}"/> or <see cref="SyncFixtureResultSpec{TSubject,TResult}"/>.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public abstract class FixtureSpecBase<TSubject, TResult> : SpecBase<TSubject, TResult>, IClassFixture<Fixture>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FixtureSpecBase{TSubject, TResult}"/> class.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        protected FixtureSpecBase(Fixture fixture) : base(fixture, false)
        {
        }

        /// <summary>
        /// Gets the test data from the test fixture at the specified key or one derived from the type if no key provided.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected TData Get<TData>(string key = null)
        {
            var keyString = key ?? typeof(TData).FullName;
            if (!Fixture.TryGetData(keyString, out var value))
            {
                throw new ArgumentException($"Cannot find fixture data with the key {keyString}", nameof(key));
            }

            return value is TData d
                       ? d
                       : throw new ArgumentException($"The data at {keyString} is not of type {typeof(TData).FullName}", nameof(key));
        }

        /// <summary>
        /// Adds the specified data to the test fixture at the specified key or one derived from the type if no key provided.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="key">The key.</param>
        protected void Put<TData>(TData data, string key = null)
        {
            var keyString = key ?? typeof(TData).FullName;
            Fixture.AddData(keyString, data);
        }
    }
}