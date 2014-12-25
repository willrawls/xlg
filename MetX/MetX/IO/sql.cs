using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Xml;
using System.Data.SqlClient;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;

namespace MetX.IO
{
	
	/// <summary>Helper functions to simplify working with sql connections</summary>
	public static class sql
	{
		/// <summary>This returns the string representation of a column from a reader. Supports DBNull (default return value), guid, int, and datetime. All other types simply call GetString() on the reader.</summary>
		/// <param name="rst">The reader</param>
		/// <param name="Index">The column to retreive as a string</param>
		/// <param name="DefaultReturnValue">If the column is DBNull, this will be the return value</param>
		/// <returns>String representation of the field</returns>
		/// 
		/// <example><c>GetString(reader, 0, "Something");</c>
		/// Retrieves field 0. If it's DbNull, the string "Something" will be returned. </example>
		public static string GetString(SqlDataReader rst,  int Index , string DefaultReturnValue )
		{
			if (rst.IsDBNull(Index))
				return DefaultReturnValue;
			else if ( rst.GetFieldType(Index).Equals(Guid.Empty.GetType()) ) 
				return rst.GetGuid(Index).ToString();
			else if ( rst.GetFieldType(Index).Equals(int.MinValue.GetType()) ) 
				return rst.GetInt32(Index).ToString();
			else if ( rst.GetFieldType(Index).Equals(DateTime.MinValue.GetType()) ) 
				return rst.GetDateTime(Index).ToString();
			else
				return rst.GetString(Index);
		}

        /// <summary>This returns the string representation of a column from a reader. Supports DBNull (as an empty string), guid, int, and datetime. All other types simply call GetString() on the reader.</summary>
        /// <param name="rst">The reader</param>
        /// <param name="Index">The column to retreive</param>
        /// <returns>String representation of the field</returns>
        /// 
        /// <example><c>GetString(reader, 0);</c>
        /// Retrieves field 0. If it's DbNull, the string "" will be returned. </example>
        public static string GetString(SqlDataReader rst, int Index)
		{
            return GetString(rst, Index, string.Empty);
		}

        /// <summary>This returns the DateTime representation of a column from a reader. Supports DBNull (as an empty string), guid, int, and datetime. All other types simply call GetDateTime() on the reader.</summary>
        /// <param name="rst">The reader</param>
        /// <param name="Index">The column to retreive</param>
        /// <returns>String representation of the field</returns>
        /// 
        /// <example><c>GetDateTime(reader, 0);</c>
        /// Retrieves field 0. If it's DbNull, DateTime.MinValue will be returned. </example>
        public static DateTime GetDateTime(SqlDataReader rst, int Index)
		{
			DateTime ReturnValue = new DateTime(0);

			if (rst.IsDBNull(Index))
				return ReturnValue;
			else if ( rst.GetFieldType(Index).Equals(DateTime.MinValue.GetType()) )
				ReturnValue = rst.GetDateTime(Index);

			return ReturnValue;
		}

		/// <summary>Returns the first value of the first row of the passed sql as a string. If DbNull is the value, DefaultReturnValue is returned instead.</summary>
		/// <param name="SQL">The sql to return. While it doesn't matter if the SQL actually only returns one row with one column, it is recommended the SQL be tailored to do so.</param>
		/// <param name="ConnectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
		/// <param name="DefaultReturnValue">The value to return when DbNull is encountered.</param>
		/// <returns>String value of the first column of the first row</returns>
		public static string RetrieveSingleStringValue(string SQL, string ConnectionName, string DefaultReturnValue)
		{
            SqlDataReader rst = null;
			SqlCommand cmd = new SqlCommand(SQL);
			string ReturnValue;
            SqlConnection conn = GetConnection(ConnectionName);

			try
			{
				cmd.Connection = conn;
                
				rst = cmd.ExecuteReader(CommandBehavior.SingleRow);
				if (rst.Read())
				{
					if (rst.IsDBNull(0))
						ReturnValue = DefaultReturnValue;
					else if ( rst.GetFieldType(0).Equals(typeof(Guid)) ) 
						ReturnValue = rst.GetGuid(0).ToString();
					else
						ReturnValue = GetString(rst, 0);
				}
				else
					ReturnValue = DefaultReturnValue;
			}
			catch( System.Exception E )
			{
				throw new Exception("SQL: " + SQL.ToString() +  "\r\n\r\n" + E.ToString() );
			}
			finally
			{
				if( !Equals(rst, null) )
					rst.Close();
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}
			return ReturnValue;
		}

