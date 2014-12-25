/// <summary>Part of the XLG MetX Library</summary>
/// <author>William M. Rawls</author>
using System;
using System.Configuration;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Web;

namespace MetX.Data
{
	/// <summary>Contains static functions for working with the standard XLG Link, Comment, and Note tables (Depricated)</summary>
	public class DatabaseUpdates
	{
		/// <summary>C#CD: </summary>
		/// <param name="NotificationID">C#CD: </param>
		/// <param name="Linker">C#CD: </param>
		/// <param name="Description">C#CD: </param>
		/// <param name="LinkID">C#CD: </param>
		/// <param name="LinkName">C#CD: </param>
		public static void SaveLink(string NotificationID, string Linker, string Description, string LinkID, string LinkName)
		{
			string Conditions;
			string SQL = "";

			NotificationID = NotificationID + "";
			Linker = Linker + "";
			Description = Description + "";
			LinkID = LinkID + "";
			LinkName = LinkName + "";
			if ((NotificationID).Length ==  0 ||  (Linker).Length ==  0 ||  (Description).Length ==  0 ||  (LinkName).Length ==  0)
				// Exit Sub
				return;
			if ((LinkID).Length ==  0 ||  LinkID ==  "(NULL)")
				Conditions = "LinkID IS NULL";
			else
				Conditions = "LinkID='" + LinkID + "'";
			Description = Worker.NormalizeText(Description);
			if (Conditions ==  "LinkID IS NULL")
				SQL = "INSERT INTO [Link] (NotificationID, LinkName, Linker, Description) VALUES ('" + NotificationID.Replace("'", "''") + "', '" + LinkName.Replace("'", "''") + "', '" + Linker.Replace("'", "''") + "', '" + Description.Replace("'", "''") + "')";
			else
				SQL = "UPDATE [Link] SET LinkName = '" + LinkName.Replace("'", "''") + "', Linker = '" + Linker.Replace("'", "''") + "', Description = '" + Description.Replace("'", "''") + "'";
			sql.Execute(SQL);
		}

		/// <summary>C#CD: </summary>
		/// <param name="NotificationID">C#CD: </param>
		/// <param name="Noter">C#CD: </param>
		/// <param name="Description">C#CD: </param>
		/// <param name="NoteID">C#CD: </param>
		public static void SaveNote(string NotificationID, string Noter, string Description, string NoteID)
		{
			NotificationID = NotificationID + "";
			Noter = Noter + "";
			Description = Description + "";
			NoteID = NoteID + "";
			if ((NotificationID).Length ==  0 ||  (Noter).Length ==  0 ||  (Description).Length ==  0)
				// Exit Sub
				return;
			Description = Worker.NormalizeText(Description);
			if ((NoteID).Length ==  0 ||  NoteID ==  "(NULL)")
				sql.Execute("INSERT INTO Note (NotificationID, Noter, Description) VALUES('" + NotificationID + "','" + Noter.Replace("'", "''") + "','" + Description.Replace("'", "''") + "')");
			else
				sql.Execute("UPDATE Note SET NotificationID='" + NotificationID + "', Noter='" + Noter.Replace("'", "''") + "', Description='" + Description.Replace("'", "''") + "' WHERE NoteID='" + NoteID + "'");
		}


		/// <summary>C#CD: </summary>
		/// <param name="RelativeName">C#CD: </param>
		/// <param name="CommentName">C#CD: </param>
		/// <param name="Contents">C#CD: </param>
		public static void UpdateComment(string RelativeName, string CommentName, string Contents)
		{
			sql.Execute("UPDATE Comment SET Contents = '" + Contents.Replace("'", "''") + "' WHERE RelativeName='" + RelativeName.Replace("'", "''") + "' AND CommentName='" + CommentName.Replace("'", "''") + "'");
		}


		/// <summary>C#CD: </summary>
		/// <param name="RelativeName">C#CD: </param>
		/// <param name="Request">C#CD: </param>
		public static void UpdateComments(string RelativeName, HttpRequest Request)
		{
			System.Data.DataRow rst = sql.ToDataRow("SELECT * FROM Comment WHERE RelativeName='" + RelativeName + "'");
			sql.Execute("UPDATE Comment SET Contents = '" + rst["CommentName"].ToString().Replace("'", "''") + "' WHERE RelativeName='" + RelativeName + "'");
		}

		/// <summary>C#CD: </summary>
		/// <param name="NotificationID">C#CD: </param>
		/// <param name="sTable">C#CD: </param>
		/// <param name="sValues">C#CD: </param>
		/// <param name="sConditions">C#CD: </param>
		public static void UpdateLinks(string NotificationID, string sTable, string sValues, string sConditions)
		{
			string[] vsValues;
			string InsertSQL;

			if ((sValues).Length >  0)
			{
				sValues = sValues.Replace(";", ",");
				sValues = sValues.Replace(" ", ",");
				if (sValues.Substring(0, 1) ==  ",")
				{
					sValues = sValues.Substring(1);
					if (sValues.Substring(sValues.Length - 1, 1) ==  ",")
						sValues = sValues.Substring(0, sValues.Length - 1);
					vsValues = sValues.Split(',');
					foreach (string CurrValue in vsValues)
					{
						string CurrValueTemp = CurrValue.Trim();
						if (CurrValueTemp.Length >  0)
						{
                            sql.Execute("INSERT INTO " + sTable + " VALUES('" + NotificationID + "','" + CurrValue + "')");
						}
					}
				}
			}
		}
	}
}
