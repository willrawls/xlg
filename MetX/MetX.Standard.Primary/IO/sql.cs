using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Xml;
using MetX.Standard.Strings;

namespace MetX.Standard.Primary.IO
{
	
	/// <summary>Helper functions to simplify working with sql connections</summary>
	public static class Sql
	{
		/// <summary>This returns the string representation of a column from a reader. Supports DBNull (default return value), guid, int, and datetime. All other types simply call GetString() on the reader.</summary>
		/// <param name="rst">The reader</param>
		/// <param name="index">The column to retreive as a string</param>
		/// <param name="defaultReturnValue">If the column is DBNull, this will be the return value</param>
		/// <returns>String representation of the field</returns>
		/// 
		/// <example><c>GetString(reader, 0, "Something");</c>
		/// Retrieves field 0. If it's DbNull, the string "Something" will be returned. </example>
		public static string GetString(SqlDataReader rst,  int index , string defaultReturnValue )
		{
		    if (rst.IsDBNull(index))
				return defaultReturnValue;
		    if ( rst.GetFieldType(index) == Guid.Empty.GetType() ) 
		        return rst.GetGuid(index).ToString();
		    if ( rst.GetFieldType(index) == int.MinValue.GetType() ) 
		        return rst.GetInt32(index).ToString();
		    if ( rst.GetFieldType(index) == DateTime.MinValue.GetType() ) 
		        return rst.GetDateTime(index).ToString(CultureInfo.InvariantCulture);
		    return rst.GetString(index);
		}

	    /// <summary>This returns the string representation of a column from a reader. Supports DBNull (as an empty string), guid, int, and datetime. All other types simply call GetString() on the reader.</summary>
        /// <param name="rst">The reader</param>
        /// <param name="index">The column to retreive</param>
        /// <returns>String representation of the field</returns>
        /// 
        /// <example><c>GetString(reader, 0);</c>
        /// Retrieves field 0. If it's DbNull, the string "" will be returned. </example>
        public static string GetString(SqlDataReader rst, int index)
		{
            return GetString(rst, index, string.Empty);
		}

        /// <summary>This returns the DateTime representation of a column from a reader. Supports DBNull (as an empty string), guid, int, and datetime. All other types simply call GetDateTime() on the reader.</summary>
        /// <param name="rst">The reader</param>
        /// <param name="index">The column to retreive</param>
        /// <returns>String representation of the field</returns>
        /// 
        /// <example><c>GetDateTime(reader, 0);</c>
        /// Retrieves field 0. If it's DbNull, DateTime.MinValue will be returned. </example>
        public static DateTime GetDateTime(SqlDataReader rst, int index)
		{
			var returnValue = new DateTime(0);

			if (rst.IsDBNull(index))
				return returnValue;
            if ( rst.GetFieldType(index) == DateTime.MinValue.GetType() )
                returnValue = rst.GetDateTime(index);

            return returnValue;
		}

		/// <summary>Returns the first value of the first row of the passed sql as a string. If DbNull is the value, DefaultReturnValue is returned instead.</summary>
		/// <param name="sql">The sql to return. While it doesn't matter if the SQL actually only returns one row with one column, it is recommended the SQL be tailored to do so.</param>
		/// <param name="connectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
		/// <param name="defaultReturnValue">The value to return when DbNull is encountered.</param>
		/// <returns>String value of the first column of the first row</returns>
		public static string RetrieveSingleStringValue(string sql, string connectionName, string defaultReturnValue)
		{
            SqlDataReader rst = null;
			var cmd = new SqlCommand(sql);
			string returnValue;
            var conn = GetConnection(connectionName);

			try
			{
				cmd.Connection = conn;
                
				rst = cmd.ExecuteReader(CommandBehavior.SingleRow);
				if (rst.Read())
				{
					if (rst.IsDBNull(0))
						returnValue = defaultReturnValue;
					else if ( rst.GetFieldType(0) == typeof(Guid) ) 
						returnValue = rst.GetGuid(0).ToString();
					else
						returnValue = GetString(rst, 0);
				}
				else
					returnValue = defaultReturnValue;
			}
			catch( Exception e )
			{
				throw new Exception("SQL: " + sql +  "\r\n\r\n" + e );
			}
			finally
			{
                rst?.Close();
                cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}
			return returnValue;
		}

        /// <summary>Returns the first value of the first row of the passed sql as a string. If DbNull is the value, A blank string is returned instead.</summary>
        /// <param name="sql">The sql to return. While it doesn't matter if the SQL actually only returns one row with one column, it is recommended the SQL be tailored to do so.</param>
        /// <returns>String value of the first column of the first row</returns>
        public static string RetrieveSingleStringValue(string sql)
        {
            return RetrieveSingleStringValue(sql, null);
        }

