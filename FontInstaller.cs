using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace KioskSetup
{
    internal static class FontInstaller
    {
        private const int WM_FONTCHANGE = 0x001D;
        private const int HWND_BROADCAST = 0xffff;

        [DllImport("gdi32.dll", EntryPoint = "AddFontResourceW", SetLastError = true)]
        private static extern int AddFontResource([In] [MarshalAs(UnmanagedType.LPWStr)] string lpFileName);

        [DllImport("user32")]
        private static extern int SendMessage(IntPtr hwnd, int message, int wParam, IntPtr lParam);

        [DllImport("kernel32")]
        private static extern int WriteProfileString(string lpszSection, string lpszKeyName, string lpszString);

        internal static void InstallFont()
        {
            var files = GetSourceFonts();

            foreach (var file in files)
            {
                var fontName = GetFontName(file);
                if(fontName == null)
                    return;

                var applyFontRet = ApplyFont(fontName);
                if(applyFontRet == false)
                    continue;

                var installFontRet = InstallFont(file, fontName);
                if(installFontRet == false)
                    continue;
            }
        }

        private static string[] GetSourceFonts()
        {
            return FileUtility.GetFiles(Setting.fontsDir);
        }

        private static bool ApplyFont(string fontName)
        {
            try
            {
                //var defaultKey = Registry.CurrentUser.CreateSubKey("Console");
                //if (defaultKey == null) return;

                //defaultKey.SetValue("FaceName", FontName);
                //defaultKey.Close();

                var masterKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts");
                if (masterKey == null) return false;

                masterKey.SetValue("FaceName", fontName);
                masterKey.Close();

                return true;
            }
            catch (Exception ex)
            {
                UserInterface.ShowMessage("خطا 119: خطا در نصب فونت ها");
                Logger.Log("ApplyFont Error: " + ex.Message);
                return false;
            }
        }

        private static bool InstallFont(string file, string fontName)
        {
            if(file == null) return false;

            try
            {
                SendMessage((IntPtr) HWND_BROADCAST, WM_FONTCHANGE, 0, IntPtr.Zero);

                var windowsFontsPaths = FileUtility.GetWindowsFontsPath();
                FileUtility.CopyFile(Setting.fontsDir, windowsFontsPaths, file);

                var fontPath = Path.Combine(FileUtility.GetWindowsFontsPath(), Path.GetFileName(file));

                var result = AddFontResource(fontPath);
                var error = Marshal.GetLastWin32Error();
                if (error != 0)
                {
                    UserInterface.ShowMessage("خطا 120: خطا در نصب فونت ها");
                    Logger.Log("InstallFont Error: " + new Win32Exception(error).Message);
                    return false;
                }

                if (result == 0)
                {
                    //Font is already installed     
                }
                else
                {
                    //Font installed successfully    
                }

                WriteProfileString("fonts", fontName + " " + "(All Res)", fontPath);

                return true;
            }
            catch (Exception ex)
            {
                UserInterface.ShowMessage("خطا 120: خطا در نصب فونت ها");
                Logger.Log("InstallFont Error: " + ex.Message);
                return false;
            }
        }

        private static string GetFontName(string fontPath)
        {
            try
            {
                var privateFonts = new PrivateFontCollection();
                privateFonts.AddFontFile(fontPath);
                var font = new Font(privateFonts.Families[0], 12);
                return font.Name;
            }
            catch (Exception ex)
            {
                UserInterface.ShowMessage("خطا 124: خطا در نصب فونت ها");
                Logger.Log("GetFontName Error: " + ex.Message);
                return null;
            }
        }
    }
}

