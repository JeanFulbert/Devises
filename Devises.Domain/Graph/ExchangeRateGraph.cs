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
            if (!this.matrix.ContainsKey(from))
            {
                return new List<ExchangeRateEdge>();
            }

            var resultNode = this.GetShortestPathWithBreadthFirst(from, to);
            
            var fullPath =
                resultNode != null
                ? this.ConvertResultNodeToEdges(resultNode)
                : new List<ExchangeRateEdge>();

            return fullPath;
        }

        private CurrencySearchTreeNode GetShortestPathWithBreadthFirst(
            Currency source,
            Currency destination)
        {
            var unvisitedCurrencies = new HashSet<Currency>(this.matrix.Keys);
            var nodeToVisit = new Queue<CurrencySearchTreeNode>();
            CurrencySearchTreeNode resultNode = null;

            nodeToVisit.Enqueue(new CurrencySearchTreeNode(source));

            while (nodeToVisit.Any())
            {
                var currentNode = nodeToVisit.Dequeue();
                unvisitedCurrencies.Remove(currentNode.Currency);

                var children =
                    this.matrix[currentNode.Currency].Keys
                        .Where(c => unvisitedCurrencies.Contains(c))
                        .ToList();

                var nodes = currentNode.AddChildren(children);
                var destinationNode = nodes.FirstOrDefault(n => n.Currency == destination);
                if (destinationNode != null)
                {
                    resultNode = destinationNode;
                    break;
                }

                foreach (var node in nodes)
                {
                    nodeToVisit.Enqueue(node);
                }
            }

            return resultNode;
        }

        private IReadOnlyCollection<ExchangeRateEdge> ConvertResultNodeToEdges(CurrencySearchTreeNode resultNode)
        {
            var currenciesFromRoot = resultNode.GetAllCurrenciesFromRoot().ToList();
            var result =
                currenciesFromRoot
                    .Zip(
                        currenciesFromRoot.Skip(1),
                        (c1, c2) => new ExchangeRateEdge(c1, c2, this.matrix[c1][c2]))
                    .ToList();
            return result;
        }
    }
}