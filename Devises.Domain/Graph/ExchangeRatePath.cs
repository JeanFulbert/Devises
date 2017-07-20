using System;
using System.Collections.Generic;
using System.Linq;

namespace Devises.Domain.Graph
{
    public class ExchangeRatePath : ValueObject<ExchangeRatePath>
    {
        private readonly Currency initial;
        private IReadOnlyCollection<Destination> successiveCollection = new List<Destination>();

        private ExchangeRatePath(Currency initial)
        {
            if (initial == null)
            {
                throw new ArgumentNullException(nameof(initial));
            }

            this.initial = initial;
        }

        public ExchangeRatePath To(Currency destination, ExchangeRate rate) =>
            new ExchangeRatePath(this.initial)
            {
                successiveCollection =
                    this.successiveCollection
                        .Concat(new[] {new Destination(destination, rate)})
                        .ToList()
            };

        public Currency LastCurrency =>
            !this.successiveCollection.IsNullOrEmpty()
                ? this.successiveCollection.Last().Currency
                : this.initial;

        public decimal ApplyRatesTo(decimal value)
        {
            if (this.successiveCollection.IsNullOrEmpty())
            {
                return value;
            }

            var valueWithFullRate =
                this.successiveCollection
                    .Select(e => (decimal)e.Rate)
                    .Aggregate(value, (acc, r) => acc * r);

            return valueWithFullRate;
        }

        public static ExchangeRatePath From(Currency initial) =>
            new ExchangeRatePath(initial);

        protected override bool EqualsCore(ExchangeRatePath other) =>
            this.initial == other.initial &&
            this.successiveCollection.Count == other.successiveCollection.Count &&
            this.successiveCollection.SequenceEqual(other.successiveCollection);

        protected override int GetHashCodeCore() =>
            this.initial.GetHashCode()
                .CombineWithAllElementsOf(this.successiveCollection);

        private class Destination : ValueObject<Destination>
        {
            public Destination(Currency currency, ExchangeRate rate)
            {
                this.Currency = currency;
                this.Rate = rate;
            }

            public Currency Currency { get; }

            public ExchangeRate Rate { get; }

            protected override bool EqualsCore(Destination other) =>
                this.Currency == other.Currency &&
                this.Rate == other.Rate;

            protected override int GetHashCodeCore() =>
                HashCode.Combine(
                    this.Currency,
                    this.Rate);
        }
    }
}