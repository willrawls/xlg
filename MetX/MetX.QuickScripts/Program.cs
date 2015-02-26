using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using XLG.Pipeliner;

namespace MetX.QuickScripts
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
            string filePath = null;

            filePath = (args.Length > 0 && File.Exists(args[0]) && args[0].EndsWith(".xlgq"))
                ? args[0]
                : Path.Combine(path, "Default.xlgq");
            //MessageBox.Show(filePath);
            Application.Run(new QuickScriptEditor(filePath));
        }
    }
}
