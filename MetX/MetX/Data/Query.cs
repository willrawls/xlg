using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Collections;

namespace MetX.Data
{
    #region enums
    /// <summary>
    /// Enum for General SQL Functions
    /// </summary>
    public enum AggregateFunction
    {
        None,
        Count,
        Sum,
        Avg,
        Min,
        Max,
        StdDev,
        Var
    }

    public enum WhereOptions
    {
        And,
        AndWithBeginParen,
        //AndWithEndParen,
        Or,
        OrWithBeginParen,
        //OrWithEndParen,
		BeginParen,
		EndParen,
        None
    }

    /// <summary>
    /// SQL Comparison Operators
    /// </summary>
    public enum Comparison
    {
        Equals,
        NotEquals,
        Like,
        NotLike,
        GreaterThan,
        GreaterOrEquals,
        LessThan,
        LessOrEquals,
        Blank,
        Is,
        IsNot,
        In,
        NotIn
    }
    #endregion

    #region Support Classes
    public class Join
    {
        public string FromColumn;
        public string ToColumn;
        public string JoinTable;
        public string JoinType = "INNER JOIN";
    }

    /// <summary>
    /// Creates an aggregate function call for ANSI SQL
    /// </summary>
    public class Aggregate
    {
        public string AggregateString = string.Empty;
        public Aggregate() {  }
        public Aggregate(DataProvider Instance, AggregateFunction agg, string columnName, string alias)
        {
            AggregateString = Enum.GetName(typeof(AggregateFunction), agg).ToUpper() + "(" + Instance.validIdentifier(columnName) + ") as '" + alias + "'";
        }
        public static Aggregate New(DataProvider Instance, AggregateFunction agg, string columnName, string alias) { return new Aggregate(Instance, agg, columnName, alias); }
    }

    /// <summary>
    /// Creates a WHERE clause for a SQL Statement
    /// </summary>
    public class Where
    {
        public string TableName;
        public string ColumnName;
        public Comparison Comparison;
        public string ParameterName;
        public WhereOptions Options;

        private object m_ParameterValue;

        public Where() { }

        public Where(string tableName, string ColumnName, Comparison Comparison, object Value)
        {
            this.TableName = tableName;
            this.ColumnName = ColumnName;
            this.Comparison = Comparison;
            this.m_ParameterValue = Value;
        }

        public Where(string ColumnName, Comparison Comparison, object Value)
        {
            this.ColumnName = ColumnName;
            this.Comparison = Comparison;
            this.m_ParameterValue = Value;
        }

        public Where(string ColumnName, object Value)
        {
            this.ColumnName = ColumnName;
            this.Comparison = Comparison.Equals;
            this.m_ParameterValue = Value;
        }

        public Where(string tableName, string ColumnName, Comparison Comparison, object Value, WhereOptions Options)
        {
            this.TableName = tableName;
            this.ColumnName = ColumnName;
            this.Comparison = Comparison;
            this.m_ParameterValue = Value;
            this.Options = Options;
        }

		public Where(WhereOptions Options)
		{
			this.Options = Options;
		}

		public Where(string ColumnName, Comparison Comparison, object Value, WhereOptions Options)
        {
            this.ColumnName = ColumnName;
            this.Comparison = Comparison;
            this.m_ParameterValue = Value;
            this.Options = Options;
        }

        public Where(string ColumnName, object Value, WhereOptions Options)
        {
            this.ColumnName = ColumnName;
            this.Comparison = Comparison.Equals;
            this.m_ParameterValue = Value;
            this.Options = Options;
        }

        public object ParameterValue
        {
            get
            {
                if (Comparison != Comparison.In && Comparison != Comparison.NotIn)
                    return m_ParameterValue;
                else
                    return InPhrase(m_ParameterValue);
            }
        }

        public void AddValue(string ValueToAddToInClause)
        {
            List<string> pv;
            if (m_ParameterValue is Array)
            {
                Array t = (Array)m_ParameterValue;
                pv = new List<string>();
                m_ParameterValue = pv;
                foreach (object CurrValue in t)
                    pv.Add(CurrValue.ToString());
            }
            else
                pv = (List<string>)m_ParameterValue;
            pv.Add(ValueToAddToInClause);
        }

