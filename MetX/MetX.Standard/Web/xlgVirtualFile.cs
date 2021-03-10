using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Security.Permissions;

namespace MetX.Web
{
    /// <summary>An in memory virutal path provider. Also provides virtual access to built in Resources with real file overriding and caching</summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class xlgVirtualFile : VirtualFile
    {
        private string content;
        private xlgPathProvider spp;

        /// <summary>Returns true if the virtual file exists</summary>
        public bool Exists
        {
            get { return (content != null); }
        }

        /// <summary>Basic constructor</summary>
        /// <param name="virtualPath">The path to manage</param>
        /// <param name="provider">The parent xlgPathProvider</param>
        public xlgVirtualFile(string virtualPath, xlgPathProvider provider)
            : base(virtualPath)
        {
            this.spp = provider;

            string _namespace = VirtualPath.Substring(VirtualPath.IndexOf("/__xlg") + 7);
            string _classname = Path.GetFileName(_namespace);
            _namespace = _namespace.Substring(0, _namespace.IndexOf("/"));
            string _filename = "~/" + _classname + ".cs";
            if (_classname.StartsWith("class") || _classname.StartsWith("default"))
                _classname = "_" + _classname;
            content = "<%@ Page language=\"c#\" Inherits=\"" + _namespace + "." + _classname.Replace(".aspx", string.Empty) + "\" CodeFile=\"" + _filename + "\" %>";
        }

        /// <summary>Opens a virtual file</summary>
        /// <returns>A stream of the file's contents</returns>
        public override Stream Open()
        {
            // Put the page content on the stream.
            Stream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);

            writer.Write(content);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }
}
