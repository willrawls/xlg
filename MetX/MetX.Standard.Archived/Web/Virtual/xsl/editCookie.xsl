<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
               
                xmlns:xlg="urn:xlg"
                version="1.0">
  <xsl:output method="html" />
  <xsl:template match="/xlgDoc">
    <HTML>
      <HEAD>
        <title>Security Cookie Editor</title>
        <LINK REL="StyleSheet" TYPE="text/css" HREF="/xlgSupport/css/Styles.css" />
      </HEAD>
      <body>
		    <H1>Security Cookie Editor</H1>
		    <H2>Application: <font color="blue"><xsl:value-of select="/xlgDoc/@Application"/></font></H2>
        <H3><a class="buttonLink"><xsl:attribute name="href">editApplication.aspx?Application=<xsl:value-of select="/xlgDoc/@Application"/></xsl:attribute>Return to Security Application</a></H3>
        <H2>Select the user to set the security cookie to</H2>
        <H3>Current Cookie Value (<xsl:value-of select="/xlgDoc/@CookieName"/>) = '<xsl:value-of select="/xlgDoc/@CookieValue"/>'</H3>
        <table border="1" cellpadding="3" cellspacing="1">
           <tr class="subHeaderContent">
            <td width="100">&amp;nbsp;</td>
             <td>Set cookie to this user</td>
             <td>User Name</td>
             <td>Full Name</td>
             <td>Category</td>
           </tr>
        <xsl:if test="/xlgDoc/@Application='%%YOURAPPNAME%%'">
           <tr>
            <td></td>
             <td><a class="buttonLink" href="editCookie.aspx?Application=%%YOURAPPNAME%%&amp;UserName=%%YOURUSERNAME%%&amp;PostAction=setcookie">Set</a></td>
             <td>%%YOURUSERNAME%%</td>
             <td>William M. Rawls</td>
             <td></td>
           </tr>
           <tr>
            <td></td>
             <td><a class="buttonLink" href="editCookie.aspx?Application=%%YOURAPPNAME%%&amp;UserName=%%YOURUSERID%%&amp;PostAction=setcookie">Set</a></td>
             <td>%%YOURUSERID%%</td>
             <td>Rob Reisinger</td>
             <td></td>
           </tr>
           <tr>
            <td></td>
             <td><a class="buttonLink" href="editCookie.aspx?Application=%%YOURAPPNAME%%&amp;UserName=admin&amp;PostAction=setcookie">Set</a></td>
             <td>admin</td>
             <td>AcademyX admin</td>
             <td></td>
           </tr>
           <tr>
            <td></td>
             <td><a class="buttonLink" href="editCookie.aspx?Application=%%YOURAPPNAME%%&amp;UserName=10005&amp;PostAction=setcookie">Set</a></td>
             <td>10005</td>
             <td>Jeff Allen</td>
             <td></td>
           </tr>
        </xsl:if>
        <xsl:for-each select="/xlgDoc/Members/User">
          <tr>
						<xsl:choose>
							<xsl:when test="position() mod 2 = 0">
								<xsl:attribute name="class">contentDataRow1</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="class">contentDataRow2</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
            <td></td>
            <td>
              <a class="buttonLink"><xsl:attribute name="href">editCookie.aspx?UserName=<xsl:value-of select="@UserName" />&amp;PostAction=setcookie</xsl:attribute>Set</a>
            </td>
            <td nowrap="nowrap"><xsl:value-of select="@UserName" /></td>
            <td nowrap="nowrap"><xsl:value-of select="@FullName" /></td>
            <td nowrap="nowrap"><xsl:value-of select="@Category" /></td>
          </tr>
        </xsl:for-each>
        </table>
      </body>
    </HTML>
  </xsl:template>
</xsl:stylesheet>