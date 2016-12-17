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
                new[]
                {
                    new ExchangeRateEdge(Currencies.Euro, Currencies.Dollar, new ExchangeRate(2m)),
                    new ExchangeRateEdge(Currencies.Dollar, Currencies.Yuan, new ExchangeRate(3.5m)),
                };

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
