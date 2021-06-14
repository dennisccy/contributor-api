using System;
namespace contributor_api.Model
{
    public class Price
    {
        public CurrencyPair CurrencyPair { get; }
        public double Bid { get; }
        public double Ask { get; }
        public string Contributor { get; }
        public DateTime ContributionTime { get; }

        public Price(string baseCurrency, string quoteCurrency, double bid, double ask, string contributor)
        {
            if (!IsValidValue(bid) || !IsValidValue(ask))
            {
                throw new ArgumentException($"Invalid bid/ask: {bid}/{ask}");
            }

            CurrencyPair = new CurrencyPair(baseCurrency, quoteCurrency);
            Bid = bid;
            Ask = ask;
            Contributor = contributor;
            ContributionTime = DateTime.Now;
        }

        private bool IsValidValue(double value)
        {
            return !double.IsNaN(value) && value > 0;
        }
    }
}
