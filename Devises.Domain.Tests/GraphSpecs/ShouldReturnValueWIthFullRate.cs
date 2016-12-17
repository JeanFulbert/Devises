namespace Devises.Domain.Tests.GraphSpecs
{
    using System;
    using Devises.Domain.Graph;
    using NUnit.Framework;

    [TestFixture]
    public class ShouldReturnValueWIthFullRate
    {
        [Test]
        public void WhenRouteIsComplex()
        {
            var sut = new ExchangeRateGraph();
            sut.AddEdge(new ExchangeRateEdge(Currencies.AustralianDollar, Currencies.SwissFranc, new ExchangeRate(0.9661m)));
            sut.AddEdge(new ExchangeRateEdge(Currencies.Yen, Currencies.Wu, new ExchangeRate(13.1151m)));
            sut.AddEdge(new ExchangeRateEdge(Currencies.Euro, Currencies.SwissFranc, new ExchangeRate(1.2053m)));
            sut.AddEdge(new ExchangeRateEdge(Currencies.AustralianDollar, Currencies.Yen, new ExchangeRate(86.0305m)));
            sut.AddEdge(new ExchangeRateEdge(Currencies.Euro, Currencies.Dollar, new ExchangeRate(1.2989m)));
            sut.AddEdge(new ExchangeRateEdge(Currencies.Yen, Currencies.Rupee, new ExchangeRate(0.6571m)));

            var fullValue =
                sut.GetShortestPathBetween(Currencies.Euro, Currencies.Yen)
                    .ApplyRates(550);
            var actual = decimal.Round(fullValue, 0);

            var expected = 59033m;

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
