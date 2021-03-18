namespace MetX.Standard.Generation.CSharp.Project
{
    public static class XPaths
    {
        public const string Project = "/Project";
        public const string PropertyGroup = Project + "/PropertyGroup";
        public const string EmitCompilerGeneratedFiles = PropertyGroup + "/EmitCompilerGeneratedFiles";
        public const string CompilerGeneratedFilesOutputPath = PropertyGroup + "/CompilerGeneratedFilesOutputPath";
    }
}