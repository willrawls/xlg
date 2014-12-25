namespace MetX.Data {
   /// <summary>Provides access to static virtual file content for files</summary>
   public partial class DefaultXlg {
       /// <summary>The static contents of the file: "C:\data\code\wmr\MetX\Data\DataDefault.xml" as it existed at compile time.</summary>
       public const string xml = "<xlgDoc IncludeNamespace=\"true\" ConnectionStringName=\"[Default]\">\r\n  <Render>\r\n    <Xsls Path=\"~\" UrlExtension=\"aspx\">\r\n      <Include Name=\"*\" />\r\n    </Xsls>\r\n	  <Tables>\r\n      <Include Name=\"*\" />\r\n    </Tables>\r\n	  <StoredProcedures>\r\n      <Include Name=\"*\" />\r\n    </StoredProcedures>\r\n  </Render>\r\n</xlgDoc>";
       /// <summary>Returns xml inside a StringBuilder.</summary>
       /// <returns>A StringBuilder with the compile time file contents</returns>
       public static System.Text.StringBuilder xmlStringBuilder { get { return new System.Text.StringBuilder(xml); } }
   }
}
