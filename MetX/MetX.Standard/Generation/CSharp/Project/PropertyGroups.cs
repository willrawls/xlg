using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace MetX.Standard.Generation.CSharp.Project
{
    public class PropertyGroups
    {
        public Modifier Parent;
        public XmlNodeList Nodes => Parent.Document.SelectNodes(XPaths.PropertyGroup);

        public PropertyGroups(Modifier parent)
        {
            Parent = parent;
        }

        public bool PropertyGroupForGeneratorsMissing => Parent.Document.SelectSingleNode(XPaths.EmitCompilerGeneratedFiles) == null;
        public XmlElement PropertyGroupForGenerators
        {
            get
            {
                var node = Parent.Document.SelectSingleNode(XPaths.EmitCompilerGeneratedFiles) 
                           ?? Parent.MakeXPath(XPaths.EmitCompilerGeneratedFiles);
                return (XmlElement) node;
            }
        }
        
        public bool EmitCompilerGeneratedFilesMissing => Parent.NodeIsMissing(XPaths.EmitCompilerGeneratedFiles);
        public bool EmitCompilerGeneratedFiles
        {
            get
            {
                var node = Parent.Document.SelectSingleNode(XPaths.EmitCompilerGeneratedFiles);
                if (node == null)
                    return false;
                return node.InnerText == "true";
            }
            set => Parent.UpdateInnerText(XPaths.EmitCompilerGeneratedFiles, value);
        }
        
        public bool CompilerGeneratedFilesOutputPathMissing => Parent.NodeIsMissing(XPaths.CompilerGeneratedFilesOutputPath);
        public string CompilerGeneratedFilesOutputPath
        {
            get
            {
                var node = Parent.Document.SelectSingleNode(XPaths.CompilerGeneratedFilesOutputPath);
                if (node == null)
                    return "";
                return node.InnerText;
            }
            set => Parent.UpdateInnerText(XPaths.CompilerGeneratedFilesOutputPath, value);
        }
        
    }
}