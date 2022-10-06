using System.Diagnostics;
using System.IO;

namespace MetX.Fimm;

public static class Shared
{
    private static CommonDirectoryHelper _directoryHelper = null;
    private static CommonSettingsHelper _settingsHelper = null;

    public static CommonDirectoryHelper Dirs
    {
        get 
        {
            if (_directoryHelper != null) return _directoryHelper;

            _directoryHelper = new CommonDirectoryHelper();
            var settings = new CommonSettingsHelper(_directoryHelper);

//             _directoryHelper.Initialize(settings);
             
            Debug.WriteLine("Shared.Dirs: Created CommonDirectoryHelper");

            return _directoryHelper;
        }
        set => _directoryHelper = value;
    }

    public static CommonSettingsHelper Settings
    {
        get 
        {
            if(_settingsHelper == null)
            {
                _settingsHelper = new CommonSettingsHelper(Dirs);
                Debug.WriteLine("Shared.Settings: Created CommonSettingsHelper");
            }
            
            return _settingsHelper;
        }
        set => _settingsHelper = value;
    }

    public static CommonDirectoryHelper InitializeDirs(string settingsFilePath = "", bool removeAllSettings = false)
    {
        _directoryHelper = new CommonDirectoryHelper(settingsFilePath);
        _settingsHelper = new CommonSettingsHelper(_directoryHelper);

        if (!removeAllSettings || !File.Exists(settingsFilePath)) return _directoryHelper;

        Debug.WriteLine($"Shared.Dirs: Initializing to settings file at {settingsFilePath}");
        _directoryHelper.Settings.ResetSettingsFile();
        return _directoryHelper;
    }

}