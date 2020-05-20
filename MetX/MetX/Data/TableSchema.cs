using System;
using System.Collections.Generic;
using System.Data;

namespace MetX.Data
{
    /// <summary>C#CD: </summary>
    [Serializable]
    public class TableSchema
    {
        /// <summary>
        /// Holds information about the base table - this class should be
        /// static for each object
        /// </summary>
        [Serializable]
        public class Table
        {
            public TableColumnCollection Columns = new TableColumnCollection();

            /// <summary>The field list as it goes into the SELECT sql statement used for Select, Count, Exists and paging type queries</summary>
            public string FIELD_LIST;

            public TableIndexCollection Indexes = new TableIndexCollection();

            /// <summary>The basic INSERT INTO sql statement used for Insert type queries</summary>
            public string INSERT_SQL;

            public DataProvider Instance;
            public TableKeyCollection Keys = new TableKeyCollection();
            public string Name;
            public string Schema;

            /// <summary>The basic UPDATE sql statement used for Update type querie</summary>
            public string UPDATE_SQL;

            /// <summary>C#CD: </summary>
            public Table() { }

            /// <summary>C#CD: </summary>
            /// <param name="tableName">C#CD: </param>
            /// <param name="schemaName"></param>
            public Table(string tableName, string schemaName)
            {
                Name = tableName;
                Schema = schemaName;
            }

            public string FullName
            {
                get
                {
                    if (string.IsNullOrEmpty(Schema))
                        Schema = "dbo";
                    return "[" + Schema + "].[" + Name + "]";
                }
            }

            public TableColumn PrimaryKey
            {
                get
                {
                    return Columns?.GetPrimaryKey();
                }
            }

            /// <summary>The basic SELECT sql statement used for Select, Count, Exists and paging type queries</summary>
            public string SelectSql { get { return "SELECT " + FIELD_LIST + " FROM [" + Name + "] "; } }
        }

        /// <summary>
        /// A helper class to help define the columns in an underlying table
        /// </summary>
        [Serializable]
        public class TableColumn
        {
            public bool AutoIncrement;
            public string ColumnName;
            public DbType DataType;
            public string DomainName;
            public bool IsForiegnKey;
            public bool IsIdentity;
            public bool IsIndexed;
            public bool IsNullable;
            public bool IsPrimaryKey;
            public int MaxLength;
            public int Precision;
            public int Scale;
            public string SourceType;

            /// <summary>C#CD: </summary>
            public TableColumn() { }

            /// <summary>C#CD: </summary>
            /// <param name="columnName">C#CD: </param>
            /// <param name="dbType">C#CD: </param>
            /// <param name="isPrimaryKey">C#CD: </param>
            /// <param name="isForeignKey">C#CD: </param>
            public TableColumn(string columnName, DbType dbType, bool isPrimaryKey, bool isForeignKey)
            {
                ColumnName = columnName;
                IsPrimaryKey = isPrimaryKey;
                IsForiegnKey = isForeignKey;
                DataType = dbType;
            }

            public TableColumn(string columnName, DbType dbType, bool autoIncrement, int maxLength, bool isNullable, bool isPrimaryKey, bool isForeignKey)
            {
                ColumnName = columnName;
                IsPrimaryKey = isPrimaryKey;
                IsForiegnKey = isForeignKey;
                DataType = dbType;
                this.AutoIncrement = autoIncrement;
                this.MaxLength = maxLength;
                this.IsNullable = isNullable;
            }
        }

        /// <summary>C#CD: </summary>
        [Serializable]
        public class TableColumnCollection : List<TableColumn>
        {
            public TableColumn PrimaryKeyColumn = null;

            private Dictionary<string, TableColumn> _iList = new Dictionary<string, TableColumn>();

            public TableColumnCollection()
            {
            }

            #region Collection Methods

            public new void Add(TableColumn column)
            {
                _iList.Add(column.ColumnName.ToLower(), column);
                base.Add(column);
            }

            /// <summary>C#CD: </summary>
            /// <param name="name">C#CD: </param>
            /// <param name="dbType">C#CD: </param>
            /// <param name="isNullable">C#CD: </param>
            /// <param name="isPrimaryKey">C#CD: </param>
            /// <param name="isForeignKey">C#CD: </param>
            public void Add(string name, DbType dbType, bool isNullable, bool isPrimaryKey, bool isForeignKey)
            {
                TableColumn col = new TableColumn();
                col.IsPrimaryKey = isPrimaryKey;
                col.IsForiegnKey = isForeignKey;
                col.IsNullable = isNullable;
                col.DataType = dbType;
                col.ColumnName = name;

                if (!Contains(name))
                {
                    Add(col);
                    _iList.Add(name.ToLower(), col);
                }
                if (isPrimaryKey)
                    PrimaryKeyColumn = col;
            }

