using System.ComponentModel;

namespace MetX.Five.QuickScripts
{
    public enum PostBuildAction
    {
        [Description("Run now from command line")]
        RunNow,

        [Description("Open (temp) project in Visual Studio")]
        OpenTempProjectVisualStudio,

        [Description("Copy full path to project folder")]
        CopyProjectFolderPath,

        [Description("Copy full path to executable")]
        CopyExePath,

        [Description("Copy project to another folder and open in Visual Studio")]
        CloneProjectAndOpen,

        [Description("Open bin folder in CMD")]
        OpenBinFolderInCommandLine,

        [Description("Open bin folder in Explorer")]
        OpenBinFolderInExplorer,

        [Description("Open project folder in CMD")]
        OpenProjectFolderInCommandLine,

        [Description("Open project folder in Explorer")]
        OpenProjectFolderOnExplorer,
        [Description("Do Nothing")] DoNothing,
    }
}