using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MetX.Data
{
    public class FWTDataReader : IDataReader 
    {
        public FWTConnection connection;
        public FWTCommand command;
        CommandBehavior behavior;
        string buffer;

        public FWTDataReader(FWTCommand command, CommandBehavior behavior)
        {
            this.command = command;
            this.behavior = behavior;
            this.connection = (FWTConnection)command.Connection;
        }

        #region IDataReader Members

        public void Close()
        {
            
        }

        public int Depth => 1;

        public DataTable GetSchemaTable()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsClosed => command.Connection.State != ConnectionState.Open;

        public bool NextResult()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Read()
        {
            if (connection.sr != null)
            {
                if (!connection.sr.EndOfStream)
                    buffer = connection.sr.ReadLine();
                else
                    buffer = null;
            }
            else
                buffer = null;
            if (connection.sr.EndOfStream && behavior == CommandBehavior.CloseConnection)
                connection.Close();
            return buffer != null;
        }

        public int RecordsAffected => 1;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            connection = null;
            command = null;
            buffer = null;
        }

        #endregion

        #region IDataRecord Members

        public int FieldCount => throw new Exception("The method or operation is not implemented.");

        public bool GetBoolean(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public byte GetByte(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public char GetChar(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IDataReader GetData(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GetDataTypeName(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DateTime GetDateTime(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public decimal GetDecimal(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public double GetDouble(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Type GetFieldType(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float GetFloat(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Guid GetGuid(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public short GetInt16(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int GetInt32(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public long GetInt64(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GetName(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int GetOrdinal(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GetString(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object GetValue(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int GetValues(object[] values)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsDBNull(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object this[string name] => throw new Exception("The method or operation is not implemented.");

        public object this[int i] => throw new Exception("The method or operation is not implemented.");

        #endregion
    }
}
