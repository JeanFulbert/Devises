namespace Devises.Domain.Graph
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
            var shortestPathLeaf = this.SearchShortestPathWithBreadthFirstSearch(from, to);
            var shortestPath = this.GetShortestPathFromLeafNode(shortestPathLeaf);
            return shortestPath;
        }

        private CurrencySearchTreeNode SearchShortestPathWithBreadthFirstSearch(Currency from, Currency to)
        {
            if (!this.matrix.ContainsKey(from) || !this.matrix.ContainsKey(to))
            {
                return null;
            }

            var nodesToVisit = new Queue<CurrencySearchTreeNode>();
            var unvisitedCurrencies = new HashSet<Currency>(this.matrix.Keys);

            CurrencySearchTreeNode shortestPathLeaf = null;

            nodesToVisit.Enqueue(new CurrencySearchTreeNode(from));
            while (nodesToVisit.Any())
            {
                var node = nodesToVisit.Dequeue();
                if (node.Currency == to)
                {
                    shortestPathLeaf = node;
                    break;
                }

                unvisitedCurrencies.Remove(node.Currency);

                var currenciesToVisit =
                    this.matrix[node.Currency].Keys
                        .Where(c => unvisitedCurrencies.Contains(c))
                        .ToList();

                foreach (var currencyToVisit in currenciesToVisit)
                {
                    var child = node.AddChild(currencyToVisit);
                    nodesToVisit.Enqueue(child);
                }
            }

            return shortestPathLeaf;
        }

        private IReadOnlyCollection<ExchangeRateEdge> GetShortestPathFromLeafNode(CurrencySearchTreeNode shortestPathLeaf)
        {
            if (shortestPathLeaf == null)
            {
                return new List<ExchangeRateEdge>();
            }

            var currenciesFromRoot = shortestPathLeaf.GetAllCurrenciesFromRoot();

            var edges =
                currenciesFromRoot
                    .Zip(
                        currenciesFromRoot.Skip(1),
                        this.BuildExchangeRateEdge)
                    .ToList();

            return edges;
        }

        private ExchangeRateEdge BuildExchangeRateEdge(Currency cFrom, Currency cTo)
        {
            var rate = this.matrix[cFrom][cTo];
            return new ExchangeRateEdge(cFrom, cTo, rate);
        }
    }
}