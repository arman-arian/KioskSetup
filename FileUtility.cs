using System;
using System.IO;

namespace KioskSetup
{
    internal static class FileUtility
    {
        /// <summary>Get Source Files</summary>
        /// <param name="src">source address</param>
        /// <param name="searchPattern">search pattern; .bmp .exe ...</param>
        /// <returns>Files Path</returns>
        internal static string[] GetFiles(string src, string searchPattern = "*.*")
        {
            return Directory.GetFiles(src, searchPattern);
        }

        /// <summary>Copy Files</summary>
        /// <param name="from">from address</param>
        /// <param name="to">to address</param>
        /// <param name="fileList">list of files</param>
        /// <param name="overwrite">overwrite?</param>
        internal static void CopyFiles(string from, string to, string[] fileList, bool overwrite = true)
        {
            // Copy files. 
            foreach (var f in fileList)
            {
                // Remove path from the file name. 
                var fName = f.Substring(from.Length + 1);

                // Use the Path.Combine method to safely append the file name to the path. 
                // Will overwrite if the destination file already exists.
                File.Copy(Path.Combine(from, fName), Path.Combine(to, fName), overwrite);
            }
        }

        /// <summary>Copy Files</summary>
        /// <param name="from">from address</param>
        /// <param name="to">to address</param>
        /// <param name="file">list of files</param>
        /// <param name="overwrite">overwrite?</param>
        internal static void CopyFile(string from, string to, string file, bool overwrite = true)
        {
            // Remove path from the file name. 
            var fName = file.Substring(from.Length + 1);

            // Use the Path.Combine method to safely append the file name to the path. 
            // Will overwrite if the destination file already exists.
            File.Copy(Path.Combine(from, fName), Path.Combine(to, fName), overwrite);
        }

        /// <summary>Get Temp Path</summary>
        /// <returns>Temp Path</returns>
        internal static string GetTempPath()
        {
            return Path.GetTempPath();
        }

        /// <summary>Get Fonts Path</summary>
        /// <returns></returns>
        internal static string GetWindowsFontsPath()
        {
            var winPath = Environment.GetEnvironmentVariable("windir");
            return winPath != null ? Path.Combine(winPath, "Fonts") : null;
        }
    }
}