        public void AddRangeOfValues(IEnumerable<string> ValuesToAddToInClause)
        {
            List<string> pv;
            if (m_ParameterValue is Array)
            {
                Array t = (Array)m_ParameterValue;
                pv = new List<string>();
                m_ParameterValue = pv;
                foreach (object CurrValue in t)
                    pv.Add(CurrValue.ToString());
            }
            else
                pv = (List<string>)m_ParameterValue;
            pv.AddRange(ValuesToAddToInClause);
        }

        private string InPhrase(object Value)
        {
            StringBuilder sValue = new StringBuilder();
            if (Value == null)
                sValue.Append("(NULL");
            if (Value is string)
            {
                if (((string)Value).StartsWith("("))
                    return (string)Value;
                else
                {
                    sValue.Append("(");
                    if (!((string)Value).StartsWith("'"))
                    {
                        sValue.Append("'");
                        sValue.Append(((string)Value).Replace("'", "''"));
                        sValue.Append("'");
                    }
                    else
                        sValue.Append((string)Value);
                }
            }
            else if (Value is string[])
            {
                foreach (string CurrValue in (string[])Value)
                {
                    if (CurrValue != null && CurrValue.Length > 0)
                    {
                        if (sValue.Length > 0) sValue.Append(","); else sValue.Append("(");
                        sValue.Append("'");
                        sValue.Append(CurrValue.Replace("'", "''"));
                        sValue.Append("'");
                    }
                }
            }
            else if (Value is int[])
            {
                foreach (int CurrValue in (int[])Value)
                {
                    if (sValue.Length > 0) sValue.Append(","); else sValue.Append("(");
                    sValue.Append(CurrValue.ToString());
                }
            }
            else if (Value is double[])
            {
                foreach (double CurrValue in (double[])Value)
                {
                    if (sValue.Length > 0) sValue.Append(","); else sValue.Append("(");
                    sValue.Append(CurrValue.ToString());
                }
            }
            else if (Value is Array)
            {
                foreach (object CurrValue in (Array)Value)
                {
                    if (CurrValue != null)
                    {
                        if (sValue.Length > 0) sValue.Append(","); else sValue.Append("(");
                        sValue.Append("'");
                        sValue.Append(CurrValue.ToString().Replace("'", "''"));
                        sValue.Append("'");
                    }
                }
            }
            else if (Value is List<string>)
            {
                foreach (string CurrValue in (List<string>)Value)
                {
                    if (CurrValue != null && CurrValue.Length > 0)
                    {
                        if (sValue.Length > 0) sValue.Append(","); else sValue.Append("(");
                        sValue.Append("'");
                        sValue.Append(CurrValue.Replace("'", "''"));
                        sValue.Append("'");
                    }
                }
            }
            else
            {
                sValue.Append("(");
                sValue.Append("'");
                sValue.Append(Value.ToString().Replace("'", "''"));
                sValue.Append("'");
            }
            sValue.Append(")");
            return sValue.ToString();
        }
    }

    /// <summary>
    /// Creates an ORDER BY statement for ANSI SQL
    /// </summary>
    public class OrderBy
    {
        public string OrderString;

        public OrderBy() { }
        public OrderBy(string orderString) { this.OrderString = orderString; }
        public static OrderBy Desc(DataProvider Instance, string columnName) { return new OrderBy(" ORDER BY " + Instance.validIdentifier(columnName) + " DESC"); }
        public static OrderBy Asc(DataProvider Instance, string columnName) { return new OrderBy(" ORDER BY " + Instance.validIdentifier(columnName)); }
        public static OrderBy Any(DataProvider Instance, string orderString) { return new OrderBy(orderString); }
    }
    #endregion

