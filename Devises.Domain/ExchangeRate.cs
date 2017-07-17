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
            GetHashCodeCombiner.Combine(this.rate);

        public ExchangeRate Invert() =>
            new ExchangeRate(1 / this.rate);


        public static implicit operator decimal(ExchangeRate e)
        {
            return e.rate;
        }

        public static ExchangeRate operator *(ExchangeRate a, ExchangeRate b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return new ExchangeRate(a.rate * b.rate);
        }

        public static decimal operator *(ExchangeRate a, decimal b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            return a.rate * b;
        }

        public override string ToString() =>
            this.rate.ToString(CultureInfo.InvariantCulture);
    }
}
