using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using MetX.Library;
using Microsoft.VisualBasic;

namespace MetX.Data
{
    /// <summary>C#CD: </summary>
    public class SqlDataProvider : DataProvider
    {
        public SqlDataProvider(string connectionString) { this.connectionString = connectionString; }
        public override IDbConnection NewConnection() { return new SqlConnection(connectionString); }

        public override string SelectStatement(string top, int page, QueryType qType)
        {
            if (page == 0 || qType != QueryType.Select)
            {
                return base.SelectStatement(top, page, qType);
            }
            return "SELECT TOP 100 PERCENT ROW_NUMBER() OVER ([orderByClause]) AS Row, ";
        }

        public override string HandlePage(string query, int offset, int limit, QueryType qType)
        {
            return "WITH U AS (" + query + ") SELECT * FROM U WHERE Row BETWEEN " + offset + " AND " + (offset + limit - 1);
        }

        /// <summary>Converts a SQL statement into a series of elements via SQLXML. If a "FOR XML" phrase is not found "FOR XML AUTO" is added to the SQL</summary>
        /// <param name="tagName">The element name to wrap the returned xml element(s). If null or blank, no tag wraps the returned xml string</param>
        /// <param name="tagAttributes">The attributes to add to the TagName element</param>
        /// <param name="sql">The SQL to convert to an xml string</param>
        /// <returns>The xml string attribute based representation of the SQL statement</returns>
        public override string ToXml(string tagName, string tagAttributes, string sql)
        {
            if (!sql.Contains("FOR XML")) sql += " FOR XML AUTO";

            StringBuilder returnValue = new StringBuilder();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            try
            {
                if (!string.IsNullOrEmpty(tagName))
                {
                    if (!string.IsNullOrEmpty(tagAttributes))
                    {
                        returnValue.AppendLine("<" + tagName + " " + tagAttributes + ">");
                    }
                    else
                    {
                        returnValue.AppendLine("<" + tagName + ">");
                    }
                }
                XmlReader xr = cmd.ExecuteXmlReader();
                xr.Read();
                while (xr.ReadState != ReadState.EndOfFile)
                {
                    returnValue.Append(xr.ReadOuterXml());
                }
                xr.Close();
                if (!string.IsNullOrEmpty(tagName))
                {
                    returnValue.AppendLine("</" + tagName + ">");
                }
            }
            catch (Exception e)
            {
                throw new Exception("SQL = " + sql + "\r\n\r\n" + e.ToString());
            } finally
            {
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }
            return returnValue.ToString();
        }

        /// <summary>Converts a SQL statement into a series of elements via SQLXML. If a "FOR XML" phrase is not found "FOR XML AUTO" is added to the SQL</summary>
        /// <param name="output">The StringBuilder to output xml into</param>
        /// <param name="tagName">The element name to wrap the returned xml element(s). If null or blank, no tag wraps the returned xml string</param>
        /// <param name="tagAttributes">The attributes to add to the TagName element</param>
        /// <param name="sql">The SQL to convert to an xml string</param>
        /// <returns>The xml string attribute based representation of the SQL statement</returns>
        public void OuterXml(StringBuilder output, string tagName, string tagAttributes, string sql)
        {
            if (sql.IndexOf("FOR XML", StringComparison.Ordinal) == -1)
            {
                sql += " FOR XML AUTO";
            }
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            try
            {
                if (!string.IsNullOrEmpty(tagName))
                {
                    if (!string.IsNullOrEmpty(tagAttributes))
                    {
                        output.AppendLine("<" + tagName + " " + tagAttributes + ">");
                    }
                    else
                    {
                        output.AppendLine("<" + tagName + ">");
                    }
                }
                XmlReader xr = cmd.ExecuteXmlReader();
                xr.Read();
                while (xr.ReadState != ReadState.EndOfFile)
                {
                    output.Append(xr.ReadOuterXml());
                }
                xr.Close();
                if (!string.IsNullOrEmpty(tagName))
                {
                    output.AppendLine("</" + tagName + ">");
                }
            }
            catch (Exception e)
            {
                throw new Exception("SQL = " + sql + "\r\n\r\n" + e.ToString());
            } finally
            {
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }

        private void AddParams(SqlCommand cmd, QueryCommand qry)
        {
            if (qry.Parameters == null)
            {
                return;
            }
            foreach (QueryParameter param in qry.Parameters)
            {
                SqlParameter sqlParam = new SqlParameter(param.ParameterName, param.DataType);
                sqlParam.Direction = param.Direction;
                if (param.ParameterValue != null)
                {
                    sqlParam.Value = param.ParameterValue;
                }
                cmd.Parameters.Add(sqlParam);
            }
        }

        /// <summary>C#CD: </summary>
        /// <param name="qry">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override IDataReader GetReader(QueryCommand qry)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(qry.CommandSql, conn) {CommandType = qry.CommandType};
            AddParams(cmd, qry);
            IDataReader ret = null;
            try
            {
                ret = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw new Exception(qry.ToXML() + "\r\n\r\n" + ex.ToString());
            }
            return ret;
        }

