using System;
using System.Collections.Generic;
using System.Text;

namespace Metta.Web.Virtual.xsl
{
    /// <summary>C#CD: </summary>
    public static class Resource
    {
        /// <summary>C#CD: </summary>
        /// <param name="ResourceName">C#CD: </param>
        public static string Get(string ResourceName)
        {
            System.IO.Stream st = null;
            try
            {
                System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
                st = a.GetManifestResourceStream(typeof(Metta.Web.Virtual.xsl.Resource), ResourceName);
            }
            catch { }
            System.IO.StreamReader sr = new System.IO.StreamReader(st);
            string ret = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            if (st != null)
            {
                st.Close();
                st.Dispose();
            }
            return ret;
        }
    }
}
