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

namespace MetX.IO
{
	/// <summary>Helper functions for the file system</summary>
	public static class FileSystem
	{
		
		/// <summary>Deletes all files in a folder older than a certain number of minutes.</summary>
		/// <param name="Minutes">The number of minutes ago the file must be created to be deleted. (So 5 means it must be at least 5 minutes old to be deleted).</param>
		/// <param name="sPath">The folder to delete files from</param>
		public static void CleanFilesOlderThan(int Minutes, string sPath)
		{
			string[] Files = Directory.GetFiles(sPath);
			foreach (string CurrFile in Files)
                if(DateTime.Now.Subtract(File.GetCreationTime(CurrFile)).TotalMinutes > Minutes)
					File.Delete(CurrFile);
		}

        
        /// <summary>Deletes all sub folders older than a certain number of minutes.</summary>
        /// <param name="Minutes">The number of minutes ago a folder must be created to be deleted (So 5 means it must be at least 5 minutes old to be deleted)</param>
        /// <param name="sPath">The path to retrieve delete sub folders for. NOTE: The path itself will not be removed. So passing "C:\X\Y" would delete "C:\X\Y\Z" (if it's more than 5 minutes old) but not "C:\X\Y" (no matter how old it is)</param>
        public static void CleanFoldersOlderThan(int Minutes, string sPath)
        {
            string[] Dirs = Directory.GetDirectories(sPath);
            foreach (string CurrDir in Dirs)
                if (DateTime.Now.Subtract(File.GetCreationTime(CurrDir)).TotalMinutes > Minutes)
                    Directory.Delete(CurrDir, true);
        }

        
        /// <summary>Copies the contents of a folder (including subfolders) from one location to another</summary>
        /// <param name="Source">The path from which files and subfolders should be copied</param>
        /// <param name="Dest">The path to which those files and folders should be copied</param>
        /// <returns>True if the operation was successful, otherwise an exception is thrown</returns>
        public static bool DeepCopy(DirectoryInfo Source, DirectoryInfo Dest)
		{
			FileSystemInfo[] SourceContents = Source.GetFileSystemInfos();
			FileInfo CurrSourceFile;

			foreach (FileSystemInfo CurrSource in SourceContents)
			{
				if (CurrSource.Attributes ==  FileAttributes.Directory)
					DeepCopy((DirectoryInfo)CurrSource, Dest.CreateSubdirectory(CurrSource.Name));
				else
				{
					CurrSourceFile = (FileInfo)CurrSource;
					CurrSourceFile.CopyTo(Dest.FullName + @"\" + CurrSourceFile.Name);
				}
			}
			return true;
		}

        /// <summary>Iterates the contents of a folder (including subfolders) generating an xml serializable object hierarchy along the way</summary>
        /// <param name="Source">The path from which files and subfolders to iterate</param>
        /// <returns>True if the operation was successful, otherwise an exception is thrown</returns>
        public static xlgFolder DeepContents(DirectoryInfo Source)
        {
            xlgFolder ret = new xlgFolder(Source.FullName, Source.Name, Source.CreationTime, Source.LastWriteTime);
            return DeepContents(ret, Source);
        }

        /// <summary>Iterates the contents of a folder (including subfolders) generating an xml serializable object hierarchy along the way</summary>
        /// <param name="Source">The path from which files and subfolders to iterate</param>
        /// <returns>True if the operation was successful, otherwise an exception is thrown</returns>
        public static xlgFolder DeepContents(xlgFolder Target, DirectoryInfo Source)
        {
            FileSystemInfo[] SourceContents = Source.GetFileSystemInfos();
            FileInfo CurrSourceFile;

            foreach (FileSystemInfo CurrSource in SourceContents)
            {
                if ((CurrSource.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    Target.Folders.Add(
                        DeepContents(
                            new xlgFolder(CurrSource.FullName, CurrSource.Name, CurrSource.CreationTime, CurrSource.LastWriteTime), 
                            (DirectoryInfo) CurrSource));
                }
                else
                {
                    FileInfo fi = (FileInfo) CurrSource;
                    Target.Files.Add(
                        new xlgFile(fi.FullName, fi.Name, fi.Extension, fi.Length, fi.CreationTime, fi.LastWriteTime));
                }
            }
            return Target;
        }

        /// <summary>Reads the entire contents of a file and returns it as a string</summary>
        /// <param name="Filename">The path and filename to read</param>
        /// <returns>The contents of the file or a blank string if the file does not exist.</returns>
        public static string FileToString(string Filename)
		{
			string ReturnValue;

			if (File.Exists(Filename))
			{
				FileStream St = File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				StreamReader Sr = new StreamReader(St);
				ReturnValue = Sr.ReadToEnd();
				Sr.Close();
				St.Close();
			}
			else
				ReturnValue = string.Empty;

			return ReturnValue;
		}

        
        /// <summary>Writes a string to a file. If the file already exists, it will be deleted first (effectively overwriting the file)</summary>
        /// <param name="Filename">The path and filename of the file to overwrite</param>
        /// <param name="FileContents">The contents of the file to write</param>
        public static void StringToFile(string Filename, string FileContents)
		{
            File.WriteAllText(Filename, FileContents, Encoding.Unicode);
            //StreamWriter Sw = File.CreateText(Filename);
            //Sw.NewLine = "";
            //Sw.WriteLine(FileContents);
            //Sw.Close();
		}

        
        /// <summary>Given a path, it returns the parent folder (So for "C:\X\Y\Z", "C:\X\Y" would be returned.</summary>
        /// <param name="Path">The path to find the parent for</param>
        /// <returns>The path of the parent directory</returns>
        public static string ParentDir(string Path)
		{
			if( Path.Length > 0)
			{
				DirectoryInfo dir = new DirectoryInfo(Path);
				return dir.Parent.ToString();
			}
			else 
				return string.Empty;
		}

        public static string InsureFolderExists(string Path, bool StripOffFilename)
        {
            string ret = string.Empty;
            if (!Directory.Exists(Path))
            {
                string Folder = (StripOffFilename
                    ? Token.Before(Path, Token.Count(Path, @"\"), @"\")
                    : Path);
                if (!Directory.Exists(Folder))
                {
                    if (MessageBox.Show("The folder '" + Folder + "' does not exist.\n\n\tCreate it now?", "CREATE FOLDER?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Directory.CreateDirectory(Folder);
                        ret = Folder.EndsWith(@"\") ? Folder : Folder + @"\";
                    }
                }
                else
                    ret = Folder.EndsWith(@"\") ? Folder : Folder + @"\";
            }
            return ret;
        }


        /// <summary>
        /// Runs a command line, waits for it to finish, gathers it's output from strin and returns the output.
        /// </summary>
        /// <param name="Filename">The filename to execute</param>
        /// <param name="Arguments">Any (optional) arguments to pass to the executable</param>
        /// <param name="WorkingFolder">The folder that the executing environment should initially be set to</param>
        /// <param name="WaitTime">The number of seconds to wait before killing the process. If the value is less than 1, 60 seconds is assumed.</param>
        /// <returns>The strout/strerr output by the executable</returns>
        public static string GatherOutput(string Filename, string Arguments, string WorkingFolder, int WaitTime)
        {
            Process p = new Process();

            p.StartInfo.FileName = Filename;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;

            if (Arguments.Trim().Length > 0)
            {
                p.StartInfo.Arguments = Arguments;
            }
            if (WorkingFolder != null && WorkingFolder.Trim().Length > 0)
            {
                p.StartInfo.WorkingDirectory = WorkingFolder;
            }
            if (WaitTime < 1)
            {
                WaitTime = 60;
            }
            WaitTime *= 1000;

            p.Start();

            string output = p.StandardOutput.ReadToEnd();
            string sError = p.StandardError.ReadToEnd();

            if (!(p.WaitForExit(WaitTime)))
            {
                p.Kill();
            }
            p.Close();

            return (output + System.Environment.NewLine + sError).Replace("\\x000C", string.Empty).Replace(System.Environment.NewLine + System.Environment.NewLine, System.Environment.NewLine).Replace(System.Environment.NewLine + System.Environment.NewLine, System.Environment.NewLine);
        }
    }
}
