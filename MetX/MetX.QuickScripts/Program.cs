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

            filePath = (args.Length > 0 && File.Exists(args[0]) && args[0].EndsWith(".xlgq"))
                ? args[0]
                : Path.Combine(path, "Default.xlgq");
            //MessageBox.Show(filePath);
            Application.Run(new QuickScriptEditor(filePath));
        }
    }
}
