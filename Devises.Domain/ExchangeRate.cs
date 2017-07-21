namespace Devises.Domain
{
    using Devises.Domain.Utils;
    using System;

    public class ExchangeRate : ValueObject<ExchangeRate>
    {
        public ExchangeRate(Currency from, Currency to, Rate rate)
        {
            if (from == null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            if (rate == null)
            {
                throw new ArgumentNullException(nameof(rate));
            }

            if (from == to)
            {
                throw new ArgumentException("An exchange rate must be between two different currencies");
            }

            this.From = from;
            this.To = to;
            this.Rate = rate;
        }

        public Currency From { get; }

        public Currency To { get; }

        public Rate Rate { get; }

        public ExchangeRate Invert() =>
            new ExchangeRate(this.To, this.From, this.Rate.Invert());

        protected override bool EqualsCore(ExchangeRate other) =>
            this.From == other.From &&
            this.To == other.To &&
            this.Rate == other.Rate;

        protected override int GetHashCodeCore() =>
            HashCode.Combine(
                this.From,
                this.To,
                this.Rate);

        public override string ToString() =>
            $"{this.From} -> {this.To} = {this.Rate}";
    }
}
