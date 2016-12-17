namespace Devises.Domain.Files
{
    using System;
    using System.Collections.Generic;
    using Devises.Domain;
    using Devises.Domain.Graph;

    public class FileContent
    {
        private readonly Currency source;
        private readonly Currency destination;
        private readonly decimal value;
        private readonly IReadOnlyCollection<ExchangeRateEdge> edges;

        public FileContent(
            Currency source, 
            Currency destination, 
            decimal value,
            IReadOnlyCollection<ExchangeRateEdge> edges)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (edges == null)
            {
                throw new ArgumentNullException(nameof(edges));
            }

            this.source = source;
            this.destination = destination;
            this.value = value;
            this.edges = edges;
        }

        public decimal GetValue()
        {
            var graph = new ExchangeRateGraph();
            foreach (var exchangeRateEdge in this.edges)
            {
                graph.AddEdge(exchangeRateEdge);
            }

            var shortestPath = graph.GetShortestPathBetween(this.source, this.destination);
            if (shortestPath.IsNullOrEmpty())
            {
                throw new NoCurrencyPathFoundException();
            }

            var convertedValue = shortestPath.ApplyRates(this.value);
            return convertedValue;
        }
    }
}
