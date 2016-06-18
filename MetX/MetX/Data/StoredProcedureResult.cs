using System.Data;
using System.Text;

namespace MetX.Data
{
    public class StoredProcedureResult
    {
        public IDataReader Reader;
        public int ReturnValue = int.MinValue;
        public IDataParameterCollection Parameters;

        public StoredProcedureResult(IDataParameterCollection Parameters)
        {
            this.Parameters = Parameters;
        }

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            while (Reader.Read())
                ret.Append(Reader[0]);
            Reader.Close();
            Reader.Dispose();
            return ret.ToString();
        }

        public void Close()
        {
            if (Reader != null && !Reader.IsClosed)
            {
                Reader.Close();
                Reader.Dispose();
            }
            Reader = null;
            Parameters = null;
        }
    }
}