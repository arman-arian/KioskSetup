using System;
using System.Diagnostics;
using System.IO;

namespace KioskSetup
{
    internal static class Logger
    {
        internal static void Log(string sErrMsg)
        {
            try
            {
                var sPathName = Setting.logPath;
                var isFileExists = Directory.Exists(sPathName);
                if (!isFileExists)
                    Directory.CreateDirectory(sPathName);

                //sLogFormat used to create log files format :
                // dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
                var sLogFormat = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "/" +
                                 Process.GetCurrentProcess().ProcessName +
                                 " ==> ";

                var sw = new StreamWriter(sPathName + Setting.logFileName, true);
                sw.WriteLine(sLogFormat + sErrMsg);
                sw.WriteLine("");
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }
    }
}