        /// <summary>C#CD: </summary>
        /// <param name="qry">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override StoredProcedureResult GetStoredProcedureResult(QueryCommand qry)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(qry.CommandSql, conn);
            cmd.CommandType = qry.CommandType;
            AddParams(cmd, qry);
            if (!cmd.Parameters.Contains("@ReturnValue"))
            {
                SqlParameter returnValue = new SqlParameter();
                returnValue.ParameterName = "@ReturnValue";
                returnValue.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(returnValue);
            }
            StoredProcedureResult ret = new StoredProcedureResult(cmd.Parameters);
            try
            {
                ret.Reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw new Exception(qry.ToXML() + "\r\n\r\n" + ex.ToString());
            }
            if (cmd.Parameters.Contains("@ReturnValue") && cmd.Parameters["@ReturnValue"].Value != null)
            {
                ret.ReturnValue = (int) cmd.Parameters["@ReturnValue"].Value;
            }
            return ret;
        }

        /// <summary>C#CD: </summary>
        /// <param name="qry">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override DataSet ToDataSet(QueryCommand qry)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(qry.CommandSql, conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        AddParams(cmd, qry);
                        conn.Open();
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows == null || ds.Tables[0].Rows.Count == 0)
                        {
                            return null;
                        }
                        return ds;
                    }
                }
            }
        }

        /// <summary>C#CD: </summary>
        /// <param name="qry">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override object ExecuteScalar(QueryCommand qry)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(qry.CommandSql);
                AddParams(cmd, qry);
                cmd.Connection = conn;
                conn.Open();
                object ret = cmd.ExecuteScalar();
                conn.Close();
                conn.Dispose();
                cmd.Dispose();
                return ret;
            }
        }

        /// <summary>C#CD: </summary>
        /// <param name="qry">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override int ExecuteQuery(QueryCommand qry)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(qry.CommandSql);
                AddParams(cmd, qry);
                cmd.Connection = conn;
                conn.Open();
                int ret = cmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();
                cmd.Dispose();
                return ret;
            }
        }

        /// <summary>C#CD: </summary>
        /// <param name="tableName">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override TableSchema.Table GetTableSchema(string tableName)
        {
            string schemaName = null;
            TableSchema.Table tableSchema = null;
            TableSchema.TableColumnCollection columns = new TableSchema.TableColumnCollection();

            QueryCommand cmd = new QueryCommand(TABLE_COLUMN_SQL + ";" + INDEX_SQL);
            cmd.AddParameter("@tblName", tableName);
            using (IDataReader rdr = GetReader(cmd))
            {
                //get information about both the table and it's columns

                TableSchema.TableColumn column;
                while (rdr.Read())
                {
                    int maxLength = 100;
                    int.TryParse(rdr["MaxLength"].ToString(), out maxLength);
                    column = new TableSchema.TableColumn
                    {
                        ColumnName = rdr["ColumnName"].ToString(),
                        DataType = GetDbType(rdr["DataType"].ToString().ToLower()),
                        SourceType = rdr["DataType"].ToString(),
                        AutoIncrement = rdr["isIdentity"].ToString() == "1",
                        IsNullable = rdr["IsNullable"].ToString() == "YES",
                        DomainName = rdr["DomainName"].ToString(),
                        MaxLength = maxLength,
                    };
                    columns.Add(column);
                    if (schemaName == null)
                    {
                        schemaName = rdr["Owner"].ToString();
                    }
                }
                rdr.NextResult();

                tableSchema = new TableSchema.Table(tableName, schemaName);

                string colName = string.Empty;
                string constraintType = string.Empty;

                while (rdr.Read())
                {
                    colName = rdr["ColumnName"].ToString();
                    constraintType = rdr["constraintType"].ToString();
                    column = columns.GetColumn(colName);

                    if (constraintType == "PRIMARY KEY")
                    {
                        column.IsPrimaryKey = true;
                    }
                    else if (constraintType == "FOREIGN KEY")
                    {
                        column.IsForiegnKey = true;
                    }
                }
            }

            cmd = new QueryCommand(COMPLETE_INDEX_INFO_SQL);
            cmd.AddParameter("@tblName", tableName);
            using (IDataReader rdr = GetReader(cmd))
            {
                if (rdr.Read())
                {
                    TableSchema.TableIndex currIndex = new TableSchema.TableIndex(rdr);
                    tableSchema.Indexes.Add(currIndex);
                    while (rdr.Read())
                    {
                        if (rdr["name"].ToString() != currIndex.Name)
                        {
                            currIndex = new TableSchema.TableIndex(rdr);
                            tableSchema.Indexes.Add(currIndex);
                        }
                        else
                        {
                            currIndex.Columns.Add(rdr["column"].ToString());
                        }
                    }

                    foreach (TableSchema.TableIndex currItem in tableSchema.Indexes)
                    {
                        if (currItem.Columns.Count == 1)
                        {
                            currItem.Name = currItem.Columns[0];
                            columns.GetColumn(currItem.Name).IsIndexed = true;
                        }
                    }
                }
                // Build up a list of indexes

                rdr.Close();
                rdr.Dispose();
            }
            tableSchema.Columns = columns;

            using (IDataReader rdr = GetReader(new QueryCommand("sp_help '" + tableSchema.FullName + "'")))
            {
                rdr.NextResult();
                rdr.NextResult();
                rdr.NextResult();
                rdr.NextResult();
                rdr.NextResult();
                rdr.NextResult();
                while (rdr.Read())
                {
                    string constraintType = rdr["constraint_type"].ToString();
                    if (constraintType.Contains("DEFAULT")){continue;}

                    TableSchema.TableKey key = null;
                    if (constraintType.Contains("PRIMARY"))
                    {
                        if (tableSchema.Keys.Find("Primary") != null)
                        {
                            continue;
                        }
                        key = new TableSchema.TableKey();
                        key.Name = "Primary";
                        key.IsPrimary = true;
                        foreach (string currCol in Strings.Split(rdr["constraint_keys"].ToString(), ", ", -1, CompareMethod.Text))
                        {
                            if (currCol.Length > 0)
                            {
                                key.Columns.Add(new TableSchema.TableKeyColumn(currCol.Trim(), null));
                            }
                        }
                        tableSchema.Keys.Add(key);
                        key = null;
                    }
                    else if (constraintType.Contains("FOREIGN"))
                    {
                        string raw = rdr["constraint_keys"].ToString();
                        key = new TableSchema.TableKey();
                        foreach (string currCol in raw.Split(new char[] {','}))
                        {
                            if (currCol.Length > 0)
                            {
                                key.Columns.Add(new TableSchema.TableKeyColumn(currCol.Trim(), null));
                            }
                        }
                        rdr.Read();
                        raw = rdr["constraint_keys"].ToString();
                        string relatedTableName = StringExtensions.FirstToken(StringExtensions.TokensAfter(raw, 1, "dbo."), "(").Trim();
                        if (tableSchema.Keys.Find(relatedTableName) == null)
                        {
                            key.Name = relatedTableName;
                            foreach (string currCol in StringExtensions.TokenBetween(raw, "(", ")").Split(new char[] {','}))
                            {
                                if (currCol.Length > 0)
                                {
                                    key.Columns.Add(new TableSchema.TableKeyColumn(currCol.Trim(), null));
                                }
                            }
                            tableSchema.Keys.Add(key);
                        }
                    }
                }
            }
            return tableSchema;
        }

        /// <summary>C#CD: </summary>
        /// <param name="spName">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override IDataReader GetSPParams(string spName)
        {
            QueryCommand cmd = new QueryCommand(SP_PARAM_SQL);
            cmd.AddParameter("@spName", spName);
            return GetReader(cmd);
        }

        /// <summary>C#CD: </summary>
        /// <returns>C#CD: </returns>
        public override string[] GetSPList()
        {
            QueryCommand cmd = new QueryCommand(SP_SQL);
            string sList = "";
            using (IDataReader rdr = GetReader(cmd))
            {
                while (rdr.Read())
                {
                    sList += rdr[0].ToString() + "|";
                }
                rdr.Close();
                rdr.Dispose();
                if (sList.Length > 0)
                {
                    sList = sList.Remove(sList.Length - 1, 1);
                }
                return sList.Split('|');
            }
        }

        /// <summary>C#CD: </summary>
        /// <returns>C#CD: </returns>
        public override string[] GetTableList()
        {
            QueryCommand cmd = new QueryCommand(TABLE_SQL);
            string sList = string.Empty;
            using (IDataReader rdr = GetReader(cmd))
            {
                while (rdr.Read())
                {
                    sList += rdr["Name"].ToString() + "|";
                }
                rdr.Close();
                rdr.Dispose();
                if (sList.Length > 0)
                {
                    sList = sList.Remove(sList.Length - 1, 1);
                }
            }
            string[] ret = sList.Split('|');
            Array.Sort<string>(ret);
            return ret;
        }

        /// <summary>C#CD: </summary>
        /// <param name="fkColumnName">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override string GetForeignKeyTableName(string fkColumnName)
        {
            QueryCommand cmd = new QueryCommand(GET_TABLE_SQL);
            cmd.AddParameter("@columnName", fkColumnName);

            object result = ExecuteScalar(cmd);
            return result.ToString();
        }

        /// <summary>C#CD: </summary>
        /// <param name="sqlType">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override DbType GetDbType(string sqlType)
        {
            switch (sqlType)
            {
                case "bigint":
                    return DbType.Int64;
                case "binary":
                    return DbType.Binary;
                case "bit":
                    return DbType.Boolean;
                case "char":
                    return DbType.AnsiStringFixedLength;
                case "datetime":
                    return DbType.DateTime;
                case "decimal":
                    return DbType.Decimal;
                case "float":
                    return DbType.Decimal;
                case "image":
                    return DbType.Binary;
                case "int":
                    return DbType.Int32;
                case "money":
                    return DbType.Currency;
                case "nchar":
                    return DbType.String;
                case "ntext":
                    return DbType.String;
                case "numeric":
                    return DbType.Decimal;
                case "nvarchar":
                    return DbType.String;
                case "real":
                    return DbType.Decimal;
                case "smalldatetime":
                    return DbType.DateTime;
                case "smallint":
                    return DbType.Int16;
                case "smallmoney":
                    return DbType.Currency;
                case "sql_variant":
                    return DbType.String;
                case "sysname":
                    return DbType.String;
                case "text":
                    return DbType.String;
                case "timestamp":
                    return DbType.Time;
                case "tinyint":
                    return DbType.Int16;
                case "uniqueidentifier":
                    return DbType.Guid;
                case "varbinary":
                    return DbType.Binary;
                case "varchar":
                    return DbType.String;
                default:
                    return DbType.String;
            }
        }

        /// <summary>C#CD: </summary>
        /// <param name="qry">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override IDbCommand GetCommand(QueryCommand qry)
        {
            SqlCommand cmd = new SqlCommand(qry.CommandSql);
            AddParams(cmd, qry);
            return cmd;
        }

        #region Schema Bits

        private const string TABLE_COLUMN_SQL = @"
