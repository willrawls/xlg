using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Sybase.Data.AseClient;

namespace MetX.Data
{
    /// <summary>C#CD: </summary>
    public class SybaseDataProvider : DataProvider
    {
        public SybaseDataProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override IDbConnection NewConnection()
        {
            return new AseConnection(connectionString);
        }

        public override string ToXml(string TagName, string TagAttributes, string SQL)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string topStatement
        {
            get
            {
                return string.Empty;
            }
        }

        public override string commandSeparator
        {
            get
            {
                return " ";
            }
        }

        public override string selectStatement(string Top, int Page, QueryType qType)
        {
            if (!string.IsNullOrEmpty(Top))
                return "SET rowcount " + Top + " SELECT ";
            return "SELECT ";
        }

        public override string handlePage(string query, int offset, int limit, QueryType qType)
        {
            throw new Exception("Not currently supported.");
            // return "WITH U AS (" + query + ") SELECT * FROM U WHERE Row BETWEEN " + offset + " AND " + (offset + limit - 1);
        }



        public override string validIdentifier(string identifier)
        {
            if (identifier != null && identifier.IndexOf(" ") > 0)
                return "\"" + identifier + "\"";
            return identifier;
        }

        public void AddParams(AseCommand cmd, QueryCommand qry)
        {
            AseParameter sqlParam = null;
            if (qry.Parameters != null)
            {
                foreach (QueryParameter param in qry.Parameters)
                {
                    sqlParam = new AseParameter();
                    sqlParam.ParameterName = param.ParameterName;
                    sqlParam.DbType = param.DataType;
                    sqlParam.Direction = param.Direction;
                    if (param.ParameterValue != null)
                        sqlParam.Value = param.ParameterValue;
                    cmd.Parameters.Add(sqlParam);
                }
            }
        }