        /// <summary>Returns the first value of the first row of the passed sql as a string. If DbNull is the value, A blank string is returned instead.</summary>
        /// <param name="SQL">The sql to return. While it doesn't matter if the SQL actually only returns one row with one column, it is recommended the SQL be tailored to do so.</param>
        /// <returns>String value of the first column of the first row</returns>
        public static string RetrieveSingleStringValue(string SQL)
        {
            return RetrieveSingleStringValue(SQL, null);
        }

        public static string RetrieveSingleStringValue(IDataReader idr)
        {
            if (idr != null)
                if (idr.Read())
                {
                    string ret = idr.GetString(0);
                    idr.Close();
                    idr.Dispose();
                    return ret;
                }
                else
                {
                    idr.Close();
                    idr.Dispose();
                }
            return null;
        }

        /// <summary>Returns the first value of the first row of the passed sql as a string. If DbNull is the value, a blank string is returned instead.</summary>
        /// <param name="SQL">The sql to return. While it doesn't matter if the SQL actually only returns one row with one column, it is recommended the SQL be tailored to do so.</param>
        /// <param name="ConnectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        /// <returns>String value of the first column of the first row</returns>
        public static string RetrieveSingleStringValue(string SQL, string ConnectionName)
		{
            string DefaultReturnValue  = "" ;
			Guid g = new Guid();
			SqlDataReader rst = null;
			string ReturnValue;
            SqlConnection conn = GetConnection(ConnectionName);
            SqlCommand cmd = new SqlCommand(SQL, conn);
			try
			{
				rst = cmd.ExecuteReader(CommandBehavior.SingleRow);
				if (rst.Read())
				{
					if (rst.IsDBNull(0))
						ReturnValue = DefaultReturnValue;
					else if ( rst.GetFieldType(0).Equals( g.GetType()) ) 
						ReturnValue = rst.GetGuid(0).ToString();
					else
						ReturnValue = GetString(rst, 0);
				}
				else
					ReturnValue = DefaultReturnValue;
			}
			catch( System.Exception E )
			{
				throw new Exception("SQL: " + SQL.ToString() +  "\r\n\r\n" + E.ToString() );
			}
			finally
			{
				if( !Equals(rst, null) )
					rst.Close();
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}
			return ReturnValue;
		}
        

        /// <summary>Converts a SQL statement into a series of elements via SQLXML. If a "FOR XML" phrase is not found "FOR XML AUTO" is added to the SQL</summary>
        /// <param name="TagName">The element name to wrap the returned xml element(s). If null or blank, no tag wraps the returned xml string</param>
        /// <param name="TagAttributes">The attributes to add to the TagName element</param>
        /// <param name="SQL">The SQL to convert to an xml string</param>
        /// <param name="ConnectionName">The Connection from Web.config to use</param>
        /// <returns>The xml string attribute based representation of the SQL statement</returns>
        public static string ToXml(string TagName, string TagAttributes, string SQL, string ConnectionName)
        {
            if(SQL.IndexOf("FOR XML") == -1) SQL += " FOR XML AUTO";
            StringBuilder ReturnValue = new StringBuilder();
            System.Data.SqlClient.SqlConnection conn = GetConnection(ConnectionName);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(SQL, conn);
            try
            {
                if (TagName != null && TagName.Length > 0)
                    if (TagAttributes != null && TagAttributes.Length > 0)
                        ReturnValue.AppendLine("<" + TagName + " " + TagAttributes + ">");
                    else
                        ReturnValue.AppendLine("<" + TagName + ">");
                System.Xml.XmlReader xr = cmd.ExecuteXmlReader();
                xr.Read();
                while (xr.ReadState != System.Xml.ReadState.EndOfFile)
                    ReturnValue.Append(xr.ReadOuterXml());
                xr.Close();
                if (TagName != null && TagName.Length > 0)
                    ReturnValue.AppendLine("</" + TagName + ">");
            }
            catch (Exception e)
            {
                throw new Exception("SQL = " + SQL + "\r\n\r\n" + e.ToString());
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }
            return ReturnValue.ToString();
        }

