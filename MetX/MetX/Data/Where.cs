using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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

        private object _mParameterValue;

        public Where() { }

        public Where(string tableName, string columnName, Comparison comparison, object value)
        {
            TableName = tableName;
            this.ColumnName = columnName;
            this.Comparison = comparison;
            _mParameterValue = value;
        }

        public Where(string columnName, Comparison comparison, object value)
        {
            this.ColumnName = columnName;
            this.Comparison = comparison;
            _mParameterValue = value;
        }

        public Where(string columnName, object value)
        {
            this.ColumnName = columnName;
            Comparison = Comparison.Equals;
            _mParameterValue = value;
        }

        public Where(string tableName, string columnName, Comparison comparison, object value, WhereOptions options)
        {
            TableName = tableName;
            this.ColumnName = columnName;
            this.Comparison = comparison;
            _mParameterValue = value;
            this.Options = options;
        }

        public Where(WhereOptions options)
        {
            this.Options = options;
        }

        public Where(string columnName, Comparison comparison, object value, WhereOptions options)
        {
            this.ColumnName = columnName;
            this.Comparison = comparison;
            _mParameterValue = value;
            this.Options = options;
        }

        public Where(string columnName, object value, WhereOptions options)
        {
            this.ColumnName = columnName;
            Comparison = Comparison.Equals;
            _mParameterValue = value;
            this.Options = options;
        }

        public object ParameterValue
        {
            get
            {
                if (Comparison != Comparison.In && Comparison != Comparison.NotIn)
                    return _mParameterValue;
                else
                    return InPhrase(_mParameterValue);
            }
        }

        public void AddValue(string valueToAddToInClause)
        {
            List<string> pv;
            if (_mParameterValue is Array)
            {
                var t = (Array)_mParameterValue;
                pv = new List<string>();
                _mParameterValue = pv;
                foreach (var currValue in t)
                    pv.Add(currValue.ToString());
            }
            else
                pv = (List<string>)_mParameterValue;
            pv.Add(valueToAddToInClause);
        }

        public void AddRangeOfValues(IEnumerable<string> valuesToAddToInClause)
        {
            List<string> pv;
            if (_mParameterValue is Array)
            {
                var t = (Array)_mParameterValue;
                pv = new List<string>();
                _mParameterValue = pv;
                foreach (var currValue in t)
                    pv.Add(currValue.ToString());
            }
            else
                pv = (List<string>)_mParameterValue;
            pv.AddRange(valuesToAddToInClause);
        }

        private string InPhrase(object value)
        {
            var sValue = new StringBuilder();
            if (value == null)
                sValue.Append("(NULL");
            if (value is string)
            {
                if (((string)value).StartsWith("("))
                    return (string)value;
                else
                {
                    sValue.Append("(");
                    if (!((string)value).StartsWith("'"))
                    {
                        sValue.Append("'");
                        sValue.Append(((string)value).Replace("'", "''"));
                        sValue.Append("'");
                    }
                    else
                        sValue.Append((string)value);
                }
            }
            else if (value is string[])
            {
                foreach (var currValue in (string[])value)
                {
                    if (currValue != null && currValue.Length > 0)
                    {
                        if (sValue.Length > 0) sValue.Append(","); else sValue.Append("(");
                        sValue.Append("'");
                        sValue.Append(currValue.Replace("'", "''"));
                        sValue.Append("'");
                    }
                }
            }
            else if (value is int[])
            {
                foreach (var currValue in (int[])value)
                {
                    if (sValue.Length > 0) sValue.Append(","); else sValue.Append("(");
                    sValue.Append(currValue.ToString());
                }
            }
            else if (value is double[])
            {
                foreach (var currValue in (double[])value)
                {
                    if (sValue.Length > 0) sValue.Append(","); else sValue.Append("(");
                    sValue.Append(currValue.ToString(CultureInfo.InvariantCulture));
                }
            }
            else if (value is Array)
            {
                foreach (var currValue in (Array)value)
                {
                    if (currValue == null) continue;

                    sValue.Append(sValue.Length > 0 ? "," : "(");
                    sValue.Append("'");
                    sValue.Append(currValue.ToString()?.Replace("'", "''"));
                    sValue.Append("'");
                }
            }
            else if (value is List<string> list)
            {
                foreach (var currValue in list)
                {
                    if (!string.IsNullOrEmpty(currValue))
                    {
                        if (sValue.Length > 0) sValue.Append(","); else sValue.Append("(");
                        sValue.Append("'");
                        sValue.Append(currValue.Replace("'", "''"));
                        sValue.Append("'");
                    }
                }
            }
            else
            {
                sValue.Append("(");
                sValue.Append("'");
                sValue.Append(value.AsString().Replace("'", "''"));
                sValue.Append("'");
            }
            sValue.Append(")");
            return sValue.ToString();
        }
    }
}