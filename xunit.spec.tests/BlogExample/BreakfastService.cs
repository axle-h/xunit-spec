using System.Threading.Tasks;

namespace Xunit.Spec.Tests.BlogExample
{
    public class Bacon
    {
        public bool IsSmoked { get; set; }
    }

    public class BaconSandwich
    {
        public bool IsYummy { get; set; }

        public string Owner { get; set; }
    }

    /// <summary>
    /// A repository of <see cref="Bacon"/>.
    /// </summary>
    public interface IBaconRepository
    {
        /// <summary>
        /// Gets and removes a slice of bacon from this bacon repository, hopefully of the preferred type, or null we've already eaten it all.
        /// </summary>
        /// <param name="preferSmoked">if set to <c>true</c> then we'll try to get smoked bacon, but we may still return un-smoked. Sorry.</param>
        /// <returns>A slice of bacon, or null if we've already eaten it all.</returns>
        Task<Bacon> TakeBaconOfPreferredTypeAsync(bool preferSmoked);
    }

    /// <summary>
    /// A service for constructing breakfast items.
    /// </summary>
    public class BreakfastService
    {
        private readonly IBaconRepository _baconRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BreakfastService"/> class.
        /// </summary>
        /// <param name="baconRepository">The bacon repository.</param>
        public BreakfastService(IBaconRepository baconRepository)
        {
            _baconRepository = baconRepository;
        }

        /// <summary>
        /// Make a bacon sandwich for the specified owner with the specified preference for smoked bacon.
        /// </summary>
        /// <param name="preferSmokedBacon">if set to <c>true</c> then we prefer smoked bacon.</param>
        /// <param name="owner">The desired owner of the bacon sandwich.</param>
        /// <returns>A bacon sandwich or <c>null</c> if no bacon is available.</returns>
        public async Task<BaconSandwich> MakeMeABaconSandwichAsync(bool preferSmokedBacon, string owner)
        {
            var bacon = await _baconRepository.TakeBaconOfPreferredTypeAsync(preferSmokedBacon);
            if (bacon == null)
            {
                return null;
            }

            var isYummy = bacon.IsSmoked == preferSmokedBacon;

            /*
            if (isYummy)
            {
                owner = "Alex";
            }
            */

            return new BaconSandwich
                   {
                       IsYummy = isYummy,
                       Owner = owner
                   };
        }
    }
}
