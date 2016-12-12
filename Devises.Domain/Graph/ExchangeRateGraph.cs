namespace Devises.Domain.Graph
{
    using System;
    using System.Collections.Generic;

    public class ExchangeRateGraph
    {
        private readonly Dictionary<Currency, Dictionary<Currency, ExchangeRate>> matrix =
            new Dictionary<Currency, Dictionary<Currency, ExchangeRate>>();

        public void AddEdge(ExchangeRateEdge edge)
        {
            if (edge == null)
            {
                throw new ArgumentNullException(nameof(edge));
            }

            this.AddEdgeSafe(edge);
            this.AddEdgeSafe(edge.Invert());
        }

        private void AddEdgeSafe(ExchangeRateEdge edge)
        {
            if (!this.matrix.ContainsKey(edge.From))
                this.matrix.Add(edge.From, new Dictionary<Currency, ExchangeRate>());

            if (!this.matrix[edge.From].ContainsKey(edge.To))
                this.matrix[edge.From].Add(edge.To, edge.Rate);
            else
                this.matrix[edge.From][edge.To] = edge.Rate;
        }

        public IReadOnlyCollection<ExchangeRateEdge> GetShortestPathBetween(Currency from, Currency to)
        {
            if (matrix.ContainsKey(from) && matrix.ContainsKey(to))
            {
                return new[] { new ExchangeRateEdge(from, to, matrix[from][to]) };
            }

            return new List<ExchangeRateEdge>();
        }

        private IReadOnlyCollection<ExchangeRateEdge> GetShortestPathBetween(Currency from, Currency to, IReadOnlyCollection<Currency> usedCurrencies)
        {
            return new List<ExchangeRateEdge>();
        }
    }
}