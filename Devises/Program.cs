namespace Devises
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("File path is missing.");
                return;
            }

            var filePath = args[0];

            var fileReader = new FileReader();
            try
            {
                var content = fileReader.GetContent(filePath);
                var value = decimal.Round(content.GetValue(), 0, MidpointRounding.AwayFromZero);
                Console.WriteLine(value);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Invalid file: " + e.Message);
            }
            catch (FormatException e)
            {
                Console.WriteLine("File badly formatted: " + e.Message);
            }
        }
    }
}
