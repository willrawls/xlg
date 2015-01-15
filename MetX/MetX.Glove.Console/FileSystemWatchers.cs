using System.Collections.Generic;
using System.IO;
using MetX.Data;

namespace MetX.Glove
{
    internal class FileSystemWatchers : List<FileSystemWatcher>
    {
        public bool EnableRaisingEvents
        {
            set
            {
                if (Count <= 0)
                {
                    return;
                }
                foreach (FileSystemWatcher fsw in this)
                {
                    fsw.EnableRaisingEvents = value;
                }
            }
        }

        public bool IsActive = false;

        public void Begin(XlgSettings settings, FileSystemEventHandler onchange, ErrorEventHandler onerror)
        {
            List<string> directories = new List<string> {GloveMain.AppData.SupportPath};
            AddIfDifferent(directories, Path.GetDirectoryName(settings.Filename));
            foreach (XlgSource setting in settings.Sources)
            {
                AddIfDifferent(directories, setting.BasePath);
                AddIfDifferent(directories, Path.GetDirectoryName(setting.OutputFilename));
            }

            foreach (string directory in directories)
            {
                FileSystemWatcher fsw = new FileSystemWatcher(directory);
                fsw.Changed += onchange;
                fsw.Created += onchange;
                fsw.Deleted += onchange;
                fsw.Error += onerror;
                fsw.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
                fsw.EnableRaisingEvents = true;
                this.Add(fsw);
            }
            IsActive = true;
        }

        private static void AddIfDifferent(List<string> directories, string currDir)
        {
            currDir = Worker.nzString(currDir).ToLower();
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
            foreach (FileSystemWatcher watcher in this)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
            IsActive = false;
            Clear();
        }
    }
}