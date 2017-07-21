namespace Devises.Domain
{
    using Devises.Domain.Utils;
    using System;
    using System.Globalization;

    public class Rate : ValueObject<Rate>
    {
        private readonly decimal rate;

        public Rate(decimal rate)
        {
            if (rate <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rate));
            }

            this.rate = Decimal.Round(rate, 4);
        }

        protected override bool EqualsCore(Rate other) =>
            this.rate == other.rate;

        protected override int GetHashCodeCore() =>
            HashCode.Combine(this.rate);

        public Rate Invert() =>
            new Rate(1 / this.rate);


        public static implicit operator decimal(Rate e)
        {
            return e.rate;
        }

        public static Rate operator *(Rate a, Rate b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return new Rate(a.rate * b.rate);
        }

        public static decimal operator *(Rate a, decimal b)
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
