using System.Collections.Generic;
using System.Linq;
using System.Xml;
using MetX.Standard.Library;

namespace MetX.Standard.Generation.CSharp.Project
{
    public class PropertyGroups
    {
        public ClientCsProjGenerator Parent;
        public XmlNodeList Nodes => Parent.Document.SelectNodes(XPaths.PropertyGroup);

        public PropertyGroups(ClientCsProjGenerator parent)
        {
            Parent = parent;
        }

        public bool PropertyGroupForGeneratorsMissing => Parent.IsElementMissing(XPaths.EmitCompilerGeneratedFiles);
        public XmlElement PropertyGroupForGenerators
        {
            get
            {
                var node = Parent.GetNodeFromCacheOrDocument(XPaths.EmitCompilerGeneratedFiles, false);
                if (node != null) 
                    return (XmlElement) node.ParentNode;
                
                node = Parent.MakeXPath(XPaths.EmitCompilerGeneratedFiles);
                node.InnerText = "true";
                return (XmlElement) node.ParentNode;
            }
        }
        
        public bool EmitCompilerGeneratedFilesMissing => Parent.IsElementMissing(XPaths.EmitCompilerGeneratedFiles);
        public bool EmitCompilerGeneratedFiles
        {
            get => Parent.InnerTextAt(XPaths.EmitCompilerGeneratedFiles).AsString().ToLower() == "true";
            set => Parent.SetElementInnerText(XPaths.EmitCompilerGeneratedFiles, value);
        }
        
        public bool CompilerGeneratedFilesOutputPathMissing => Parent.IsElementMissing(XPaths.CompilerGeneratedFilesOutputPath);
        public string CompilerGeneratedFilesOutputPath
        {
            get => Parent.InnerTextAt(XPaths.CompilerGeneratedFilesOutputPath).BlankToNull();
            set => Parent.SetElementInnerText(XPaths.CompilerGeneratedFilesOutputPath, value.BlankToNull());
        }
        
        public bool LangVersionMissing => Parent.IsElementMissing(XPaths.LangVersion);
        public string LangVersion
        {
            get => Parent.InnerTextAt(XPaths.LangVersion).BlankToNull();
            set => Parent.SetElementInnerText(XPaths.LangVersion, value.BlankToNull());
        }
    }
}