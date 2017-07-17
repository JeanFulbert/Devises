namespace Devises.Domain.Graph
{
    using System;

    public class ExchangeRateEdge : ValueObject<ExchangeRateEdge>
    {
        public ExchangeRateEdge(Currency from, Currency to, ExchangeRate rate)
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

        public ExchangeRate Rate { get; }

        public ExchangeRateEdge Invert() =>
            new ExchangeRateEdge(this.To, this.From, this.Rate.Invert());

        protected override bool EqualsCore(ExchangeRateEdge other) =>
            this.From == other.From &&
            this.To == other.To &&
            this.Rate == other.Rate;

        protected override int GetHashCodeCore() =>
            GetHashCodeCombiner.Combine(
                this.From,
                this.To,
                this.Rate);

        public override string ToString() =>
            $"{this.From} -> {this.To} = {this.Rate}";
    }
}
