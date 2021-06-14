using System;
using System.Collections.Generic;
using contributor_api.Model;
using contributor_api.Service.Interface;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace contributor_api.Service
{
    public class CacheService : IDataStore
    {
        private readonly ILogger<CacheService> _logger;
        private List<Price> _prices;

        public CacheService(ILogger<CacheService> logger)
        {
            _logger = logger;
            _prices = new List<Price>();
        }

        public async Task AddPriceAsync(Price price)
        {
            await AddPrice(price);
        }

        public async Task<Price> GetLatestPriceAsync(CurrencyPair currencyPair)
        {
            return await GetLatestPrice(currencyPair);
        }

        private Task AddPrice(Price price)
        {
            _prices.Add(price);
            _logger.LogInformation($"{price.CurrencyPair.Iso} {price.Bid}/{price.Ask} is contributed by {price.Contributor}");

            return Task.CompletedTask;
        }

        private Task<Price> GetLatestPrice(CurrencyPair currencyPair)
        {
            var prices = _prices.Where(x => x.CurrencyPair.Iso == currencyPair.Iso);

            if (prices.Any())
            {
                var latestPrice = prices.Last();
                _logger.LogInformation($"Get {latestPrice.CurrencyPair.Iso} latest price: {latestPrice.Bid}/{latestPrice.Ask}");
                return Task.FromResult<Price>(latestPrice);
            }
            else
            {
                throw new ArgumentException($"No price for {currencyPair.Iso}");
            }
        }
    }
}
