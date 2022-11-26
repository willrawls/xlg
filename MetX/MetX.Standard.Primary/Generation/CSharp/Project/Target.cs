using System.Xml;
using MetX.Standard.Strings;

namespace MetX.Standard.Primary.Generation.CSharp.Project
{
    public class Target
    {
        public IGenerateCsProj Parent { get; set; }
        public XmlElement TargetElement { get; set; }
        public string Name { get; set; }
        public string AfterTargets { get; set; }
        public string BeforeTargets { get; set; }
        public string GenerationPath { get; set; }
        
        public XmlElement ItemGroup { get; set; }

        public Target(IGenerateCsProj parent, XmlElement element)
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

        public Target(IGenerateCsProj parent, bool addSource) : this(parent, addSource ? "Generated" : "")
        {
        }
        public Target(IGenerateCsProj parent, string generationPath)
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
            //Parent.ProjectNode.RemoveChild(TargetElement);
            TargetElement?.ParentNode?.RemoveChild(TargetElement);

            ItemGroup = null;
            TargetElement = null;
        }
    }
}