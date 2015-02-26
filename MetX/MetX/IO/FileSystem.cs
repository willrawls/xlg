using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Windows.Forms;

using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Data.SqlClient;
using System.Web.Caching;
using System.Reflection;

using MetX.IO;
using MetX.Data;
using MetX.Library;

namespace MetX.IO
{
	/// <summary>Helper functions for the file system</summary>
	public static class FileSystem
	{
		
		/// <summary>Deletes all files in a folder older than a certain number of minutes.</summary>
		/// <param name="minutes">The number of minutes ago the file must be created to be deleted. (So 5 means it must be at least 5 minutes old to be deleted).</param>
		/// <param name="sPath">The folder to delete files from</param>
		public static void CleanFilesOlderThan(int minutes, string sPath)
		{
			string[] files = Directory.GetFiles(sPath);
			foreach (string currFile in files)
                if(DateTime.Now.Subtract(File.GetCreationTime(currFile)).TotalMinutes > minutes)
					File.Delete(currFile);
		}

        
        /// <summary>Deletes all sub folders older than a certain number of minutes.</summary>
        /// <param name="minutes">The number of minutes ago a folder must be created to be deleted (So 5 means it must be at least 5 minutes old to be deleted)</param>
        /// <param name="sPath">The path to retrieve delete sub folders for. NOTE: The path itself will not be removed. So passing "C:\X\Y" would delete "C:\X\Y\Z" (if it's more than 5 minutes old) but not "C:\X\Y" (no matter how old it is)</param>
        public static void CleanFoldersOlderThan(int minutes, string sPath)
        {
            string[] dirs = Directory.GetDirectories(sPath);
            foreach (string currDir in dirs)
                if (DateTime.Now.Subtract(File.GetCreationTime(currDir)).TotalMinutes > minutes)
                    Directory.Delete(currDir, true);
        }

        
        /// <summary>Copies the contents of a folder (including subfolders) from one location to another</summary>
        /// <param name="source">The path from which files and subfolders should be copied</param>
        /// <param name="dest">The path to which those files and folders should be copied</param>
        /// <returns>True if the operation was successful, otherwise an exception is thrown</returns>
        public static bool DeepCopy(DirectoryInfo source, DirectoryInfo dest)
		{
			FileSystemInfo[] sourceContents = source.GetFileSystemInfos();
			FileInfo currSourceFile;

			foreach (FileSystemInfo currSource in sourceContents)
			{
				if (currSource.Attributes ==  FileAttributes.Directory)
					DeepCopy((DirectoryInfo)currSource, dest.CreateSubdirectory(currSource.Name));
				else
				{
					currSourceFile = (FileInfo)currSource;
					currSourceFile.CopyTo(dest.FullName + @"\" + currSourceFile.Name);
				}
			}
			return true;
		}

        /// <summary>Iterates the contents of a folder (including subfolders) generating an xml serializable object hierarchy along the way</summary>
        /// <param name="source">The path from which files and subfolders to iterate</param>
        /// <returns>True if the operation was successful, otherwise an exception is thrown</returns>
        public static xlgFolder DeepContents(DirectoryInfo source)
        {
            xlgFolder ret = new xlgFolder(source.FullName, source.Name, source.CreationTime, source.LastWriteTime);
            return DeepContents(ret, source);
        }

        /// <summary>Iterates the contents of a folder (including subfolders) generating an xml serializable object hierarchy along the way</summary>
        /// <param name="source">The path from which files and subfolders to iterate</param>
        /// <returns>True if the operation was successful, otherwise an exception is thrown</returns>
        public static xlgFolder DeepContents(xlgFolder target, DirectoryInfo source)
        {
            FileSystemInfo[] sourceContents = source.GetFileSystemInfos();
            FileInfo currSourceFile;

            foreach (FileSystemInfo currSource in sourceContents)
            {
                if ((currSource.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    target.Folders.Add(
                        DeepContents(
                            new xlgFolder(currSource.FullName, currSource.Name, currSource.CreationTime, currSource.LastWriteTime), 
                            (DirectoryInfo) currSource));
                }
                else
                {
                    FileInfo fi = (FileInfo) currSource;
                    target.Files.Add(
                        new xlgFile(fi.FullName, fi.Name, fi.Extension, fi.Length, fi.CreationTime, fi.LastWriteTime));
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
				FileStream st = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				StreamReader sr = new StreamReader(st);
				returnValue = sr.ReadToEnd();
				sr.Close();
				st.Close();
			}
			else
				returnValue = string.Empty;

			return returnValue;
		}

        
        /// <summary>Writes a string to a file. If the file already exists, it will be deleted first (effectively overwriting the file)</summary>
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
            DirectoryInfo dir = new DirectoryInfo(path);
            return dir.Parent.AsString();
        }

	    public static string InsureFolderExists(string path, bool stripOffFilename)
        {
            string ret = string.Empty;
            if (!Directory.Exists(path))
            {
                string folder = (stripOffFilename
                    ? path.TokensBefore(path.TokenCount(@"\"), @"\")
                    : path);
                if (!Directory.Exists(folder))
                {
                    if (MessageBox.Show("The folder '" + folder + "' does not exist.\n\n\tCreate it now?", "CREATE FOLDER?", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
        /// Runs a command line, waits for it to finish, gathers it's output from strin and returns the output.
        /// </summary>
        /// <param name="filename">The filename to execute</param>
        /// <param name="arguments">Any (optional) arguments to pass to the executable</param>
        /// <param name="workingFolder">The folder that the executing environment should initially be set to</param>
        /// <param name="waitTime">The number of seconds to wait before killing the process. If the value is less than 1, 60 seconds is assumed.</param>
        /// <returns>The strout/strerr output by the executable</returns>
        public static string GatherOutput(string filename, string arguments, string workingFolder, int waitTime)
        {
            Process p = new Process();

            p.StartInfo.FileName = filename;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;

            if (arguments.Trim().Length > 0)
            {
                p.StartInfo.Arguments = arguments;
            }
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

            string output = p.StandardOutput.ReadToEnd();
            string sError = p.StandardError.ReadToEnd();

            if (!(p.WaitForExit(waitTime)))
            {
                p.Kill();
            }
            p.Close();

            return (output + Environment.NewLine + sError).Replace("\\x000C", string.Empty).Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine).Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
        }
    }
}
