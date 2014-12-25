using System;
using System.Configuration;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Web;

using MetX.IO;

namespace MetX.IO
{
	
	/// <summary>Static functions providing simplified access to several of the XLG Meta Data pattern features.</summary>
	public static class Element
	{
		/// <summary>Uses Notification table returns all details of child records relative to a NotificationID (all Notification records with ParentNotifcationID matching NotificationID)
		/// <para>Assumes Default.SqlClient connection exists</para>
		/// </summary>
		/// <param name="sNotificationID">The NotificationID to retrieve</param>
		/// <returns>An xml string of "Notification" Elements (one per record) wrapped in a "ChildNotifications" element.</returns>
		/// 
		/// <example><c>Append(xmlChildNotifications(Security.UserID))</c></example>
		public static string xmlChildNotifications(string sNotificationID)
		{
			return sql.ToXml("ChildNotifications", string.Empty, "SELECT * FROM Notification WHERE ParentNotificationID=\'" + sNotificationID + "\'");
		}
		
        /// <summary>Uses Note table returns all details of each note for NotificationID</summary>
        /// <param name="sNotificationID">The NotificationID to retrieve notes for</param>
        /// <returns>xml string of "Note" elements (one per record) wrapped in a "Notes" element.
        /// NOTE: Returns an empty string if
        /// </returns>
        /// 
        /// <example><c>Append(xmlRelatedNotes(Security.UserID))</c></example>
        public static string xmlRelatedNotes(string sNotificationID)
		{
            if (sNotificationID != null && sNotificationID.Length > 16)
                return sql.ToXml("Notes", string.Empty, "SELECT * FROM Note Where NotificationID='" + sNotificationID + "'");
            return string.Empty;
		}

        /// <summary>Uses Link table returns all details of each link for NotificationID</summary>
        /// <param name="sNotificationID">The NotificationID to retrieve notes for</param>
        /// <returns>xml string of "Link" elements (one per record) wrapped in a "Links" element</returns>
        /// 
        /// <example><c>Append(xmlRelatedLinks(Security.UserID))</c></example>
        public static string xmlRelatedLinks(string sNotificationID)
		{
			if(sNotificationID != null && sNotificationID.Length > 16)
				return sql.ToXml("Links", string.Empty, "SELECT * FROM Link Where NotificationID='" + sNotificationID + "'");
			return string.Empty;
		}
		
		/// <summary>Wraps a list of strings in xml.</summary>
		/// <param name="ElementName">The name of each element and the base name of the wrapping element</param>
		/// <param name="Items">The list of items to wrap in xml</param>
		/// 
		/// <example><code>
		/// Generic.List&amp;amp;amp;amp;lt;String&amp;amp;amp;amp;gt; Items = new Generic.List&amp;amp;amp;amp;lt;String&amp;amp;amp;amp;gt;();
		/// Items.Add("Fred");
		/// Items.Add("George");
		/// Items.Add("Mary");
		/// AppendLine(xmlListNode("Item", Items));
		/// </code>
		/// </example>
		public static string xmlListNode(string ElementName, System.Collections.Generic.IList<String> Items)
		{
			StringBuilder Output = new StringBuilder();
            Output.AppendLine("<" + ElementName + "s>");
            foreach (string CurrNode in Items)
                Output.Append("<" + ElementName + " Name=\"" + xml.AttributeEncode(CurrNode) + "\"/>\r\n");
            Output.AppendLine("</" + ElementName + "s>");
            return Output.ToString();
		}
		
        /// <summary>Builds an xml string from the distinct contents of a field in the Notification table.</summary>
        /// <param name="FieldName">The field name</param>
        /// <returns>xml string of FieldName elements (one per record) with one attribute "Name" (equal to the value of each distinct FieldName value) wrapped in a FieldName + "s" element</returns>
        /// 
        /// <example><c>AppendLine(xmlUniqueList("Category"))</c></example>
        public static string xmlUniqueList(string FieldName)
		{
			return sql.ToXml(FieldName + "s", string.Empty, "SELECT DISTINCT " + FieldName + " As Name FROM Notification " + FieldName + " WHERE (NOT " + FieldName + " Is NULL) AND (NOT " + FieldName + "='') ORDER BY " + FieldName);
		}


