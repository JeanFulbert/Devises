namespace Devises.Domain.Graph
{
    using Devises.Domain.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ExchangeRateGraph
    {
        private readonly Dictionary<Currency, Dictionary<Currency, Rate>> matrix =
            new Dictionary<Currency, Dictionary<Currency, Rate>>();

        public void AddEdge(ExchangeRate edge)
        {
            if (edge == null)
            {
                throw new ArgumentNullException(nameof(edge));
            }

            this.AddEdgeSafe(edge);
            this.AddEdgeSafe(edge.Invert());
        }

        private void AddEdgeSafe(ExchangeRate edge)
        {
            if (!this.matrix.ContainsKey(edge.From))
                this.matrix.Add(edge.From, new Dictionary<Currency, Rate>());

            if (!this.matrix[edge.From].ContainsKey(edge.To))
                this.matrix[edge.From].Add(edge.To, edge.Rate);
            else
                this.matrix[edge.From][edge.To] = edge.Rate;
        }

        public ExchangeRatePath GetShortestPathBetween(Currency from, Currency to)
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

                var children = node.CreateChildren(currenciesToVisit);
                nodesToVisit.EnqueueAll(children);
            }

            return shortestPathLeaf;
        }
        
        private ExchangeRatePath GetShortestPathFromLeafNode(CurrencySearchTreeNode shortestPathLeaf)
        {
            if (shortestPathLeaf == null)
            {
                return null;
            }

            var currenciesFromRoot = shortestPathLeaf.GetAllCurrenciesFromRoot();

            var headCurrency = currenciesFromRoot.First();
            var tailCurrency = currenciesFromRoot.Skip(1);

            var path =
                tailCurrency
                    .Aggregate(
                        ExchangeRatePath.From(headCurrency),
                        this.AddDestinationRate);

            return path;
        }

        private ExchangeRatePath AddDestinationRate(ExchangeRatePath path, Currency currency)
        {
            var rate = this.matrix[path.LastCurrency][currency];
            return path.To(currency, rate);
        }
    }
}