    /// <summary>
    /// Creates a SQL Statement and SQL Commands
    /// </summary>
    public class Query
    {
        /// <summary>
        /// Takes the enum value and returns the proper SQL 
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        string GetComparisonOperator(Comparison comp)
        {
            string sOut = "=";
            switch (comp)
            {
                case Comparison.Blank:
                    sOut = " ";
                    break;
                case Comparison.GreaterThan:
                    sOut = ">";
                    break;
                case Comparison.GreaterOrEquals:
                    sOut = ">=";
                    break;
                case Comparison.LessThan:
                    sOut = "<";
                    break;
                case Comparison.LessOrEquals:
                    sOut = "<=";
                    break;
                case Comparison.Like:
                    sOut = " LIKE ";
                    break;
                case Comparison.NotEquals:
                    sOut = " <> ";
                    break;
                case Comparison.NotLike:
                    sOut = " NOT LIKE ";
                    break;
                case Comparison.Is:
                    sOut = " IS ";
                    break;
                case Comparison.IsNot:
                    sOut = " IS NOT ";
                    break;
                case Comparison.In:
                    sOut = " IN ";
                    break;
                case Comparison.NotIn:
                    sOut = " NOT IN ";
                    break;
            }
            return sOut;
        }

        #region props

        TableSchema.Table table;

        public QueryType QueryType = QueryType.Select;
        public string Top = "";
        public bool Distinct;
        public string SelectList = " * ";
        public OrderBy OrderBy;
        private List<Aggregate> aggregates;
        public List<Where> wheres;
        public List<Join> joins;
        public DataProvider Instance;
        protected WhereOptions LastOption = WhereOptions.None;
		//protected int ParenStack;
        //protected int ParenOpen;
        protected int Page;
        #endregion

        #region .ctors
        public Query(TableSchema.Table tbl)
        {
            table = tbl;
            bool TriedAgain = false;
TryAgain:
            if (table.Instance != null)
                Instance = table.Instance;
            else if(!TriedAgain)
            {
                TriedAgain = true;
                System.Threading.Thread.Sleep(50);
                goto TryAgain;
            }
            SelectList = tbl.FIELD_LIST;
        }
        public Query(TableSchema.Table tbl, Where[] whereClauses) : this(tbl)
        {
            foreach (Where CurrWhere in whereClauses)
            {
                CurrWhere.TableName = table.Name;
                AddWhere(CurrWhere);
            }
        }
        #endregion

        #region Add Methods for adding WHERE, Aggregates, and Joins
        Dictionary<string, object> updateSettings;
        public void AddUpdateColumn(string columnName, object value)
        {
            if (updateSettings == null) updateSettings = new Dictionary<string, object>();

            //boolean massage for MySQL
            if (Worker.nzString(value).ToLower() == "false") value = 0;
            else if (Worker.nzString(value).ToLower() == "true") value = 1;

            if (updateSettings.ContainsKey(columnName)) updateSettings[columnName] = value;
            else                                        updateSettings.Add(columnName, value);
            QueryType = QueryType.Update;
        }
        /// <summary>C#CD: </summary>
        /// <param name="ToTableName">C#CD: </param>
        /// <param name="FromColumnName">C#CD: </param>
        /// <param name="ToColumnName">C#CD: </param>
        public void AddInnerJoin(string ToTableName, string FromColumnName, string ToColumnName)
        {
            if (joins == null) joins = new List<Join>();

            Join j = new Join();
            j.ToColumn = ToColumnName;
            j.JoinTable = ToTableName;
            j.FromColumn = FromColumnName;
            joins.Add(j);
        }
        /// <summary>C#CD: </summary>
        /// <param name="ToTableName">C#CD: </param>
        public void AddInnerJoin(string ToTableName)
        {
            if (joins == null) joins = new List<Join>();

            Join j = new Join();
            j.ToColumn = table.PrimaryKey.ColumnName;
            j.JoinTable = ToTableName;
            j.FromColumn = table.PrimaryKey.ColumnName;

            joins.Add(j);
        }
        /// <summary>C#CD: </summary>
        /// <param name="columnName">C#CD: </param>
        /// <param name="func">C#CD: </param>
        /// <param name="alias">C#CD: </param>
        public void AddAggregate(string columnName, AggregateFunction func, string alias)
        {
            Aggregate agg = Aggregate.New(Instance, func, columnName, alias);
            AddAggregate(agg);
        }
        /// <summary>C#CD: </summary>
        /// <param name="agg">C#CD: </param>
        public void AddAggregate(Aggregate agg)
        {
            if (aggregates == null) aggregates = new List<Aggregate>();
            aggregates.Add(agg);
        }

