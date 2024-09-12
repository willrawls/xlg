using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MetX.Fimm.Glove.Pipelines;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;

namespace MetX.Fimm.Glove
{
    public class FileSystemWatchers : List<FileSystemWatcher>
    {
        public bool EnableRaisingEvents
        {
            set
            {
                if (Count <= 0)
                {
                    return;
                }
                foreach (var fsw in this)
                {
                    fsw.EnableRaisingEvents = value;
                }
            }
        }

        public bool IsActive;

        public void Begin(XlgSettings settings, FileSystemEventHandler onchange, ErrorEventHandler onerror)
        {
            var directories = new List<string>
            {
                Shared.Dirs.CurrentSupportFolderPath
            };
            AddIfDifferent(directories, Path.GetDirectoryName(settings.Filename));
            foreach (var setting in settings.Sources)
            {
                AddIfDifferent(directories, setting.BasePath);
                AddIfDifferent(directories, Path.GetDirectoryName(setting.OutputFilename));
                AddIfDifferent(directories, Path.GetDirectoryName(setting.XslFilename));
                AddIfDifferent(directories, Path.GetDirectoryName(setting.XlgDocFilename));
            }

            foreach (var directory in directories)
            {
                if(Directory.Exists(directory))
                {
                    var fsw = new FileSystemWatcher(directory);
                    fsw.Changed += onchange;
                    fsw.Created += onchange;
                    fsw.Deleted += onchange;
                    fsw.Error += onerror;
                    fsw.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
                    fsw.EnableRaisingEvents = true;
                    Add(fsw);
                }
                else
                {
                    Debug.WriteLine($"FileSystemWatchers: Begin: Directory not found: {directory}");
                }
            }
            IsActive = true;
        }

        private static void AddIfDifferent(List<string> directories, string currDir)
        {
            currDir = currDir.AsStringFromObject().ToLower();
            if (!currDir.EndsWith(@"\"))
            {
                currDir += @"\";
            }
            if (!directories.Contains(currDir) && Directory.Exists(currDir))
            {
                directories.Add(currDir);
            }
        }

        public void End()
        {
            if (!IsActive)
            {
                return;
            }
            foreach (var watcher in this)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
            IsActive = false;
            Clear();
        }
    }
}