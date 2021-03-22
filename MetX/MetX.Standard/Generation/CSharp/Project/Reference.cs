using System.Xml;
using MetX.Standard.Library;

namespace MetX.Standard.Generation.CSharp.Project
{
    public class Reference : IRefer
    {
        public Modifier Parent { get; set; }
        public string Include { get; set; }
        public string HintPath { get; set; }

        public Reference(Modifier parent, string include, string hintPath = null)
        {
            Parent = parent;
            Include = include;
            
            if (hintPath.IsEmpty())
            {
                // Maybe don't need a hint path most of the time
                // C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\5.0.0\ref\net5.0\System.Configuration.ConfigurationManager.dll
                // C:\Program Files\dotnet
                //    \packs\Microsoft.WindowsDesktop.App.Ref
                //    \5.0.0\ref\net5.0
                //    \System.Configuration.ConfigurationManager.dll
                // 
            }
            HintPath = hintPath;
                
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

                referenceElement = Parent.Document.CreateElement("Reference");
                referenceElement.SetAttribute("Include", Include);
                itemGroup.AppendChild(referenceElement);

                if (HintPath.IsNotEmpty())
                {
                    
                    var hintPathElement = Parent.Document.CreateElement("HintPath");
                    referenceElement.AppendChild(hintPathElement);
                    hintPathElement.InnerText = HintPath;
                }
            }
            else if (referenceElement.FirstChild == null)
            {
                if (HintPath.IsNotEmpty())
                {
                    var hintPathElement = Parent.Document.CreateElement("HintPath");
                    referenceElement.AppendChild(hintPathElement);
                    hintPathElement.InnerText = HintPath;
                }
            }
            else
            {
                referenceElement.FirstChild.InnerText = HintPath;
            }
            return referenceElement;
        }
    }
}