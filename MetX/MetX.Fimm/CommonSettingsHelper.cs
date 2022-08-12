using System.IO;
using MetX.Standard.Library.ML;
using MetX.Standard.Primary.IO;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;

namespace MetX.Fimm;

public class CommonSettingsHelper
{
    private CommonDirectoryHelper _commonDirectoryHelper;
    public AssocArray ForHost = new();

    private object _syncRoot = new object();

    public CommonSettingsHelper(CommonDirectoryHelper commonDirectoryHelper)
    {
        _commonDirectoryHelper = commonDirectoryHelper;
    }

    public void StageSettingsIfNeeded()
    {
        lock (_syncRoot)
        {
            var settingsFilePath = _commonDirectoryHelper.SettingsFilePath;
            if (!File.Exists(settingsFilePath))
            {
                ForHost[Constants.LastScriptFilenameKey].Value = _commonDirectoryHelper.DefaultScriptFile();
                ForHost.SaveXmlToFile(settingsFilePath, true);
            }
            else
            {
                ForHost = AssocArray.Load(settingsFilePath);
            }
        }
    }

    public void ResetSettingsFile(bool overwriteSettingsFile = true)
    {
        lock (_syncRoot)
        {
            if (ForHost == null)
                ForHost = new();
            else 
                ForHost.Items.Clear();

            if (overwriteSettingsFile)
                FileSystem.SafelyDeleteFile(_commonDirectoryHelper.SettingsFilePath);
        }
    }

    public string this[string name]
    {
        get => FromSettingsFile(name);
        set => ToSettingsFile(name, value);
    }

    public string FromSettingsFile(string name)
    {
        lock (_syncRoot)
        {
            if (ForHost.FilePath.IsEmpty())
            {
                ForHost = Xml.LoadFile<AssocArray>(_commonDirectoryHelper.SettingsFilePath);
                ForHost.FilePath = _commonDirectoryHelper.SettingsFilePath;
            }

            var value = ForHost[name].Value;

            if (!_commonDirectoryHelper.Paths.ContainsKey(name)) return value;

            if (_commonDirectoryHelper.Paths[name].Value.IsEmpty()
                && ForHost[name].Value.IsNotEmpty())
            {
                _commonDirectoryHelper.Paths[name].Value = ForHost[name].Value;
            }
            return value;
        }
    }

    public bool ToSettingsFile(string name, string value)
    {
        lock (_syncRoot)
        {
            ForHost[name].Value = value;
            ForHost.SaveXmlToFile(_commonDirectoryHelper.SettingsFilePath, true);
            return true;
        }
    }
}