        /// <summary>Builds an xml string from the distinct contents of a field in the Notification table limited by a particular type of notification.</summary>
        /// <param name="NotifyingMessageName">The value of the NotifyingMessageName field to limit the list to</param>
        /// <param name="FieldName">The field name</param>
        /// <returns>xml string of FieldName elements (one per record) with one attribute "Name" (equal to the value of each distinct FieldName value) wrapped in a FieldName + "s" element</returns>
        /// 
        /// <example><c>AppendLine(xmlUniqueList("Category"))</c></example>
        public static string xmlUniqueList(string NotifyingMessageName, string FieldName)
		{
			return sql.ToXml(FieldName + "s", string.Empty, "SELECT DISTINCT " + FieldName + " As Name FROM Notification " + FieldName + " WHERE (NOT " + FieldName + " Is NULL) AND (NOT " + FieldName + "='') AND NotifyingMessageName='" + NotifyingMessageName + "' ORDER BY " + FieldName);
		}

        /// <summary>Builds an xml string from the distinct contents of a field in the Notification table limited by a particular type of notification.</summary>
        /// <param name="NotifyingMessageName">The value of the NotifyingMessageName field to limit the list to</param>
        /// <param name="FieldName">The field name</param>
        /// <param name="TagName">The name of the tag for each element.</param>
        /// <returns>xml string of TagName elements (one per record) with one attribute "Name" (equal to the value of each distinct FieldName value) wrapped in a TagName + "s" element</returns>
        /// 
        /// <example><c>AppendLine(xmlUniqueList("Category"))</c></example>
        public static string xmlUniqueList(string NotifyingMessageName, string FieldName, string TagName)
		{
            if(NotifyingMessageName != null && NotifyingMessageName.Length > 0)
			    return sql.ToXml(TagName + "s", string.Empty, "SELECT DISTINCT " + FieldName + " As Name FROM Notification " + TagName + " WHERE (NOT " + FieldName + " Is NULL) AND (NOT " + FieldName + "='') AND NotifyingMessageName='" + NotifyingMessageName + "' ORDER BY " + FieldName);
            return sql.ToXml(TagName + "s", string.Empty, "SELECT DISTINCT " + FieldName + " As Name FROM Notification " + TagName + " WHERE (NOT " + FieldName + " Is NULL) AND (NOT " + FieldName + "='') ORDER BY " + FieldName);
		}

		/// <summary>A distinct list of the values in the NotifyingMessageName field in the Notification table</summary>
		/// <returns>xml string of "NotifyingMessageName" elements with a "Name" attribute equal to each distinct value wrapped in a "NotifyingMessageNames" element.</returns>
		/// 
		/// <example><c>AppendLine(xmlNotifyingMessageNames())</c></example>
		public static string xmlNotifyingMessageNames()
		{
			return sql.ToXml("NotifyingMessageNames", string.Empty, "SELECT DISTINCT NotifyingMessageName Name FROM Notification NotifyingMessageName WHERE (NOT NotifyingMessageName IS NULL) AND (NotifyingMessageName != '') ORDER BY NotifyingMessageName");
		}

        /// <summary>A distinct list of the values in the Notifier field in the Notification table</summary>
        /// <returns>xml string of "Notifier" elements with a "Name" attribute equal to each distinct value wrapped in a "Notifiers" element.</returns>
        /// 
        /// <example><c>AppendLine(xmlNotifiers())</c></example>
        public static string xmlNotifiers()
		{
			return sql.ToXml("Notifiers", string.Empty, "SELECT DISTINCT Notifier As Name FROM Notification Notifier WHERE (NOT Notifier IS NULL) AND (Notifier != '') ORDER BY Notifier");
		}