		/// <summary>C#CD: </summary>
		public void BeginParen()
		{
			switch (LastOption)
			{
				case WhereOptions.And:
					wheres.Add(new Where(WhereOptions.AndWithBeginParen));
					break;
				case WhereOptions.Or:
					wheres.Add(new Where(WhereOptions.OrWithBeginParen));
					break;
				default:
					wheres.Add(new Where(WhereOptions.BeginParen));
					break;
			}
		}

		/// <summary>C#CD: </summary>
		public void EndParen()
		{
			wheres.Add(new Where(WhereOptions.EndParen));
			//switch (LastOption)
			//{
			//    case WhereOptions.And:
			//        wheres.Add(new Where(WhereOptions.AndWithEndParen));
			//        break;
			//    case WhereOptions.Or:
			//        wheres.Add(new Where(WhereOptions.OrWithEndParen));
			//        break;
			//    default:
			//        wheres.Add(new Where(WhereOptions.EndParen));
			//        break;
			//}
		}

        /// <summary>C#CD: </summary>
        /// <param name="where">C#CD: </param>
        public void AddWhere(Where where)
        {
            if (wheres == null) wheres = new List<Where>();
			if (LastOption != WhereOptions.None)
			{
				where.Options = LastOption;
				LastOption = WhereOptions.None;
			}
            wheres.Add(where);
        }
        /// <summary>C#CD: </summary>
        /// <param name="columnName">C#CD: </param>
        /// <param name="paramValue">C#CD: </param>
        public void AddWhere(string columnName, object paramValue)
        {
            AddWhere(table.Name, columnName, Comparison.Equals, paramValue);

        }
        /// <summary>C#CD: </summary>
        /// <param name="tableName">C#CD: </param>
        /// <param name="columnName">C#CD: </param>
        /// <param name="paramValue">C#CD: </param>
        public void AddWhere(string tableName, string columnName, object paramValue)
        {
            AddWhere(tableName, columnName, Comparison.Equals, paramValue);

        }
        /// <summary>C#CD: </summary>
        /// <param name="columnName">C#CD: </param>
        /// <param name="comp">C#CD: </param>
        /// <param name="paramValue">C#CD: </param>
        public void AddWhere(string columnName, Comparison comp, object paramValue)
        {
            AddWhere(table.Name, columnName, comp, paramValue);

        }
        /// <summary>C#CD: </summary>
        /// <param name="tableName">C#CD: </param>
        /// <param name="columnName">C#CD: </param>
        /// <param name="comp">C#CD: </param>
        /// <param name="paramValue">C#CD: </param>
        public void AddWhere(string tableName, string columnName, Comparison comp, object paramValue)
        {
            AddWhere(new Where(tableName, columnName, comp, paramValue));
        }
        #endregion

        #region Command Builders
        public static DbType ConvertToDbType(Type t)
        {
            switch(t.Name)
            {
                case "Boolean": return DbType.Boolean;
                case "Int16": return DbType.Int16;
                case "Int32": return DbType.Int32;
                case "Int64": return DbType.Int64;
                case "Double": return DbType.Double;
                case "Decimal": return DbType.Decimal;
                case "String": return DbType.AnsiString;
                case "DateTime": return DbType.DateTime;
                case "Byte[]": return DbType.Binary;
                case "Guid": return DbType.Guid;
                case "Char[]": return DbType.AnsiStringFixedLength;
                case "Single": return DbType.Single;
                case "Byte": return DbType.Byte;
            }
            return DbType.Object;
        }

        /// <summary>
        /// Creates a SELECT command based on the Query object's settings.
        /// If you need a more complex query you should consider using a Stored Procedure
        /// </summary>
        public QueryCommand BuildSelectCommand()
        {
            QueryCommand cmd = new QueryCommand(GetSelectSql());
            if(wheres != null)
                foreach (Where where in wheres)
                    if(where.Comparison != Comparison.In && where.Comparison != Comparison.NotIn)
                        if(where.ParameterValue != null)
                            cmd.AddParameter("@_" + where.ParameterName, where.ParameterValue, ConvertToDbType(where.ParameterValue.GetType()));
            return cmd;
        }

