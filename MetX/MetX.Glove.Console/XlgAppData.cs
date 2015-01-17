using System;
using System.IO;
using System.Web.Configuration;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

using MetX;
using MetX.IO;
using MetX.Security;
using MetX.Data;
using MetX.Library;

namespace MetX.Glove
{
	[Serializable]
	public class XlgAppData
	{
		[XmlAttribute]
		public string BasePath;
		[XmlAttribute]
		public string SupportPath;
		[XmlAttribute]
		public string ParentNamespace;
		[XmlAttribute]
		public string TextEditor;
		[XmlAttribute]
		public string LastXlgsFile;

		static Microsoft.Win32.RegistryKey AppDataRegKey;

		public XlgAppData() { }
		public string ToXml() { return Xml.ToXml<XlgAppData>(this, true); }
		public static XlgAppData FromXml(string XmlDoc) { return Xml.FromXml<XlgAppData>(XmlDoc); }

		public static XlgAppData ResetPreferences()
		{
			string Drive = System.Environment.CurrentDirectory.Substring(0, 1);

			XlgAppData ret = new XlgAppData();
			ret.BasePath = Drive + @":\data\code\xlg\DAL\";
			ret.SupportPath = Drive + @":\data\code\xlg\Support\";
			ret.LastXlgsFile = ret.SupportPath + "Default.xlgs";
			ret.ParentNamespace = "xlg.dal";
			ret.TextEditor = ret.SupportPath + "Notepad2.exe";

			ret.Save();
			return ret;
		}

		public static XlgAppData Load()
		{
			AppDataRegKey = Application.UserAppDataRegistry;
			List<string> ValueNames = new List<string>(AppDataRegKey.GetValueNames());

			XlgAppData ret = null;
			try
			{
				if (ValueNames == null || ValueNames.Count == 0 || !ValueNames.Contains("Preferences"))
					ret = ResetPreferences();
				else
				{
					try { ret = XlgAppData.FromXml(AppDataRegKey.GetValue("Preferences") as string); }
					catch { ret = ResetPreferences(); }
				}
			}
			finally
			{
				if (AppDataRegKey != null)
				{
					AppDataRegKey.Close();
					AppDataRegKey = null;
				}
			}
			return ret;
		}

		public void Save()
		{
			bool OpenedKey = false;
			if (AppDataRegKey == null)
			{
				AppDataRegKey = Application.UserAppDataRegistry;
				OpenedKey = true;
			}

			AppDataRegKey.SetValue("Preferences", ToXml(), Microsoft.Win32.RegistryValueKind.String);

			if (OpenedKey && AppDataRegKey != null)
			{
				AppDataRegKey.Close();
				AppDataRegKey = null;
			}
		}
	}
}