        /// <summary>C#CD: </summary>
        /// <param name="qry">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override System.Data.IDataReader GetReader(QueryCommand qry)
        {
            AseConnection conn = new AseConnection(connectionString);
            conn.Open();
            AseCommand cmd = new AseCommand(qry.CommandSql, conn);
            cmd.CommandTimeout = CommandTimeout;
            cmd.CommandType = qry.CommandType;
            AddParams(cmd, qry);
            System.Data.IDataReader ret = null;
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
            AseConnection conn = new AseConnection(connectionString);
            conn.Open();
            AseCommand cmd = new AseCommand(qry.CommandSql, conn);
            cmd.CommandTimeout = CommandTimeout;
            cmd.CommandType = qry.CommandType;
            AddParams(cmd, qry);
            StoredProcedureResult ret = new StoredProcedureResult(cmd.Parameters);
            try
            {
                ret.Reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
        public override System.Data.DataSet ToDataSet(QueryCommand qry)
        {
			using (AseConnection conn = new AseConnection(connectionString))
			using (AseCommand cmd = new AseCommand(qry.CommandSql, conn))
			using (AseDataAdapter da = new AseDataAdapter(cmd))
			{
				cmd.CommandTimeout = CommandTimeout;
				AddParams(cmd, qry);
				conn.Open();
				DataSet ds = new DataSet();
				da.Fill(ds);
				return ds;
			}
        }

        /// <summary>C#CD: </summary>
        /// <param name="qry">C#CD: </param>
        /// <returns>C#CD: </returns>
		public override object ExecuteScalar(QueryCommand qry)
		{
			using (AseConnection conn = new AseConnection(connectionString))
				using (AseCommand cmd = new AseCommand(qry.CommandSql, conn))
				{
					cmd.CommandTimeout = CommandTimeout;
					AddParams(cmd, qry);
					conn.Open();
					object ret = cmd.ExecuteScalar();
					return ret;
				}
		}

        /// <summary>C#CD: </summary>
        /// <param name="qry">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override int ExecuteQuery(QueryCommand qry)
        {
            using (AseConnection conn = new AseConnection(connectionString))
            {
                AseCommand cmd = new AseCommand(qry.CommandSql);
                cmd.CommandTimeout = CommandTimeout;
                AddParams(cmd, qry);
                cmd.Connection = conn;
                conn.Open();
                int ret = cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
                return ret;
            }
        }

        /// <summary>C#CD: </summary>
        /// <param name="tableName">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override TableSchema.Table GetTableSchema(string tableName)
        {
            TableSchema.TableColumn column;
            TableSchema.Table tbl = new TableSchema.Table(tableName);
            tbl.Columns = new TableSchema.TableColumnCollection();
            tbl.Keys = new TableSchema.TableKeyCollection();

            QueryCommand cmd = new QueryCommand(TABLE_COLUMN_SQL);
            cmd.AddParameter("@tblName", tableName);
            using (IDataReader rdr = GetReader(cmd))
            {
                //get information about both the table and it's columns

                while (rdr.Read())
                {
                    column = new TableSchema.TableColumn();
                    column.ColumnName = rdr["ColumnName"].ToString();
                    column.DataType = GetDbType(rdr["DataType"].ToString().ToLower());
                    int maxLength = 100;
                    int.TryParse(rdr["MaxLength"].ToString(), out maxLength);
                    column.MaxLength = maxLength;
                    column.IsNullable = rdr["IsNullable"].ToString() == "1";
                    column.IsIdentity = rdr["IsIdentity"].ToString() == "1";
					if(column.ColumnName.Length < 29)  // The version of Sybase driver we're using doesn't support columns with more than 28 characters in it... And I'm not allowed to upgrade the driver in production.
						tbl.Columns.Add(column);
                }
            }

            cmd = new QueryCommand(INDEX_SQL);
            cmd.AddParameter("@tblName", tableName);
            using (IDataReader rdr = GetReader(cmd))
            {
                string t1, t2, constraintType;
                string[] c1, c2;
                while (rdr.Read())
                {
                    t1 = rdr["object"].ToString();
                    t2 = rdr["related_object"].ToString();
                    c1 = Microsoft.VisualBasic.Strings.Split(
                        rdr["object_keys"].ToString().Replace(", *", string.Empty), 
                        ", ", -1, Microsoft.VisualBasic.CompareMethod.Text);
                    c2 = Microsoft.VisualBasic.Strings.Split(
                        rdr["related_keys"].ToString().Replace(", *", string.Empty),
                        ", ", -1, Microsoft.VisualBasic.CompareMethod.Text);
                    constraintType = rdr["keytype"].ToString();

                    if (t2 == tbl.Name)
                    {
                        string[] ta = c2; c2 = c1; c1 = ta;
                        string ts = t2; t2 = t1; t1 = ts;
                    }

                    TableSchema.TableKey key = new TableSchema.TableKey();
                    key.Name = (t2 == " -- none --" ? "Primary" : t2);
                    key.IsPrimary = (constraintType == "primary");
                    tbl.Keys.Add(key);
                    for (int i = 0; i < c1.Length; i++)
                    {
                        if (c1[i].Length > 0)
                        {
                            key.Columns.Add(new TableSchema.TableKeyColumn(c1[i], (t2 == " -- none --" ? null : c2[i])));
                            if (tbl.Columns.Contains(c1[i]))
                            {
                                column = tbl.Columns.GetColumn(c1[i]);
                                if (constraintType == "primary") column.IsPrimaryKey = true;
                                else if (constraintType == "foreign") column.IsForiegnKey = true;
                            }
                        }
                    }
                }
            }

            cmd = new QueryCommand("sp_helpconstraint [" + tableName + "]");
            try
            {
                using (IDataReader rdr = GetReader(cmd))
                {
                    string t1 = null, t2 = null, constraintType = null;
                    string[] c1 = new string[0], c2 = new string[0];
                    while (rdr.Read())
                    {
                        string raw = rdr["definition"].ToString();
                        if (raw.IndexOf("FOREIGN KEY") > -1)
                        {
                            t1 = Token.First(raw, " FOREIGN KEY");
                            t2 = Token.Between(raw, "REFERENCES ", "(").Trim();
                            c1 = Microsoft.VisualBasic.Strings.Split(
                                Token.Between(raw, "(", ")").Trim(),
                                ", ", -1, Microsoft.VisualBasic.CompareMethod.Text);
                            c2 = Microsoft.VisualBasic.Strings.Split(
                                Token.Get(Token.Get(raw, 3, "("), 1, ")").Trim(),
                                ", ", -1, Microsoft.VisualBasic.CompareMethod.Text);
                            constraintType = "Foreign";
                        }
                        else if (raw.IndexOf("KEY INDEX") > -1)
                        {
                            t1 = Token.First(raw, " KEY INDEX");
                            t2 = "Primary";
                            c1 = Microsoft.VisualBasic.Strings.Split(
                                Token.Between(raw, "(", ")").Trim(),
                                ", ", -1, Microsoft.VisualBasic.CompareMethod.Text);
                            c2 = new string[0];
                            constraintType = "Primary";
                        }
                        if (constraintType != null)
                        {
                            if (t2 == tbl.Name)
                            {
                                string[] ta = c2; c2 = c1; c1 = ta;
                                string ts = t2; t2 = t1; t1 = ts;
                            }

                            if (t2 != null && t2.Length > 0 && tbl.Keys.Find(t2) == null)
                            {
                                TableSchema.TableKey key = new TableSchema.TableKey();
                                key.IsPrimary = (constraintType == "primary");
                                key.Name = t2;
                                for (int i = 0; i < c1.Length; i++)
                                {
                                    if (c1[i].Length > 0)
                                    {
                                        key.Columns.Add(new TableSchema.TableKeyColumn(c1[i], (key.IsPrimary ? null : (c2.Length < i + 1 ? c1[i] : c2[i]))));
                                        if (tbl.Columns.Contains(c1[i]))
                                        {
                                            column = tbl.Columns.GetColumn(c1[i]);
                                            if (constraintType == "Primary") column.IsPrimaryKey = true;
                                            else if (constraintType == "Foreign") column.IsForiegnKey = true;
                                        }
                                    }
                                }
                                tbl.Keys.Add(key);
                            }
                        }
                    }
                }
            }
            catch //(Exception e)
            {
                // Console.WriteLine(e.ToString());
                cmd = null;
            }

            cmd = new QueryCommand(COMPLETE_INDEX_INFO_SQL);
            cmd.AddParameter("@tblName", tableName);
            using (IDataReader rdr = GetReader(cmd))
            {
                if (rdr.Read())
                {
                    TableSchema.TableIndex CurrIndex = new TableSchema.TableIndex();
                    CurrIndex.Name = rdr["index_name"].ToString().Trim();
                    CurrIndex.IsClustered = rdr["index_description"].ToString().Contains("clustered") && !rdr["index_description"].ToString().Contains("nonclustered");
                    foreach (string CurrColumn in rdr["index_keys"].ToString().Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries))
                        CurrIndex.Columns.Add(CurrColumn.Trim());
                    tbl.Indexes.Add(CurrIndex);

                    foreach(TableSchema.TableIndex CurrItem in tbl.Indexes)
                        if (CurrItem.Columns.Count == 1)
                        {
                            CurrItem.Name = CurrItem.Columns[0];
                            tbl.Columns.GetColumn(CurrItem.Name).IsIndexed = true;
                        }
                }
                // Build up a list of indexes

                rdr.Close();
                rdr.Dispose();
            }
            return tbl;
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
            using (IDataReader rdr = GetReader(cmd))
            {
                List<string> sList = new List<string>();
                while (rdr.Read())
                    sList.Add(rdr[0].ToString());
                rdr.Close();
                rdr.Dispose();
                return sList.ToArray();
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
                    sList += rdr["Name"].ToString() + "|";
                rdr.Close();
                rdr.Dispose();
                if (sList.Length > 0)
                    sList = sList.Remove(sList.Length - 1, 1);
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
            //QueryCommand cmd = new QueryCommand(GET_TABLE_SQL);
            //cmd.AddParameter("@columnName", fkColumnName);

            //object result = ExecuteScalar(cmd);
            //return result.ToString();
            return string.Empty;
        }
        /// <summary>C#CD: </summary>
        /// <param name="sqlType">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override DbType GetDbType(string sqlType)
        {
            switch (sqlType)
            {
                case "45": return DbType.Binary; //binary
                case "50": return DbType.Boolean; //bit
                case "47": return DbType.AnsiStringFixedLength; //char
                case "49": return DbType.Date; //date
                case "123": return DbType.Date; //daten
                case "61": return DbType.DateTime; //datetime
                case "111": return DbType.DateTime; //datetimn
                case "55": return DbType.Decimal; //decimal
                case "106": return DbType.Decimal; //decimaln
                case "36": return DbType.String; //extended type
                case "62": return DbType.Decimal; //float
                case "109": return DbType.Decimal; //floatn
                case "34": return DbType.Binary; //image
                case "56": return DbType.Int32; //int
                case "38": return DbType.Int32; //intn
                case "60": return DbType.Currency; //money
				case "110": return DbType.Currency; //moneyn
                //case "47": return DbType.String; //nchar
                case "63": return DbType.Decimal; //numeric
                case "108": return DbType.Decimal; //numericn
                case "39": return DbType.String; //nvarchar
                case "59": return DbType.Decimal; //real
                case "58": return DbType.DateTime; //smalldatetime
                case "52": return DbType.Int16; //smallint
                case "122": return DbType.Currency; //smallmoney
                //case "39": return DbType.String; //sysname
                case "35": return DbType.String; //text
                case "51": return DbType.Time; //time
                case "147": return DbType.Time; //timen
                case "37": return DbType.Time; //timestamp
                case "48": return DbType.Int16; //tinyint
                //case "47": return DbType.String; //udt_code_req
                //case "39": return DbType.String; //udt_description_req
                //case "56": return DbType.String; //udt_internal_surrogate_number
                //case "47": return DbType.String; //udt_yesno_req
                case "135": return DbType.AnsiStringFixedLength; //unichar
                case "155": return DbType.String; //univarchar
                //case "37": return DbType.Binary; //varbinary
                //case "39": return DbType.String; //varchar
                default: return DbType.String;
            }
        }

        /// <summary>C#CD: </summary>
        /// <param name="qry">C#CD: </param>
        /// <returns>C#CD: </returns>
        public override IDbCommand GetCommand(QueryCommand qry)
        {
            AseCommand cmd = new AseCommand(qry.CommandSql);
            cmd.CommandTimeout = CommandTimeout;
            AddParams(cmd, qry);
            return cmd;
        }

        #region Schema Bits
        const string TABLE_SQL = "SELECT name As Name FROM sysobjects WHERE type='U' ORDER BY name";
        const string TABLE_COLUMN_SQL = "SELECT syscolumns.name AS ColumnName, colid AS OrdinalPosition, \"IsNullable\" = case when status & 8 > 0 then 1 else 0 end, \"IsIdentity\" = case when status & 128 > 0 then 1 else 0 end, syscolumns.type AS DataType, length AS MaxLength FROM syscolumns JOIN sysobjects ON syscolumns.id=sysobjects.id WHERE sysobjects.name=@tblName";

        const string SP_PARAM_SQL = "SELECT syscolumns.name AS Name, colid AS OrdinalPosition, 0 As IsResult, status2 As ParamType, status AS IsNullable, syscolumns.type AS DataType, length AS DataLength, 0 As IsIdentity FROM syscolumns JOIN sysobjects ON syscolumns.id=sysobjects.id WHERE sysobjects.name=@spName";

        //const string SP_PARAM_SQL = "SELECT  SPECIFIC_NAME AS SPName, ORDINAL_POSITION AS OrdinalPosition,  " +
        //                  "PARAMETER_MODE AS ParamType, IS_RESULT AS IsResult, PARAMETER_NAME AS Name, DATA_TYPE AS DataType,  " +
        //                  "CHARACTER_MAXIMUM_LENGTH AS DataLength, REPLACE(PARAMETER_NAME, '@', '') AS CleanName " +
        //                  "FROM         INFORMATION_SCHEMA.PARAMETERS " +
        //                  "WHERE SPECIFIC_NAME=@spName";

        const string SP_SQL = "SELECT name As Name FROM sysobjects WHERE type='P'";

        const string INDEX_SQL = "sp_helpkey @tblName";

        //const string GET_TABLE_SQL = "SELECT KCU.TABLE_NAME as TableName " +
        //            "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU  " +
        //            "JOIN  INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC  " +
        //            "ON KCU.CONSTRAINT_NAME=TC.CONSTRAINT_NAME " +
        //            "WHERE KCU.COLUMN_NAME=@columnName AND TC.CONSTRAINT_TYPE='PRIMARY KEY' ";

        const string COMPLETE_INDEX_INFO_SQL = "sp_helpindex @tblName";
        #endregion
    }
}
