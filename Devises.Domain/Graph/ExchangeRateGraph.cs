namespace Devises.Domain.Graph
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

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
            if (this.matrix.ContainsKey(from) && this.matrix[from].ContainsKey(to))
            {
                return new List<ExchangeRateEdge> {new ExchangeRateEdge(from, to, this.matrix[from][to])};
            }

            var currenciesToVisit = new Queue<Currency>();
            var rootNode = new CurrencySearchTreeNode(from);

            currenciesToVisit.Enqueue(from);
            while (currenciesToVisit.Count > 0)
            {
                var currency = currenciesToVisit.Dequeue();
                if (currency == to)
                {
                    rootNode.AddChild(to);
                }
            }
        }
    }
}