using System;

namespace MetX.Five;

public static class Shared
{
    private static CommonDirectoryHelper _dirs = null;
    public static CommonDirectoryHelper Dirs
    {
        get => _dirs ??= new CommonDirectoryHelper();
        set => _dirs = value;
    }

    public static CommonDirectoryHelper InitializeDirs(string settingsFilePath = "", bool removeAllSettings = false)
    {
        _dirs = new CommonDirectoryHelper(settingsFilePath);
        if (removeAllSettings)
        {
            _dirs.ResetSettingsFile();
        }
        return _dirs;
    }
}