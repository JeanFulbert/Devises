namespace Devises.Domain.Tests.GraphSpecs
{
    using Devises.Domain.Graph;
    using NUnit.Framework;

    [TestFixture]
    public class ShouldReturnEmptyList
    {
        [Test]
        public void WhenFromCurrencyDoesNotExists()
        {
            var sut = new ExchangeRateGraph();
            sut.AddEdge(new ExchangeRateEdge(Currencies.Euro, Currencies.Dollar, new ExchangeRate(1.06m)));

            var actual = sut.GetShortestPathBetween(Currencies.Yuan, Currencies.Euro);

            Assert.That(actual, Is.Empty);
        }

        [Test]
        public void WhenToCurrencyDoesNotExists()
        {
            var sut = new ExchangeRateGraph();
            sut.AddEdge(new ExchangeRateEdge(Currencies.Euro, Currencies.Dollar, new ExchangeRate(1.06m)));

            var actual = sut.GetShortestPathBetween(Currencies.Euro, Currencies.Yuan);

            Assert.That(actual, Is.Empty);
        }

        [Test]
        public void WhenPathContainsHole()
        {
            var sut = new ExchangeRateGraph();
            sut.AddEdge(new ExchangeRateEdge(Currencies.Euro, Currencies.Dollar, new ExchangeRate(2)));
            sut.AddEdge(new ExchangeRateEdge(Currencies.Yen, Currencies.Yuan, new ExchangeRate(3)));

            var actual = sut.GetShortestPathBetween(Currencies.Euro, Currencies.Yuan);

            Assert.That(actual, Is.Empty);
        }

    }
}
