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
            sut.AddEdge(new ExchangeRate(Currencies.Euro, Currencies.Dollar, new Rate(2m)));
            sut.AddEdge(new ExchangeRate(Currencies.Dollar, Currencies.Yen, new Rate(2.5m)));
            sut.AddEdge(new ExchangeRate(Currencies.Yen, Currencies.Yuan, new Rate(3m)));
            sut.AddEdge(new ExchangeRate(Currencies.Dollar, Currencies.Yuan, new Rate(3.5m)));

            var actual = sut.GetShortestPathBetween(Currencies.Euro, Currencies.Yuan);

            var expected =
                ExchangeRatePath
                    .From(Currencies.Euro)
                    .To(Currencies.Dollar, new Rate(2m))
                    .To(Currencies.Yuan, new Rate(3.5m));
            
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void WhenRouteIsComplex()
        {
            var sut = new ExchangeRateGraph();
            sut.AddEdge(new ExchangeRate(Currencies.AustralianDollar, Currencies.SwissFranc, new Rate(0.9661m)));
            sut.AddEdge(new ExchangeRate(Currencies.Yen, Currencies.Wu, new Rate(13.1151m)));
            sut.AddEdge(new ExchangeRate(Currencies.Euro, Currencies.SwissFranc, new Rate(1.2053m)));
            sut.AddEdge(new ExchangeRate(Currencies.AustralianDollar, Currencies.Yen, new Rate(86.0305m)));
            sut.AddEdge(new ExchangeRate(Currencies.Euro, Currencies.Dollar, new Rate(1.2989m)));
            sut.AddEdge(new ExchangeRate(Currencies.Yen, Currencies.Rupee, new Rate(0.6571m)));

            var actual = sut.GetShortestPathBetween(Currencies.Euro, Currencies.Yen);

            var expected =
                ExchangeRatePath
                    .From(Currencies.Euro)
                    .To(Currencies.SwissFranc, new Rate(1.2053m))
                    .To(Currencies.AustralianDollar, new Rate(1.0351m))
                    .To(Currencies.Yen, new Rate(86.0305m));
            
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
