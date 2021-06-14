using System;
using contributor_api.Model;
using NUnit.Framework;

namespace contributor_api_test.Domains
{
    [TestFixture]
    public class CurrencyPairTest
    {
        [Test]
        public void CreateNewCurrencyPair()
        {
            var ccyPair = new CurrencyPair("EUR", "USD");

            Assert.AreEqual("EUR", ccyPair.BaseCurrency);
            Assert.AreEqual("USD", ccyPair.QuoteCurrency);
            Assert.AreEqual("EUR/USD", ccyPair.Iso);
        }

        [Test]
        public void CreateNewCurrencyPair_InvalidCurrency()
        {
            var ex = Assert.Throws<ArgumentException>(() => new CurrencyPair("EURR", "USD"));
            Assert.IsTrue(ex.Message.Contains("Invalid"));

            ex = Assert.Throws<ArgumentException>(() => new CurrencyPair("EUR", "USDD"));
            Assert.IsTrue(ex.Message.Contains("Invalid"));
        }
    }
}