SELECT TABLE_CATALOG AS [Database], TABLE_SCHEMA AS Owner, TABLE_NAME AS TableName, COLUMN_NAME AS ColumnName,
 ORDINAL_POSITION AS OrdinalPosition, COLUMN_DEFAULT AS DefaultSetting, IS_NULLABLE AS IsNullable, DATA_TYPE AS DataType, DOMAIN_NAME As DomainName, 
 CHARACTER_MAXIMUM_LENGTH AS MaxLength, DATETIME_PRECISION AS DatePrecision,COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') as IsIdentity 
 FROM         INFORMATION_SCHEMA.COLUMNS 
 WHERE     (TABLE_NAME = @tblName) ";

        private const string SP_PARAM_SQL = "SELECT  SPECIFIC_NAME AS SPName, ORDINAL_POSITION AS OrdinalPosition,  " +
                                            "PARAMETER_MODE AS ParamType, IS_RESULT AS IsResult, PARAMETER_NAME AS Name, DATA_TYPE AS DataType,  " +
                                            "CHARACTER_MAXIMUM_LENGTH AS DataLength, REPLACE(PARAMETER_NAME, '@', '') AS CleanName " +
                                            "FROM         INFORMATION_SCHEMA.PARAMETERS " +
                                            "WHERE SPECIFIC_NAME=@spName";

        private const string SP_SQL = "	SELECT  SPECIFIC_NAME AS SPName, ROUTINE_DEFINITION AS SQL, CREATED AS CreatedOn, " +
                                      "LAST_ALTERED AS ModifiedOn " +
                                      "FROM         INFORMATION_SCHEMA.ROUTINES " +
                                      "WHERE ROUTINE_TYPE='PROCEDURE'";

        private const string TABLE_SQL = "SELECT     TABLE_CATALOG AS [Database], TABLE_SCHEMA AS Owner, TABLE_NAME AS Name, TABLE_TYPE " +
                                         "FROM         INFORMATION_SCHEMA.TABLES " +
                                         "WHERE     (TABLE_TYPE = 'BASE TABLE') AND (TABLE_NAME <> N'sysdiagrams') " +
                                         "ORDER BY TABLE_NAME";

        private const string INDEX_SQL = "SELECT " +
                                         "KCU.TABLE_NAME as TableName, " +
                                         "KCU.COLUMN_NAME as ColumnName, " +
                                         "TC.CONSTRAINT_TYPE as ConstraintType " +
                                         "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU " +
                                         "JOIN  INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC " +
                                         "ON KCU.CONSTRAINT_NAME=TC.CONSTRAINT_NAME " +
                                         "WHERE KCU.TABLE_NAME=@tblName";

        private const string GET_TABLE_SQL = "SELECT KCU.TABLE_NAME as TableName " +
                                             "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU  " +
                                             "JOIN  INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC  " +
                                             "ON KCU.CONSTRAINT_NAME=TC.CONSTRAINT_NAME " +
                                             "WHERE KCU.COLUMN_NAME=@columnName AND TC.CONSTRAINT_TYPE='PRIMARY KEY' ";

        private const string COMPLETE_INDEX_INFO_SQL =
            "SELECT " +
            "[table] = object_name(i.id), i.name, " +
            "isclustered = indexproperty(i.id, i.name, 'IsClustered'), " +
            "[column] = col_name(i.id, ik.colid), ik.keyno " +
            "FROM " +
            "sysindexes i JOIN sysindexkeys ik " +
            "ON i.id = ik.id AND i.indid = ik.indid " +
            "WHERE " +
            "i.indid BETWEEN 1 AND 254 " +
            "AND indexproperty(i.id, name, 'IsHypothetical') = 0 " +
            "AND indexproperty(i.id, name, 'IsStatistics') = 0 " +
            "AND indexproperty(i.id, name, 'IsAutoStatistics') = 0 " +
            "AND objectproperty(i.id, 'IsMsShipped') = 0 " +
            "AND object_name(i.id)=@tblName " +
            "ORDER BY [table], [isclustered] DESC, i.name, ik.keyno ";

        #endregion
    }
}