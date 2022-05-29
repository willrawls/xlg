using System;
using System.Globalization;
using System.IO;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.ML;
using MetX.Standard.Primary.IO;
using MetX.Standard.XDString;

namespace MetX.Five;

public class CommonDirectoryHelper
{
    public AssocArray Paths = new();

    public bool Initialized;

    private string _settingsFilePath = "";

    public AssocArray XlgUserSettings = new();

    private readonly object _syncRoot = new();

    public string DefaultBasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Constants.MyDocumentsXlgFolderName);

    public CommonDirectoryHelper(string settingsFilePath = "")
    {
        _settingsFilePath = settingsFilePath ?? "";
        Initialize();
    }

    public string Details
    {
        get
        {
            var details = Paths.ToString();
            return details;
        }
    }

    public string LastScriptFilePath
    {
        get
        {
            var path = FromSettingsFile(Constants.LastScriptFilenameKey);
            if (path.IsNotEmpty()) return path;

            path = Path.Combine(Paths[Constants.ScriptsFolderName].Value, DefaultScriptFile());
            LastScriptFilePath = path;
            return path;
        }
        set => ToSettingsFile(Constants.LastScriptFilenameKey, value);
    }

    public string SettingsFilePath
    {
        get
        {
            if (_settingsFilePath.IsEmpty())
                _settingsFilePath = Path.Combine(Paths[Constants.MyDocumentsXlgFolderName].Value, "xlgUserSettings.xml");
            return _settingsFilePath;
        }
        set => _settingsFilePath = value;
    }


    public string ScriptArchivePath
    {
        get
        {
            var path = Paths[Constants.OldScriptsFolderName].Value;
            if (path.IsNotEmpty()) return path;

            return Paths[Constants.OldScriptsFolderName].Value;
        }
    }

    public string StaticTemplatesPath
    {
        get
        {
            var path = FileSystem.FindAscendantDirectory(AppDomain.CurrentDomain.BaseDirectory, Constants.TemplatesFolderName, 5);
            if (!Directory.Exists(path))
                path = FileSystem.FindAscendantDirectory(AppDomain.CurrentDomain.BaseDirectory, Constants.TemplateManagerFolderName, 5);
            return Directory.Exists(path)
                ? path
                : "";
        }
    }

    public string StaticSupportPath
    {
        get
        {
            var path = FileSystem.FindAscendantDirectory(AppDomain.CurrentDomain.BaseDirectory, Constants.SupportFolderName, 5);
            return Directory.Exists(path)
                ? path
                : "";
        }
    }

    public string CurrentSupportFolderPath => Paths[Constants.SupportFolderName].Value;
    public string CurrentTemplateFolderPath => Paths[Constants.TemplatesFolderName].Value;

    public void Initialize()
    {
        Initialized = false;
        InitializeFoldersIfNeeded();
    }

    public string DefaultScriptFile()
    {
        return Path.Combine(Paths[Constants.ScriptsFolderName].Value, "Default.xlgq");
    }

    private void InitializeFoldersIfNeeded()
    {
        if (Initialized)
            return;

        lock (_syncRoot)
        {
            var basePath = DefaultBasePath;
            Paths[Constants.MyDocumentsXlgFolderName].Value = basePath;

            if(_settingsFilePath.IsEmpty())
                _settingsFilePath = Path.Combine(Paths[Constants.MyDocumentsXlgFolderName].Value, "xlgUserSettings.xml");

            // Top level
            Paths[Constants.ProcessorsFolderName].Value = Path.Combine(basePath, Constants.ProcessorsFolderName);

            var pipes = Path.Combine(basePath, Constants.PipesFolderName);
            Paths[Constants.PipesFolderName].Value = pipes;

            var scripts = Path.Combine(basePath, Constants.ScriptsFolderName);
            Paths[Constants.ScriptsFolderName].Value = scripts;

            var archiveTopLevel = Path.Combine(basePath, Constants.ArchiveFolderName);
            Paths[Constants.ArchiveFolderName].Value = archiveTopLevel;

            // Archives
            Paths[Constants.OldProcessorsFolderName].Value = Path.Combine(archiveTopLevel, Constants.OldProcessorsFolderName);
            Paths[Constants.OldPipesFolderName].Value = Path.Combine(archiveTopLevel, Constants.OldPipesFolderName);
            Paths[Constants.OldScriptsFolderName].Value = Path.Combine(archiveTopLevel, Constants.OldScriptsFolderName);

            // Sub folders
            Paths[Constants.TemplatesFolderName].Value = Path.Combine(scripts, Constants.TemplatesFolderName);
            Paths[Constants.SupportFolderName].Value = Path.Combine(pipes, Constants.SupportFolderName);

            foreach (var item in Paths.Items)
                if (!Directory.Exists(item.Value))
                    Directory.CreateDirectory(item.Value);

            StageSettingsIfNeeded();
            StageStaticSupportIfNeeded();
            StageStaticTemplatesIfNeeded();

            ToSettingsFile("Initialized", DateTime.UtcNow.ToString("s"));
            Initialized = true;
        }
    }

    public void StageSettingsIfNeeded()
    {
        lock (_syncRoot)
        {
            var settingsFilePath = SettingsFilePath;
            if (!File.Exists(settingsFilePath))
            {
                XlgUserSettings[Constants.LastScriptFilenameKey].Value = DefaultScriptFile();
                XlgUserSettings.SaveXmlToFile(settingsFilePath, true);
            }
            else
            {
                XlgUserSettings = AssocArray.Load(settingsFilePath);
            }
        }
    }

    public bool StageStaticSupportIfNeeded()
    {
        var supportFolder = Paths[Constants.SupportFolderName].Value;
        var entries = Directory.GetDirectories(supportFolder);
        if (entries.IsNotEmpty()) return true;

        var staticSupportPath = StaticSupportPath;
        if (staticSupportPath.IsEmpty())
            return false;

        FileSystem.DeepCopy(staticSupportPath, supportFolder);
        return true;
    }

    public bool StageStaticTemplatesIfNeeded()
    {
        var destinationTemplateFolder = Paths[Constants.TemplatesFolderName].Value;
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

    public void ResetSettingsFile(bool overwriteSettingsFile = true)
    {
        lock (_syncRoot)
        {
            if (XlgUserSettings == null)
                XlgUserSettings = new();
            else 
                XlgUserSettings.Items.Clear();

            if (overwriteSettingsFile)
                FileSystem.SafelyDeleteFile(SettingsFilePath);
        }
    }

    public string FromSettingsFile(string name)
    {
        lock (_syncRoot)
        {
            if (XlgUserSettings.FilePath.IsEmpty())
            {
                XlgUserSettings = Xml.LoadFile<AssocArray>(SettingsFilePath);
                XlgUserSettings.FilePath = SettingsFilePath;
            }

            var value = XlgUserSettings[name].Value;

            if (!Paths.ContainsKey(name)) return value;

            if (Paths[name].Value.IsEmpty()
                && XlgUserSettings[name].Value.IsNotEmpty())
            {
                Paths[name].Value = XlgUserSettings[name].Value;
            }
            return value;
        }
    }

    public bool ToSettingsFile(string name, string value)
    {
        lock (_syncRoot)
        {
            XlgUserSettings[name].Value = value;
            XlgUserSettings.SaveXmlToFile(SettingsFilePath, true);
            return true;
        }
    }

    public string ResolveVariables(string target)
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