        public static string RetrieveSingleStringValue(IDataReader idr)
        {
            if (idr != null)
                if (idr.Read())
                {
                    var ret = idr.GetString(0);
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
        /// <param name="sql">The sql to return. While it doesn't matter if the SQL actually only returns one row with one column, it is recommended the SQL be tailored to do so.</param>
        /// <param name="connectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        /// <returns>String value of the first column of the first row</returns>
        public static string RetrieveSingleStringValue(string sql, string connectionName)
		{
			var g = new Guid();
			SqlDataReader rst = null;
			string returnValue;
            var conn = GetConnection(connectionName);
            var cmd = new SqlCommand(sql, conn);
			try
			{
				rst = cmd.ExecuteReader(CommandBehavior.SingleRow);
				if (rst.Read())
				{
					if (rst.IsDBNull(0))
						returnValue = string.Empty ;
					else if ( rst.GetFieldType(0) == g.GetType() ) 
						returnValue = rst.GetGuid(0).ToString();
					else
						returnValue = GetString(rst, 0);
				}
				else
					returnValue = string.Empty ;
			}
			catch( Exception e )
			{
				throw new Exception("SQL: " + sql +  "\r\n\r\n" + e );
			}
			finally
			{
                rst?.Close();
                cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}
			return returnValue;
		}
        

        /// <summary>Converts a SQL statement into a series of elements via SQLXML. If a "FOR XML" phrase is not found "FOR XML AUTO" is added to the SQL</summary>
        /// <param name="tagName">The element name to wrap the returned xml element(s). If null or blank, no tag wraps the returned xml string</param>
        /// <param name="tagAttributes">The attributes to add to the TagName element</param>
        /// <param name="sql">The SQL to convert to an xml string</param>
        /// <param name="connectionName">The Connection from Web.config to use</param>
        /// <returns>The xml string attribute based representation of the SQL statement</returns>
        public static string ToXml(string tagName, string tagAttributes, string sql, string connectionName = null)
        {
            if(sql.IndexOf("FOR XML", StringComparison.Ordinal) == -1) sql += " FOR XML AUTO";
            var returnValue = new StringBuilder();
            var conn = GetConnection(connectionName);
            var cmd = new SqlCommand(sql, conn);
            try
            {
                if (tagName != null && tagName.Length > 0)
                    if (tagAttributes != null && tagAttributes.Length > 0)
                        returnValue.AppendLine("<" + tagName + " " + tagAttributes + ">");
                    else
                        returnValue.AppendLine("<" + tagName + ">");
                var xr = cmd.ExecuteXmlReader();
                xr.Read();
                while (xr.ReadState != ReadState.EndOfFile)
                    returnValue.Append(xr.ReadOuterXml());
                xr.Close();
                if (tagName != null && tagName.Length > 0)
                    returnValue.AppendLine("</" + tagName + ">");
            }
            catch (Exception e)
            {
                throw new Exception("SQL = " + sql + "\r\n\r\n" + e);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }
            return returnValue.ToString();
        }

        /// <summary>Converts a SQL statement into a series of elements via SQLXML. If a "FOR XML" phrase is not found "FOR XML AUTO" is added to the SQL</summary>
        /// <param name="sql">The SQL to convert to an xml string</param>
        /// <param name="connectionName">The Connection from Web.config to use</param>
        /// <returns>The xml string attribute based representation of the SQL statement</returns>
        public static string ToXml(string sql, string connectionName)
		{
            return ToXml(null, null, sql, connectionName);
        }

        /// <summary>Converts a SQL statement into a series of elements via SQLXML. If a "FOR XML" phrase is not found "FOR XML AUTO" is added to the SQL</summary>
        /// <param name="sql">The SQL to convert to an xml string</param>
        /// <returns>The xml string attribute based representation of the SQL statement</returns>
        public static string ToXml(string sql)
        {
            return ToXml(null, null, sql, null);
        }
        
        /// <summary>Simply runs the SQL statement and returns a DataSet</summary>
        /// <param name="sql">The SQL to run</param>
        /// <param name="connectionName">The Connection from Web.config to use</param>
        /// <returns>A DataSet object with the results</returns>
        public static DataSet ToDataSet(string sql, string connectionName)
		{
            var conn = GetConnection(connectionName);
			var cmd = new SqlCommand(sql, conn);
			var returnValue = new DataSet();

			try
			{
				var ra = new SqlDataAdapter(cmd);
				ra.Fill(returnValue);
			}
			catch (Exception e)
			{
				throw new Exception("SQL = " + sql + "\r\n\r\n" + e);
			}
			finally
			{
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}

			return returnValue;
		}

        /// <summary>Simply runs the SQL statement and returns a DataSet</summary>
        /// <param name="sql">The SQL to run</param>
        /// <returns>A DataSet object with the results</returns>
        public static DataSet ToDataSet(string sql)
		{
			var conn = DefaultConnection;
			var cmd = new SqlCommand(sql);
			var returnValue = new DataSet();

			try
			{
				cmd.Connection = conn;
				var ra = new SqlDataAdapter(cmd);
				ra.Fill(returnValue);
			}
			catch (Exception e)
			{
				throw new Exception("SQL = " + sql + "\r\n\r\n" + e);
			}
			finally
			{
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}

			return returnValue;
		}

        /// <summary>Returns the first column as a DateTime value of the first row of the passed sql. If DbNull is the value, DateTime.MinValue is returned instead.</summary>
        /// <param name="sql">The sql to return. While it doesn't matter if the SQL actually only returns one row with one column, it is recommended the SQL be tailored to do so.</param>
        /// <param name="connectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        /// <returns>DateTime value of the first column of the first row</returns>
        public static DateTime RetrieveSingleDateValue(string sql, string connectionName = null)
		{
            var conn = GetConnection(connectionName);
			var cmd = new SqlCommand(sql, conn);
			SqlDataReader rst = null;
			DateTime returnValue;
			try
			{
				rst = cmd.ExecuteReader(CommandBehavior.SingleRow);
				if (rst.Read())
				{
					returnValue = rst.IsDBNull(0) 
						? new DateTime(0) 
						: rst.GetDateTime(0);
				}
				else
					returnValue = new DateTime(0);
			}
			catch( Exception e)
			{
				throw new Exception("SQL = " + sql + "\r\n\r\n" + e);
			}
			finally
			{
                rst?.Close();
                cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}
			return returnValue;
		}

        /// <summary>Returns the first column as an int value of the first row of the passed sql. If DbNull is the value, DefaultReturnValue is returned instead.</summary>
        /// <param name="sql">The sql to return. While it doesn't matter if the SQL actually only returns one row with one column, it is recommended the SQL be tailored to do so.</param>
        /// <param name="connectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        /// <param name="defaultReturnValue">The value to return if DbNull is encountered</param>
        /// <returns>int value of the first column of the first row</returns>
        public static int RetrieveSingleIntegerValue(string sql, string connectionName = null, int defaultReturnValue = 0)
		{
            var conn = GetConnection(connectionName);
			var cmd = new SqlCommand(sql, conn);
			SqlDataReader rst = null;
			int returnValue;
			try
			{
				rst = cmd.ExecuteReader(CommandBehavior.SingleRow);
				if (rst.Read())
				{
					returnValue = rst.IsDBNull(0) 
						? defaultReturnValue 
						: rst.GetInt32(0);
				}
				else
					returnValue = defaultReturnValue;
			}
			catch( Exception e)
			{
				throw new Exception("SQL = " + sql + "\r\n\r\n" + e );
			}
			finally
			{
                rst?.Close();
                cmd.Dispose();
				conn.Close();				
				conn.Dispose();
			}
			return returnValue;
		}

        /// <summary>Converts the SQL passed in into a DataSet and returns the DataTable found, otherwise null is returned.</summary>
        /// <param name="sql">The sql to return. While it doesn't matter if the SQL actually only returns one resultset of values, it is recommended the SQL be tailored to do so.</param>
        /// <param name="connectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        /// <returns>A DataTable object representing the SQL statement or null</returns>
        public static DataTable ToDataTable(string sql, string connectionName)
        {
            var ds = ToDataSet(sql, connectionName);
            if (ds.Tables.Count < 1)
                return null;
            if (ds.Tables[0].Rows.Count < 1)
                return null;
            return ds.Tables[0];
        }

        /// <summary>Converts the SQL passed in into a DataSet and returns the DataTable found, otherwise null is returned.</summary>
        /// <param name="sql">The sql to return. While it doesn't matter if the SQL actually only returns one resultset of values, it is recommended the SQL be tailored to do so.</param>
        /// <returns>A DataTable object representing the SQL statement or null</returns>
        public static DataTable ToDataTable(string sql)
        {
            var ds = ToDataSet(sql);
            if (ds.Tables.Count < 1)
                return null;
            if (ds.Tables[0].Rows.Count < 1)
                return null;
            return ds.Tables[0];
        }

        /// <summary>Converts the SQL passed in into a DataSet and returns the DataTable found, otherwise null is returned.</summary>
        /// <param name="sql">The sql to return. While it doesn't matter if the SQL actually only returns one resultset of values, it is recommended the SQL be tailored to do so.</param>
        /// <param name="connectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        /// <param name="toFill">The DataTable to fill (could be a strongly typed DataTable)</param>
        /// <returns>A DataTable object representing the SQL statement or null</returns>
        public static DataTable ToDataTable(string sql, string connectionName, DataTable toFill)
        {
            var conn = GetConnection(connectionName);
            var cmd = new SqlCommand(sql, conn);
            toFill ??= new DataTable();
            try
            {
                var ra = new SqlDataAdapter(cmd);
                
                ra.Fill(toFill);
            }
            catch (Exception e)
            {
                throw new Exception("SQL = " + sql + "\r\n\r\n" + e);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }

            return toFill.Rows.Count < 1 ? null : toFill;
        }

        /// <summary>Converts the SQL passed in into a DataSet and returns the DataTable found, otherwise null is returned.</summary>
        /// <param name="sql">The sql to return. While it doesn't matter if the SQL actually only returns one resultset of values, it is recommended the SQL be tailored to do so.</param>
        /// <param name="toFill">The DataTable to fill (could be a strongly typed DataTable)</param>
        /// <returns>A DataTable object representing the SQL statement or null</returns>
        public static DataTable ToDataTable(string sql, DataTable toFill)
        {
            return ToDataTable(sql, null, toFill);
        }

        /// <summary>Converts the SQL passed in into a DataSet and returns the DataRowCollection inside the first DataTable found, otherwise null is returned.</summary>
        /// <param name="sql">The sql to return. While it doesn't matter if the SQL actually only returns one resultset of values, it is recommended the SQL be tailored to do so.</param>
        /// <param name="connectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        /// <returns>A DataRowCollection object representing the SQL statement or null</returns>
        public static DataRowCollection ToDataRows(string sql, string connectionName)
		{
            var ds = ToDataSet(sql, connectionName);
			if (ds.Tables.Count <  1)
				return null;
            if (ds.Tables[0].Rows.Count <  1)
                return null;
            return ds.Tables[0].Rows;
		}

        /// <summary>Converts the SQL passed in into a DataSet and returns the DataRowCollection inside the first DataTable found, otherwise null is returned.</summary>
        /// <param name="sql">The sql to return. While it doesn't matter if the SQL actually only returns one resultset of values, it is recommended the SQL be tailored to do so.</param>
        /// <returns>A DataRowCollection object representing the SQL statement or null</returns>
        public static DataRowCollection ToDataRows(string sql)
		{
			var ds = ToDataSet(sql);
			if (ds.Tables.Count <  1)
				return null;
            if (ds.Tables[0].Rows.Count <  1)
                return null;
            return ds.Tables[0].Rows;
		}

        /// <summary>Converts the SQL passed in into a DataSet and returns the first DataRow inside the first DataTable found, otherwise null is returned.</summary>
        /// <param name="sql">The sql to return. While it doesn't matter if the SQL actually only returns one DataRow of values, it is recommended the SQL be tailored to do so.</param>
        /// <param name="connectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        /// <returns>A DataRow object representing the SQL statement or null</returns>
        public static DataRow ToDataRow(string sql, string connectionName)
		{
            var ds = ToDataSet(sql, connectionName);
			if (ds.Tables.Count <  1)
				return null;
            if (ds.Tables[0].Rows.Count <  1)
                return null;
            return ds.Tables[0].Rows[0];
		}

        /// <summary>Converts the SQL passed in into a DataSet and returns the first DataRow inside the first DataTable found, otherwise null is returned.</summary>
        /// <param name="sql">The sql to return. While it doesn't matter if the SQL actually only returns one DataRow of values, it is recommended the SQL be tailored to do so.</param>
        /// <returns>A DataRow object representing the SQL statement or null</returns>
        public static DataRow ToDataRow(string sql)
		{
			var ds = ToDataSet(sql);
			if (ds.Tables.Count <  1)
				return null;
            if (ds.Tables[0].Rows.Count <  1)
                return null;
            return ds.Tables[0].Rows[0];
		}

		/// <summary>Updates a field in the Notification table for a single NotificationID</summary>
		/// <param name="notificationId">The NotificationID to update</param>
		/// <param name="fieldName">The field to update</param>
		/// <param name="newValue">The new value of the field</param>
		/// <param name="maxLength">If the field has a maximum length, specify it and this function will insure the field does not exceed that length</param>
		/// <returns>True if the record was updated</returns>
		public static bool UpdateNotificationField(string notificationId, string fieldName, string newValue, int maxLength)
		{
            var conn = DefaultConnection;
			var cmd = new SqlCommand("SET ARITHABORT ON", conn);
			cmd.ExecuteNonQuery();
			cmd.CommandText = "UPDATE Notification SET " + fieldName + "='" + newValue.Substring(0, maxLength) + "' WHERE NotificationID='" + notificationId + "'";
			var recordCount = cmd.ExecuteNonQuery();
			cmd.Dispose();
			conn.Close();
			conn.Dispose();
			return recordCount >  0;
		}
		
        /// <summary>Executes a series of SQL statements on the same connection</summary>
        /// <param name="sqlArray">The list of SQLs to execute</param>
        /// <param name="connectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        public static void Execute(List<string> sqlArray, string connectionName)
		{
			if (sqlArray.Count >  0)
			{
                var conn = GetConnection(connectionName);
				var cmd = new SqlCommand();
                cmd.Connection = conn;
				var errorAtSql = - 1;
				foreach (var sql in sqlArray)
				{
					try
					{
						errorAtSql = 1;
						cmd.CommandText = "SET ARITHABORT ON";
						cmd.ExecuteNonQuery();
						cmd.CommandText = sql;
						cmd.ExecuteNonQuery();
					}
					catch (Exception e)
					{
						conn.Close();
						var sb = new StringBuilder();
						int currSql;
						sb.Append("Exception while executing SQL # " + errorAtSql + ": " + sql + "\r\n\r\n" + e + "\r\n");
						sb.Append("\r\n------------------------------------------------------------------------\r\n");
						for (currSql=0; currSql <= sqlArray.Count - 1; currSql++)
							sb.Append(currSql + ": " + sqlArray[currSql] + "\r\n\r\n");
						cmd.Dispose();
						conn.Dispose();
						throw new Exception(sb.ToString());
					}
				}
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}
		}

        /// <summary>Executes a series of SQL statements on the same Default connection</summary>
        /// <param name="sqlArray">The list of SQLs to execute</param>
        public static void Execute(List<string> sqlArray)
		{
            Execute(sqlArray, DefaultConnectionString);
		}

        /// <summary>Executes a SQL statement</summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="connectionName">The name of the connection to use. If that connection doesn't exist, the "Default" connection is used.</param>
        /// <returns>The number of records affected</returns>
        public static int Execute(string sql, string connectionName)
		{
			var recordCount = 0;
            if (sql != null && sql.Length > 0)
            {
                var conn = GetConnection(connectionName);
                var cmd = new SqlCommand("SET ARITHABORT ON", conn);
                try
                {
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = sql;
                    recordCount = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new Exception("SQL = " + sql + "\r\n\r\n" + e);
                }
                finally
                {
                    cmd.Dispose();
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            }
			return recordCount;
		}

        /// <summary>Executes a SQL statement</summary>
        /// <param name="sql">The SQL to execute</param>
        /// <returns>The number of records affected</returns>
        public static int Execute(string sql)
		{
            return Execute(sql, DefaultConnectionString);
		}
		

        /// <summary>Returns a SqlConnection object given the connection name from Web.config. If that entry is blank or missing, Default.SqlClient is used.</summary>
        /// <param name="connectionName">The Web.config key name containing the connection string</param>
        /// <returns>An open SqlConnection object to the appropriate database (remember to close it)</returns>
        public static SqlConnection GetConnection(string connectionName)
        {
            var ret = new SqlConnection(GetConnectionString(connectionName));
            ret.Open();
            return ret;
        }

        /// <summary>Returns the Web.config connection string named. If that entry is blank or missing, Default.SqlClient is returned.</summary>
        /// <param name="connectionName">The Web.config key name</param>
        /// <returns>A connection string</returns>
        public static string GetConnectionString(string connectionName)
        {
            if (connectionName.IsEmpty())
                return DefaultConnectionString;
            var settings = ConfigurationManager.ConnectionStrings[connectionName];
            var connectionString = settings?.ConnectionString;
            if (connectionString.IsEmpty())
                return DefaultConnectionString;
            return connectionString;
        }

        /// <summary>Returns the value of the Web.config key "Default.SqlClient"</summary>
        public static string DefaultConnectionString => ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

        /// <summary>Returns a SqlClonnetion object using DefaultConnectionString</summary>
        public static SqlConnection DefaultConnection
        {
            get
            {
                var ret = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
                ret.Open();
                return ret;
            }
        }
    }
}
