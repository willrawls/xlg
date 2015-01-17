using System;
using System.IO;
using System.Web;
using MetX;
using MetX.Library;

namespace MetX.IO
{
    /// <summary>Implements a XmlResolver which tracks which files are loaded so PageCache dependencies can easily be implemented. Additionally xlg type Theme support is added.</summary>
    public class xlgThemeResolver : xlgUrnResolver
    {
        /// <summary>The relative path to the theme directory</summary>
        public string ThemePath;
        /// <summary>The string within the URL that triggers a conversion</summary>
        public string pathTrigger = "theme/default/";
        /// <summary>The base path for all themes</summary>
        public string basePath = "theme/";
        /// <summary>The xml class from the xlgHandler</summary>
        public Xsl Transformer;

        private string m_ThemeName = "default";

        /// <summary>Initializes the Theme Resolver</summary>
        /// <param name="Transformer">The xml class from your xlgHandler</param>
        /// <param name="ThemeName">The name of the theme (blue, red, YourClientName, etc)</param>
        /// <param name="pathTrigger">The string within the URL that triggers a conversion. The part of the string so converted.</param>
        /// <param name="basePath">The relative base path for all themes</param>
        /// <param name="ThemePath">The relative path to the specific theme directory</param>
        public xlgThemeResolver(Xsl Transformer, string ThemeName, string pathTrigger, string basePath, string ThemePath) : base()
        {
            this.Transformer = Transformer;
            this.pathTrigger = pathTrigger;
            this.basePath = basePath;
            this.ThemeName = ThemeName;
            this.ThemePath = ThemePath;
        }

        /// <summary>Initializes the Theme Resolver</summary>
        /// <param name="Transformer">The xml class from your xlgHandler</param>
        /// <param name="ThemeName">The name of the theme (blue, red, YourClientName, etc)</param>
        /// <param name="pathTrigger">The string within the URL that triggers a conversion. The part of the string so converted.</param>
        /// <param name="basePath">The relative base path for all themes</param>
        public xlgThemeResolver(Xsl Transformer, string ThemeName, string pathTrigger, string basePath)
            : base()
        {
            this.Transformer = Transformer;
            this.pathTrigger = pathTrigger;
            this.basePath = basePath;
            this.ThemeName = ThemeName;
        }

        /// <summary>Initializes the Theme Resolver. This is the one you should use most often.</summary>
        /// <param name="Transformer">The xml class from your xlgHandler</param>
        /// <param name="ThemeName">The name of the theme (blue, red, YourClientName, etc)</param>
        public xlgThemeResolver(Xsl Transformer, string ThemeName)
            : base()
        {
            this.Transformer = Transformer;
            this.ThemeName = ThemeName;
        }

        /// <summary>Initializes the Theme Resolver</summary>
        /// <param name="Transformer">The xml class from your xlgHandler</param>
        public xlgThemeResolver(Xsl Transformer)
            : base()
        {
            this.Transformer = Transformer;
            this.ThemeName = "default";
        }

        /// <summary>The name of the theme to resolve to (when available)</summary>
        public string ThemeName
        {
            get
            {
                return m_ThemeName;
            }
            set
            {
                m_ThemeName = value;
                ThemePath = basePath + value;
                if (!ThemePath.EndsWith("/"))
                    ThemePath += "/";
                Transformer.PageCacheSubKey = value;
                Transformer.XlgUrn.SetThemePath("~/" + ThemePath, "~/" + pathTrigger);
            }
        }

        /// <summary>Resolves and retrieves the requested entity via URI</summary>
        /// <param name="absoluteUri">The URI to resolve</param>
        /// <param name="role">N/A</param>
        /// <param name="ofObjectToReturn">Type of object to return</param>
        /// <returns>The resolved entity</returns>
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if (absoluteUri.AbsoluteUri.Contains(pathTrigger) && ThemePath.Length > 0)
            {
                string url = absoluteUri.AbsoluteUri.Replace(pathTrigger, ThemePath);
                string filename = url.Replace("file:///", string.Empty).Replace("/", @"\");
                if (!File.Exists(filename))
                    url = absoluteUri.AbsoluteUri;
                return base.GetEntity(new Uri(url), role, ofObjectToReturn);
            }
            return base.GetEntity(absoluteUri, role, ofObjectToReturn);
        }
    }
}