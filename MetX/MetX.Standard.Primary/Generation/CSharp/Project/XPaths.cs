namespace MetX.Standard.Primary.Generation.CSharp.Project
{
    public static class XPaths
    {
        public const string Project = "/Project";
        public const string PropertyGroup = Project + "/PropertyGroup";
        public const string Target = Project + "/Target";
        public const string AddSourceGeneratedFiles = Target + "[@Name='AddSourceGeneratedFiles']";
        public const string RemoveSourceGeneratedFiles = Target + "[@Name='RemoveSourceGeneratedFiles']";
        public const string EmitCompilerGeneratedFiles = PropertyGroup + "/EmitCompilerGeneratedFiles";
        public const string CompilerGeneratedFilesOutputPath = PropertyGroup + "/CompilerGeneratedFilesOutputPath";
        public const string LangVersion = PropertyGroup + "/LangVersion";
        public const string ItemGroup = Project + "/ItemGroup";
        public const string PackageReference = ItemGroup + "/PackageReference";
        public const string ProjectReference = ItemGroup + "/ProjectReference";
        public const string Reference = ItemGroup + "/Reference";
        public const string ItemGroupWithAtLeastOnePackageReference = ItemGroup + "[PackageReference]";
        public const string ItemGroupWithAtLeastOneReference = ItemGroup + "[Reference]";

        public static string TargetByName(string name) => Target + $"[@Name='{name}']";
        public static string PackageReferenceByNameAndVersion(string name, string version) => ItemGroup + $"[@Name='{name}' and @Version='{version}']";
        
        public static string ReferenceByInclude(string include) => Reference + $"[@Include='{include}']";
        
        public static string ProjectReferenceByInclude(string include) => ProjectReference + $"[@Include='{include}']";
    }
}