        /// <summary>Converts a SQL statement into a series of elements via SQLXML. If a "FOR XML" phrase is not found "FOR XML AUTO" is added to the SQL</summary>
        /// <param name="TagName">The element name to wrap the returned xml element(s). If null or blank, no tag wraps the returned xml string</param>
        /// <param name="TagAttributes">The attributes to add to the TagName element</param>
        /// <param name="SQL">The SQL to convert to an xml string</param>
        /// <returns>The xml string attribute based representation of the SQL statement</returns>
        public static string ToXml(string TagName, string TagAttributes, string SQL)
		{
            return ToXml(TagName, TagAttributes, SQL, null);
        }

        /// <summary>Converts a SQL statement into a series of elements via SQLXML. If a "FOR XML" phrase is not found "FOR XML AUTO" is added to the SQL</summary>
        /// <param name="SQL">The SQL to convert to an xml string</param>
        /// <param name="ConnectionName">The Connection from Web.config to use</param>
        /// <returns>The xml string attribute based representation of the SQL statement</returns>
        public static string ToXml(string SQL, string ConnectionName)
		{
            return ToXml(null, null, SQL, ConnectionName);
        }

        /// <summary>Converts a SQL statement into a series of elements via SQLXML. If a "FOR XML" phrase is not found "FOR XML AUTO" is added to the SQL</summary>
        /// <param name="SQL">The SQL to convert to an xml string</param>
        /// <returns>The xml string attribute based representation of the SQL statement</returns>
        public static string ToXml(string SQL)
        {
            return ToXml(null, null, SQL, null);
        }
        
        /// <summary>Simply runs the SQL statement and returns a DataSet</summary>
        /// <param name="SQL">The SQL to run</param>
        /// <param name="ConnectionName">The Connection from Web.config to use</param>
        /// <returns>A DataSet object with the results</returns>
        public static DataSet ToDataSet(string SQL, string ConnectionName)
		{
            SqlConnection conn = GetConnection(ConnectionName);
			SqlCommand cmd = new SqlCommand(SQL, conn);
			DataSet ReturnValue = new DataSet();

			try
			{
				System.Data.SqlClient.SqlDataAdapter RA = new System.Data.SqlClient.SqlDataAdapter(cmd);
				RA.Fill(ReturnValue);
			}
			catch (Exception E)
			{
				throw new Exception("SQL = " + SQL + "\r\n\r\n" + E.ToString());
			}
			finally
			{
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}

			return ReturnValue;
		}

        /// <summary>Simply runs the SQL statement and returns a DataSet</summary>
        /// <param name="SQL">The SQL to run</param>
        /// <returns>A DataSet object with the results</returns>
        public static DataSet ToDataSet(string SQL)
		{
			SqlConnection conn = DefaultConnection;
			SqlCommand cmd = new SqlCommand(SQL);
			DataSet ReturnValue = new DataSet();

			try
			{
				cmd.Connection = conn;
				System.Data.SqlClient.SqlDataAdapter RA = new System.Data.SqlClient.SqlDataAdapter(cmd);
				RA.Fill(ReturnValue);
			}
			catch (Exception E)
			{
				throw new Exception("SQL = " + SQL + "\r\n\r\n" + E.ToString());
			}
			finally
			{
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}

			return ReturnValue;
		}

