using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Watcher
{
    public class TextFileReader : ITextFileReader
    {
        readonly Encoding _encoder;
        readonly string _filePath;

        public TextFileReader(Encoding encoder, string filePath)
        {
            _encoder = encoder;
            _filePath = filePath;
        }

        public async Task<string> ReadFileAsync()
        {
            if (new FileInfo(_filePath).Exists)
            {
                Char[] buffer;
                using (var streamReader = new StreamReader(_filePath, _encoder))
                {
                    buffer = new char[(int)streamReader.BaseStream.Length];
                    await streamReader.ReadAsync(buffer, 0, (int)streamReader.BaseStream.Length);
                }
                return new string(buffer);
            }
            else
                throw new Exception("File does not exists");
        }




    }
}