        /// <summary>Builds an xml string of "Notification" and child "Related" elements from the Notification table using the passed WHERE and ORDER BY clauses to limit and order the elements. Related records are child records.</summary>
        /// <param name="NotificationID">The NotificationID to return detail for</param>
        /// <param name="OrderByClause">The ORDER BY clause (not including the words "ORDER BY") to order the notification elements. If a null or blank is passed, the elements are ordered however they return from the database (usually by DateCreated ascending)</param>
        /// <param name="OnClause">(May be null or blank) The ON clause (not including the word "ON") connecting the self join Notification to Related. If a null or blank is passed, the tables are joined on the ParentNotificationID field only. Even if a value is passed, ParentNotificationID will also be joined on.</param>
        /// <returns>An xml string of "Notification" elements with child "Related" elements each with attributes equal to each field in the Notification table</returns>
        public static string xmlNotificationDetail(Guid NotificationID, string OrderByClause, string OnClause)
		{
            return xmlNotificationDetail("Notification.NotificationID='" + NotificationID.ToString() + "'", OrderByClause, OnClause);
		}
		
		/// <summary>Builds an xml string of "Notification" and child "Related" elements from the Notification table using the passed WHERE and ORDER BY clauses to limit and order the elements. Related records are child records.</summary>
		/// <param name="WhereClause">The WHERE clause (not including the word "WHERE") to limit notification records on. If blank or null, this function will return an empty string.</param>
		/// <param name="OrderByClause">The ORDER BY clause (not including the words "ORDER BY") to order the notification elements. If a null or blank is passed, the elements are ordered however they return from the database (usually by DateCreated ascending)</param>
		/// <param name="OnClause">(May be null or blank) The ON clause (not including the word "ON") connecting the self join Notification to Related. If a null or blank is passed, the tables are joined on the ParentNotificationID field only. Even if a value is passed, ParentNotificationID will also be joined on.</param>
		/// <returns>An xml string of "Notification" elements with child "Related" elements each with attributes equal to each field in the Notification table</returns>
		public static string xmlNotificationDetail(string WhereClause, string OrderByClause, string OnClause)
		{
			if(WhereClause == null || WhereClause.Length == 0)
				return string.Empty;
            if (OnClause == null || OnClause.Length == 0)
                OnClause = "Related.ParentNotificationID=Notification.NotificationID";
            else
                OnClause = "Related.ParentNotificationID=Notification.NotificationID or (" + OnClause + ")";
            if (OrderByClause == null)
                OrderByClause = "";
            else if (OrderByClause.Length > 0)
                OrderByClause = " ORDER BY " + OrderByClause;
            return sql.ToXml("SELECT * FROM Notification LEFT JOIN Notification Related ON " + OnClause + " WHERE " + WhereClause + OrderByClause);
		}


		/// <summary>Builds an xml "Notification" element from the Notification table for the passed NotificationID.</summary>
		/// <param name="NotificationID">The NotificationID to return </param>
		/// <returns>An xml string of a single "Notification" element with attributes equal to each field in the Notification table</returns>
		public static string xmlNotification(Guid NotificationID)
		{
			return xmlNotification("NotificationID='" + NotificationID.ToString() + "'");
		}
		
        /// <summary>Builds an xml string of "Notification" elements from the Notification table using the passed WHERE clause. If no where clause is passed, this function returns a blank string.</summary>
        /// <param name="WhereClause">The WHERE clause (not including the word "WHERE") to limit notification records on. If blank or null, this function will return an empty string.</param>
        /// <returns>An xml string of "Notification" elements each with attributes equal to each field in the Notification table</returns>
        public static string xmlNotification(string WhereClause)
		{
			if(WhereClause == null || WhereClause.Length == 0)
				return string.Empty;
			return sql.ToXml("SELECT * FROM Notification WHERE " + WhereClause);
		}
	}
}
