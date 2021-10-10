namespace Utility.Database;

public record ConnectionPath(string Path)
{
    public string Directory => System.IO.Path.GetDirectoryName(Path);
    public FileInfo AsFileInfo => new FileInfo(Path);
    public string Size => GetSize(AsFileInfo.Length);


    private static string GetSize(long value)
    {
        string[] SizeSuffixes = { "Bytes", "KB", "MB", "GB", "TB" };
        int suffix = 0;
        decimal decimalValue = value;

        while (Math.Round(decimalValue / 1024) >= 1)
        {
            decimalValue /= 1024;
            suffix++;
        }

        return string.Format("{0} {1}", Math.Round(decimalValue, 2), SizeSuffixes[suffix]);
    }
}
