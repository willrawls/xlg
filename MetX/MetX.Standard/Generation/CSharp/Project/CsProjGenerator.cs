using System.IO;
using System.Linq;
using System.Xml;
using MetX.Standard.Library;

namespace MetX.Standard.Generation.CSharp.Project
{
    public abstract class CsProjGenerator : IGenerateCsProj
    {
        protected CsProjGenerator()
        {
        }

        protected CsProjGenerator(CsProjGeneratorOptions options, XmlDocument document = null)
        {
            options.AssertValid();

            FilePath = Path.Combine(options.OutputPath, options.Filename);
            Document = document;
        }

        protected CsProjGenerator(string filePath)
        {
            var document = new XmlDocument();
            document.Load(filePath);
            Document = document;
            FilePath = filePath;
            PropertyGroups = new PropertyGroups(this);
            Targets = new Targets(this);
            ItemGroup = new ItemGroup(this);
        }

        public abstract IGenerateCsProj WithDefaultTargetTemplate();

        public IGenerateCsProj Setup()
        {
            if (Options.GenerationSet.IsEmpty())
                Options.GenerationSet = "Default";

            if (Options.OutputPath.IsEmpty())
                Options.OutputPath = @".\";

            if (!Directory.Exists(Options.OutputPath))
                Directory.CreateDirectory(Options.OutputPath);

            if (Options.TargetTemplate.IsEmpty()) Options.TargetTemplate = DefaultTargetTemplate;

            Document = new XmlDocument();

            Options = Options;
            return this;
        }

        public abstract IGenerateCsProj Generate();

        public CsProjGeneratorOptions Options { get; set; }

        public string DefaultTargetTemplate { get; set; }
        public XmlNode ProjectNode => GetOrCreateElement(XPaths.Project, false);
        public PropertyGroups PropertyGroups { get; set; }
        public Targets Targets { get; set; }
        public string FilePath { get; set; }
        public XmlDocument Document { get; set; }
        public ItemGroup ItemGroup { get; set; }

        public XmlNode GetOrCreateElement(string xpath, bool addIfMissing)
        {
            if (xpath.IsEmpty())
                return null;

            var node = Document.SelectSingleNode(xpath);
            if (node == null && addIfMissing)
                node = MakeXPath(xpath);
            return node;
        }

        public IGenerateCsProj WithOptions(CsProjGeneratorOptions options)
        {
            Options = options;
            return this;
        }

        public IGenerateCsProj WithTarget(string target)
        {
            Options.TargetTemplate = target;
            return this;
        }

        public IGenerateCsProj Save()
        {
            if (FilePath.IsEmpty())
                return this;

            Document?.Save(FilePath);
            return this;
        }

        public void SetElementInnerText(string xpath, bool value)
        {
            SetElementInnerText(xpath, value ? "true" : "false");
        }

        public void SetElementInnerText(string xpath, string innerText)
        {
            var node = (XmlElement) GetOrCreateElement(xpath, true)
                       ?? MakeXPath(Document, xpath);
            node.InnerText = innerText ?? "";
        }

        public string InnerTextAt(string xpath, bool blankMeansNull = true)
        {
            var node = GetOrCreateElement(xpath, false);
            if (node == null)
                return blankMeansNull ? null : "";
            return node.InnerText;
        }

        public XmlNode MakeXPath(string xpath)
        {
            if (xpath.IsEmpty())
                return null;

            return MakeXPath(Document, xpath);
        }

        public XmlNode MakeXPath(XmlNode parent, string xpath)
        {
            var originalXPath = xpath;
            while (true)
            {
                // grab the next node name in the xpath; or return parent if empty
                var partsOfXPath = xpath.Trim('/').Split('/');
                var nextNodeInXPath = partsOfXPath.First();
                if (string.IsNullOrEmpty(nextNodeInXPath)) return parent;

                // get or create the node from the name
                var node = parent.SelectSingleNode(nextNodeInXPath);
                if (node == null) node = parent.AppendChild(Document.CreateElement(nextNodeInXPath));

                // rejoin the remainder of the array as an xpath expression and recurse
                var rest = string.Join("/", partsOfXPath.Skip(1).ToArray());
                parent = node;
                xpath = rest;
            }
        }

        public bool IsElementMissing(string xpath)
        {
            var node = Document.SelectSingleNode(xpath);
            return node == null;
        }
    }
}