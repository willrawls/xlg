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
    public class xlgVirtualDirectory : VirtualDirectory
    {
        xlgPathProvider spp;

        /// <summary>Return true if the virtual path exists</summary>
        public bool Exists
        {
            get { return true; }
        }

        /// <summary>Basic constructor</summary>
        /// <param name="virtualDir">The virtual directory to manage</param>
        /// <param name="provider">The xlg path provider parent</param>
        public xlgVirtualDirectory(string virtualDir, xlgPathProvider provider)
            : base(virtualDir)
        {
            spp = provider;
        }

        private ArrayList children = new ArrayList();

        /// <summary>Child objects</summary>
        public override IEnumerable Children
        {
            get { return children; }
        }

        private ArrayList directories = new ArrayList();

        /// <summary>Virtual sub directories in this virtual folder</summary>
        public override IEnumerable Directories
        {
            get { return directories; }
        }

        private ArrayList files = new ArrayList();

        /// <summary>Virutal files in this virtual folder</summary>
        public override IEnumerable Files
        {
            get { return files; }
        }
    }
}
