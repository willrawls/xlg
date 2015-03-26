using System;
using System.IO;
using System.Windows.Forms;

namespace XLG.QuickScripts
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "QuickScripts");
            Directory.CreateDirectory(path);
            string filePath;

            if (args.Length > 0 && File.Exists(args[0]) && args[0].EndsWith(".xlgq")) filePath = args[0];
            else
            {
                filePath = QuickScriptEditor.GetLastKnownPath();
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) filePath = Path.Combine(path, "Default.xlgq");
            }
            Application.Run(new QuickScriptEditor(filePath));
        }
    }
}
