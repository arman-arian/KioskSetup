namespace KioskSetup
{
    internal static class Setting
    {
        internal static string srcDir = @"\\192.168.2.101\Software Group\Arman\Test";
        internal static string desDir = @"c:\current";
        internal static string welcomeMsg = "Kiosk Setup v1.0";
        internal static string successMsg = "Setup finieshed";
        internal static string uncUsername = "arman";
        internal static string uncPassword = "toseh_20120";
        internal static bool uncAuthentication = true;
        internal static string searchPattern = "*.*";
        internal static string setupBatFilename = "setup.bat";
        internal static string shortcutExeFile = "\\test.exe";
        internal static string shortcutName = "Kiosk";
        internal static string logPath = FileUtility.GetTempPath() + @"KioskSetup\";
        internal static string logFileName = "log.txt";
        internal static string fontsDir = @"C:\Users\Arman-PC\Desktop\fonts5";
    }
}

