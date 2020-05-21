using System.Collections.Generic;
using System.IO;
using MetX.Library;
using MetX.Pipelines;

namespace XLG.Pipeliner
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
            var directories = new List<string> { GloveMain.AppData.SupportPath };
            AddIfDifferent(directories, Path.GetDirectoryName(settings.Filename));
            foreach (var setting in settings.Sources)
            {
                AddIfDifferent(directories, setting.BasePath);
                AddIfDifferent(directories, Path.GetDirectoryName(setting.OutputFilename));
            }

            foreach (var directory in directories)
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
            IsActive = true;
        }

        private static void AddIfDifferent(List<string> directories, string currDir)
        {
            currDir = currDir.AsString().ToLower();
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