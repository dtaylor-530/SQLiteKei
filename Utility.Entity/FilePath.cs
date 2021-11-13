using System.Text.Json.Serialization;

namespace Utility
{

    public record FilePath(string Path)
    {
        public static implicit operator string(FilePath fileName)
        {
            return fileName.Path;
        }

        [JsonIgnore]
        //[Newtonsoft.Json.JsonIgnore]
        public string BaseName => System.IO.Path.GetFileNameWithoutExtension(Path);

        [JsonIgnore]
        //[Newtonsoft.Json.JsonIgnore]
        public string? Parent => new FileInfo(Path).DirectoryName;

        [JsonIgnore]
        //[Newtonsoft.Json.JsonIgnore]
        public string Name => new FileInfo(Path).Name;

        [JsonIgnore]
        //[Newtonsoft.Json.JsonIgnore]
        public FileInfo AsFileInfo => new(Path);

        [JsonIgnore]
        //[Newtonsoft.Json.JsonIgnore]
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

}
