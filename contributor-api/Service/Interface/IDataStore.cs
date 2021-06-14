using System.Threading.Tasks;
using contributor_api.Model;

namespace contributor_api.Service.Interface
{
    public interface IDataStore
    {
        /// <summary>
        /// Add price from DS
        /// </summary>
        /// <param name="price"></param>
        Task AddPriceAsync(Price price);

        /// <summary>
        /// Get the latest price from DS
        /// </summary>
        /// <param name="currencyPair"></param>
        /// <returns>Price info</returns>
        Task<Price> GetLatestPriceAsync(CurrencyPair currencyPair);
    }
}
