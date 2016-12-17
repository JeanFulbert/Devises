namespace Devises.Domain.Tests.FilesSpecs
{
    using System;
    using Devises.Domain.Files;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class ShouldThrowAnException
    {
        [Test]
        public void WhenUnsuffisantLinesWithEmptyLines()
        {
            var sut = new FileConverter();

            Assert.Throws<InvalidOperationException>(
                () => sut.Convert(new List<string>()));
        }

        [Test]
        public void WhenUnsuffisantLinesWithOneLine()
        {
            var sut = new FileConverter();

            Assert.Throws<InvalidOperationException>(
                () => sut.Convert(new List<string> { "EUR;500;USD" }));
        }

        [Test]
        public void WhenUnsuffisantLinesWithTwoLines()
        {
            var sut = new FileConverter();

            Assert.Throws<InvalidOperationException>(
                () => sut.Convert(new List<string> { "EUR;500;USD", "0" }));
        }

        [Test]
        public void WhenHeaderIsBadlyFormatted()
        {
            var sut = new FileConverter();

            Assert.Throws<InvalidOperationException>(
                () => sut.Convert(new List<string> { "bbbbbbb", "1", "EUR;USD;1.2" }));
        }

        [Test]
        public void WhenNumberOfLinesIsBadlyFormatted()
        {
            var sut = new FileConverter();

            Assert.Throws<InvalidOperationException>(
                () => sut.Convert(new List<string> { "EUR;500;USD", "bbbbb", "EUR;USD;1.2" }));
        }

        [Test]
        public void WhenOneOfOtherLinesIsBadlyFormatted()
        {
            var sut = new FileConverter();

            Assert.Throws<InvalidOperationException>(
                () => sut.Convert(new List<string>
                {
                    "EUR;500;USD",
                    "2",
                    "EUR;USD;1.2",
                    "bbbbbbbbbb"
                }));
        }
    }
}