        private void AddWherePhraseToSql(ref string sql, QueryCommand cmd)
        {
            if (wheres != null && wheres.Count > 0)
            {
                int i = 1;
				bool NeedAHinge = false;
				foreach (Where where in wheres)
				{
					//Append the SQL
					if (i == 1)
					{
						sql += " WHERE ";
						NeedAHinge = false;
					}
					switch (where.Options)
					{
						case WhereOptions.And: if(NeedAHinge) sql += " AND "; break;
						case WhereOptions.Or: if (NeedAHinge) sql += " OR "; break;
						case WhereOptions.AndWithBeginParen: sql += " AND ("; NeedAHinge = false;  break;
						case WhereOptions.OrWithBeginParen: sql += " OR ("; NeedAHinge = false; break;
						case WhereOptions.BeginParen: sql += "("; NeedAHinge = false; break;
						case WhereOptions.EndParen: sql += ")"; NeedAHinge = true; break;
						default: if (NeedAHinge) { sql += " AND "; NeedAHinge = false; } break;
					}
					//}
					if (where.ColumnName != null)
					{
						where.ParameterName = where.ColumnName + i.ToString();

						if (where.Comparison != Comparison.In && where.Comparison != Comparison.NotIn)
							sql += Instance.validIdentifier(where.ColumnName) + " " + GetComparisonOperator(where.Comparison) + (where.ParameterValue == null || where.ParameterValue == DBNull.Value ? "NULL" : " @_" + where.ParameterName);
						else
							sql += Instance.validIdentifier(where.ColumnName) + " " + GetComparisonOperator(where.Comparison) + " " + (string)where.ParameterValue;

						if (cmd != null)
							cmd.AddParameter("@_" + where.ParameterName, where.ParameterValue);
						NeedAHinge = true; 
					}
					i++;
				}
            }
        }

        /// <summary>
        /// Builds a Delete command based on a give WHERE condition
        /// </summary>
        public QueryCommand BuildDeleteCommand()
        {
            string sql = "DELETE FROM " + table.Name;
            //stub this out
            QueryCommand cmd = new QueryCommand(sql);
            if (wheres != null && wheres.Count > 0)
                AddWherePhraseToSql(ref sql, cmd);
            else
                sql = GetDeleteSql();
            cmd.CommandSql = sql;
            return cmd;
        }

        /// <summary>
        /// Builds an update query for this table with the passed-in hash values
        /// </summary>
        public QueryCommand BuildUpdateCommand()
        {
            if (updateSettings == null)
                throw new Exception("No update settings have been set. Use Query.AddUpdateSetting to add some in");

            string sql = "UPDATE " + table.Name;
            QueryCommand cmd = new QueryCommand(sql);

            TableSchema.TableColumn column = null;
            //append the update statements
            string setClause = " SET ";
            foreach (KeyValuePair<string, object> CurrColumn in updateSettings)
            {
                column = table.Columns.GetColumn(CurrColumn.Key);
                if (column != null)
                {
                    sql += setClause + CurrColumn.Key + "=@" + CurrColumn.Key + ",";
                    cmd.AddParameter("@_" + CurrColumn.Key, CurrColumn.Value);
                    setClause = string.Empty;
                }
                else
                {
                    //there's no column in this table that corresponds to the passed-in hash
                    throw new Exception("There is no column in " + table.Name + " called " + CurrColumn.Key);
                }
            }
            //trim the comma
            sql = sql.Remove(sql.Length - 1, 1);


            //string whereClause = " WHERE ";
            if (wheres != null && wheres.Count > 0)
                AddWherePhraseToSql(ref sql, cmd);
            //if (wheres != null && wheres.Count > 0)
            //{
            //    int i = 1;
            //    foreach (Where where in wheres)
            //    {
            //        //Append the SQL
            //        where.ParameterName = where.ColumnName + (i++).ToString();
            //        sql += whereClause + where.ColumnName + " " + GetComparisonOperator(where.Comparison) + " @_" + where.ParameterName;
            //        cmd.AddParameter("@_" + where.ParameterName, where.ParameterValue);
            //        whereClause = " AND ";
            //    }
            //}
            cmd.CommandSql = sql;
            return cmd;
        }
        #endregion

