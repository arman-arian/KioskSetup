using System;
using System.IO;
using IWshRuntimeLibrary;

namespace KioskSetup
{
    /// <summary>Link</summary>
    internal class Link
    {
        /// <summary>Check to see if a shortcut exists in a given directory with a specified file name</summary>
        /// <param name="DirectoryPath">The directory in which to look</param>
        /// <param name="LinkPathName">The name of the shortcut (without the .lnk extension) or the full path to a file of the same name</param>
        /// <returns>Returns true if the link exists</returns>
        internal static bool Exists(string DirectoryPath, string LinkPathName)
        {
            // Get some file and directory information
            var SpecialDir = new DirectoryInfo(DirectoryPath);

            // First get the filename for the original file and create a new file
            // name for a link in the Startup directory

            var originalfile = new FileInfo(LinkPathName);
            var NewFileName = SpecialDir.FullName + "\\" + originalfile.Name + ".lnk";
            var linkfile = new FileInfo(NewFileName);
            return linkfile.Exists;
        }

        /// <summary>Check to see if a shell link exists to the given path in the specified special folder</summary>
        /// <param name="folder"></param>
        /// <param name="LinkPathName"></param>
        /// <returns>return true if it exists</returns>
        internal static bool Exists(Environment.SpecialFolder folder, string LinkPathName)
        {
            return Exists(Environment.GetFolderPath(folder), LinkPathName);
        }

        /// <summary>Update the specified folder by creating or deleting a Shell Link if necessary</summary>
        /// <param name="folder">A SpecialFolder in which the link will reside</param>
        /// <param name="TargetPathName">The path name of the target file for the link</param>
        /// <param name="LinkPathName">The file name for the link itself or, if a path name the directory information will be ignored.</param>
        /// <param name="install">If true, create the link, otherwise delete it</param>
        internal static void Update(Environment.SpecialFolder folder, string TargetPathName, string LinkPathName, bool install)
        {
            // Get some file and directory information
            Update(Environment.GetFolderPath(folder), TargetPathName, LinkPathName, install);
        }

        // boolean variable "install" determines whether the link should be there or not.
        // Update the folder by creating or deleting the link as required.

        /// <summary>Update the specified folder by creating or deleting a Shell Link if necessary</summary>
        /// <param name="DirectoryPath">The full path of the directory in which the link will reside</param>
        /// <param name="TargetPathName">The path name of the target file for the link</param>
        /// <param name="LinkPathName">The file name for the link itself or, if a path name the directory information will be ignored.</param>
        /// <param name="Create">If true, create the link, otherwise delete it</param>
        internal static void Update(string DirectoryPath, string TargetPathName, string LinkPathName, bool Create)
        {
            // Get some file and directory information
            var SpecialDir = new DirectoryInfo(DirectoryPath);

            // First get the filename for the original file and create a new file
            // name for a link in the Startup directory

            var OriginalFile = new FileInfo(LinkPathName);
            var NewFileName = SpecialDir.FullName + "\\" + OriginalFile.Name + ".lnk";
            var LinkFile = new FileInfo(NewFileName);

            if (Create) // If the link doesn't exist, create it
            {
                if (LinkFile.Exists) return; // We're all done if it already exists
                //Place a shortcut to the file in the special folder 
                try
                {
                    // Create a shortcut in the special folder for the file
                    // Making use of the Windows Scripting Host
                    var shell = new WshShell();
                    var link = (IWshShortcut)shell.CreateShortcut(LinkFile.FullName);
                    link.TargetPath = TargetPathName;
                    link.Save();
                }
                catch
                {
                    throw new Exception(string.Format("Shell Link Error: Unable to create link in special directory: {0}", NewFileName));
                }
            }
            else // otherwise delete it from the startup directory
            {
                if (!LinkFile.Exists) return; // It doesn't exist so we are done!

                try
                {
                    LinkFile.Delete();
                }
                catch
                {
                    throw new Exception(string.Format("Shell Link Error: Error deleting link in special directory: {0}", NewFileName));
                }
            }
        }
    }
}
