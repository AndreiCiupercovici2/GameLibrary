using System.Globalization;

namespace GameLibrary.Helpers
{
    public static class CurrencyExtension
    {
        private const decimal ExchangeRateUsdToRon = 4.31m;

        public static string ToCurrency(this decimal priceInUsd)
        {
            var currentCulture = CultureInfo.CurrentUICulture;

            if (currentCulture.Name == "ro-RO")
            {
                decimal priceInRon = priceInUsd * ExchangeRateUsdToRon;
                return priceInRon.ToString("C", currentCulture);
            }

            return priceInUsd.ToString("C", new CultureInfo("en-US"));
        }
    }
}