        #region SQL Builders
        //this is only used with the SQL constructors below
        //it's not used in the command builders above, which need to set the parameters
        //right at the time of the command build
        string BuildWhere()
        {
            string sql = string.Empty;
            if (wheres != null && wheres.Count > 0)
            {
                int i = 1;
                foreach (Where where in wheres)
                {
                    //Append the SQL
                    if (i == 1)
                        sql += " WHERE ";
                    else
                    {
                        switch (where.Options)
                        {
                            case WhereOptions.And: sql += " AND "; break;
                            case WhereOptions.Or: sql += " OR "; break;
							case WhereOptions.BeginParen: sql += "("; break;
							case WhereOptions.EndParen: sql += ")"; break;
                            //case WhereOptions.AndWithBeginParen: sql += " AND ("; break;
                            //case WhereOptions.AndWithEndParen: sql += " AND "; break;
                            //case WhereOptions.OrWithBeginParen: sql += " OR ("; break;
                            //case WhereOptions.OrWithEndParen: sql += " OR "; break;
                        }
                    }
					if(where.ColumnName != null)
						sql += where.ColumnName + " " + GetComparisonOperator(where.Comparison) + " @_" + where.ColumnName + i.ToString();
					i++;
                    //switch (where.Options)
                    //{
                    //    case WhereOptions.AndWithEndParen: sql += ") "; break;
                    //    case WhereOptions.OrWithEndParen: sql += ") "; break;
                    //}
                }
            }
            return sql;
        }
        /// <summary>
        /// Creates a SELECT statement based on the Query object settings
        /// </summary>
        /// <returns>C#CD: </returns>
        public string GetSelectSql()
        {

            string client = Instance.GetType().Name;
            //different rules for how to do TOP
            string topStatement = Instance.topStatement;
            string select = Instance.selectStatement( Top, Page, QueryType);
            string groupBy = string.Empty;
            string where = string.Empty;
            string order = string.Empty;
            string join = string.Empty;
            string query = string.Empty;
            string whereOperator = " WHERE ";

            if (aggregates != null && aggregates.Count > 0)
            {
                //if there's an aggregate, do it up first
                string aggList = string.Empty;
                string thisAggregate = string.Empty;
                groupBy = " GROUP BY ";

                //select * on an aggregate doesn't make sense
                if (SelectList.Trim() == "*")
                    SelectList = string.Empty;

                foreach (Aggregate agg in aggregates)
                {
                    thisAggregate = agg.AggregateString + ",";
                    if (Distinct)
                        thisAggregate = thisAggregate.Replace("(", "(DISTINCT ");
                    aggList += thisAggregate;
                }

                //remove trailing comma
                if (aggList.Length > 0)
                    aggList = aggList.Remove(aggList.Length - 1, 1);

                //set the select to the aggregate
                select += aggList;

                //if the passed-in select list is not empty,
                //we need to use a GROUP BY with a HAVING
                //since we will want to group our aggregate
                //with the passed-in columns
                if (SelectList.Length > 0)
                {
                    select += ", " + SelectList;

                    //need to build a GROUP BY and Having
                    //SQL Rules dictate that whatever's in the select list (minus aggregate functions
                    //needs to be in the GROUP BY. Same for HAVING
                    //if there is a term in HAVING, then it needs to be in the GROUP BY

                    //can't have aliases in the GROUP BY 
                    if (SelectList.ToLower().Contains(" as "))
                    {
                        //first, split the SelectList by commas
                        if (SelectList.Length > 0)
                        {
                            string[] selectCols = SelectList.Split(',');
                            if (selectCols.Length > 0)
                            {
                                //string the as bits off each one, and append on to the GROUP BY
                                foreach (string sCol in selectCols)
                                    groupBy += Instance.validIdentifier(sCol.Substring(0, sCol.ToLower().IndexOf(" as "))) + ",";

                                //remove the trailing comma
                                groupBy = groupBy.Remove(groupBy.Length - 1, 1);

                            }
                        }
                    }
                    else
                        groupBy += SelectList;


                    //if there are columns in the where list, append on a comma
                    if (wheres != null && wheres.Count > 0 && groupBy.Trim() != "GROUP BY")
                        groupBy += ",";

                    //use the WHEREs to append on the bits in the HAVING clause
                    if (wheres != null)
                    {
                        whereOperator = " HAVING ";
                        foreach (Where wHaving in wheres)
                        {
                            if (!groupBy.Contains(wHaving.ColumnName))
                                groupBy += Instance.validIdentifier(wHaving.ColumnName) + ",";
                        }
                    }

                    //trim off the last comma
                    if (groupBy.Trim().EndsWith(","))
                        groupBy = groupBy.Remove(groupBy.Length - 1, 1);
                }
                else
                {
                    //there were no passed-in columns
                    //so we can use a WHERE here without a GROUP BY
                    whereOperator = " WHERE ";
                    groupBy = " ";
                }
            }
            else
            {
                //prepend on the "DISTINCT" term
                if (Distinct)
                    select += " DISTINCT ";

				switch (QueryType)
				{
					case QueryType.Count:
						if (SelectList.IndexOf(",") == -1)
							select += "COUNT(" + SelectList.Trim() + ")";
						else
							select += "COUNT(*)";
						break;
					case QueryType.Max:
						if (SelectList.IndexOf(",") == -1)
							select += "MAX(" + SelectList.Trim() + ")";
						else
							select += "MAX(*)";
						break;
					case QueryType.Min:
						if (SelectList.IndexOf(",") == -1)
							select += "MIN(" + SelectList.Trim() + ")";
						else 
							select += "MIN(*)";
						break;
					default:
						select += SelectList; 
						break;
				}
                    
            }

            //append on the SelectList, which is a property that can be set
            //and is "*" by default

            select += " FROM " + Instance.validIdentifier(table.Name) + " ";


            //joins
            if (joins != null && joins.Count > 0)
                foreach (Join j in joins)
                    join += " " + j.JoinType + " " + Instance.validIdentifier(j.JoinTable) + " ON " + Instance.validIdentifier(table.Name) + "." + Instance.validIdentifier(j.FromColumn) + "=" + Instance.validIdentifier(j.JoinTable) + "." + Instance.validIdentifier(j.ToColumn) + " ";

            //now for the wheres...
            //MUST USE parameters to avoid injection issues
            //the following line is my favorite... Moe, Larry, Curly...
            //int i = 1;
            if (wheres != null)
                AddWherePhraseToSql(ref where, null);

            //Finally, do the orderby 
			if (OrderBy != null && (QueryType != QueryType.Count && QueryType != QueryType.Min && QueryType != QueryType.Max))
                order = OrderBy.OrderString;

            if(Page == 0)
                query = select + join + groupBy + where + order;
            else
                query = select.Replace("[orderByClause]", order) + join + groupBy + where + order;

            if (client.Contains("MySqlDataProvider"))
                if (!Top.Contains("%") && !Top.ToLower().Contains("percent"))
                    query += " LIMIT " + Top;

            int PageSize;
			if (Page > 0 && !string.IsNullOrEmpty(Top) && int.TryParse(Top, out PageSize) && PageSize > 0 && (QueryType != QueryType.Count && QueryType != QueryType.Min && QueryType != QueryType.Max))
                query = Instance.handlePage(query, ((Page - 1) * PageSize) + 1, PageSize, QueryType);

            if (QueryType == QueryType.Exists)
                query = "IF EXISTS(" + query + ") SELECT 1 ELSE SELECT 0";
            return query;
        }

