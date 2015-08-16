using System.Diagnostics;

namespace KioskSetup
{
    internal static class Executer
    {
        internal static void RunExe(string filePath, string fileName)
        {
            var targetDir = string.Format(filePath);
            var proc = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = targetDir,
                    FileName = fileName,
                    Arguments = string.Format("10"),
                    CreateNoWindow = false
                }
            };
            //this is argument
            proc.Start();
            proc.WaitForExit();
        }
    }
}
