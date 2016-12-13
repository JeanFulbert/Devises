namespace Devises.Domain.Tests.GraphSpecs
{
    using Devises.Domain.Graph;
    using NUnit.Framework;

    [TestFixture]
    public class ShouldReturnComplexPath
    {
        [Test]
        public void WhenRouteIsStraightForward()
        {
            var sut = new ExchangeRateGraph();
            sut.AddEdge(new ExchangeRateEdge(Currencies.Euro, Currencies.Dollar, new ExchangeRate(2m)));
            sut.AddEdge(new ExchangeRateEdge(Currencies.Dollar, Currencies.Yuan, new ExchangeRate(3m)));

            var actual = sut.GetShortestPathBetween(Currencies.Euro, Currencies.Yuan);

            var expected =
                new[]
                {
                    new ExchangeRateEdge(Currencies.Euro, Currencies.Dollar, new ExchangeRate(2)),
                    new ExchangeRateEdge(Currencies.Dollar, Currencies.Yuan, new ExchangeRate(3)),
                };

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void WhenRouteIsStraightBackward()
        {
            var sut = new ExchangeRateGraph();
            sut.AddEdge(new ExchangeRateEdge(Currencies.Euro, Currencies.Dollar, new ExchangeRate(2m)));
            sut.AddEdge(new ExchangeRateEdge(Currencies.Dollar, Currencies.Yuan, new ExchangeRate(3m)));

            var actual = sut.GetShortestPathBetween(Currencies.Yuan, Currencies.Euro);

            var expected =
                new[]
                {
                    new ExchangeRateEdge(Currencies.Yuan, Currencies.Dollar, new ExchangeRate(0.3333m)),
                    new ExchangeRateEdge(Currencies.Dollar, Currencies.Euro, new ExchangeRate(0.5m)),
                };

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
