using System;
using System.Diagnostics;
using System.IO;

namespace MetX.Five;

public static class Shared
{
    private static CommonDirectoryHelper _dirs = null;
    private static CommonSettingsHelper _settings = null;
    public static CommonDirectoryHelper Dirs
    {
        get 
        {
            if (_dirs != null) return _dirs;

            _dirs = new CommonDirectoryHelper();
            Debug.WriteLine("Shared.Dirs: Created CommonDirectoryHelper");

            return _dirs;
        }
        set => _dirs = value;
    }

    public static CommonSettingsHelper Settings
    {
        get 
        {
            if(_settings == null)
            {
                _settings = new CommonSettingsHelper(Dirs);
                Debug.WriteLine("Shared.Settings: Created CommonSettingsHelper");
            }
            
            return _settings;
        }
        set => _settings = value;
    }

    public static CommonDirectoryHelper InitializeDirs(string settingsFilePath = "", bool removeAllSettings = false)
    {
        _dirs = new CommonDirectoryHelper(settingsFilePath);
        _settings = new CommonSettingsHelper(_dirs);

        if (!removeAllSettings || !File.Exists(settingsFilePath)) return _dirs;

        Debug.WriteLine($"Shared.Dirs: Initializing to settings file at {settingsFilePath}");
        _dirs.Settings.ResetSettingsFile();
        return _dirs;
    }

}