namespace Devises.Domain.Tests.TreeSpecs
{
    using Devises.Domain.Graph;
    using NUnit.Framework;

    [TestFixture]
    public class CurrencySearchTreeSpecs
    {
        [Test]
        public void ShouldReturnFullPath()
        {
            var sut =
                new CurrencySearchTreeNode(Currencies.Euro)
                    .CreateChild(Currencies.Dollar)
                    .CreateChild(Currencies.Yuan);

            var actual = sut.GetAllCurrenciesFromRoot();

            var expected = new[] { Currencies.Euro, Currencies.Dollar, Currencies.Yuan };
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
