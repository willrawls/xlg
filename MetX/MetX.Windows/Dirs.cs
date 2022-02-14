using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using MetX.Standard.IO;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using MetX.Standard.XDString;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Win32;

namespace MetX.Windows
{
    public class Dirs
    {
        public static string DefaultBasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), MyDocumentsXlgFolderName);

        public const string MyDocumentsXlgFolderName = "XLG";

        public const string ProcessorsFolderName = "Processors";
        public const string PipesFolderName = "Pipes";
        public const string ScriptsFolderName = "Scripts";
        public const string LastScriptFilenameKey = "LastScriptFilename";

        public const string ArchiveFolderName = "Archive";
        public const string OldProcessorsFolderName = "OldProcessors";
        public const string OldPipesFolderName = "OldPipes";
        public const string OldScriptsFolderName = "OldScripts";
        public const string TemplatesFolderName = "Templates";
        public const string SupportFolderName = "Support";

        public const string RegistryKeySuffix = "_Key";

        public static AssocArray Paths = new();

        static Dirs()
        {
            InitializeFolders();
            Debug.WriteLine(Details);
        }

        public static string Details
        {
            get
            {
                var details = Paths.ToString();
                return details;
            }
        }

        public static string LastScriptFilePath
        {
            get
            {
                var path = FromRegistry(LastScriptFilenameKey);
                if (path.IsNotEmpty()) return path;

                path = Path.Combine(Paths[ScriptsFolderName].Value, DefaultScriptFile);
                LastScriptFilePath = path;
                return path;
            }
            set => ToRegistry(LastScriptFilenameKey, value);
        }

        public static string DefaultScriptFile { get; } = Path.Combine(Paths[ScriptsFolderName].Value, "Default.xlgq");

        public static string ScriptArchivePath
        {
            get
            {
                var path = Paths[OldScriptsFolderName].Value;
                if (path.IsNotEmpty()) return path;

                InitializeFolders();
                return Paths[OldScriptsFolderName].Value;
            }
        }

        private static void InitializeFolders()
        {
            var basePath = DefaultBasePath;
            Paths[MyDocumentsXlgFolderName].Value = basePath;

            // Top level
            Paths[ProcessorsFolderName].Value = Path.Combine(basePath, ProcessorsFolderName);
            
            var pipes = Path.Combine(basePath, PipesFolderName);
            Paths[PipesFolderName].Value = pipes;
            
            var scripts = Path.Combine(basePath, ScriptsFolderName);
            Paths[ScriptsFolderName].Value = scripts;
            
            var archiveTopLevel = Path.Combine(basePath, ArchiveFolderName);
            Paths[ArchiveFolderName].Value = archiveTopLevel;
                
            // Archives
            Paths[OldProcessorsFolderName].Value = Path.Combine(archiveTopLevel, OldProcessorsFolderName);
            Paths[OldPipesFolderName].Value = Path.Combine(archiveTopLevel, OldPipesFolderName);
            Paths[OldScriptsFolderName].Value = Path.Combine(archiveTopLevel, OldScriptsFolderName);

            // Sub folders
            Paths[TemplatesFolderName].Value = Path.Combine(scripts, TemplatesFolderName);
            Paths[SupportFolderName].Value = Path.Combine(pipes, SupportFolderName);
            
            foreach (var item in Paths.Items)
                if(!Directory.Exists(item.Value))
                    Directory.CreateDirectory(item.Value);

            StageStaticSupportIfNeeded();
            StageStaticTemplatesIfNeeded();
        }

        private static bool StageStaticSupportIfNeeded()
        {
            var supportFolder = Paths[SupportFolderName].Value;
            var entries = Directory.GetDirectories(supportFolder);
            if (entries.IsNotEmpty()) return true;

            var staticSupportPath = StaticSupportPath;
            if (staticSupportPath.IsEmpty())
                return false;

            FileSystem.DeepCopy(staticSupportPath, supportFolder);
            return true;
        }

        private static bool StageStaticTemplatesIfNeeded()
        {
            var destinationTemplateFolder = Paths[TemplatesFolderName].Value;
            var entries = Directory.GetDirectories(destinationTemplateFolder);
            if (entries.IsNotEmpty()) return true;
            
            var path = StaticTemplatesPath;
            if (path.IsEmpty()) return false;
            
            FileSystem.DeepCopy(path, destinationTemplateFolder);
            /*
            foreach (var folder in Directory.GetDirectories(path))
                FileSystem.DeepCopy(path, Path.Combine(templatesFolder, folder));
            */
            return true;
        }

        public static string StaticTemplatesPath
        {
            get
            {
                var path = FileSystem.FindAscendantDirectory(AppDomain.CurrentDomain.BaseDirectory, TemplatesFolderName, 5);
                return Directory.Exists(path) 
                    ? path 
                    : "";
            }
        }

        public static string StaticSupportPath
        {
            get
            {
                var path = FileSystem.FindAscendantDirectory(AppDomain.CurrentDomain.BaseDirectory, SupportFolderName, 5);
                return Directory.Exists(path) 
                    ? path 
                    : "";
            }
        }

        public static string CurrentSupportFolderPath => Paths[SupportFolderName].Value;
        public static string CurrentTemplateFolderPath => Paths[TemplatesFolderName].Value;

        public static string FromRegistry(string name)
        {
            RegistryKey registryKey = null;
            try
            {
                registryKey = Application.UserAppDataRegistry;
                if (registryKey == null)
                    return "";

                var value = registryKey.GetValue(name + RegistryKeySuffix) as string;
                if(value.IsEmpty())
                    return Paths[name].Value;
                Paths[name].Value = value;
                return value;
            }
            finally
            {
                registryKey?.Close();
            }
        }

        public static bool ToRegistry(string name, string value)
        {
            RegistryKey registryKey = null;
            try
            {
                registryKey = Application.UserAppDataRegistry;
                if (registryKey == null)
                    return false;
                registryKey.SetValue(name + RegistryKeySuffix, value);
                Paths[name].Value = value;
                return true;
            }
            finally
            {
                registryKey?.Close();
            }
        }
    }
}