        /// <summary>C#CD: </summary>
        /// <returns>C#CD: </returns>
        public string GetUpdateSql()
        {
            //split the TablNames and loop out the SQL
            string updateSQL = "UPDATE " + Instance.validIdentifier(table.Name) + " SET ";
            string cols = "";

            foreach (TableSchema.TableColumn col in table.Columns)
            {
                //don't want to change the created bits
                if (!col.IsPrimaryKey && col.ColumnName.ToLower() != "createdby" && col.ColumnName.ToLower() != "createdon")
                    cols += col.ColumnName + "=@" + col.ColumnName + ",";
            }
            cols = cols.Remove(cols.Length - 1, 1);
            
            updateSQL += cols;

            if (wheres == null || wheres.Count == 0)
                updateSQL += " WHERE " + table.PrimaryKey.ColumnName + "=@" + table.PrimaryKey.ColumnName + Instance.commandSeparator; // + "SELECT @" + table.PrimaryKey.ColumnName + " as id";
            else
                updateSQL += BuildWhere();
            return updateSQL;
        }
        /// <summary>
        /// Loops the TableColums[] array for the object, creating a SQL string
        /// for use as an INSERT statement
        /// </summary>
        /// <returns>C#CD: </returns>
        public string GetInsertSql()
        {

            //split the TablNames and loop out the SQL
            string insertSQL = "INSERT INTO " + Instance.validIdentifier(table.Name) + " ";

            string cols = "";
            string pars = "";

            //if table columns are null toss an exception
            foreach (TableSchema.TableColumn col in table.Columns)
            {
                if (!col.AutoIncrement)
                {
                    cols += Instance.validIdentifier(col.ColumnName) + ",";
                    pars += "@_" + col.ColumnName + ",";
                }
            }
            cols = cols.Remove(cols.Length - 1, 1);
            pars = pars.Remove(pars.Length - 1, 1);
            insertSQL += "(" + cols + ") ";



            insertSQL += "VALUES(" + pars + ");";

            //the default convention is that every table has a unique, auto-increment key
            //this is for setting the PK after save. @@IDENTITY or IDENTITY_SCOPE can 
            //be used, but don't translate to other DBs. MAX does so it's used here
            if ( table.PrimaryKey != null && table.PrimaryKey.DataType != DbType.Guid)
                insertSQL += " SELECT MAX(" + Instance.validIdentifier(table.PrimaryKey.ColumnName) + ") FROM " + Instance.validIdentifier(table.Name) + " as newID";

            return insertSQL;
        }


