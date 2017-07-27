namespace Devises.Domain.Tests.GraphSpecs
{
    using Devises.Domain.Graph;
    using NUnit.Framework;

    [TestFixture]
    public class ShouldReturnValueWithFullRate
    {
        [Test]
        public void WhenRouteIsComplex()
        {
            var sut = new ExchangeRateGraph();
            sut.Add(new ExchangeRate(Currencies.AustralianDollar, Currencies.SwissFranc, new Rate(0.9661m)));
            sut.Add(new ExchangeRate(Currencies.Yen, Currencies.Wu, new Rate(13.1151m)));
            sut.Add(new ExchangeRate(Currencies.Euro, Currencies.SwissFranc, new Rate(1.2053m)));
            sut.Add(new ExchangeRate(Currencies.AustralianDollar, Currencies.Yen, new Rate(86.0305m)));
            sut.Add(new ExchangeRate(Currencies.Euro, Currencies.Dollar, new Rate(1.2989m)));
            sut.Add(new ExchangeRate(Currencies.Yen, Currencies.Rupee, new Rate(0.6571m)));

            var fullValue =
                sut.GetShortestPathBetween(Currencies.Euro, Currencies.Yen)
                    .ApplyRatesTo(550);
            var actual = decimal.Round(fullValue, 0);

            Assert.That(actual, Is.EqualTo(59033m));
        }
    }
}
