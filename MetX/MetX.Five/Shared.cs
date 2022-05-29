using System;
using System.Diagnostics;
using System.IO;

namespace MetX.Five;

public static class Shared
{
    private static CommonDirectoryHelper _dirs = null;
    public static CommonDirectoryHelper Dirs
    {
        get 
        {
            if(_dirs == null)
            {
                _dirs = new CommonDirectoryHelper();
                Debug.WriteLine("Shared.Dirs: Created CommonDirectoryHelper");
            }
            
            return _dirs;
        }
        set => _dirs = value;
    }

    public static CommonDirectoryHelper InitializeDirs(string settingsFilePath = "", bool removeAllSettings = false)
    {
        _dirs = new CommonDirectoryHelper(settingsFilePath);
        if (!removeAllSettings || !File.Exists(settingsFilePath)) return _dirs;

        Debug.WriteLine($"Shared.Dirs: Initializing to settings file at {settingsFilePath}");
        _dirs.ResetSettingsFile();
        return _dirs;
    }
}