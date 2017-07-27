namespace Devises.Domain.Files
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Devises.Domain;
    using Devises.Domain.Graph;
    using Devises.Domain.Utils;

    public class FileContent : ValueObject<FileContent>
    {
        private readonly Currency source;
        private readonly Currency destination;
        private readonly decimal value;
        private readonly IReadOnlyCollection<ExchangeRate> echangeRates;

        public FileContent(
            Currency source,
            Currency destination,
            decimal value,
            IReadOnlyCollection<ExchangeRate> echangeRates)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.destination = destination ?? throw new ArgumentNullException(nameof(destination));
            this.value = value;
            this.echangeRates = echangeRates ?? throw new ArgumentNullException(nameof(echangeRates));
        }

        public decimal GetValue()
        {
            var graph = new ExchangeRateGraph();
            foreach (var exchRate in this.echangeRates)
            {
                graph.Add(exchRate);
            }

            var shortestPath = graph.GetShortestPathBetween(this.source, this.destination);
            if (shortestPath == null)
            {
                throw new NoCurrencyPathFoundException();
            }

            var convertedValue = shortestPath.ApplyRatesTo(this.value);
            return convertedValue;
        }

        protected override bool EqualsCore(FileContent other) =>
            this.source == other.source &&
            this.destination == other.destination &&
            this.value == other.value &&
            this.echangeRates.SequenceEqual(other.echangeRates);

        protected override int GetHashCodeCore() =>
            HashCode
                .Combine(
                    this.source,
                    this.destination,
                    this.value)
                .CombineWithAllElementsOf(this.echangeRates);
    }
}
