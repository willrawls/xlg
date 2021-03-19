using System.Dynamic;
using System.Xml;
using MetX.Standard.Library;

namespace MetX.Standard.Generation.CSharp.Project
{
    public class Target
    {
        public Modifier Parent { get; set; }
        public XmlElement TargetElement { get; set; }
        public string Name { get; set; }
        public string AfterTargets { get; set; }
        public string BeforeTargets { get; set; }
        public string GenerationPath { get; set; }
        
        public XmlElement ItemGroup { get; set; }

        public Target(Modifier parent, XmlElement element)
        {
            Parent = parent;
            TargetElement = element;
            if (element == null)
            {
                return;
            }
            
            Name = TargetElement.GetAttribute("Name");
            
            if (TargetElement.HasAttribute("AfterTargets"))
            {
                AfterTargets = TargetElement.GetAttribute("AfterTargets");
                ItemGroup = (XmlElement) TargetElement.SelectSingleNode("ItemGroup");
            }
            
            if (TargetElement.HasAttribute("BeforeTargets"))
            {
                BeforeTargets = TargetElement.GetAttribute("BeforeTargets");
            }
        }

        public Target(Modifier parent, bool addSource) : this(parent, addSource ? "Generated" : "")
        {
        }
        public Target(Modifier parent, string generationPath)
        {
            Parent = parent;
            TargetElement = Parent.Document.CreateElement("Target");
            Parent.Document.DocumentElement.AppendChild(TargetElement);

            Name = generationPath.IsEmpty() ? "RemoveSourceGeneratedFiles" : "AddSourceGeneratedFiles" ;
            TargetElement.SetAttribute("Name", Name);
            
            if(generationPath.IsEmpty())
            {
                BeforeTargets = "CoreCompile";
                TargetElement.SetAttribute("BeforeTargets", "CoreCompile");
            }
            else
            {
                AfterTargets = "CoreCompile";
                TargetElement.SetAttribute("AfterTargets", "CoreCompile");
                ItemGroup = Parent.Document.CreateElement("ItemGroup");
                TargetElement.AppendChild(ItemGroup);

                GenerationPath = generationPath;
                var compile = Parent.Document.CreateElement("Compile");
                compile.SetAttribute("Include", @$"{generationPath}\**");
                ItemGroup.AppendChild(compile);
            }
            
        }

        public void Remove()
        {
            if(TargetElement != null)
            {
                Parent.ProjectNode.RemoveChild(TargetElement);
                TargetElement?.ParentNode?.RemoveChild(TargetElement);
            }

            ItemGroup = null;
            TargetElement = null;
        }
    }
    public class Targets
    {
        public Modifier Parent;

        public Target AddSourceGeneratedFiles { get; set; }
        public Target RemoveSourceGeneratedFiles { get; set; }

        public Targets(Modifier parent)
        {
            Parent = parent;
            AddSourceGeneratedFiles = new Target(Parent, (XmlElement) Parent.Document.SelectSingleNode(XPaths.AddSourceGeneratedFiles));
            RemoveSourceGeneratedFiles = new Target(Parent, (XmlElement) Parent.Document.SelectSingleNode(XPaths.RemoveSourceGeneratedFiles));
        }

        public void Setup()
        {
            AddSourceGeneratedFiles = new Target(Parent, true);
            RemoveSourceGeneratedFiles = new Target(Parent, false);
        }

        public void Remove()
        {
            AddSourceGeneratedFiles.Remove();
            RemoveSourceGeneratedFiles.Remove();
            
            AddSourceGeneratedFiles = null;
            RemoveSourceGeneratedFiles = null;
        }
    }
}