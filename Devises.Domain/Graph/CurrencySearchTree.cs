﻿namespace Devises.Domain.Graph
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CurrencySearchTreeNode
    {
        private CurrencySearchTreeNode parent;
        private readonly List<CurrencySearchTreeNode> children = new List<CurrencySearchTreeNode>();

        public CurrencySearchTreeNode(Currency currency)
        {
            if (currency == null)
            {
                throw new ArgumentNullException(nameof(currency));
            }

            this.Currency = currency;
        }

        public Currency Currency { get; }

        public IReadOnlyCollection<CurrencySearchTreeNode> Children => this.children;

        public void AddChild(Currency child)
        {
            this.children.Add(new CurrencySearchTreeNode(child) { parent = this });
        }

        public IEnumerable<Currency> GetAllCurrenciesFromRoot()
        {
            var nodeStack = new Stack<CurrencySearchTreeNode>();

            var currentCurrency = this;
            while (currentCurrency != null)
            {
                nodeStack.Push(currentCurrency);
                currentCurrency = currentCurrency.parent;
            }

            var currenciesFromRoot =
                nodeStack
                    .Select(n => n.Currency)
                    .ToList();

            return currenciesFromRoot;
        }
    }
}