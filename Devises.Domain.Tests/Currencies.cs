namespace Devises.Domain.Tests
{
    public static class Currencies
    {
        public static Currency Euro { get; } = new Currency("EUR");
        public static Currency Dollar { get; } = new Currency("USD");
        public static Currency Yuan { get; } = new Currency("CNY");
    }
}
