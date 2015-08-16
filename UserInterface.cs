using System.Windows.Forms;

namespace KioskSetup
{
    internal static class UserInterface
    {
        /// <summary>Show Message</summary>
        /// <param name="err"></param>
        internal static void ShowMessage(string err)
        {
            MessageBox.Show(err);
        }
    }
}