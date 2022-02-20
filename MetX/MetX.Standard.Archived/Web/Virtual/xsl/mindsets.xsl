<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
               
                xmlns:xlg="urn:xlg"
                version="1.0">
  <xsl:output method="html" />
  <xsl:template match="/xlgDoc">
    <HTML>
      <HEAD>
        <title>Mindsets</title>
        <LINK REL="StyleSheet" TYPE="text/css" HREF="/xlgSupport/css/Styles.css" />
      </HEAD>
      <body>
		    <H1>XLG Mindset Administrator</H1>
		    <P>Select a mindset to edit:</P>
        <table>
        <xsl:for-each select="/xlgDoc/Mindsets/Mindset">
          <tr>
            <td width="100">&amp;nbsp;</td>
            <td>
              <a class="buttonLink"><xsl:attribute name="href">editMindset.aspx?MindsetID=<xsl:value-of select="@MindsetID" /></xsl:attribute><xsl:value-of select="@Name" /></a>
              <table cellspacing="0" cellpadding="0">
                <tr>
                  <td><xsl:value-of select="@Requires"/></td>
                  <td><xsl:value-of select="xlg:sXmlDate(@DateCreated)"/></td>
                  <td><xsl:value-of select="@ShortDescription"/></td>
                </tr>
                <tr>
                  <td><xsl:value-of select="@LongDescription"/></td>
                </tr>
                <tr>
                  <td><xsl:value-of select="@StartPage"/></td>
                </tr>
              </table>
              <br />
              <br />
            </td>
          </tr>
        </xsl:for-each>
        </table>
      </body>
    </HTML>
  </xsl:template>
</xsl:stylesheet>