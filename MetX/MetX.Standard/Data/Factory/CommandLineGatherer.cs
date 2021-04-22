using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using MetX.Standard.IO;
using MetX.Standard.Library;

namespace MetX.Standard.Data.Factory
{
    public class CommandLineGatherer : GatherProvider
    {
        public override int GatherNow(StringBuilder sb, string[] args)
        {
            var pathToExecutable = args[0];
            if (!File.Exists(pathToExecutable)
                && !pathToExecutable.Contains("\\"))
            {
                var fullPath = Environment.GetEnvironmentVariable("PATH")?.ToUpper();
                if(fullPath.IsNotEmpty())
                {
                    var paths = fullPath.Split(';').Distinct().ToArray();
                    foreach (var path in paths)
                    {
                        var potentialLocation = Path.Combine(path, pathToExecutable);
                        if (File.Exists(potentialLocation))
                        {
                            pathToExecutable = potentialLocation;
                        }
                    }
                }
                
            }

            switch (args.Length)
            {
                case 1:
                {
                    if (pathToExecutable.Contains("\n"))
                    {
                        var tempFile = Path.GetTempPath() + Guid.NewGuid().ToString("N") + ".bat";
                        File.WriteAllText(tempFile, pathToExecutable);

                        var cmdExePath = "cmd.exe"; //Environment.ExpandEnvironmentVariables("%SystemRoot%");
                        var arguments = "/C \"" + tempFile + "\"";

                        sb.Append(FileSystem.GatherOutput(cmdExePath, arguments, null, -1));

                        try
                        {
                            File.Delete(tempFile);
                        }
                        catch (Exception e)
                        {
                            // Ignored since it's in TEMP
                            Debug.WriteLine(e);
                        }
                    }
                    else
                    {
                        sb.Append(FileSystem.GatherOutput(pathToExecutable, null, null, -1));
                    }
                    return 0;
                }

                case 2:
                {
                    sb.Append(FileSystem.GatherOutput(pathToExecutable, args[1], null, -1));
                    return 0;
                }   
                
                case 3:
                {
                    sb.Append(FileSystem.GatherOutput(pathToExecutable, args[1], args[2], -1));
                    return 0;
                }   
                
                case 4:
                {
                    sb.Append(FileSystem.GatherOutput(pathToExecutable, args[1], args[2], int.Parse(args[3])));
                    return 0;
                }
            }

            return -1;
        }
    }
}