namespace Devises.Domain.Graph
{
    using Devises.Domain.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ExchangeRateGraph
    {
        private readonly Dictionary<Currency, Dictionary<Currency, Rate>> rateByCurrencies =
            new Dictionary<Currency, Dictionary<Currency, Rate>>();

        public void Add(ExchangeRate edge)
        {
            if (edge == null)
            {
                throw new ArgumentNullException(nameof(edge));
            }

            this.AddSafe(edge);
            this.AddSafe(edge.Invert());
        }

        private void AddSafe(ExchangeRate edge)
        {
            if (!this.rateByCurrencies.ContainsKey(edge.From))
                this.rateByCurrencies.Add(edge.From, new Dictionary<Currency, Rate>());

            if (!this.rateByCurrencies[edge.From].ContainsKey(edge.To))
                this.rateByCurrencies[edge.From].Add(edge.To, edge.Rate);
            else
                this.rateByCurrencies[edge.From][edge.To] = edge.Rate;
        }

        public ExchangeRatePath GetShortestPathBetween(Currency from, Currency to)
        {
            var shortestPathLeaf = this.SearchShortestPathWithBreadthFirstSearch(from, to);
            var shortestPath = this.GetShortestPathFromLeafNode(shortestPathLeaf);
            return shortestPath;
        }

        private CurrencySearchTreeNode SearchShortestPathWithBreadthFirstSearch(Currency from, Currency to)
        {
            if (!this.rateByCurrencies.ContainsKey(from) || !this.rateByCurrencies.ContainsKey(to))
            {
                return null;
            }

            // To search the shortest path,
            // we build a tree of all pathes already visited (nodesToVisit).
            // We loop on these nodes until a path to the destination currency
            // is found, or until that all currencies has been visited.

            // As we dive into the graph using Breadth first search,
            // we ensure that we don't use again visited currencies
            // to avoid circular pathes (unvisitedCurrencies).

            var nodesToVisit = new Queue<CurrencySearchTreeNode>();
            var unvisitedCurrencies = new HashSet<Currency>(this.rateByCurrencies.Keys);

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
                    this.rateByCurrencies[node.Currency].Keys
                        .Where(destinationCurr => unvisitedCurrencies.Contains(destinationCurr))
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
            var rate = this.rateByCurrencies[path.LastCurrency][currency];
            return path.To(currency, rate);
        }
    }
}