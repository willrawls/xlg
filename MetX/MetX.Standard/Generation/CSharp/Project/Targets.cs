using System.Dynamic;
using System.Xml;

namespace MetX.Standard.Generation.CSharp.Project
{
    public class Targets
    {
        public ClientCsProjGenerator Parent;

        public Target AddSourceGeneratedFiles { get; set; }
        public Target RemoveSourceGeneratedFiles { get; set; }

        public Targets(ClientCsProjGenerator parent)
        {
            Parent = parent;
            AddSourceGeneratedFiles = new Target(Parent, (XmlElement) Parent.Document.SelectSingleNode(XPaths.AddSourceGeneratedFiles));
            RemoveSourceGeneratedFiles = new Target(Parent, (XmlElement) Parent.Document.SelectSingleNode(XPaths.RemoveSourceGeneratedFiles));
        }

        public void Insert()
        {
            Remove();            
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