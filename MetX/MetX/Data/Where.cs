using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MetX.Library;

namespace MetX.Data
{
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
                foreach (string CurrValue in (IList)Value)
                {
                    if (!string.IsNullOrEmpty(CurrValue))
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
                sValue.Append(Value.AsString().Replace("'", "''"));
                sValue.Append("'");
            }
            sValue.Append(")");
            return sValue.ToString();
        }
    }
}