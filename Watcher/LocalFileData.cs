using System;
using System.IO;

namespace Watcher;

public class LocalFileData
{
    readonly string _folderCopy;
    readonly string _folderProccess;
    readonly string _folderFail;

    public LocalFileData()
    {
        //Merging application Folder (where the *.exe is located)
        string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileManagment");
        //Holds data copies, which we want to be proccessed
        _folderCopy = Path.Combine(basePath, "Copy");
        //Holds data that is currently in process
        _folderProccess = Path.Combine(basePath, "Proccess");
        //Holds data which failed
        _folderFail = Path.Combine(basePath, "Fail");
        //Check if the Directories Exists, if not => create them
        if (!ValidateDirectoriesExsist(basePath)) CreateDirectories(basePath);
    }

    public string GetCopyFolderPath() => _folderCopy;
    public string GetProcessFolderPath() => _folderProccess;
    public string GetFailFolderPath() => _folderFail;

    /// <summary>
    /// Creates all requered Directories
    /// </summary>
    /// <param name="basePath">Base directory path</param>
    private void CreateDirectories(string basePath)
    {
        Directory.CreateDirectory(basePath);
        Directory.CreateDirectory(_folderProccess);
        Directory.CreateDirectory(_folderCopy);
        Directory.CreateDirectory(_folderFail);
    }

    /// <summary>
    /// Checks if all directorier are created
    /// </summary>
    /// <param name="basePath">Bae directory path</param>
    /// <returns></returns>
    private bool ValidateDirectoriesExsist(string basePath)
        => Directory.Exists(basePath)
        && Directory.Exists(_folderFail)
        && Directory.Exists(_folderCopy)
        && Directory.Exists(_folderProccess);
}