        /// <summary>Returns the first column as a DateTime value of the first row of the passed sql. If DbNull is the value, DateTime.MinValue is returned instead.</summary>
        /// <param name="SQL">The sql to return. While it doesn't matter if the SQL actually only returns one row with one column, it is recommended the SQL be tailored to do so.</param>
        /// <param name="ConnectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        /// <returns>DateTime value of the first column of the first row</returns>
        public static DateTime RetrieveSingleDateValue(string SQL, string ConnectionName)
		{
            SqlConnection conn = GetConnection(ConnectionName);
			SqlCommand cmd = new SqlCommand(SQL, conn);
			SqlDataReader rst = null;
			DateTime ReturnValue;
			try
			{
				rst = cmd.ExecuteReader(CommandBehavior.SingleRow);
				if (rst.Read())
				{
					if (rst.IsDBNull(0))
						ReturnValue = new DateTime(0);
					else
						ReturnValue = rst.GetDateTime(0);
				}
				else
					ReturnValue = new DateTime(0);
			}
			catch( Exception E)
			{
				throw new Exception("SQL = " + SQL + "\r\n\r\n" + E.ToString());
			}
			finally
			{
				if(!Equals(rst, null))
					rst.Close();
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}
			return ReturnValue;
		}

        /// <summary>Returns the first column as a DateTime value of the first row of the passed sql. If DbNull is the value, DateTime.MinValue is returned instead.</summary>
        /// <param name="SQL">The sql to return. While it doesn't matter if the SQL actually only returns one row with one column, it is recommended the SQL be tailored to do so.</param>
        /// <returns>DateTime value of the first column of the first row</returns>
        public static DateTime RetrieveSingleDateValue(string SQL)
		{
            return RetrieveSingleDateValue(SQL, null);
		}

        /// <summary>Returns the first column as an int value of the first row of the passed sql. If DbNull is the value, DefaultReturnValue is returned instead.</summary>
        /// <param name="SQL">The sql to return. While it doesn't matter if the SQL actually only returns one row with one column, it is recommended the SQL be tailored to do so.</param>
        /// <param name="ConnectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        /// <param name="DefaultReturnValue">The value to return if DbNull is encountered</param>
        /// <returns>int value of the first column of the first row</returns>
        public static int RetrieveSingleIntegerValue(string SQL, string ConnectionName, int DefaultReturnValue)
		{
            SqlConnection conn = GetConnection(ConnectionName);
			SqlCommand cmd = new SqlCommand(SQL, conn);
			SqlDataReader rst = null;
			int ReturnValue;
			try
			{
				rst = cmd.ExecuteReader(CommandBehavior.SingleRow);
				if (rst.Read())
				{
					if (rst.IsDBNull(0))
						ReturnValue = DefaultReturnValue;
					else
						ReturnValue = rst.GetInt32(0);
				}
				else
					ReturnValue = DefaultReturnValue;
			}
			catch( Exception E)
			{
				throw new Exception("SQL = " + SQL + "\r\n\r\n" + E.ToString() );
			}
			finally
			{
				if(!Equals(rst, null))
					rst.Close();
				cmd.Dispose();
				conn.Close();				
				conn.Dispose();
			}
			return ReturnValue;
		}

        /// <summary>Returns the first column as an int value of the first row of the passed sql. If DbNull is the value, 0 is returned instead.</summary>
        /// <param name="SQL">The sql to return. While it doesn't matter if the SQL actually only returns one row with one column, it is recommended the SQL be tailored to do so.</param>
        /// <param name="ConnectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        /// <returns>int value of the first column of the first row</returns>
        public static int RetrieveSingleIntegerValue(string SQL, string ConnectionName)
		{
            return RetrieveSingleIntegerValue(SQL, ConnectionName, 0);
		}

