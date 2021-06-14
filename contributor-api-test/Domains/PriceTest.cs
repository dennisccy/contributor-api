using System;
using contributor_api.Model;
using NUnit.Framework;

namespace contributor_api_test.Domains
{
    [TestFixture]
    public class PriceTest
    {
        [Test]
        public void CreateNewPrice()
        {
            var price = new Price("EUR", "USD", 1.2, 1.21, "test");

            Assert.AreEqual("EUR", price.CurrencyPair.BaseCurrency);
            Assert.AreEqual("USD", price.CurrencyPair.QuoteCurrency);
            Assert.AreEqual(1.2, price.Bid);
            Assert.AreEqual(1.21, price.Ask);
            Assert.Less(default(DateTime), price.ContributionTime);
        }

        [Test]
        public void CreateNewPrice_InvalidBidAsk()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Price("EUR", "USE", -1.2, 1.22, "test"));
            Assert.IsTrue(ex.Message.Contains("Invalid"));

            ex = Assert.Throws<ArgumentException>(() => new Price("EUR", "USE", 1.2, -1.22, "test"));
            Assert.IsTrue(ex.Message.Contains("Invalid"));

            ex = Assert.Throws<ArgumentException>(() => new Price("EUR", "USE", -1.2, -1.22, "test"));
            Assert.IsTrue(ex.Message.Contains("Invalid"));
        }
    }
}
