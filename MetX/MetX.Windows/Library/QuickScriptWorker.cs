using System;
using System.Diagnostics;
using System.IO;
using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Primary.IO;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;
using MetX.Windows.WinApi;

namespace MetX.Windows.Library
{
    public class QuickScriptWorker
    {
        public static Exception ViewText(IGenerationHost host, string source, bool isCSharpCode)
        {
            try
            {
                var tempFile = Path.Combine(Path.GetTempPath(),
                    $"qscript{Guid.NewGuid().ToString().Substring(1, 6)}{(isCSharpCode ? ".cs" : ".txt")}");
                File.WriteAllText(tempFile, source);
                FileSystem.FireAndForget("notepad", tempFile);

            }
            catch (Exception ex)
            {
                return ex;
            }

            return null;
        }

        public static Process ViewFile(IGenerationHost host, string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath)) return null;
                if(!File.Exists(filePath))
                {
                    host.MessageBox.Show($"File not found: {filePath}");
                    return null;
                }
                
                var process = Process.Start(PathToBestNotepad, $"\"{filePath}\"");
                ActiveWindow.Move(process);
                return process;
            }
            catch (Exception ex)
            {
                host.MessageBox.Show(ex.ToString());
            }

            return null;
        }

        private static string _pathToBestNotepad;

        public static string PathToBestNotepad
        {
            get
            {
                if (_pathToBestNotepad.IsNotEmpty())
                    return _pathToBestNotepad;

                _pathToBestNotepad = @"c:\Program Files (x86)\Notepad++\notepad++.exe";

                if (!File.Exists(_pathToBestNotepad))
                    _pathToBestNotepad = @"c:\Program Files\Notepad2\Notepad2.exe";
            
                if (!File.Exists(_pathToBestNotepad))
                    _pathToBestNotepad = "notepad";

                return _pathToBestNotepad;
            }
        }

        public static void OpenFolderInCommandLine(string folderPath, IGenerationHost host)
        {
            try
            {
                if (string.IsNullOrEmpty(folderPath)) return;
                if (!Directory.Exists(folderPath))
                {
                    host.MessageBox.Show($"Folder not found: {folderPath}");
                }

                var arguments = $"/k \"cd /d {folderPath}\"";
                var process = Process.Start("cmd.exe", arguments);
                ActiveWindow.Move(process);
            }
            catch (Exception ex)
            {
                host.MessageBox.Show(ex.ToString());
            }
        }

        public static void RunInCommandLine(string exeFilePath, string workingFolder, IGenerationHost host)
        {
            try
            {
                if (string.IsNullOrEmpty(exeFilePath)) return;
                if(!File.Exists(exeFilePath))
                {
                    host.MessageBox.Show($"File not found: {exeFilePath}");
                    return;
                }

                var arguments = $"/k \"{exeFilePath}\"";

                var process = new Process
                {
                    StartInfo =
                    {
                        WorkingDirectory = workingFolder,
                        FileName = "cmd.exe", 
                        Arguments = arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = false,
                        RedirectStandardError = false,
                        WindowStyle = ProcessWindowStyle.Normal,
                        CreateNoWindow = false,
                        
                    }
                };
                if(process.Start())
                    ActiveWindow.Move(process);
                
            }
            catch (Exception ex)
            {
                host.MessageBox.Show(ex.ToString());
            }
        }

        public static void ViewFolderInExplorer(string folderPath, IGenerationHost host)
        {
            try
            {
                if (string.IsNullOrEmpty(folderPath)) return;
                if (!Directory.Exists(folderPath))
                {
                    host.MessageBox.Show($"Folder not found: {folderPath}");
                }

                var process = Process.Start("explorer.exe", $"\"{folderPath}\"");
                ActiveWindow.Move(process);
            }
            catch (Exception ex)
            {
                host.MessageBox.Show(ex.ToString());
            }
        }
    }
}