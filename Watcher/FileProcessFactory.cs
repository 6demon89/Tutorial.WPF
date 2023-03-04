using System;
using System.Text;
using System.IO;

namespace Watcher;

public class FileProcessFactory
{
    /// <summary>
    /// Get File Reader/Process Instance based on the File Extention 
    /// </summary>
    /// <param name="file"> </param>
    /// <returns></returns>
    public ITextFileReader GetFileReader(FileInfo file)
    {
        if (file is null)
            throw new Exception("no file was provided");
        //Check file extention, convert it ToLower, so that we wont get issues.
        //Each extention will provide own reader
        switch (file.Extension.ToLower())
        {
            case ".txt":
                return new TextFileReader(Encoding.UTF8, file.FullName);
            default:
                throw new NotImplementedException();
        }
    }
}
