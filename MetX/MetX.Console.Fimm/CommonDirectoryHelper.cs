using System;
using System.IO;
using MetX.Standard.Primary.IO;
using MetX.Standard.Strings;

namespace MetX.Fimm;

public class CommonDirectoryHelper
{
    private string _settingsFilePath;

    public CommonDirectoryHelper(string settingsFilePath = "")
    {
        _settingsFilePath = settingsFilePath ?? "";
        Settings = new CommonSettingsHelper(this);
        Initialize(Settings);
    }

    public bool Initialized { get; set; }
    public AssocArray Paths { get; set; } = new();

    public string DefaultBasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        Constants.MyDocumentsXlgFolderName);

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
            var path = Settings.FromSettingsFile(Constants.LastScriptFilenameKey);
            if (path.IsNotEmpty()) return path;

            path = Path.Combine(Paths[Constants.ScriptsFolderName].Value, DefaultScriptFile());
            LastScriptFilePath = path;
            return path;
        }
        set => Settings.ToSettingsFile(Constants.LastScriptFilenameKey, value);
    }

    public string SettingsFilePath
    {
        get
        {
            if (_settingsFilePath.IsEmpty())
                _settingsFilePath =
                    Path.Combine(Paths[Constants.MyDocumentsXlgFolderName].Value, "xlgUserSettings.xml");
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
            var path = FileSystem.FindAscendantDirectory(AppDomain.CurrentDomain.BaseDirectory,
                Constants.TemplatesFolderName, 5);
            if (!Directory.Exists(path))
                path = FileSystem.FindAscendantDirectory(AppDomain.CurrentDomain.BaseDirectory,
                    Constants.TemplateManagerFolderName, 5);
            return Directory.Exists(path)
                ? path
                : "";
        }
    }

    public string StaticSupportPath
    {
        get
        {
            var path = FileSystem.FindAscendantDirectory(AppDomain.CurrentDomain.BaseDirectory,
                Constants.SupportFolderName, 5);
            return Directory.Exists(path)
                ? path
                : "";
        }
    }

    public string CurrentSupportFolderPath => Paths[Constants.SupportFolderName].Value;
    public string CurrentTemplateFolderPath => Paths[Constants.TemplatesFolderName].Value;
    public string ScriptsFolderName => Paths[Constants.ScriptsFolderName].Value;
    public string FimmFolderPath => Paths[Constants.FimmFolderName].Value;

    public CommonSettingsHelper Settings { get; private set; }

    public object SyncRoot { get; } = new();

    public string DefaultScriptFile()
    {
        var userName = Environment.UserName;
        return Path.Combine(Paths[Constants.ScriptsFolderName].Value, $"Default_{userName}.xlgq");
    }

    public void Initialize(CommonSettingsHelper settings)
    {
        Initialized = false;
        Settings = settings;
        InitializeFoldersIfNeeded();
    }

    private void InitializeFoldersIfNeeded()
    {
        if (Initialized)
            return;

        lock (SyncRoot)
        {
            var basePath = DefaultBasePath;
            Paths[Constants.MyDocumentsXlgFolderName].Value = basePath;

            if (_settingsFilePath.IsEmpty())
                _settingsFilePath =
                    Path.Combine(Paths[Constants.MyDocumentsXlgFolderName].Value, "xlgUserSettings.xml");

            // Top level
            Paths[Constants.ProcessorsFolderName].Value = Path.Combine(basePath, Constants.ProcessorsFolderName);

            var pipes = Path.Combine(basePath, Constants.PipesFolderName);
            Paths[Constants.PipesFolderName].Value = pipes;

            var scripts = Path.Combine(basePath, Constants.ScriptsFolderName);
            Paths[Constants.ScriptsFolderName].Value = scripts;

            var archiveTopLevel = Path.Combine(basePath, Constants.ArchiveFolderName);
            Paths[Constants.ArchiveFolderName].Value = archiveTopLevel;

            // Archives
            Paths[Constants.OldProcessorsFolderName].Value =
                Path.Combine(archiveTopLevel, Constants.OldProcessorsFolderName);
            Paths[Constants.OldPipesFolderName].Value = Path.Combine(archiveTopLevel, Constants.OldPipesFolderName);
            Paths[Constants.OldScriptsFolderName].Value = Path.Combine(archiveTopLevel, Constants.OldScriptsFolderName);

            // Sub folders
            Paths[Constants.TemplatesFolderName].Value = Path.Combine(scripts, Constants.TemplatesFolderName);
            Paths[Constants.SupportFolderName].Value = Path.Combine(pipes, Constants.SupportFolderName);

            // Fimm
            Paths[Constants.FimmFolderName].Value = Path.Combine(basePath, Constants.FimmFolderName);


            foreach (var item in Paths.Items)
                if (!Directory.Exists(item.Value))
                    Directory.CreateDirectory(item.Value);

            Settings.StageSettingsIfNeeded();
            StageStaticSupportIfNeeded();
            StageStaticTemplatesIfNeeded();

            Settings.ToSettingsFile("Initialized", DateTime.UtcNow.ToString("s"));
            Initialized = true;
        }
    }

    public string ResolveVariables(string target)
    {
        if (target.IsEmpty()) return "";
        if (!target.Contains("%")) return target;

        var result = Paths.Resolve(target);
        result = Shared.Settings.ForHost.Resolve(result);
        return result;
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

        return true;
    }

    public bool RestageStaticTemplates()
    {
        var destinationTemplateFolder = Paths[Constants.TemplatesFolderName].Value;
        var entries = Directory.GetDirectories(destinationTemplateFolder);
        
        var path = StaticTemplatesPath;
        if (path.IsEmpty()) return false;

        FileSystem.DeepCopy(path, destinationTemplateFolder);

        return true;
    }
}