using System.IO;

namespace Manchu.Extensions
{
    public static class FileInfoExtensions
    {
        public static string GetExtension(this FileInfo @this)
        {
            return Path.GetExtension(@this.FullName);
        }

        public static string GetFileNameWithoutExtension(this FileInfo @this)
        {
            return Path.GetFileNameWithoutExtension(@this.FullName);
        }
    }
}