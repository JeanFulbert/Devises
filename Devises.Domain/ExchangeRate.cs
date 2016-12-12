namespace Devises.Domain
{
    using System;
    using System.Globalization;

    public class ExchangeRate : ValueObject<ExchangeRate>
    {
        private readonly decimal rate;

        public ExchangeRate(decimal rate)
        {
            if (rate <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rate));
            }

            this.rate = Decimal.Round(rate, 4);
        }

        protected override bool EqualsCore(ExchangeRate other) =>
            this.rate == other.rate;

        protected override int GetHashCodeCore() =>
            GetHashCodeCombiner.GetCombinedHashCode(this.rate);

        public ExchangeRate Invert() =>
            new ExchangeRate(1 / this.rate);

        public override string ToString() =>
            this.rate.ToString(CultureInfo.InvariantCulture);
    }
}
