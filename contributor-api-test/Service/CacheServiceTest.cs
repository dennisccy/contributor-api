using System;
using System.Threading.Tasks;
using contributor_api.Model;
using contributor_api.Service;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace contributor_api_test.Service
{
    [TestFixture]
    public class CacheServiceTest
    {
        private Mock<ILogger<CacheService>> _mockLogger;
        private CacheService _service;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<CacheService>>();
            _service = new CacheService(_mockLogger.Object);
        }

        [Test]
        public async Task AddOnePrice_And_GetLatestPrice()
        {
            await _service.AddPriceAsync(new Price("EUR", "USD", 1.2, 1.21, "test"));

            var latestPrice = await _service.GetLatestPriceAsync(new CurrencyPair("EUR", "USD"));
            Assert.AreEqual("EUR", latestPrice.CurrencyPair.BaseCurrency);
            Assert.AreEqual("USD", latestPrice.CurrencyPair.QuoteCurrency);
            Assert.AreEqual(1.2, latestPrice.Bid);
            Assert.AreEqual(1.21, latestPrice.Ask);
            Assert.AreEqual("test", latestPrice.Contributor);
            Assert.Less(default(DateTime), latestPrice.ContributionTime);
        }

        [Test]
        public async Task AddThreePrices_And_GetLatestPrice_SameCcy()
        {
            await _service.AddPriceAsync(new Price("EUR", "USD", 1.21, 1.24, "test1"));
            await _service.AddPriceAsync(new Price("EUR", "USD", 1.22, 1.25, "test2"));
            await _service.AddPriceAsync(new Price("EUR", "USD", 1.23, 1.26, "test3"));

            var latestPrice = await _service.GetLatestPriceAsync(new CurrencyPair("EUR", "USD"));
            Assert.AreEqual("EUR", latestPrice.CurrencyPair.BaseCurrency);
            Assert.AreEqual("USD", latestPrice.CurrencyPair.QuoteCurrency);
            Assert.AreEqual(1.23, latestPrice.Bid);
            Assert.AreEqual(1.26, latestPrice.Ask);
            Assert.AreEqual("test3", latestPrice.Contributor);
            Assert.Less(default(DateTime), latestPrice.ContributionTime);
        }

        [Test]
        public async Task AddThreePrices_And_GetLatestPrice_DiffCcy()
        {
            await _service.AddPriceAsync(new Price("EUR", "USD", 1.21, 1.24, "test1"));
            await _service.AddPriceAsync(new Price("EUR", "USD", 1.31, 1.34, "test12"));
            await _service.AddPriceAsync(new Price("USD", "JPY", 1.22, 1.25, "test2"));
            await _service.AddPriceAsync(new Price("USD", "CHF", 1.23, 1.26, "test3"));

            var latestPrice = await _service.GetLatestPriceAsync(new CurrencyPair("EUR", "USD"));
            Assert.AreEqual("EUR", latestPrice.CurrencyPair.BaseCurrency);
            Assert.AreEqual("USD", latestPrice.CurrencyPair.QuoteCurrency);
            Assert.AreEqual(1.31, latestPrice.Bid);
            Assert.AreEqual(1.34, latestPrice.Ask);
            Assert.AreEqual("test12", latestPrice.Contributor);
            Assert.Less(default(DateTime), latestPrice.ContributionTime);

            latestPrice = await _service.GetLatestPriceAsync(new CurrencyPair("USD", "JPY"));
            Assert.AreEqual("USD", latestPrice.CurrencyPair.BaseCurrency);
            Assert.AreEqual("JPY", latestPrice.CurrencyPair.QuoteCurrency);
            Assert.AreEqual(1.22, latestPrice.Bid);
            Assert.AreEqual(1.25, latestPrice.Ask);
            Assert.AreEqual("test2", latestPrice.Contributor);
            Assert.Less(default(DateTime), latestPrice.ContributionTime);
        }

        [Test]
        public async Task AddThreePrices_And_GetNonExistCcy()
        {
            await _service.AddPriceAsync(new Price("EUR", "USD", 1.21, 1.24, "test1"));
            await _service.AddPriceAsync(new Price("USD", "JPY", 1.22, 1.25, "test2"));
            await _service.AddPriceAsync(new Price("USD", "CHF", 1.23, 1.26, "test3"));

            var ex = Assert.ThrowsAsync<ArgumentException>(() => _service.GetLatestPriceAsync(new CurrencyPair("USD", "EUR")));
            Assert.True(ex.Message.Contains("No price"));
        }
    }
}
