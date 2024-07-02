using System.Text;
using Microsoft.VisualBasic.FileIO;

Console.OutputEncoding = Encoding.UTF8;

Console.Write("Enter text which you want to delete: ");
var textToRemove = Console.ReadLine();
Console.Write($"Enter path to root directory with the files with text {textToRemove} in the title: ");
var rootDir = Console.ReadLine();

static void RemoveTextFromTitle(string? rootDir, string? textToRemove)
{
    if (string.IsNullOrEmpty(rootDir) || string.IsNullOrEmpty(textToRemove))
        return;

    // Recursively dig into subdirs and rename files & dirs
    string[] subdirs = Directory.GetDirectories(rootDir);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write($"Root: ");
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine(rootDir);

    if (subdirs.Length != 0)
    {
        foreach (string subdir in subdirs)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\t{subdir}");
            Console.ForegroundColor = ConsoleColor.Gray;

            RemoveTextFromTitle(subdir, textToRemove);
            var dirInfo = new DirectoryInfo(subdir);
            if (dirInfo.Exists && dirInfo.Name.Contains(textToRemove))
            {
                var newName = dirInfo.Name.Replace(textToRemove, "").Trim();
                FileSystem.RenameDirectory(dirInfo.FullName, newName);

                Console.Write($"\t{dirInfo.FullName}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" is renamed to ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"{newName}");
            }
        }
    }

    // Removing text from files
    string[] filePaths = Directory.GetFiles(rootDir);
    foreach (string filePath in filePaths)
    {
        var fileInfo = new FileInfo(filePath);
        if (fileInfo.Exists && fileInfo.Name.Contains(textToRemove))
        {
            var newName = fileInfo.Name.Replace(textToRemove, "").Trim();
            FileSystem.RenameFile(fileInfo.FullName, newName);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\t\t{fileInfo.Name} --> {newName}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }

}

RemoveTextFromTitle(rootDir, textToRemove);
Console.ReadKey();