        /// <summary>Returns the first column as an int value of the first row of the passed sql. If DbNull is the value, 0 is returned instead.</summary>
        /// <param name="SQL">The sql to return. While it doesn't matter if the SQL actually only returns one row with one column, it is recommended the SQL be tailored to do so.</param>
        /// <returns>int value of the first column of the first row</returns>
        public static int RetrieveSingleIntegerValue(string SQL)
		{
            return RetrieveSingleIntegerValue(SQL, null, 0);
		}

        /// <summary>Converts the SQL passed in into a DataSet and returns the DataTable found, otherwise null is returned.</summary>
        /// <param name="SQL">The sql to return. While it doesn't matter if the SQL actually only returns one resultset of values, it is recommended the SQL be tailored to do so.</param>
        /// <param name="ConnectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        /// <returns>A DataTable object representing the SQL statement or null</returns>
        public static DataTable ToDataTable(string SQL, string ConnectionName)
        {
            DataSet ds = ToDataSet(SQL, ConnectionName);
            if (ds.Tables.Count < 1)
                return null;
            else if (ds.Tables[0].Rows.Count < 1)
                return null;
            else
                return ds.Tables[0];
        }

        /// <summary>Converts the SQL passed in into a DataSet and returns the DataTable found, otherwise null is returned.</summary>
        /// <param name="SQL">The sql to return. While it doesn't matter if the SQL actually only returns one resultset of values, it is recommended the SQL be tailored to do so.</param>
        /// <returns>A DataTable object representing the SQL statement or null</returns>
        public static DataTable ToDataTable(string SQL)
        {
            DataSet ds = ToDataSet(SQL);
            if (ds.Tables.Count < 1)
                return null;
            else if (ds.Tables[0].Rows.Count < 1)
                return null;
            else
                return ds.Tables[0];
        }

        /// <summary>Converts the SQL passed in into a DataSet and returns the DataTable found, otherwise null is returned.</summary>
        /// <param name="SQL">The sql to return. While it doesn't matter if the SQL actually only returns one resultset of values, it is recommended the SQL be tailored to do so.</param>
        /// <param name="ConnectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        /// <param name="ToFill">The DataTable to fill (could be a strongly typed DataTable)</param>
        /// <returns>A DataTable object representing the SQL statement or null</returns>
        public static DataTable ToDataTable(string SQL, string ConnectionName, DataTable ToFill)
        {
            SqlConnection conn = GetConnection(ConnectionName);
            SqlCommand cmd = new SqlCommand(SQL, conn);
            if (ToFill == null)
                ToFill = new DataTable();
            try
            {
                System.Data.SqlClient.SqlDataAdapter RA = new System.Data.SqlClient.SqlDataAdapter(cmd);
                
                RA.Fill(ToFill);
            }
            catch (Exception E)
            {
                throw new Exception("SQL = " + SQL + "\r\n\r\n" + E.ToString());
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }

            if (ToFill == null)
                return null;
            else if (ToFill.Rows.Count < 1)
                return null;
            else
                return ToFill;
        }

        /// <summary>Converts the SQL passed in into a DataSet and returns the DataTable found, otherwise null is returned.</summary>
        /// <param name="SQL">The sql to return. While it doesn't matter if the SQL actually only returns one resultset of values, it is recommended the SQL be tailored to do so.</param>
        /// <param name="ToFill">The DataTable to fill (could be a strongly typed DataTable)</param>
        /// <returns>A DataTable object representing the SQL statement or null</returns>
        public static DataTable ToDataTable(string SQL, DataTable ToFill)
        {
            return ToDataTable(SQL, null, ToFill);
        }

