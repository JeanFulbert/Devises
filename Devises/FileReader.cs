namespace Devises
{
    using System.IO;
    using Devises.Domain.Files;

    public class FileReader
    {
        private readonly FileConverter fileConverter = new FileConverter();

        public FileContent GetContent(string path)
        {
            var lines = File.ReadAllLines(path);
            var fileContent = this.fileConverter.Convert(lines);
            return fileContent;
        }
    }
}
