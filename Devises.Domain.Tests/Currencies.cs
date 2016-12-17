namespace Devises.Domain.Tests
{
    public static class Currencies
    {
        public static Currency Euro { get; } = new Currency("EUR");
        public static Currency Dollar { get; } = new Currency("USD");
        public static Currency Yuan { get; } = new Currency("CNY");
        public static Currency Yen { get; } = new Currency("JPY");
        public static Currency SwissFranc { get; } = new Currency("CHF");
        public static Currency AustralianDollar { get; } = new Currency("AUD");
        public static Currency Wu { get; } = new Currency("KWU");
        public static Currency Rupee { get; } = new Currency("INR");
    }
}
