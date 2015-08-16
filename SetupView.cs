using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace KioskSetup
{
    internal partial class SetupView : Form
    {
        private readonly BackgroundWorker _worker;

        internal SetupView()
        {
            InitializeComponent();

            _worker = new BackgroundWorker {WorkerReportsProgress = true};
            _worker.DoWork += _worker_DoWork;
            _worker.ProgressChanged += _worker_ProgressChanged;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
        }

        private void SetupView_Load(object sender, EventArgs e)
        {
            _worker.RunWorkerAsync();
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            _worker.ReportProgress(0);

            e.Result = "Started";

            if (Setting.uncAuthentication)
            {
                var ctsRet = ConnectToSharing(Setting.srcDir, Setting.uncUsername, Setting.uncPassword);
                if (ctsRet == false)
                    return;
            }

            var fileList = GetFiles(Setting.srcDir, Setting.searchPattern);
            if (fileList == null)
                return;

            _worker.ReportProgress(20);

            var cfRet = CopyFiles(Setting.srcDir, Setting.desDir, fileList);
            if (cfRet == false)
                return;

            _worker.ReportProgress(80);

            var cstdRet = CreateShortcutToDesktop(Setting.desDir + Setting.shortcutExeFile, Setting.shortcutName);
            if (cstdRet == false)
                return;

            _worker.ReportProgress(90);

            var reRet = RunExe(Setting.desDir, Setting.setupBatFilename);
            if (reRet == false)
                return;

            _worker.ReportProgress(100);

            e.Result = "Finished";
        }

        private void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pBar.Value = e.ProgressPercentage;
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        private static bool ConnectToSharing(string src, string username, string password)
        {
            try
            {
                var ret = PinvokeWindowsNetworking.connectToRemote(src, username, password);
                if (ret == null) return true;

                Logger.Log("ConnectToSharing Error: " + ret);

                if (ret == "Error: Unknown, 55")
                {
                    UserInterface.ShowMessage("خطا 99: مسیر فایل های مبدا موجود نیست");
                    return false;
                }

                if (ret == "Error: Unknown, 1219")
                {
                    UserInterface.ShowMessage(
                        "خطا 100: امکان اتصال به شبکه وجود ندارد. برنامه را در حالت مدیر سیستم اجرا کنید");
                    return false;
                }

                if (ret == "Error: Bad Net Name")
                {
                    UserInterface.ShowMessage("خطا 101: مسیر فایل های مبدا معتبر نیست");
                    return false;
                }

                UserInterface.ShowMessage("خطا 102: نام کاربری یا کلمه عبور صحیح نیست");
                return false;
            }
            catch (Exception ex)
            {
                UserInterface.ShowMessage("خطا 103: مسیر فایل های مبدا معتبر نیست");
                Logger.Log("ConnectToSharing Error: " + ex.Message);
                return false;
            }
        }

        private static string[] GetFiles(string src, string searchPattern)
        {
            string[] fileList;

            try
            {
                fileList = FileUtility.GetFiles(src, searchPattern);
            }
            catch (DirectoryNotFoundException ex)
            {
                UserInterface.ShowMessage("خطای 104: مسیر فایل های مبدا موجود نیست");
                Logger.Log("GetFiles Error: " + ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                UserInterface.ShowMessage("خطای 105: خطا در دریافت فایل ها");
                Logger.Log("GetFiles Error: " + ex.Message);
                return null;
            }

            if (fileList.Length == 0)
            {
                UserInterface.ShowMessage("خطای 106: فایلی برای نصب یافت نشد");
                Logger.Log("GetFiles Error: No Files");
                return null;
            }

            return fileList;
        }

        private static bool CopyFiles(string from, string to, string[] fileList, bool overwrite = true)
        {
            try
            {
                FileUtility.CopyFiles(from, to, fileList, overwrite);
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                UserInterface.ShowMessage("خطا 107: خطای امنیتی");
                Logger.Log("CopyFiles Error: " + ex.Message);
                return false; 
            }
            catch (Exception ex)
            {
                UserInterface.ShowMessage("خطا 108: خطا در کپی کردن فایل ها");
                Logger.Log("CopyFiles Error: " + ex.Message);
                return false;
            }
        }

        private static bool CreateShortcutToDesktop(string exeLocation, string linkName)
        {
            try
            {
                Link.Update(Environment.SpecialFolder.DesktopDirectory, exeLocation, linkName, true);
                return true;
            }
            catch (Exception ex)
            {
                UserInterface.ShowMessage("خطا 109: خطا در ایجاد میانبر");
                Logger.Log("CreateShortcutToDesktop Error: " + ex.Message);
                return false;
            }
        }

        private static bool RunExe(string filePath, string fileName)
        {
            try
            {
                Executer.RunExe(filePath, fileName);
                return true;
            }
            catch (Exception ex)
            {
                UserInterface.ShowMessage("خطا 110: خطا در اجرای فایل نصب");
                Logger.Log("RunExe Error: " + ex.Message);
                return false;
            }
        }
    }
}
