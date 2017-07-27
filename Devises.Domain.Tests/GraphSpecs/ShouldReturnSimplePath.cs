namespace Devises.Domain.Tests.GraphSpecs
{
    using Devises.Domain.Graph;
    using NUnit.Framework;

    [TestFixture]
    public class ShouldReturnSimplePath
    {
        [Test]
        public void WhenDirectCurrenciesAsked()
        {
            var sut = new ExchangeRateGraph();
            sut.Add(new ExchangeRate(Currencies.Euro, Currencies.Dollar, new Rate(1.06m)));

            var actual = sut.GetShortestPathBetween(Currencies.Euro, Currencies.Dollar);

            var expected =
                ExchangeRatePath
                    .From(Currencies.Euro)
                    .To(Currencies.Dollar, new Rate(1.06m));
            
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void WhenDirectInvertedCurrenciesAsked()
        {
            var sut = new ExchangeRateGraph();
            sut.Add(new ExchangeRate(Currencies.Euro, Currencies.Dollar, new Rate(1.06m)));

            var actual = sut.GetShortestPathBetween(Currencies.Dollar, Currencies.Euro);

            var expected =
                ExchangeRatePath
                    .From(Currencies.Dollar)
                    .To(Currencies.Euro, new Rate(0.9434m));
            
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
