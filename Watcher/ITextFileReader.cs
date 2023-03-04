using System.Threading.Tasks;

namespace Watcher;

public interface ITextFileReader
{

    /// <summary>
    /// Reading file completly and returning data as a string
    /// </summary>
    Task<string> ReadFileAsync();
}