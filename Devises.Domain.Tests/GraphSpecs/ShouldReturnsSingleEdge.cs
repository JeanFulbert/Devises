namespace Devises.Domain.Tests.GraphSpecs
{
    using Devises.Domain.Graph;
    using NUnit.Framework;

    [TestFixture]
    public class ShouldReturnsSingleEdge
    {
        [Test]
        public void WhenDirectCurrenciesAsked()
        {
            var sut = new ExchangeRateGraph();
            sut.AddEdge(new ExchangeRateEdge(Currencies.Euro, Currencies.Dollar, new ExchangeRate(1.06m)));

            var actual = sut.GetShortestPathBetween(Currencies.Euro, Currencies.Dollar);

            var expected = new ExchangeRateEdge(Currencies.Euro, Currencies.Dollar, new ExchangeRate(1.06m));
            Assert.That(actual, Has.Exactly(1).EqualTo(expected));
        }

        [Test]
        public void WhenDirectInvertedCurrenciesAsked()
        {
            var sut = new ExchangeRateGraph();
            sut.AddEdge(new ExchangeRateEdge(Currencies.Euro, Currencies.Dollar, new ExchangeRate(1.06m)));

            var actual = sut.GetShortestPathBetween(Currencies.Dollar, Currencies.Euro);

            var expected = new ExchangeRateEdge(Currencies.Dollar, Currencies.Euro, new ExchangeRate(0.9434m));
            Assert.That(actual, Has.Exactly(1).EqualTo(expected));
        }
    }
}
