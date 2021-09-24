using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using MetX.Standard.Interfaces;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Pipelines;

namespace MetX.Standard.IO
{
    /// <summary>Helper functions for the file system</summary>
    public static class FileSystem
    {
        public static bool SafeDelete(string filePath)
        {
            if (filePath.IsEmpty())
                return true;

            try
            {
                if (!File.Exists(filePath))
                    return true;

                File.SetAttributes(filePath, FileAttributes.Normal);
                File.Delete(filePath);
                return true;
            }
            catch
            {
                try
                {
                    File.Move(filePath, Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".from.metx.safedelete"));
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Warning: When newFileContents is null, any existing file is DELETED (as intended)
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="newFileContents"></param>
        /// <returns></returns>
        public static bool TryWriteAllText(string filePath, string newFileContents = null)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch
                    {
                        // Ignored
                    }
                }

                if (newFileContents.IsNotEmpty())
                {
                    File.WriteAllText(filePath, newFileContents);
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return false;
        }
        
        //private static StringBuilder _outputResult;

        /// <summary>Deletes all files in a folder older than a certain number of minutes.</summary>
        /// <param name="minutes">
        ///     The number of minutes ago the file must be created to be deleted. (So 5 means it must be at least
        ///     5 minutes old to be deleted).
        /// </param>
        /// <param name="sPath">The folder to delete files from</param>
        public static void CleanFilesOlderThan(int minutes, string sPath)
        {
            var files = Directory.GetFiles(sPath);
            foreach (var currFile in files)
                if (DateTime.Now.Subtract(File.GetCreationTime(currFile)).TotalMinutes > minutes)
                    File.Delete(currFile);
        }

        /// <summary>Deletes all sub folders older than a certain number of minutes.</summary>
        /// <param name="minutes">
        ///     The number of minutes ago a folder must be created to be deleted (So 5 means it must be at least
        ///     5 minutes old to be deleted)
        /// </param>
        /// <param name="sPath">
        ///     The path to retrieve delete sub folders for. NOTE: The path itself will not be removed. So passing
        ///     "C:\X\Y" would delete "C:\X\Y\Z" (if it's more than 5 minutes old) but not "C:\X\Y" (no matter how old it is)
        /// </param>
        public static void CleanFoldersOlderThan(int minutes, string sPath)
        {
            var dirs = Directory.GetDirectories(sPath);
            foreach (var currDir in dirs)
                if (DateTime.Now.Subtract(File.GetCreationTime(currDir)).TotalMinutes > minutes)
                    Directory.Delete(currDir, true);
        }

        public static void CleanFolder(string path)
        {
            if (path.IsEmpty()
            || path.ToLower().Contains(@":\windows")
            || path.ToLower().Contains(@":\program files")
            || path.ToLower().EndsWith(@"\appdata")
            || path.ToLower().EndsWith(@"\local")
            || path.ToLower().EndsWith(@"\roaming")
            || path.ToLower().Contains(@"\\")
            )
                return;

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        /// <summary>Copies the contents of a folder (including subfolders) from one location to another</summary>
        /// <param name="source">The path from which files and subfolders should be copied</param>
        /// <param name="dest">The path to which those files and folders should be copied</param>
        /// <returns>True if the operation was successful, otherwise an exception is thrown</returns>
        public static bool DeepCopy(DirectoryInfo source, DirectoryInfo dest)
        {
            var sourceContents = source.GetFileSystemInfos();

            foreach (var currSource in sourceContents)
            {
                if (currSource.Attributes == FileAttributes.Directory)
                    DeepCopy((DirectoryInfo) currSource, dest.CreateSubdirectory(currSource.Name));
                else
                {
                    var currSourceFile = (FileInfo) currSource;
                    currSourceFile.CopyTo(dest.FullName + @"\" + currSourceFile.Name);
                }
            }
            return true;
        }

        /// <summary>
        ///     Iterates the contents of a folder (including subfolders) generating an xml serializable object hierarchy along
        ///     the way
        /// </summary>
        /// <param name="source">The path from which files and subfolders to iterate</param>
        /// <returns>True if the operation was successful, otherwise an exception is thrown</returns>
        public static XlgFolder DeepContents(DirectoryInfo source)
        {
            var ret = new XlgFolder(source.FullName, source.Name, source.CreationTime, source.LastWriteTime);
            return DeepContents(ret, source);
        }

        /// <summary>
        ///     Iterates the contents of a folder (including subfolders) generating an xml serializable object hierarchy along
        ///     the way
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source">The path from which files and subfolders to iterate</param>
        /// <returns>True if the operation was successful, otherwise an exception is thrown</returns>
        public static XlgFolder DeepContents(XlgFolder target, DirectoryInfo source)
        {
            var sourceContents = source.GetFileSystemInfos();

            foreach (var currSource in sourceContents)
            {
                if ((currSource.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    target.Folders.Add(
                        DeepContents(
                            new XlgFolder(currSource.FullName, currSource.Name, currSource.CreationTime,
                                currSource.LastWriteTime),
                            (DirectoryInfo) currSource));
                }
                else
                {
                    var fi = (FileInfo) currSource;
                    target.Files.Add(
                        new XlgFile(
                            fi.FullName.EndsWith(fi.Name)
                                ? fi.FullName.TokensBeforeLast(@"\")
                                : fi.FullName, 
                            fi.Name, fi.Extension, fi.Length, fi.CreationTime, fi.LastWriteTime));
                }
            }
            return target;
        }

        /// <summary>Reads the entire contents of a file and returns it as a string</summary>
        /// <param name="filename">The path and filename to read</param>
        /// <returns>The contents of the file or a blank string if the file does not exist.</returns>
        public static string FileToString(string filename)
        {
            string returnValue;

            if (File.Exists(filename))
            {
                var st = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var sr = new StreamReader(st);
                returnValue = sr.ReadToEnd();
                sr.Close();
                st.Close();
            }
            else
                returnValue = string.Empty;

            return returnValue;
        }

        /// <summary>
        ///     Writes a string to a file. If the file already exists, it will be deleted first (effectively overwriting the
        ///     file)
        /// </summary>
        /// <param name="filename">The path and filename of the file to overwrite</param>
        /// <param name="fileContents">The contents of the file to write</param>
        public static void StringToFile(string filename, string fileContents)
        {
            File.WriteAllText(filename, fileContents, Encoding.Unicode);
            //StreamWriter Sw = File.CreateText(Filename);
            //Sw.NewLine = "";
            //Sw.WriteLine(FileContents);
            //Sw.Close();
        }

        /// <summary>Given a path, it returns the parent folder (So for "C:\X\Y\Z", "C:\X\Y" would be returned.</summary>
        /// <param name="path">The path to find the parent for</param>
        /// <returns>The path of the parent directory</returns>
        public static string ParentDir(string path)
        {
            if (path.Length <= 0) return string.Empty;
            var dir = new DirectoryInfo(path);
            return dir.Parent.AsString();
        }

        public static string InsureFolderExists(IGenerationHost host, string path, bool stripOffFilename)
        {
            var ret = string.Empty;
            if (!Directory.Exists(path))
            {
                var folder = stripOffFilename
                    ? path.TokensBefore(path.TokenCount(@"\"), @"\")
                    : path;
                if (!Directory.Exists(folder))
                {
                    if (host.MessageBox.Show("The folder '" + folder + "' does not exist.\n\n\tCreate it now?",
                            "CREATE FOLDER?", MessageBoxChoices.YesNo) == MessageBoxResult.Yes)
                    {
                        Directory.CreateDirectory(folder);
                        ret = folder.EndsWith(@"\") ? folder : folder + @"\";
                    }
                }
                else
                    ret = folder.EndsWith(@"\") ? folder : folder + @"\";
            }
            return ret;
        }

        /// <summary>
        ///     Runs a command line, waits for it to finish, gathers it's output from string and returns the output.
        /// </summary>
        /// <param name="filename">The filename to execute</param>
        /// <param name="arguments">Any (optional) arguments to pass to the executable</param>
        /// <param name="workingFolder">The folder that the executing environment should initially be set to</param>
        /// <param name="waitTime">
        ///     The number of seconds to wait before killing the process. If the value is less than 1, 60
        ///     seconds is assumed.
        /// </param>
        /// <returns>Both the regular and error output by the executable</returns>
        public static string GatherOutput(string filename, string arguments, string workingFolder = null,
            int waitTime = 60, ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal)
        {
            var p = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = Path.GetDirectoryName(filename),
                    FileName = filename,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = false,
                    WindowStyle = windowStyle,
                    CreateNoWindow = windowStyle == ProcessWindowStyle.Hidden,
                }
            };

            if (workingFolder != null && workingFolder.Trim().Length > 0)
            {
                p.StartInfo.WorkingDirectory = workingFolder;
            }
            if (waitTime < 1)
            {
                waitTime = 60;
            }
            waitTime *= 1000;

            p.Start();
            var output = p.StandardOutput.ReadToEnd();
            if (!p.WaitForExit(waitTime))
            {
                p.Kill();
            }
            p.Close();

            var ret = output // + Environment.NewLine + sError)
                .Replace("\\x000C", string.Empty)
                .Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine)
                .Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
            while (ret.EndsWith(Environment.NewLine))
                ret = ret.Substring(0, ret.Length - 2);
            return ret;
        }

        /// <summary>
        ///     Runs a command line, waits for it to finish, gathers it's output from string and returns the output.
        /// </summary>
        /// <param name="filename">The filename to execute</param>
        /// <param name="arguments">Any (optional) arguments to pass to the executable</param>
        /// <param name="workingFolder">The folder that the executing environment should initially be set to</param>
        /// <param name="waitTime">
        ///     The number of seconds to wait before killing the process. If the value is less than 1, 60
        ///     seconds is assumed.
        /// </param>
        /// <returns>Both the regular and error output by the executable</returns>
        public static string GatherOutputAndErrors(string filename, string arguments, out string errorOutput,
            string workingFolder = null, int waitTime = 60, ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal)
        {
            errorOutput = "";
            var p = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = Path.GetDirectoryName(filename),
                    FileName = filename,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WindowStyle = windowStyle,
                    CreateNoWindow = windowStyle == ProcessWindowStyle.Hidden,
                }
            };

            if (workingFolder != null && workingFolder.Trim().Length > 0)
            {
                p.StartInfo.WorkingDirectory = workingFolder;
            }
            if (waitTime < 1)
            {
                waitTime = 60;
            }
            waitTime *= 1000;

            p.Start();
            var output = p.StandardOutput.ReadToEnd();
            if (!p.WaitForExit(waitTime))
            {
                p.Kill();
            }

            var ret = output // + Environment.NewLine + sError)
                .Replace("\\x000C", string.Empty)
                .Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine)
                .Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
            while (ret.EndsWith(Environment.NewLine))
                ret = ret.Substring(0, ret.Length - 2);

            using StreamReader errorStream = p.StandardError;
            // Read the standard error of net.exe and write it on to console.
            errorOutput = errorStream.ReadToEnd();
            return ret;
        }

        public static string FindExecutableAlongPath(string toFind, string[] alsoCheck = null)
        {
            var pathToExecutable = toFind;
            if (!File.Exists(pathToExecutable)) // && pathToExecutable.Contains("\\"))
            {
                var executableFilename = Path.GetFileName(pathToExecutable);
                if (alsoCheck.IsNotEmpty())
                {
                    foreach (var path in alsoCheck)
                    {
                        var potentialLocation = Path.Combine(path, executableFilename);
                        if (File.Exists(potentialLocation))
                        {
                            return potentialLocation;
                        }
                        else
                        {
                            potentialLocation = (AppDomain.CurrentDomain.BaseDirectory + potentialLocation)
                                .Replace(@"\\", @"\");
                            if (File.Exists(potentialLocation))
                            {
                                return potentialLocation;
                            }
                        }
                    }
                }

                var fullPath = Environment.GetEnvironmentVariable("PATH")?.ToUpper();
                if(fullPath.IsNotEmpty())
                {
                    var paths = fullPath.Split(';').Distinct().ToArray();
                    foreach (var path in paths)
                    {
                        var potentialLocation = Path.Combine(path, executableFilename);
                        if (File.Exists(potentialLocation))
                        {
                            return potentialLocation;
                        }
                    }
                }
                
            }

            return pathToExecutable;
        }
    }
}