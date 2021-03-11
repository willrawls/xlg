// ReSharper disable UnusedType.Global
namespace MetX.Standard.Web.Virtual.xsl
{
    /// <summary>C#CD: </summary>
    public static class Resource
    {
        /// <summary>C#CD: </summary>
        /// <param name="resourceName">C#CD: </param>
        public static string Get(string resourceName)
        {
            System.IO.Stream reader1 = null;
            try
            {
                var a = System.Reflection.Assembly.GetExecutingAssembly();
                reader1 = a.GetManifestResourceStream(typeof(Resource), resourceName);
            }
            catch
            {
                // Ignored
            }

            if (reader1 == null)
            {
                return string.Empty;
            }
            
            string contents;
            using (var reader2 = new System.IO.StreamReader(reader1))
            {
                contents = reader2.ReadToEnd();
                reader2.Close();
                reader2.Dispose();
            }


            reader1.Close();
            reader1.Dispose();
            return contents;
        }
    }
}