            /// <summary>C#CD: </summary>
            /// <param name="name">C#CD: </param>
            /// <param name="dbType">C#CD: </param>
            /// <param name="isNullable">C#CD: </param>
            public void Add(string name, DbType dbType, bool isNullable)
            {
                Add(name, dbType, isNullable, false, false);
            }

            public bool Contains(string columnName)
            {
                return _iList.ContainsKey(columnName.ToLower());
            }

            #endregion Collection Methods

            /// <summary>C#CD: </summary>
            /// <param name="columnName">C#CD: </param>
            public TableColumn GetColumn(string columnName)
            {
                columnName = columnName.ToLower();
                if (_iList.ContainsKey(columnName))
                    return _iList[columnName];
                return null;
            }

            /// <summary>C#CD: </summary>
            public TableColumn GetPrimaryKey()
            {
                if (PrimaryKeyColumn != null)
                    return PrimaryKeyColumn;

                TableColumn coll = null;
                foreach (TableColumn child in this)
                {
                    if (child.IsPrimaryKey)
                    {
                        coll = child;
                        break;
                    }
                }
                PrimaryKeyColumn = this[0];
                return coll;
            }
        }

        /// <summary>
        /// This is an intermediary class that holds the current value of a table column
        /// for each object instance.
        /// </summary>
        [Serializable]
        public class TableColumnSetting
        {
            /// <summary>C#CD: </summary>
            private string _columnName;

            /// <summary>C#CD: </summary>
            private object _currentValue;

            /// <summary>C#CD: </summary>
            public string ColumnName
            {
                get { return _columnName; }
                set { _columnName = value; }
            }

            /// <summary>C#CD: </summary>
            public object CurrentValue
            {
                get { return _currentValue; }
                set
                {
                    if (value is int && (int)value == int.MinValue)
                        _currentValue = DBNull.Value;
                    else if (value is DateTime && (DateTime)value == DateTime.MinValue)
                        _currentValue = DBNull.Value;
                    else
                        _currentValue = value;
                }
            }
        }

        /// <summary>C#CD: </summary>
        [Serializable]
        public class TableColumnSettingCollection : Dictionary<string, TableColumnSetting>
        {
            /// <summary>C#CD: </summary>
            /// <param name="columnName">C#CD: </param>
            /// <returns>C#CD: </returns>
            public object GetValue(string columnName)
            {
                columnName = columnName.ToLower();
                if (ContainsKey(columnName))
                    return this[columnName].CurrentValue;
                return null;
            }

            /// <summary>C#CD: </summary>
            /// <param name="columnName">C#CD: </param>
            /// <param name="oVal">C#CD: </param>
            public void SetValue(string columnName, object oVal)
            {
                columnName = columnName.ToLower();
                if (!ContainsKey(columnName))
                {
                    TableColumnSetting setting = new TableColumnSetting();
                    setting.ColumnName = columnName;
                    setting.CurrentValue = oVal;
                    Add(columnName.ToLower(), setting);
                }
                else
                {
                    this[columnName].CurrentValue = oVal;
                }
            }

            private bool Contains(string columnName)
            {
                return ContainsKey(columnName.ToLower());
            }
        }

        [Serializable]
        public class TableIndex
        {
            public List<string> Columns = new List<string>();
            public bool IsClustered;
            public string Name;

            public TableIndex()
            {
            }

            public TableIndex(IDataReader rdr)
            {
                Name = rdr["name"].ToString();
                IsClustered = rdr["isclustered"].ToString() == "1";
                Columns.Add(rdr["column"].ToString());
            }
        }

        [Serializable]
        public class TableIndexCollection : List<TableIndex>
        {
            public TableIndexCollection()
            {
            }
        }

        [Serializable]
        public class TableKey
        {
            public List<TableKeyColumn> Columns = new List<TableKeyColumn>();
            public bool IsForeign;
            public bool IsPrimary;
            public bool IsReference;
            public string Name;

            public TableKey()
            {
            }
        }

        [Serializable]
        public class TableKeyCollection : List<TableKey>
        {
            public TableKeyCollection()
            {
            }

            public TableKey Find(string toFind)
            {
                foreach (TableKey currKey in this)
                    if (currKey.Name.ToLower() == toFind.ToLower())
                        return currKey;
                return null;
            }
        }

        [Serializable]
        public class TableKeyColumn
        {
            public string Column;
            public string Related;

            public TableKeyColumn()
            {
            }

            public TableKeyColumn(string column, string related)
            {
                this.Column = column;
                this.Related = related;
            }
        }
    }
}