using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace xunit.spec
{
    /// <summary>
    /// A simple specification without a subject or mocking container.
    /// </summary>
    public abstract class SimpleSpec
    {
        /// <summary>
        /// Arranges the specification.
        /// </summary>
        protected virtual void Arrange()
        {
        }

        /// <summary>
        /// Performs the specification action.
        /// </summary>
        protected virtual void Act()
        {
        }

        /// <summary>
        /// Cleans up the specification.
        /// </summary>
        protected virtual void CleanUp()
        {
        }

        /// <summary>
        /// Called immediately after the class has been created, before it is used.
        /// </summary>
        /// <returns></returns>
        [TestInitialize]
        public void TestInitialize()
        {
            Arrange();
            Act();
        }

        /// <summary>
        /// Called when an object is no longer needed. Called just before <see cref="M:System.IDisposable.Dispose" />
        /// if the class also implements that.
        /// </summary>
        /// <returns></returns>
        [TestCleanup]
        public void TestCleanup()
        {
            CleanUp();
        }
    }
}