        /// <summary>Converts the SQL passed in into a DataSet and returns the DataRowCollection inside the first DataTable found, otherwise null is returned.</summary>
        /// <param name="SQL">The sql to return. While it doesn't matter if the SQL actually only returns one resultset of values, it is recommended the SQL be tailored to do so.</param>
        /// <param name="ConnectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        /// <returns>A DataRowCollection object representing the SQL statement or null</returns>
        public static DataRowCollection ToDataRows(string SQL, string ConnectionName)
		{
            DataSet ds = ToDataSet(SQL, ConnectionName);
			if (ds.Tables.Count <  1)
				return null;
			else if (ds.Tables[0].Rows.Count <  1)
				return null;
			else
				return ds.Tables[0].Rows;
		}

        /// <summary>Converts the SQL passed in into a DataSet and returns the DataRowCollection inside the first DataTable found, otherwise null is returned.</summary>
        /// <param name="SQL">The sql to return. While it doesn't matter if the SQL actually only returns one resultset of values, it is recommended the SQL be tailored to do so.</param>
        /// <returns>A DataRowCollection object representing the SQL statement or null</returns>
        public static DataRowCollection ToDataRows(string SQL)
		{
			DataSet ds = ToDataSet(SQL);
			if (ds.Tables.Count <  1)
				return null;
			else if (ds.Tables[0].Rows.Count <  1)
				return null;
			else
				return ds.Tables[0].Rows;
		}

        /// <summary>Converts the SQL passed in into a DataSet and returns the first DataRow inside the first DataTable found, otherwise null is returned.</summary>
        /// <param name="SQL">The sql to return. While it doesn't matter if the SQL actually only returns one DataRow of values, it is recommended the SQL be tailored to do so.</param>
        /// <param name="ConnectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        /// <returns>A DataRow object representing the SQL statement or null</returns>
        public static DataRow ToDataRow(string SQL, string ConnectionName)
		{
            DataSet ds = ToDataSet(SQL, ConnectionName);
			if (ds.Tables.Count <  1)
				return null;
			else if (ds.Tables[0].Rows.Count <  1)
				return null;
			else
				return ds.Tables[0].Rows[0];
		}

        /// <summary>Converts the SQL passed in into a DataSet and returns the first DataRow inside the first DataTable found, otherwise null is returned.</summary>
        /// <param name="SQL">The sql to return. While it doesn't matter if the SQL actually only returns one DataRow of values, it is recommended the SQL be tailored to do so.</param>
        /// <returns>A DataRow object representing the SQL statement or null</returns>
        public static DataRow ToDataRow(string SQL)
		{
			DataSet ds = ToDataSet(SQL);
			if (ds.Tables.Count <  1)
				return null;
			else if (ds.Tables[0].Rows.Count <  1)
				return null;
			else
				return ds.Tables[0].Rows[0];
		}

		/// <summary>Updates a field in the Notification table for a single NotificationID</summary>
		/// <param name="NotificationID">The NotificationID to update</param>
		/// <param name="FieldName">The field to update</param>
		/// <param name="NewValue">The new value of the field</param>
		/// <param name="MaxLength">If the field has a maximum length, specify it and this function will insure the field does not exceed that length</param>
		/// <returns>True if the record was updated</returns>
		public static bool UpdateNotificationField(string NotificationID, string FieldName, string NewValue, int MaxLength)
		{
            SqlConnection conn = DefaultConnection;
			SqlCommand cmd = new SqlCommand("SET ARITHABORT ON", conn);
			cmd.ExecuteNonQuery();
			cmd.CommandText = "UPDATE Notification SET " + FieldName + "='" + NewValue.Substring(0, MaxLength) + "' WHERE NotificationID='" + NotificationID + "'";
			int RecordCount = cmd.ExecuteNonQuery();
			cmd.Dispose();
			conn.Close();
			conn.Dispose();
			return (RecordCount >  0);
		}
		
