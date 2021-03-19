namespace MetX.Standard.Generation.CSharp.Project
{
    public static class XPaths
    {
        public const string Project = "/Project";
        public const string PropertyGroup = Project + "/PropertyGroup";
        public const string Target = Project + "/Target";
        public const string AddSourceGeneratedFiles = Project + Target + "[@Name='AddSourceGeneratedFiles']";
        public const string RemoveSourceGeneratedFiles = Project + Target + "[@Name='RemoveSourceGeneratedFiles']";
        public const string EmitCompilerGeneratedFiles = PropertyGroup + "/EmitCompilerGeneratedFiles";
        public const string CompilerGeneratedFilesOutputPath = PropertyGroup + "/CompilerGeneratedFilesOutputPath";
        public const string LangVersion = PropertyGroup + "/LangVersion";
    }
}