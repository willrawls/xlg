/*
using System.Configuration;
using System.IO;
using System.Xml;
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace MetX.IO  << Should be MetX.Web but MetX.Web is no longer a thing
{
    /// <summary>Allows for the retrieval of attachment information and location for a specified item.
    /// Use of this requires using the xlgAttachments website. Further development is required.</summary>
    /// <author>William M. Rawls</author>
	/// <summary>
	/// Summary description for Attachments.
	/// </summary>
	public class Attachments
	{
		/// <summary>The fixed name of the application</summary>
		public string AppName;
		/// <summary>A unique identifier relative to the AppName for the resource which contains attachments</summary>
		public string ParentId;
		/// <summary>The folder containing all attachments for the AppName(s)</summary>
		public string BaseFolder;
		/// <summary>Initially set to BaseFolder + AppName + "\" + ParentID + "\"</summary>
		public string DirectoryName;

		/// <summary>Default constructor</summary>
		/// <param name="baseFolder">The folder containing the Application's attachments</param>
		/// <param name="appName">The Application name (sub folder) containing the resource's attachments</param>
		/// <param name="parentId">The Unique string associated with the specific set of attachments</param>
		public Attachments(string baseFolder, string appName, string parentId)
		{
			BaseFolder = baseFolder;
			AppName = appName;
			ParentId = parentId;
			DirectoryName = baseFolder + appName + @"\" + parentId + @"\";
		}

		/// <summary>Retrieves an Xml containins the list of attacments for the AppName/ParentID combination. Element name is "Attachments" with each attachment being a child element named "Attachment" with "Filename" and "Link" attributes.</summary>
		public string OuterXml
		{
			get
			{
				var xmlDoc = new XmlDocument();
				XmlNode ret = xmlDoc.CreateElement("Attachments");
				ret.Attributes["AppName"].Value = AppName;
				ret.Attributes["ParentID"].Value = ParentId;

				if(Directory.Exists(DirectoryName))
				{
					var fileList = Directory.GetFiles(DirectoryName);
					foreach(var currFile in fileList)
					{
						XmlNode currNode = xmlDoc.CreateElement("Attachment");
						currNode.Attributes["Filename"].Value = currFile;
						currNode.Attributes["Link"].Value = "http://" + ConfigurationManager.AppSettings["AttachServer"] +"/Attach/Attachments/" + AppName + "/" + ParentId + "/" + currFile;
						ret.AppendChild(currNode);
					}
				}
				return ret.OuterXml;
			}
		}
	}
}
*/
