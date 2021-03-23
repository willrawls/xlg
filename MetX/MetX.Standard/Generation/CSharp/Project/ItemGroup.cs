using System.Collections.Generic;

namespace MetX.Standard.Generation.CSharp.Project
{
    public class ItemGroup
    {
        public ClientCsProjGenerator Parent { get; set; }

        // Microsoft.CodeAnalysis.Analyzers
        public PackageReference Analyzers { get; set; }

        // Microsoft.CodeAnalysis.Common
        public PackageReference Common { get; set; }

        // Microsoft.CodeAnalysis.CSharp.Workspaces
        public PackageReference CSharpWorkspaces { get; set;  }
        
        /*
  <ItemGroup>
    <Reference Include="System.Configuration.ConfigurationManager">
      <HintPath>C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\5.0.0\ref\net5.0\System.Configuration.ConfigurationManager.dll</HintPath>
    </Reference>
  </ItemGroup>        
         */
        public Reference ConfigurationManager { get; set;  }
        
        /*
  <ItemGroup>
    <ProjectReference Include="..\MetX.Aspects\MetX.Aspects.csproj" />
    <ProjectReference Include="..\MetX.Generators\MetX.Generators.csproj" OutputItemType="Analyzer" ReferenceOutputAssembly="false" />
  </ItemGroup>         
         */
        public ProjectReference AspectProjectReference { get; set;  }
        public ProjectReference MetXGeneratorsProjectReference { get; set;  }

        public ItemGroup(ClientCsProjGenerator parent)
        {
            Parent = parent;
            
            // package References
            Common = new PackageReference(parent, "Microsoft.CodeAnalysis.Common", "3.9.0");
            Analyzers = new PackageReference(parent, "Microsoft.CodeAnalysis.Analyzers", "3.3.2",
                "all", "runtime; build; native; contentfiles; analyzers; buildtransitive");
            CSharpWorkspaces = new PackageReference(parent, "Microsoft.CodeAnalysis.CSharp.Workspaces", "3.9.0", "all");

            ConfigurationManager = new Reference(Parent, "System.Configuration.ConfigurationManager");

            AspectProjectReference = new ProjectReference(Parent, $@"..\MetX.Aspects\MetX.Aspects.csproj");
            MetXGeneratorsProjectReference = new ProjectReference(Parent, $@"..\MetX.Generators\MetX.Generators.csproj", "Analyzer", false);
        }
    }
}