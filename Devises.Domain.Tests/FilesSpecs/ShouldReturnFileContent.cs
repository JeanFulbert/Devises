namespace Devises.Domain.Tests.FilesSpecs
{
    using System.Collections.Generic;
    using Devises.Domain.Files;
    using Devises.Domain.Graph;
    using NUnit.Framework;

    [TestFixture]
    public class ShouldReturnFileContent
    {
        [Test]
        public void WhenAllTheLinesAreValid()
        {
            var lines = new[]
            {
                "EUR;500;USD",
                "2",
                "EUR;CHF;1.2",
                "CHF;USD;1.3",
            };
            var sut = new FileConverter();

            var actual = sut.Convert(lines);

            var expected =
                new FileContent(
                    Currencies.Euro,
                    Currencies.Dollar,
                    500,
                    new List<ExchangeRate>
                    {
                        new ExchangeRate(Currencies.Euro, Currencies.SwissFranc, new Rate(1.2m)),
                        new ExchangeRate(Currencies.SwissFranc, Currencies.Dollar, new Rate(1.3m)),
                    });

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
