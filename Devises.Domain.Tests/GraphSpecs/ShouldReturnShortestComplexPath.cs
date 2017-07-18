namespace Devises.Domain.Tests.GraphSpecs
{
    using Devises.Domain.Graph;
    using NUnit.Framework;

    [TestFixture]
    public class ShouldReturnShortestComplexPath
    {
        [Test]
        public void WhenRouteIsStraightForward()
        {
            var sut = new ExchangeRateGraph();
            sut.AddEdge(new ExchangeRateEdge(Currencies.Euro, Currencies.Dollar, new ExchangeRate(2m)));
            sut.AddEdge(new ExchangeRateEdge(Currencies.Dollar, Currencies.Yen, new ExchangeRate(2.5m)));
            sut.AddEdge(new ExchangeRateEdge(Currencies.Yen, Currencies.Yuan, new ExchangeRate(3m)));
            sut.AddEdge(new ExchangeRateEdge(Currencies.Dollar, Currencies.Yuan, new ExchangeRate(3.5m)));

            var actual = sut.GetShortestPathBetween(Currencies.Euro, Currencies.Yuan);

            var expected =
                ExchangeRatePath
                    .From(Currencies.Euro)
                    .To(Currencies.Dollar, new ExchangeRate(2m))
                    .To(Currencies.Yuan, new ExchangeRate(3.5m));
            
            Assert.That(actual, Is.EqualTo(expected));
        }

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

            var actual = sut.GetShortestPathBetween(Currencies.Euro, Currencies.Yen);

            var expected =
                ExchangeRatePath
                    .From(Currencies.Euro)
                    .To(Currencies.SwissFranc, new ExchangeRate(1.2053m))
                    .To(Currencies.AustralianDollar, new ExchangeRate(1.0351m))
                    .To(Currencies.Yen, new ExchangeRate(86.0305m));
            
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
