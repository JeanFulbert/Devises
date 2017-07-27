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
            sut.Add(new ExchangeRate(Currencies.Euro, Currencies.Dollar, new Rate(2m)));
            sut.Add(new ExchangeRate(Currencies.Dollar, Currencies.Yuan, new Rate(3m)));

            var actual = sut.GetShortestPathBetween(Currencies.Euro, Currencies.Yuan);

            var expected =
                ExchangeRatePath
                    .From(Currencies.Euro)
                    .To(Currencies.Dollar, new Rate(2))
                    .To(Currencies.Yuan, new Rate(3));
            
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void WhenRouteIsStraightBackward()
        {
            var sut = new ExchangeRateGraph();
            sut.Add(new ExchangeRate(Currencies.Euro, Currencies.Dollar, new Rate(2m)));
            sut.Add(new ExchangeRate(Currencies.Dollar, Currencies.Yuan, new Rate(3m)));

            var actual = sut.GetShortestPathBetween(Currencies.Yuan, Currencies.Euro);

            var expected =
                ExchangeRatePath
                    .From(Currencies.Yuan)
                    .To(Currencies.Dollar, new Rate(0.3333m))
                    .To(Currencies.Euro, new Rate(0.5m));
            
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
