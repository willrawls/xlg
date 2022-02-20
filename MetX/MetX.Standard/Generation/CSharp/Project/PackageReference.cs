using System.Xml;
using MetX.Standard.Library.Extensions;

namespace MetX.Standard.Generation.CSharp.Project
{
    public class PackageReference : IRefer
    {
        public IGenerateCsProj Parent { get; set; }
        public string PackageName { get; set; }
        public string Version { get; set; }
        public string PrivateAssets { get; set; }
        public string IncludeAssets { get; set; }

        public PackageReference(IGenerateCsProj parent, string packageName, string version, string privateAssets = null, string includeAssets = null)
        {
            Parent = parent;
            PackageName = packageName;
            Version = version;
            PrivateAssets = privateAssets;
            IncludeAssets = includeAssets;
        }

        public XmlElement Remove()
        {
            if (Parent?.Document?.DocumentElement == null)
                return null;
            
            var element = (XmlElement) Parent.Document
                .SelectSingleNode(XPaths
                    .PackageReferenceByNameAndVersion(PackageName, Version));
            
            if (element != null)
            {
                element.ParentNode?.RemoveChild(element);
            }

            return element;
        }

        public XmlElement InsertOrUpdate()
        {
            if (Parent?.Document?.DocumentElement == null)
                return null;
            
            var element = (XmlElement) Parent.Document
                    .SelectSingleNode(XPaths
                        .PackageReferenceByNameAndVersion(PackageName, Version));

            if (element == null)
            {
                var itemGroup = Parent.Document.SelectSingleNode(XPaths.ItemGroupWithAtLeastOnePackageReference);
                if (itemGroup == null)
                {
                    itemGroup = Parent.Document.CreateElement("ItemGroup");
                    Parent.Document.DocumentElement.AppendChild(itemGroup);
                }

                element = Parent.Document.CreateElement("PackageReference");
                var includeAttribute = Parent.Document.CreateAttribute("Include");
                var versionAttribute = Parent.Document.CreateAttribute("Version");

                itemGroup.AppendChild(element);

                includeAttribute.Value = PackageName;
                versionAttribute.Value = Version;
                element.Attributes.Append(includeAttribute);
                element.Attributes.Append(versionAttribute);

                if (PrivateAssets.IsNotEmpty())
                {
                    var privateAssetsElement = Parent.Document.CreateElement("PrivateAssets");
                    privateAssetsElement.InnerText = PrivateAssets;
                    element.AppendChild(privateAssetsElement);
                }

                if (IncludeAssets.IsNotEmpty())
                {
                    var includeAssetsElement = Parent.Document.CreateElement("IncludeAssets");
                    includeAssetsElement.InnerText = IncludeAssets;
                    element.AppendChild(includeAssetsElement);
                }
            }
            return element;
        }
    }
}