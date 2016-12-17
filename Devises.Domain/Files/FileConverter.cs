namespace Devises.Domain.Files
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class FileConverter
    {
        private const string SourceDevisePattern = @"(?<source>\w{3})";
        private const string RatioPattern = @"(?<ratio>\d+(?:[\.,]\d))";
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
            var numberOfLinesLine = lines.ElementAt(1);
            var currencyTuplesLines = lines.Skip(2).ToList();

            var headerMatch = HeaderRegex.Match(headerLine);
            if (!headerMatch.Success)
            {
                throw new InvalidOperationException("Header format is not valid.");
            }

            var numberOfLinesMatch = NumberOfOtherLinesRegex.Match(numberOfLinesLine);
            if (!numberOfLinesMatch.Success)
            {
                throw new InvalidOperationException("Number of lines format is not valid.");
            }

            return null;
        }
    }
}
