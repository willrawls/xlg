// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedType.Global
namespace MetX.Standard.Web.Virtual.xsl {
   /// <summary>Provides access to static virtual file content for files</summary>
   public partial class Applications {
       /// <summary>The static contents of the file as it existed at compile time.</summary>
       public const string Xsl = "<?xml version=\"1.0\" ?>\r\n<xsl:stylesheet xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" xmlns:xlg=\"urn:xlg\" version=\"1.0\">\r\n  <xsl:output method=\"html\" />\r\n  <xsl:template match=\"/xlgDoc\">\r\n    <HTML>\r\n      <HEAD>\r\n        <title>Security Applications</title>\r\n        <LINK REL=\"StyleSheet\" TYPE=\"text/css\" HREF=\"/xlgSupport/css/Styles.css\" />\r\n      </HEAD>\r\n      <body>\r\n		    <H1>XLG Security Administrator</H1>\r\n		    <P>Select an application to edit:</P>\r\n        <table>\r\n        <xsl:for-each select=\"/xlgDoc/Applications/Application\">\r\n          <tr>\r\n            <td width=\"100\">&amp;nbsp;</td>\r\n            <td>\r\n              <a class=\"buttonLink\"><xsl:attribute name=\"href\">editApplication.aspx?Application=<xsl:value-of select=\"@ApplicationName\" /></xsl:attribute><xsl:value-of select=\"@ApplicationName\" /></a>\r\n              <br />\r\n              <br />\r\n            </td>\r\n          </tr>\r\n        </xsl:for-each>\r\n        </table>\r\n      </body>\r\n    </HTML>\r\n  </xsl:template>\r\n</xsl:stylesheet>";
       /// <summary>Returns xsl inside a StringBuilder.</summary>
       /// <returns>A StringBuilder with the compile time file contents</returns>
       public static System.Text.StringBuilder XslStringBuilder => new(Xsl);
   }
}