        /// <summary>C#CD: </summary>
        /// <returns>C#CD: </returns>
        public string GetDeleteSql()
        {
            string sql = "DELETE FROM " + table.Name;
            if (wheres == null || wheres.Count == 0)
                sql += " WHERE " + table.PrimaryKey.ColumnName + "=@" + table.PrimaryKey.ColumnName;
            else
                sql += BuildWhere();
            return sql;

        }
        #endregion

        #region Execution

        /// <summary>
        /// Returns an IDataReader using the passed-in command
        /// </summary>
        /// <returns>IDataReader</returns>
        public IDataReader ExecuteReader()
        {
            return Instance.GetReader(GetCommand());
        }

        /// <summary>
        /// Returns a DataSet based on the passed-in command
        /// </summary>
        /// <returns>C#CD: </returns>
        public DataSet ExecuteDataSet()
        {
            return Instance.ToDataSet(GetCommand());

        }
        /// <summary>
        /// Returns a scalar object based on the passed-in command
        /// </summary>
        /// <returns>C#CD: </returns>
        public object ExecuteScalar()
        {
            return Instance.ExecuteScalar(GetCommand());
        }
        /// <summary>
        /// Executes a pass-through query on the DB
        /// </summary>
        public void Execute()
        {
            Instance.ExecuteQuery(GetCommand());
        }

        QueryCommand GetCommand()
        {
            QueryCommand cmd = null;
            switch (QueryType)
            {
                case QueryType.Select:
                case QueryType.Exists:
                case QueryType.Count:
				case QueryType.Min:
				case QueryType.Max:
                    cmd = BuildSelectCommand();
                    break;
                case QueryType.Update:
                    cmd = BuildUpdateCommand();
                    break;
                case QueryType.Insert:
                    cmd = null;
                    break;
                case QueryType.Delete:
                    cmd = BuildDeleteCommand();
                    break;
            }
            return cmd;
        }
        #endregion


    }
    /// <summary>C#CD: </summary>
    public enum QueryType
    {
        /// <summary>SELECT FROM query</summary>
        Select,
        /// <summary>UPDATE query</summary>
        Update,
        /// <summary>INSERT INTO query</summary>
        Insert,
        /// <summary>DELETE FROM query</summary>
        Delete,
        /// <summary>IF EXISTS(query) SELECT 1 ELSE SELECT 0</summary>
        Exists,
        /// <summary>SELECT COUNT(*) FROM query</summary>
        Count,
		/// <summary>SELECT MAX(*) FROM query</summary>
		Max,
		/// <summary>SELECT MIN(*) FROM query</summary>
		Min

    }
}
