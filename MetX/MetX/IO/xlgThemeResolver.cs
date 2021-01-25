using System;
using System.IO;
using MetX.Library;

namespace MetX.IO
{
    /// <summary>Implements a XmlResolver which tracks which files are loaded so PageCache dependencies can easily be implemented. Additionally xlg type Theme support is added.</summary>
    public class XlgThemeResolver : XlgUrnResolver
    {
        /// <summary>The relative path to the theme directory</summary>
        public string ThemePath;
        /// <summary>The string within the URL that triggers a conversion</summary>
        public string PathTrigger = "theme/default/";
        /// <summary>The base path for all themes</summary>
        public string BasePath = "theme/";
        /// <summary>The xml class from the xlgHandler</summary>
        public Xsl Transformer;

        private string _mThemeName = "default";

        /// <summary>Initializes the Theme Resolver</summary>
        /// <param name="transformer">The xml class from your xlgHandler</param>
        /// <param name="themeName">The name of the theme (blue, red, YourClientName, etc)</param>
        /// <param name="pathTrigger">The string within the URL that triggers a conversion. The part of the string so converted.</param>
        /// <param name="basePath">The relative base path for all themes</param>
        /// <param name="themePath">The relative path to the specific theme directory</param>
        public XlgThemeResolver(Xsl transformer, string themeName, string pathTrigger, string basePath, string themePath)
        {
            Transformer = transformer;
            PathTrigger = pathTrigger;
            BasePath = basePath;
            ThemeName = themeName;
            ThemePath = themePath;
        }

        /// <summary>Initializes the Theme Resolver</summary>
        /// <param name="transformer">The xml class from your xlgHandler</param>
        /// <param name="themeName">The name of the theme (blue, red, YourClientName, etc)</param>
        /// <param name="pathTrigger">The string within the URL that triggers a conversion. The part of the string so converted.</param>
        /// <param name="basePath">The relative base path for all themes</param>
        public XlgThemeResolver(Xsl transformer, string themeName, string pathTrigger, string basePath)
        {
            Transformer = transformer;
            PathTrigger = pathTrigger;
            BasePath = basePath;
            ThemeName = themeName;
        }

        /// <summary>Initializes the Theme Resolver. This is the one you should use most often.</summary>
        /// <param name="transformer">The xml class from your xlgHandler</param>
        /// <param name="themeName">The name of the theme (blue, red, YourClientName, etc)</param>
        public XlgThemeResolver(Xsl transformer, string themeName)
        {
            Transformer = transformer;
            ThemeName = themeName;
        }

        /// <summary>Initializes the Theme Resolver</summary>
        /// <param name="transformer">The xml class from your xlgHandler</param>
        public XlgThemeResolver(Xsl transformer)
        {
            Transformer = transformer;
            ThemeName = "default";
        }

        /// <summary>The name of the theme to resolve to (when available)</summary>
        public string ThemeName
        {
            get => _mThemeName;
            set
            {
                _mThemeName = value;
                ThemePath = BasePath + value;
                if (!ThemePath.EndsWith("/"))
                    ThemePath += "/";
                Transformer.PageCacheSubKey = value;
                Transformer.XlgUrn.SetThemePath("~/" + ThemePath, "~/" + PathTrigger);
            }
        }

        /// <summary>Resolves and retrieves the requested entity via URI</summary>
        /// <param name="absoluteUri">The URI to resolve</param>
        /// <param name="role">N/A</param>
        /// <param name="ofObjectToReturn">Type of object to return</param>
        /// <returns>The resolved entity</returns>
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if (absoluteUri.AbsoluteUri.Contains(PathTrigger) && ThemePath.Length > 0)
            {
                var url = absoluteUri.AbsoluteUri.Replace(PathTrigger, ThemePath);
                var filename = url.Replace("file:///", string.Empty).Replace("/", @"\");
                if (!File.Exists(filename))
                    url = absoluteUri.AbsoluteUri;
                return base.GetEntity(new Uri(url), role, ofObjectToReturn);
            }
            return base.GetEntity(absoluteUri, role, ofObjectToReturn);
        }
    }
}