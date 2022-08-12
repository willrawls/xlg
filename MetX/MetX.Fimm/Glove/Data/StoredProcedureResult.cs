using System.Data;
using System.Text;

namespace MetX.Fimm.Glove.Data
{
    public class StoredProcedureResult
    {
        public IDataReader Reader;
        public int ReturnValue = int.MinValue;
        public IDataParameterCollection Parameters;

        public StoredProcedureResult(IDataParameterCollection parameters)
        {
            Parameters = parameters;
        }

        public override string ToString()
        {
            var ret = new StringBuilder();
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