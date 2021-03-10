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
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Medium)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.High)]
    public class xlgPathProvider : VirtualPathProvider
    {
        /// <summary>Internal initializer</summary>
        protected override void Initialize()
        {
            // Set the datafile path relative to the application's path.
            //dataFile = HostingEnvironment.ApplicationPhysicalPath + "App_Data\\XMLData.xml";
        }

        /// <summary>Determines if a virtual file exists</summary>
        /// <param name="virtualPath">The virtual path and filename to check</param>
        /// <returns>true if it exists</returns>
        public override bool FileExists(string virtualPath)
        {
            if (IsPathVirtual(virtualPath))
            {
                return true;
            }
            else
                return Previous.FileExists(virtualPath);
        }

        /// <summary>Returns a virtual file</summary>
        /// <param name="virtualPath">The virtual path and filename of the file to retreive</param>
        /// <returns>The virtual file</returns>
        public override VirtualFile GetFile(string virtualPath)
        {
            
            if (IsPathVirtual(virtualPath))
                return new xlgVirtualFile(virtualPath, this);
            else
                return Previous.GetFile(virtualPath);
        }

        /// <summary>Returns true if this is a virtual path we manage</summary>
        /// <param name="virtualPath">The virtual path to check</param>
        /// <returns>True if this is one of our virtual paths</returns>
        public bool IsPathVirtual(string virtualPath)
        {
            return virtualPath.IndexOf("/__xlg/") > -1;
        }

        /// <summary>Returns a virtual directory</summary>
        /// <param name="virtualDir">The path to the virtual directory</param>
        /// <returns>A virtual directory</returns>
        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            if (IsPathVirtual(virtualDir))
                return new xlgVirtualDirectory(virtualDir, this);
            else
                return Previous.GetDirectory(virtualDir);
        }

        /// <summary>Returns true if this is a path we manage and if the virtual directory exists</summary>
        /// <param name="virtualDir">The virtual directory path to test</param>
        /// <returns>True if the virtual path is one of ours and the virtual directory exists</returns>
        public override bool DirectoryExists(string virtualDir)
        {
            return IsPathVirtual(virtualDir);
        }
    }
}
