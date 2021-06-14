using System;
namespace contributor_api.Model
{
    public class CurrencyPair
    {
        public string BaseCurrency { get; }
        public string QuoteCurrency { get; }
        public string Iso { get; }

        public CurrencyPair(string baseCurrency, string quoteCurrency)
        {
            if (!IsValidCurrency(baseCurrency))
            {
                throw new ArgumentException($"Invalid base currency: {baseCurrency}");
            }

            if (!IsValidCurrency(quoteCurrency))
            {
                throw new ArgumentException($"Invalid quote currency: {quoteCurrency}");
            }

            BaseCurrency = baseCurrency.ToUpper();
            QuoteCurrency = quoteCurrency.ToUpper();

            Iso = BaseCurrency + "/" + QuoteCurrency;
        }

        private static bool IsValidCurrency(string currency)
        {
            return !string.IsNullOrEmpty(currency) && currency.Length == 3 ;
        }
    }
}
