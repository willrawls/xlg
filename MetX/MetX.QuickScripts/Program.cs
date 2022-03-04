using System;
using System.IO;
using System.Windows.Forms;
using MetX.Controls;
using MetX.Windows;

namespace XLG.QuickScripts;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        string filePath;
        if (args.Length > 0 && File.Exists(args[0]) && args[0].EndsWith(".xlgq"))
        {
            Dirs.LastScriptFilePath = args[0];
            filePath = args[0];
        }
        else
        {
            filePath = Dirs.LastScriptFilePath;
        }
        Application.Run(new QuickScriptEditor(filePath));
    }
}