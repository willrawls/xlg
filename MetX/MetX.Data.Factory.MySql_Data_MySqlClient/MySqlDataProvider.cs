using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace MetX.Data
{
    public class MySqlDataProvider : DataProvider
    {
        public MySqlDataProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override IDbConnection NewConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public override string ToXml(string TagName, string TagAttributes, string SQL)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override StoredProcedureResult GetStoredProcedureResult(QueryCommand cmd)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string topStatement
        {
            get
            {
                return " LIMIT ";
            }
        }

        public override string selectStatement(string Top, int Page, QueryType qType)
        {
            if (Page == 0 || qType != QueryType.Select)
                return base.selectStatement(Top, Page, qType);
            return " SELECT ";
        }

        public void AddParams(MySqlCommand cmd, QueryCommand qry)
        {
            MySqlParameter sqlParam = null;
            if (qry.Parameters != null) {
                foreach (QueryParameter param in qry.Parameters) {
                    sqlParam = new MySqlParameter();
                    sqlParam.DbType = param.DataType;
                    sqlParam.ParameterName = param.ParameterName;
                    sqlParam.Value = param.ParameterValue;
                    sqlParam.Direction = param.Direction;
                    cmd.Parameters.Add(sqlParam);
                }
            }
        }

        public override IDbCommand GetCommand(QueryCommand qry) {
            MySqlCommand cmd = new MySqlCommand(qry.CommandSql);
            AddParams(cmd, qry);
            return cmd;
        }

        public override System.Data.IDataReader GetReader(QueryCommand qry) {

            MySqlConnection conn = new MySqlConnection(connectionString);

            MySqlCommand cmd = new MySqlCommand(qry.CommandSql);
            cmd.CommandType = qry.CommandType;
            AddParams(cmd, qry);

            cmd.Connection = conn;
            conn.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);

        }

        /*
        public override System.Data.DataSet GetDataSet(QueryCommand qry) {
            DataSet ds = new DataSet();
            MySqlCommand cmd = new MySqlCommand(qry.CommandSql);
            AddParams(cmd, qry);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            
            using (MySqlConnection conn = new MySqlConnection(connectionString)) {
                cmd.Connection = conn;
                conn.Open();
                da.Fill(ds);
                conn.Close();
                cmd.Dispose();
                da.Dispose();
                return ds;
            }

        }
        */

        public override object ExecuteScalar(QueryCommand qry) {
            using (MySqlConnection conn = new MySqlConnection(connectionString)) {
                MySqlCommand cmd = new MySqlCommand(qry.CommandSql);
                AddParams(cmd, qry);

                cmd.Connection = conn;
                conn.Open();
                return cmd.ExecuteScalar();

            }
        }

        public override int ExecuteQuery(QueryCommand qry) {
            using (MySqlConnection conn = new MySqlConnection(connectionString)) {
                MySqlCommand cmd = new MySqlCommand(qry.CommandSql);
                AddParams(cmd, qry);

                cmd.Connection = conn;
                conn.Open();
                return cmd.ExecuteNonQuery();

            }
        }
        
        public override TableSchema.Table GetTableSchema(string tableName) {
            TableSchema.TableColumnCollection columns = new TableSchema.TableColumnCollection();
            TableSchema.TableColumn column;
            TableSchema.Table table = new TableSchema.Table();
            table.Name = tableName;
            string sql = " DESCRIBE "+tableName;
            using (MySqlCommand cmd = new MySqlCommand(sql)) {
                //get information about both the table and it's columns
                MySqlConnection conn = new MySqlConnection(connectionString);
                cmd.Connection = conn;
                conn.Open();
                IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                string sType = "";
                string sSize = "";
                int size = 0;

                while (rdr.Read()) {
                    column = new TableSchema.TableColumn();
                    column.ColumnName = rdr["Field"].ToString();
                    sType = rdr["Type"].ToString();
                    if (sType.IndexOf("(") > 0) {
                        sSize = sType.Substring(sType.IndexOf("("), sType.Length - sType.IndexOf("(")).Replace(")", "").Replace("(", "");
                        sType = sType.Substring(0, sType.IndexOf("("));
                    } else {
                        sSize = "0";
                    }
                    int.TryParse(sSize,out size);
                    column.MaxLength = size;
                    column.DataType = GetDbType(sType);
                    //column.DataType = sType.Substring(0,sType.IndexOf("("));
                    column.AutoIncrement = rdr["Extra"].ToString() == "auto_increment";
                    column.IsPrimaryKey = rdr["Key"].ToString() == "PRI";
                    column.IsForiegnKey = rdr["Key"].ToString() == "MUL";
                    column.IsNullable = rdr["Default"].ToString().ToLower() != "null";
                    columns.Add(column);


                }
                
                rdr.Close();


            }
            table.Columns = columns;

            return table;
        }
        public override DbType GetDbType(string mySqlType) {
            switch (mySqlType.ToLower()) {
                case "bigint": return DbType.Int64;
                case "longblob": return DbType.Binary;
                case "longtext": return DbType.String;
                case "binary": return DbType.Binary;
                case "bit": return DbType.Boolean;
                case "char": return DbType.AnsiStringFixedLength;
                case "date": return DbType.DateTime;
                case "time": return DbType.DateTime;
                case "datetime": return DbType.DateTime;
                case "decimal": return DbType.Decimal;
                case "float": return DbType.Decimal;
                case "image": return DbType.Byte;
                case "int": return DbType.Int32;
                case "currency": return DbType.Currency;
                case "money": return DbType.Currency;
                case "nchar": return DbType.String;
                case "ntext": return DbType.String;
                case "numeric": return DbType.Decimal;
                case "nvarchar": return DbType.String;
                case "real": return DbType.Decimal;
                case "smalldatetime": return DbType.DateTime;
                case "smallint": return DbType.Int16;
                case "smallmoney": return DbType.Currency;
                case "sysname": return DbType.String;
                case "text": return DbType.String;
                case "timestamp": return DbType.DateTime;
                case "tinyint": return DbType.Boolean;
                case "uniqueidentifier": return DbType.Guid;
                case "varbinary": return DbType.Byte;
                case "varchar": return DbType.String;
                default: return DbType.String;
            }

        }
        public override string[] GetSPList() {

            //can't implement this currently - there's no way in MySQL to enumerate
            //the parameters for an SP
            string [] result=new string[0];
            return result;
        }
        public override IDataReader GetSPParams(string spName) {
            //can't implement this currently - there's no way in MySQL to enumerate
            //the parameters for an SP
            return null;
        }
        public override string[] GetTableList() {
            string sql="SHOW TABLES";
            string sList="";
            using (MySqlCommand cmd = new MySqlCommand(sql)) {
                //get information about both the table and it's columns
                MySqlConnection conn = new MySqlConnection(connectionString);
                cmd.Connection = conn;
                conn.Open();
                IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                
                while(rdr.Read())
                    sList += rdr[0].ToString()+"|";
                
                rdr.Close();
                sList = sList.Remove(sList.Length - 1, 1);

            }
			string[] ret = sList.Split('|');
			Array.Sort<string>(ret);
            return ret;
        }

        public override string GetForeignKeyTableName(string fkColumnName) {
            string sql="SELECT KCU.TABLE_NAME "+
                        "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU INNER JOIN "+
                        "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC ON KCU.CONSTRAINT_NAME = TC.CONSTRAINT_NAME "+
                        "WHERE KCU.COLUMN_NAME=@columnName "+
                        "AND TC.CONSTRAINT_TYPE='PRIMARY KEY' "+
                        "LIMIT 1 ";
            QueryCommand cmd = new QueryCommand(sql);
            cmd.AddParameter("@columnName", fkColumnName);
            object result=ExecuteScalar(cmd);
            string sOut="";
            if(result!=null)
                sOut=result.ToString();
            return sOut;
        }


        public override DataSet ToDataSet(QueryCommand cmd)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string handlePage(string query, int offset, int limit, QueryType qType)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
