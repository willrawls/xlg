<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xlg="urn:xlg" version="1.0">
  <xsl:output method="html" />
  <xsl:template match="/xlgDoc">
    <HTML>
      <HEAD>
        <title>Security Applications</title>
        <LINK REL="StyleSheet" TYPE="text/css" HREF="/xlgSupport/css/Styles.css" />
      </HEAD>
      <body>
		    <H1>XLG Security Administrator</H1>
		    <P>Select an application to edit:</P>
        <table>
        <xsl:for-each select="/xlgDoc/Applications/Application">
          <tr>
            <td width="100">&amp;nbsp;</td>
            <td>
              <a class="buttonLink"><xsl:attribute name="href">editApplication.aspx?Application=<xsl:value-of select="@ApplicationName" /></xsl:attribute><xsl:value-of select="@ApplicationName" /></a>
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