using System;
using System.IO;
using System.Windows.Forms;
using MetX.Controls;

namespace XLG.QuickScripts
{
    static class Program
    {
        // static Henry _henry;

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "QuickScripts");
            Directory.CreateDirectory(path);
            string filePath;

            if (args.Length > 0 && File.Exists(args[0]) && args[0].EndsWith(".xlgq")) filePath = args[0];
            else
            {
                filePath = Context.GetLastKnownPath();
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) filePath = Path.Combine(path, "Default.xlgq");
            }
            Application.Run(new QuickScriptEditor(filePath));
        }
        
        
        public class SomeClassInMyCode
        {
            public void SomeMethodIHave()
            {
                
                HelloWorldGenerated.HelloWorld.SayHello(); // calls Console.WriteLine("Hello World!") and then prints out syntax trees
            }
        }        
    }
}
