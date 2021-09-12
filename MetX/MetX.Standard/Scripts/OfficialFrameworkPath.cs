using System;
using System.IO;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;

namespace MetX.Standard.Scripts
{
    public static class OfficialFrameworkPath
    {
        public static string Root = @"C:\Program Files (x86)\dotnet\shared";

        private static string _latestVersion;
        public static string LatestVersion
        {
            get { return _latestVersion ?? DetermineLatestVersion(); }
            set
            {
                if (value.IsNotEmpty()) return;

                if (_latestVersion.IsEmpty())
                    _latestVersion = DetermineLatestVersion();
            }
        }

        public static string SelectedVersion { get; set; } = LatestVersion;

        public static string DetermineLatestVersion()
        {
            if (_latestVersion.IsNotEmpty())
                return _latestVersion;
            var versions = Directory.GetDirectories(Path.Combine(Root, "Microsoft.NETCore.App"));
            for (int i = 0; i < versions.Length; i++)
            {
                versions[i] = versions[i]
                    .LastPathToken()
                    .Replace(".", ".A");
            }
                    
            Array.Sort(versions);
            Array.Reverse(versions);
            _latestVersion = versions[0].Replace(".A", ".");
            return _latestVersion;

        }

        public static string NETCore => @$"{Root}\Microsoft.NETCore.App\{SelectedVersion}";
        public static string WindowsDesktop => @$"{Root}\Microsoft.WindowsDesktop.App\{SelectedVersion}";
        public static string AspNetCore => @$"{Root}\Microsoft.AspNetCore.App\{SelectedVersion}";
            

        public static string GetFrameworkAssemblyPath(string assemblyName)
        {
            if (!assemblyName.ToLower().EndsWith(".dll") && !assemblyName.ToLower().EndsWith(".exe"))
                assemblyName += ".dll";
            var location = Path.Combine(NETCore, assemblyName);
            if (File.Exists(location))
                return location;

            location = Path.Combine(WindowsDesktop, assemblyName);
            if (File.Exists(location))
                return location;

            location = Path.Combine(AspNetCore, assemblyName);
            if (File.Exists(location))
                return location;

            return null;
        }
    }
}
