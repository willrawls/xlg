using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MetX.Data
{
    public class FWTConnection : IDbConnection
    {
        public StreamReader sr;
        public StreamWriter sw;
        string connectionString;
        public string XlgFilePath, DataFilePath;
        public bool AsReader;

        public FWTConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #region IDbConnection Members

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return null;
        }

        public IDbTransaction BeginTransaction()
        {
            return null;
        }

        public void ChangeDatabase(string databaseName)
        {
            // 
        }

        public void Close()
        {
            if (sr != null)
            {
                sr.Close();
                sr.Dispose();
                sr = null;
            }
            if (sw != null)
            {
                sw.Close();
                sw.Dispose();
                sw = null;
            }
        }

        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
                XlgFilePath = Token.Between(connectionString, "XlgFilePath=", ";");
                DataFilePath = Token.Between(connectionString, "DataFilePath=", ";");
                AsReader = Token.Between(connectionString, "AsReader=", ";").ToLower() == "true";
            }
        }

        public int ConnectionTimeout
        {
            get { return int.MinValue; }
        }

        public IDbCommand CreateCommand()
        {
            return new FWTCommand();
        }

        public string Database
        {
            get { return Path.GetFileNameWithoutExtension(DataFilePath); }
        }

        public void Open()
        {
            if (AsReader)
            {
                sr = new StreamReader(DataFilePath);
            }
            else
            {
                if (File.Exists(DataFilePath))
                {
                    File.SetAttributes(DataFilePath, FileAttributes.Normal);
                    File.Delete(DataFilePath);
                }
                sw = new StreamWriter(DataFilePath);
            }
        }

        public ConnectionState State
        {
            get
            {
                if (sr != null || sw != null)
                    return ConnectionState.Open;
                return ConnectionState.Closed;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion
    }

    public class FWTCommand : IDbCommand
    {
        public FWTCommand() { }

        public FWTCommand(string commandText)
        {
            this.commandText = commandText;
        }

        public FWTCommand(string commandText, FWTConnection connection)
        {
            this.commandText = commandText;
            this.connection = connection;
        }

        #region IDbCommand Members

        public void Cancel()
        {
            
        }

        string commandText;
        public string CommandText
        {
            get
            {
                return commandText;
            }
            set
            {
                commandText = value;
            }
        }

        public int CommandTimeout
        {
            get
            {
                return int.MinValue; 
            }
            set
            {
                
            }
        }

        CommandType commandType;
        public CommandType CommandType
        {
            get
            {
                return commandType;
            }
            set
            {
                commandType = value;
            }
        }

        FWTConnection connection;
        public IDbConnection Connection
        {
            get
            {
                return connection;
            }
            set
            {
                connection = (FWTConnection) value;
            }
        }

        public IDbDataParameter CreateParameter()
        {
            return null;
        }

        public int ExecuteNonQuery()
        {
            if(connection.sw != null)
                connection.sw.Write(commandText);
            if (connection.sr != null)
                commandText = connection.sr.ReadLine();
            return 1;
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            FWTDataReader ret = new FWTDataReader(this, behavior);
            return ret;
        }

        public IDataReader ExecuteReader()
        {
            return null;
            
        }

        public object ExecuteScalar()
        {
            commandText = null;
            if (connection.sr != null)
                commandText = connection.sr.ReadLine();
            return commandText;
        }

        public IDataParameterCollection Parameters
        {
            get { return null; }
        }

        public void Prepare()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IDbTransaction Transaction
        {
            get
            {
                return null; 
            }
            set
            {
                
            }
        }

        public UpdateRowSource UpdatedRowSource
        {
            get
            {
                UpdateRowSource ret = new UpdateRowSource();
                return ret;
            }
            set
            {
                
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            
        }

        #endregion
    }
}
