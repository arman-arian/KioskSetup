using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace KioskSetup
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {

            FontInstaller.InstallFont();

            //const string resource = "KioskSetup.Interop.IWshRuntimeLibrary.dll";
            //EmbeddedAssembly.Load(resource, "Interop.IWshRuntimeLibrary.dll");

            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new SetupView());
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return EmbeddedAssembly.Get(args.Name);
        }
    }
}
