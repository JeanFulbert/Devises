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
                ExchangeRatePath
                    .From(Currencies.Euro)
                    .To(Currencies.Dollar, new ExchangeRate(2))
                    .To(Currencies.Yuan, new ExchangeRate(3));
            
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
                ExchangeRatePath
                    .From(Currencies.Yuan)
                    .To(Currencies.Dollar, new ExchangeRate(0.3333m))
                    .To(Currencies.Euro, new ExchangeRate(0.5m));
            
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
