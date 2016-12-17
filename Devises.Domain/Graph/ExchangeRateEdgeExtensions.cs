namespace Devises.Domain.Graph
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ExchangeRateEdgeExtensions
    {
        public static decimal ApplyRates(
            this IReadOnlyCollection<ExchangeRateEdge> source,
            decimal value)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var valueWithFullRate =
                source
                    .Select(e => (decimal)e.Rate)
                    .Aggregate(value, (acc, r) => acc * r);

            return valueWithFullRate;
        }
    }
}
