using System.Xml;
using MetX.Standard.Strings;

namespace MetX.Standard.Primary.Generation.CSharp.Project
{
    public class ProjectReference : IRefer
    {
        public IGenerateCsProj Parent { get; set; }
        public string Include { get; set; }
        public string OutputItemType { get; set; }
        public bool? ReferenceOutputAssembly { get; set; }

        public ProjectReference(IGenerateCsProj parent, string include, string outputItemType = null, bool? referenceOutputAssembly = null)
        {
            Parent = parent;
            Include = include;
            OutputItemType = outputItemType;
            ReferenceOutputAssembly = referenceOutputAssembly;
        }

        public XmlElement GetOrCreateItemGroupElement(string xpath)
        {
            var itemGroup = Parent.Document.SelectSingleNode(xpath);
            if (itemGroup == null)
            {
                itemGroup = Parent.Document.CreateElement("ItemGroup");
                Parent.Document.DocumentElement?.AppendChild(itemGroup);
            }

            return (XmlElement) itemGroup;
        }

        public XmlElement Remove()
        {
            if (Parent?.Document?.DocumentElement == null)
                return null;

            var element = (XmlElement) Parent.Document
                .SelectSingleNode(XPaths.ReferenceByInclude(Include));

            element?.ParentNode?.RemoveChild(element);
            return element;
        }

        public XmlElement InsertOrUpdate()
        {
            if (Parent?.Document?.DocumentElement == null)
                return null;

            var referenceElement = (XmlElement) Parent.Document
                .SelectSingleNode(XPaths.ReferenceByInclude(Include));

            if (referenceElement == null)
            {
                var itemGroup = GetOrCreateItemGroupElement(XPaths.ItemGroupWithAtLeastOneReference);

                referenceElement = Parent.Document.CreateElement("ProjectReference");
                var includeAttribute = Parent.Document.CreateAttribute("Include");

                itemGroup.AppendChild(referenceElement);
                includeAttribute.Value = Include;

                if (OutputItemType.IsNotEmpty())
                {
                    referenceElement.SetAttribute("OutputItemType", OutputItemType);
                }

                if (ReferenceOutputAssembly == null) return referenceElement;
                
                var referenceOutputAssembly = ReferenceOutputAssembly.Value;
                referenceElement.SetAttribute("ReferenceOutputAssembly", referenceOutputAssembly.ToString().ToLower());
            }

            return referenceElement;
        }
    }
}