        /// <summary>Executes a series of SQL statements on the same connection</summary>
        /// <param name="SqlArray">The list of SQLs to execute</param>
        /// <param name="ConnectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        public static void Execute(List<string> SqlArray, string ConnectionName)
		{
			if (SqlArray.Count >  0)
			{
                SqlConnection conn = GetConnection(ConnectionName);
				SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
				int ErrorAtSql = - 1;
				foreach (string SQL in SqlArray)
				{
					try
					{
						ErrorAtSql = 1;
						cmd.CommandText = "SET ARITHABORT ON";
						cmd.ExecuteNonQuery();
						cmd.CommandText = SQL;
						cmd.ExecuteNonQuery();
					}
					catch (Exception e)
					{
						conn.Close();
						StringBuilder SB = new StringBuilder();
						int CurrSql;
						SB.Append("Exception while executing SQL # " + ErrorAtSql + ": " + SQL + "\r\n\r\n" + e.ToString() + "\r\n");
						SB.Append("\r\n------------------------------------------------------------------------\r\n");
						for (CurrSql=0; CurrSql <= SqlArray.Count - 1; CurrSql++)
							SB.Append(CurrSql + ": " + SqlArray[CurrSql] + "\r\n\r\n");
						cmd.Dispose();
						conn.Dispose();
						throw new Exception(SB.ToString());
					}
				}
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}
		}

        /// <summary>Executes a series of SQL statements on the same Default connection</summary>
        /// <param name="SqlArray">The list of SQLs to execute</param>
        public static void Execute(List<string> SqlArray)
		{
            Execute(SqlArray, DefaultConnectionString);
		}

        /// <summary>Executes a SQL statement</summary>
        /// <param name="SQL">The SQL to execute</param>
        /// <param name="ConnectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        /// <returns>The number of records affected</returns>
        public static int Execute(string SQL, string ConnectionName)
		{
			int RecordCount = 0;
            if (SQL != null && SQL.Length > 0)
            {
                SqlConnection conn = GetConnection(ConnectionName);
                SqlCommand cmd = new SqlCommand("SET ARITHABORT ON", conn);
                try
                {
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = SQL;
                    RecordCount = cmd.ExecuteNonQuery();
                }
                catch (Exception E)
                {
                    throw new Exception("SQL = " + SQL + "\r\n\r\n" + E.ToString());
                }
                finally
                {
                    cmd.Dispose();
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            }
			return RecordCount;
		}

        /// <summary>Executes a SQL statement</summary>
        /// <param name="SQL">The SQL to execute</param>
        /// <returns>The number of records affected</returns>
        public static int Execute(string SQL)
		{
            return Execute(SQL, DefaultConnectionString);
		}
		

        /// <summary>Returns a SqlConnection object given the connection name from Web.config. If that entry is blank or missing, Default.SqlClient is used.</summary>
        /// <param name="ConnectionName">The Web.config key name containing the connection string</param>
        /// <returns>An open SqlConnection object to the appropriate database (remember to close it)</returns>
        public static SqlConnection GetConnection(string ConnectionName)
        {
            SqlConnection ret = new SqlConnection(GetConnectionString(ConnectionName));
            ret.Open();
            return ret;
        }

        /// <summary>Returns the Web.config connection string named. If that entry is blank or missing, Default.SqlClient is returned.</summary>
        /// <param name="ConnectionName">The Web.config key name</param>
        /// <returns>A connection string</returns>
        public static string GetConnectionString(string ConnectionName)
        {
            if (ConnectionName == null || ConnectionName.Length == 0)
                return DefaultConnectionString;
            ConnectionStringSettings Settings = ConfigurationManager.ConnectionStrings[ConnectionName];
            if (Settings  == null)
                return DefaultConnectionString;
            string ConnectionString = Settings.ConnectionString;
            if (ConnectionString == null || ConnectionString.Length == 0)
                return DefaultConnectionString;
            return ConnectionString;
        }

        /// <summary>Returns the value of the Web.config key "Default.SqlClient"</summary>
        public static string DefaultConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            }
        }

        /// <summary>Returns a SqlClonnetion object using DefaultConnectionString</summary>
        public static SqlConnection DefaultConnection
        {
            get
            {
                SqlConnection ret = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
                ret.Open();
                return ret;
            }
        }
    }
}
