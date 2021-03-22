using System.Collections.Generic;

namespace MetX.Standard.Generation.CSharp.Project
{
    public class ItemGroup
    {
        public Modifier Parent { get; }
        public List<PackageReference> PackageReferences;
        
        // Microsoft.CodeAnalysis.Common
        public PackageReference Common;
        
        // Microsoft.CodeAnalysis.Analyzers
        public PackageReference Analyzers;
        
        // Microsoft.CodeAnalysis.CSharp.Workspaces
        public PackageReference CSharpWorkspaces;
        
        public ItemGroup(Modifier parent)
        {
            Parent = parent;
            Common = new PackageReference(parent, "Microsoft.CodeAnalysis.Common", "3.9.0");
            Analyzers = new PackageReference(parent, "Microsoft.CodeAnalysis.Analyzers", "3.3.2",
                "all", "runtime; build; native; contentfiles; analyzers; buildtransitive");
            CSharpWorkspaces = new PackageReference(parent, "Microsoft.CodeAnalysis.CSharp.Workspaces", "3.9.0", "all");

            PackageReferences = new List<PackageReference>
            {
                Common,
                Analyzers,
                CSharpWorkspaces,
            };
        }
    }
}