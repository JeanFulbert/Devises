namespace Devises.Domain.Files
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Devises.Domain.Graph;
    using Devises.Domain.Utils;

    public class FileConverter
    {
        private const string SourceDevisePattern = @"(?<source>\w{3})";
        private const string RatioPattern = @"(?<ratio>\d+(?:[\.,]\d+))";
        private const string DestinationDevisePattern = @"(?<dest>\w{3})";

        private static readonly Regex HeaderRegex =
            new Regex(SourceDevisePattern + @";(?<value>\d+);" + DestinationDevisePattern);
        private static readonly Regex NumberOfOtherLinesRegex = new Regex(@"^\d+$");
        private static readonly Regex CurrencyTuplesRegex =
            new Regex(
                SourceDevisePattern + ";" +
                DestinationDevisePattern + ";" +
                RatioPattern);

        public FileContent Convert(IReadOnlyList<string> lines)
        {
            if (lines.IsNullOrEmpty() || lines.Count < 3)
            {
                throw new InvalidOperationException("The file must contains at least 3 lines.");
            }

            var headerLine = lines.First();
            var numberOfLinesLine = lines[1];
            var exchangeRateLines = lines.Skip(2).ToList();

            var header = GetHeader(headerLine);
            var exchangeRates = GetExchangeRates(exchangeRateLines);

            VerifyNumberOfLines(numberOfLinesLine, exchangeRates);

            return
                new FileContent(
                    header.Source,
                    header.Destination,
                    header.Value,
                    exchangeRates);
        }

        private static Header GetHeader(string headerLine)
        {
            var header = Header.FromString(headerLine);
            if (header == null)
            {
                throw new InvalidOperationException("Header format is not valid.");
            }

            return header;
        }

        private static IReadOnlyCollection<ExchangeRate> GetExchangeRates(List<string> exchangeRateLines)
        {
            var exchangeRates =
                exchangeRateLines
                    .Select(BuildRateEdgeFromString)
                    .ToList();
            if (exchangeRates.Any(e => e == null))
            {
                throw new InvalidOperationException("One of the lines format is not valid.");
            }

            return exchangeRates;
        }

        private static void VerifyNumberOfLines(string numberOfLinesLine, IReadOnlyCollection<ExchangeRate> exchangeRates)
        {
            var numberOfLinesMatch = NumberOfOtherLinesRegex.Match(numberOfLinesLine);
            if (!numberOfLinesMatch.Success)
            {
                throw new InvalidOperationException("Number of lines format is not valid.");
            }

            var numberOfLines = int.Parse(numberOfLinesMatch.Groups[0].Value);
            if (numberOfLines <= 0)
            {
                throw new InvalidOperationException("The number of lines must be strictly positive.");
            }

            if (numberOfLines != exchangeRates.Count)
            {
                throw new InvalidOperationException("The expected number of lines doesn't match the number of lines.");
            }
        }

        private static ExchangeRate BuildRateEdgeFromString(string line)
        {
            var match = CurrencyTuplesRegex.Match(line);
            if (!match.Success)
            {
                return null;
            }

            var source = match.Groups["source"].Value;
            var dest = match.Groups["dest"].Value;
            var value = decimal.Parse(match.Groups["ratio"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

            return new ExchangeRate(new Currency(source), new Currency(dest), new Rate(value));
        }

        private class Header
        {
            private Header(Currency source, Currency destination, decimal value)
            {
                this.Source = source ?? throw new ArgumentNullException(nameof(source));
                this.Destination = destination ?? throw new ArgumentNullException(nameof(destination));
                this.Value = value;
            }

            public Currency Source { get; }
            public Currency Destination { get; }
            public decimal Value { get; }

            public static Header FromString(string line)
            {
                var headerMatch = HeaderRegex.Match(line);
                if (!headerMatch.Success)
                {
                    return null;
                }

                var source = headerMatch.Groups["source"].Value;
                var dest = headerMatch.Groups["dest"].Value;
                var value = decimal.Parse(headerMatch.Groups["value"].Value);

                return
                    new Header(
                        new Currency(source),
                        new Currency(dest),
                        value);
            }
        }
    }
}
