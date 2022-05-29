using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.ML;
using MetX.Standard.Library.Strings;
using MetX.Standard.Primary.IO;
using MetX.Standard.XDString;

namespace MetX.Five
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
        public const string TemplateManagerFolderName = @"MetX.TemplateManager\Scripts\Templates";
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
                var path = FromSettingsFile(LastScriptFilenameKey);
                if (path.IsNotEmpty()) return path;

                path = Path.Combine(Paths[ScriptsFolderName].Value, DefaultScriptFile());
                LastScriptFilePath = path;
                return path;
            }
            set => ToSettingsFile(LastScriptFilenameKey, value);
        }

        public static string DefaultScriptFile() => Path.Combine(Paths[ScriptsFolderName].Value, "Default.xlgq");

        public static string SettingsFilePath() => Path.Combine(Paths[MyDocumentsXlgFolderName].Value, "xlgUserSettings.xml");

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

            StageSettingsIfNeeded();

            StageStaticSupportIfNeeded();
            StageStaticTemplatesIfNeeded();
        }

        public static AssocArray XlgUserSettings = new();

        private static void StageSettingsIfNeeded()
        {
            var settingsFilePath = SettingsFilePath();
            if (!File.Exists(settingsFilePath))
            {
                XlgUserSettings[LastScriptFilenameKey].Value = DefaultScriptFile();
                XlgUserSettings.SaveXmlToFile(settingsFilePath, true);
            }
            else
            {
                XlgUserSettings = Xml.LoadFile<AssocArray>(settingsFilePath);
            }
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
                if (!Directory.Exists(path))
                {
                    path = FileSystem.FindAscendantDirectory(AppDomain.CurrentDomain.BaseDirectory, TemplateManagerFolderName, 5);
                }
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

        public static string FromSettingsFile(string name)
        {
            lock(SyncRoot)
            {
                var value = XlgUserSettings[name].Value;
                if (Paths[name].Value.IsEmpty() && XlgUserSettings[name].Value.IsNotEmpty())
                {
                    Paths[name].Value = XlgUserSettings[name].Value;
                }
                return Paths[name].Value;
            }

        }

        private static readonly object SyncRoot = new();

        public static bool ToSettingsFile(string name, string value)
        {
            lock(SyncRoot)
            {
                XlgUserSettings[name].Value = value;
                XlgUserSettings.SaveXmlToFile(SettingsFilePath(), true);
                return true;
            }
        }

        public static string ResolveVariables(string target)
        {
            if (target.IsEmpty())
                return "";

            var result = target;
            foreach (var item in Paths.Items) 
                result = result
                    .Replace(
                        $"%{item.Key}%", 
                        item.Value, true, CultureInfo.InvariantCulture);
            return result;
        }
    }
}