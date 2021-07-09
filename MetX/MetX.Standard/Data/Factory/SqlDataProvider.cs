using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using MetX.Standard.Library;
using MetX.Standard.Metadata;

//using Microsoft.VisualBasic;

namespace MetX.Standard.Data.Factory
{
    /// <summary>C#CD: </summary>
    public class SqlDataProvider : DataProvider
    {
        public SqlDataProvider(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>C#CD: </summary>
        /// <param name="qry">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override int ExecuteQuery(QueryCommand qry)
        {
            using var conn = new SqlConnection(ConnectionString);
            
            var cmd = new SqlCommand(qry.CommandSql);
            AddParams(cmd, qry);
            cmd.Connection = conn;
            conn.Open();
            var ret = cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
            cmd.Dispose();
            return ret;
        }

        /// <summary>C#CD: </summary>
        /// <param name="qry">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override object ExecuteScalar(QueryCommand qry)
        {
            using var conn = new SqlConnection(ConnectionString);
            
            var cmd = new SqlCommand(qry.CommandSql);
            AddParams(cmd, qry);
            cmd.Connection = conn;
            conn.Open();
            var ret = cmd.ExecuteScalar();
            conn.Close();
            conn.Dispose();
            cmd.Dispose();
            return ret;
        }

        /// <summary>C#CD: </summary>
        /// <param name="qry">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override IDbCommand GetCommand(QueryCommand qry)
        {
            var cmd = new SqlCommand(qry.CommandSql);
            AddParams(cmd, qry);
            return cmd;
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
        /// <param name="fkColumnName">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override string GetForeignKeyTableName(string fkColumnName)
        {
            var cmd = new QueryCommand(GetTableSql);
            cmd.AddParameter("@columnName", fkColumnName);

            var result = ExecuteScalar(cmd);
            return result.ToString();
        }

        /// <summary>C#CD: </summary>
        /// <param name="qry">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override IDataReader GetReader(QueryCommand qry)
        {
            var conn = new SqlConnection(ConnectionString);
            conn.Open();
            var cmd = new SqlCommand(qry.CommandSql, conn) { CommandType = qry.CommandType };
            AddParams(cmd, qry);
            IDataReader ret;
            try
            {
                ret = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw new Exception(qry.ToXml() + "\r\n\r\n" + ex);
            }
            return ret;
        }

        /// <summary>C#CD: </summary>
        /// <returns>C#CD: </returns>
        public override StoredProcedure[] GetStoredProcedureList()
        {
            var cmd = new QueryCommand(StoredProceduresSql);
            var list = new List<StoredProcedure>();
            using var reader = GetReader(cmd);
            while (reader.Read())
            {
                // ROUTINE_SCHEMA, ROUTINE_NAME, ROUTINE_DEFINITION, ROUTINE_BODY
                var schema = reader["ROUTINE_SCHEMA"].AsString();
                var name = reader["ROUTINE_NAME"].AsString();
                var definition = reader["ROUTINE_DEFINITION"].AsString();
                var body = reader["ROUTINE_BODY"].AsString();

                var storedProcedure = new StoredProcedure
                {
                    StoredProcedureName = name,
                    SchemaName = schema,
                    Definition = definition,
                    Body = body,
                };

                list.Add(storedProcedure);
            }
            reader.Close();
            reader.Dispose();
            return list.ToArray();
        }

        /// <summary>C#CD: </summary>
        /// <param name="spName">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override IDataReader GetSpParams(string spName)
        {
            var cmd = new QueryCommand(SpParamSql);
            cmd.AddParameter("@spName", spName);
            return GetReader(cmd);
        }

        /// <summary>C#CD: </summary>
        /// <param name="qry">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override StoredProcedureResult GetStoredProcedureResult(QueryCommand qry)
        {
            var conn = new SqlConnection(ConnectionString);
            conn.Open();
            var cmd = new SqlCommand(qry.CommandSql, conn) {CommandType = qry.CommandType};
            AddParams(cmd, qry);
            if (!cmd.Parameters.Contains("@ReturnValue"))
            {
                var returnValue = new SqlParameter
                {
                    ParameterName = "@ReturnValue", Direction = ParameterDirection.ReturnValue
                };
                cmd.Parameters.Add(returnValue);
            }
            var ret = new StoredProcedureResult(cmd.Parameters);
            try
            {
                ret.Reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw new Exception(qry.ToXml() + "\r\n\r\n" + ex);
            }
            if (cmd.Parameters.Contains("@ReturnValue") && cmd.Parameters["@ReturnValue"].Value != null)
            {
                ret.ReturnValue = (int)cmd.Parameters["@ReturnValue"].Value;
            }
            return ret;
        }
        

        /// <summary>C#CD: </summary>
        /// <returns>C#CD: </returns>
        public override List<OwnerTablePair> GetTableList()
        {
            var result = new List<OwnerTablePair>();
            var cmd = new QueryCommand(TableSql);
            using var rdr = GetReader(cmd);
            while (rdr.Read())
            {
                var pair = new OwnerTablePair
                {
                    Owner = rdr["Owner"].AsString("dbo"),
                    TableName = rdr["Name"].AsString(),
                };
                result.Add(pair);
            }
            rdr.Close();
            rdr.Dispose();
            return result;
        }

        /// <summary>C#CD: </summary>
        /// <returns>C#CD: </returns>
        public override View[] GetViews()
        {
            var command = new QueryCommand(ViewsSql);
            var views = new List<View>();
            using var reader = GetReader(command);
            while (reader.Read())
            {
                // schema_name, view_name, definition
                views.Add(new View
                {
                    Schema = reader["schema_name"].AsString(),
                    ViewName = reader["view_name"].AsString(),
                    TSQL = reader["definition"].AsString()
                });
            }
            reader.Close();
            return views.ToArray();
        }

        /// <summary>C#CD: </summary>
        /// <param name="tableName">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override TableSchema.Table GetTableSchema(OwnerTablePair pair)
        {
            string schemaName = null;
            TableSchema.Table tableSchema;
            var columns = new TableSchema.TableColumnCollection();

            var command = new QueryCommand(TableColumnSqlWithDescription + ";" + IndexSql);
            command.AddParameter("@ownerName", pair.Owner);
            command.AddParameter("@tblName", pair.TableName);
            using (var reader = GetReader(command))
            {
                //get information about both the table and it's columns

                TableSchema.TableColumn column;
                while (reader.Read())
                {
                    int.TryParse(reader["MaxLength"].ToString(), out var maxLength);
                    column = new TableSchema.TableColumn
                    {
                        ColumnName = reader["ColumnName"].ToString(),
                        Description = reader["Description"].ToString(),
                        DataType = GetDbType((reader["DataType"].ToString() ?? string.Empty).ToLower()),
                        SourceType = reader["DataType"].ToString(),
                        AutoIncrement = reader["isIdentity"].ToString() == "1",
                        IsNullable = reader["IsNullable"].ToString() == "YES",
                        DomainName = reader["DomainName"].ToString(),
                        Precision = ToInt(reader["Precision"]),
                        Scale = ToInt(reader["Scale"]),
                        MaxLength = maxLength,
                    };
                    columns.Add(column);
                    //schemaName ??= reader["Owner"].ToString();
                }
                reader.NextResult();

                tableSchema = new TableSchema.Table(pair.TableName, pair.Owner);

                while (reader.Read())
                {
                    var columnName = reader["ColumnName"].ToString();
                    var constraintType = reader["constraintType"].ToString();
                    column = columns.GetColumn(columnName);

                    if (constraintType == "PRIMARY KEY")
                    {
                        column.IsPrimaryKey = true;
                    }
                    else if (constraintType == "FOREIGN KEY")
                    {
                        column.IsForeignKey = true;
                    }
                }
            }

            command = new QueryCommand(CompleteIndexInfoSql);
            command.AddParameter("@ownerName", pair.Owner);
            command.AddParameter("@tblName", pair.TableName);

            using (var rdr = GetReader(command))
            {
                if (rdr.Read())
                {
                    var currIndex = new TableSchema.TableIndex(rdr);
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

                    foreach (var currItem in tableSchema.Indexes)
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

            using (var rdr = GetReader(new QueryCommand("sp_help '" + tableSchema.FullName + "'")))
            {
                rdr.NextResult();
                rdr.NextResult();
                rdr.NextResult();
                rdr.NextResult();
                rdr.NextResult();
                rdr.NextResult();
                while (rdr.Read())
                {
                    var constraintType = rdr["constraint_type"].ToString();
                    if (constraintType.Contains("DEFAULT")) { continue; }

                    if (constraintType.Contains("PRIMARY"))
                    {
                        if (tableSchema.Keys.Find("Primary") != null)
                        {
                            continue;
                        }
                        var raw = rdr["constraint_keys"].ToString().Replace("(-)", string.Empty);

                        /*
                        new List<TableSchema.TableKeyColumn>(Strings
                            .Split(raw, ", ")
                            .Where(col => col.Length > 0)
                            .Select(col => new TableSchema.TableKeyColumn(col.Trim(), null))),
                            */

                        var keyColumns = raw
                            .AllTokens(", ")
                            .Where(col => col.Length > 0)
                            .Select(col => new TableSchema.TableKeyColumn(col.Trim(), null))
                            .ToList();
                        
                        tableSchema.Keys.Add(new TableSchema.TableKey
                        {
                            Name = "Primary",
                            IsPrimary = true,
                            Columns = keyColumns,
                        });
                    }
                    else if (constraintType.Contains("FOREIGN"))
                    {
                        var raw = rdr["constraint_keys"].ToString();
                        var name = rdr["constraint_name"].ToString();
                        var key = new TableSchema.TableKey { Name = name, IsForeign = true };
                        if (key.Name == string.Empty)
                            key.Name = "Foreign" + (tableSchema.Keys.Count + 1);
                        key.Columns.AddRange(
                            raw
                                .Replace("(-)", string.Empty)
                                .Split(',')
                                .Where(col => col.Length > 0)
                                .Select(col => new TableSchema.TableKeyColumn(col.Trim(), null)));
                        tableSchema.Keys.Add(key);

                        rdr.Read();
                        raw = rdr["constraint_keys"].ToString();
                        var relatedTableName = raw.FirstToken("(").LastToken(".").Trim();
                        if (relatedTableName == string.Empty)
                            relatedTableName = "Reference" + (tableSchema.Keys.Count + 1);

                        if (tableSchema.Keys.Find(relatedTableName) != null)
                        {
                            continue;
                        }

                        tableSchema.Keys.Add(new TableSchema.TableKey
                        {
                            Name = relatedTableName,
                            IsReference = raw.Contains("REFERENCE"),
                            Columns = new List<TableSchema.TableKeyColumn>(raw.TokensAfterFirst("(")
                                .Replace("(-)", string.Empty)
                                .Split(',')
                                .Where(col => col.Length > 0)
                                .Select(col => new TableSchema.TableKeyColumn(col.Trim(), null))),
                        });
                    }
                }
            }
            return tableSchema;
        }

        public override string HandlePage(string query, int offset, int limit, QueryType qType)
        {
            return "WITH U AS (" + query + ") SELECT * FROM U WHERE Row BETWEEN " + offset + " AND " + (offset + limit - 1);
        }

        public override IDbConnection NewConnection()
        {
            return new SqlConnection(ConnectionString);
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
            var conn = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand(sql, conn);
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
                var xr = cmd.ExecuteXmlReader();
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
                throw new Exception("SQL = " + sql + "\r\n\r\n" + e);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }

        public override string SelectStatement(string top, int page, QueryType qType)
        {
            if (page == 0 || qType != QueryType.Select)
            {
                return base.SelectStatement(top, page, qType);
            }
            return "SELECT TOP 100 PERCENT ROW_NUMBER() OVER ([orderByClause]) AS Row, ";
        }

        /// <summary>C#CD: </summary>
        /// <param name="qry">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override DataSet ToDataSet(QueryCommand qry)
        {
            using var conn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(qry.CommandSql, conn);
            using var da = new SqlDataAdapter(cmd);
            AddParams(cmd, qry);
            conn.Open();
            var ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            return ds;
        }

        /// <summary>Converts a SQL statement into a series of elements via SQLXML. If a "FOR XML" phrase is not found "FOR XML AUTO" is added to the SQL</summary>
        /// <param name="tagName">The element name to wrap the returned xml element(s). If null or blank, no tag wraps the returned xml string</param>
        /// <param name="tagAttributes">The attributes to add to the TagName element</param>
        /// <param name="sql">The SQL to convert to an xml string</param>
        /// <returns>The xml string attribute based representation of the SQL statement</returns>
        public override string ToXml(string tagName, string tagAttributes, string sql)
        {
            if (!sql.Contains("FOR XML")) sql += " FOR XML AUTO";

            var returnValue = new StringBuilder();
            var conn = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand(sql, conn);
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
                var xr = cmd.ExecuteXmlReader();
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

        private void AddParams(SqlCommand cmd, QueryCommand qry)
        {
            if (qry.Parameters == null)
            {
                return;
            }
            foreach (var param in qry.Parameters)
            {
                var sqlParam = new SqlParameter(param.ParameterName, param.DataType) {Direction = param.Direction};
                if (param.ParameterValue != null)
                {
                    sqlParam.Value = param.ParameterValue;
                }
                cmd.Parameters.Add(sqlParam);
            }
        }

        private static int ToInt(object target, int defaultValue = -1)
        {
            if (target == null || target == DBNull.Value) return defaultValue;
            switch (target)
            {
                case byte targetByte:
                    return targetByte;
                case int targetInt:
                    return targetInt;
                default:
                {
                    var toParse = target.ToString();
                    return int.TryParse(toParse, out var result) ? result : defaultValue;
                }
            }
        }

        #region Schema Bits

        private const string CompleteIndexInfoSql =
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

        private const string GetTableSql = "SELECT KCU.TABLE_NAME as TableName " +
                                             "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU  " +
                                             "JOIN  INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC  " +
                                             "ON KCU.CONSTRAINT_NAME=TC.CONSTRAINT_NAME " +
                                             "WHERE KCU.COLUMN_NAME=@columnName AND TC.CONSTRAINT_TYPE='PRIMARY KEY' ";

        private const string IndexSql = "SELECT " +
                                         "KCU.TABLE_NAME as TableName, " +
                                         "KCU.COLUMN_NAME as ColumnName, " +
                                         "TC.CONSTRAINT_TYPE as ConstraintType " +
                                         "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU " +
                                         "JOIN  INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC " +
                                         "ON KCU.CONSTRAINT_NAME=TC.CONSTRAINT_NAME " +
                                         "WHERE KCU.TABLE_NAME=@tblName";

        private const string SpParamSql = "SELECT  SPECIFIC_NAME AS SPName, ORDINAL_POSITION AS OrdinalPosition,  " +
                                            "PARAMETER_MODE AS ParamType, IS_RESULT AS IsResult, PARAMETER_NAME AS Name, DATA_TYPE AS DataType,  " +
                                            "CHARACTER_MAXIMUM_LENGTH AS DataLength, REPLACE(PARAMETER_NAME, '@', '') AS CleanName " +
                                            "FROM         INFORMATION_SCHEMA.PARAMETERS " +
                                            "WHERE SPECIFIC_NAME=@spName";

        //private const string StoredProceduresSql = " SELECT name FROM dbo.sysobjects WHERE (type = 'P')";
        private const string StoredProceduresSql = @"
SELECT ROUTINE_SCHEMA, ROUTINE_NAME, ROUTINE_DEFINITION, ROUTINE_BODY
  FROM INFORMATION_SCHEMA.ROUTINES
 WHERE ROUTINE_TYPE = 'PROCEDURE' 
   AND LEFT(ROUTINE_NAME, 3) NOT IN ('sp_', 'xp_', 'ms_')
";

        private const string TableColumnSqlWithDescription = @"
select
	TABLE_CATALOG AS [Database], TABLE_SCHEMA AS Owner, TABLE_NAME AS TableName, COLUMN_NAME AS ColumnName,
	ORDINAL_POSITION AS OrdinalPosition, COLUMN_DEFAULT AS DefaultSetting, info.IS_NULLABLE AS IsNullable, DATA_TYPE AS DataType, DOMAIN_NAME As DomainName,
	CHARACTER_MAXIMUM_LENGTH AS MaxLength, DATETIME_PRECISION AS DatePrecision,COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') as IsIdentity,
	NUMERIC_PRECISION As [Precision], NUMERIC_SCALE As Scale,         
	sep.value As [DESCRIPTION]
    from sys.tables st
    inner join sys.columns sc on st.object_id = sc.object_id
    left join sys.extended_properties sep on st.object_id = sep.major_id
                                         and sc.column_id = sep.minor_id
                                         and sep.name = 'MS_Description'
	left join INFORMATION_SCHEMA.COLUMNS info on info.TABLE_NAME = st.name 
										and info.COLUMN_NAME = sc.Name
    WHERE TABLE_SCHEMA = @ownerName and info.TABLE_NAME = @tblName
";

        private const string TableSql = "SELECT     TABLE_CATALOG AS [Database], TABLE_SCHEMA AS Owner, TABLE_NAME AS Name, TABLE_TYPE " +
                                         "FROM         INFORMATION_SCHEMA.TABLES " +
                                         "WHERE     (TABLE_TYPE = 'BASE TABLE') AND (TABLE_NAME <> N'sysdiagrams') " +
                                         "ORDER BY TABLE_SCHEMA, TABLE_NAME";

        private const string ViewsSql = 
@"select schema_name(v.schema_id) as schema_name, v.name as view_name, m.definition
from sys.views v 
    join sys.sql_modules m 
		on m.object_id = v.object_id
 order by schema_name, view_name; ";

        #endregion Schema Bits
    }
}