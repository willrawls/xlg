using System;
using System.Collections.Generic;
using System.Text;

namespace Metta.Web.Virtual.xsl
{
    /// <summary>C#CD: </summary>
    public static class Resource
    {
        /// <summary>C#CD: </summary>
        /// <param name="resourceName">C#CD: </param>
        public static string Get(string resourceName)
        {
            System.IO.Stream st = null;
            try
            {
                var a = System.Reflection.Assembly.GetExecutingAssembly();
                st = a.GetManifestResourceStream(typeof(Metta.Web.Virtual.xsl.Resource), resourceName);
            }
            catch { }
            var sr = new System.IO.StreamReader(st);
            var ret = sr.ReadToEnd();
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
