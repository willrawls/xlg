using System.Linq;
using System.Xml;
using MetX.Standard.Library;

namespace MetX.Standard.Generation.CSharp.Project
{
    public class Modifier
    {
        
        public XmlNode Node => Document.SelectSingleNode(XPaths.Project);
        public PropertyGroups PropertyGroups { get; set; }
        
        public string FilePath { get; set; }
        public XmlDocument Document { get; set; }
        //public string Contents { get; set; }

        public static Modifier LoadFile(string filePath)
        {
            var document = new XmlDocument();
            document.Load(filePath);
            var modifier = new Modifier
            {
                Document = document,
                //Contents = document.InnerXml,
                FilePath = filePath,
            };
            modifier.PropertyGroups = new PropertyGroups(modifier);
            return modifier;
        }

        public Modifier SaveToFile()
        {
            if (FilePath.IsEmpty())
                return this;
            
            Document?.Save(FilePath);
            return this;
        }

        public Modifier UpdateInnerText(string xpath, string innerText)
        {
            var node = Document.SelectSingleNode(xpath) 
                       ?? MakeXPath(Document, xpath);
            node.InnerText = innerText;
            return this;
        }
        
        public XmlNode MakeXPath(string xpath)
        {
            if (xpath.IsEmpty())
                return null;
            
            return MakeXPath(Document, xpath);
        }

        public XmlNode MakeXPath(XmlNode parent, string xpath)
        {
            while (true)
            {
                // grab the next node name in the xpath; or return parent if empty
                var partsOfXPath = xpath.Trim('/').Split('/');
                var nextNodeInXPath = partsOfXPath.First();
                if (string.IsNullOrEmpty(nextNodeInXPath)) return parent;

                // get or create the node from the name
                var node = parent.SelectSingleNode(nextNodeInXPath) ?? parent.AppendChild(Document.CreateElement(nextNodeInXPath));

                // rejoin the remainder of the array as an xpath expression and recurse
                var rest = string.Join("/", partsOfXPath.Skip(1).ToArray());
                parent = node;
                xpath = rest;
            }
        }

        public bool NodeIsMissing(string xpath)
        {
            var node = Document.SelectSingleNode(XPaths.EmitCompilerGeneratedFiles);
            return node == null;
        }

        public void UpdateInnerText(string xpath, bool value)
        {
            UpdateInnerText(xpath, value ? "true" : "false");
        }
    }
}