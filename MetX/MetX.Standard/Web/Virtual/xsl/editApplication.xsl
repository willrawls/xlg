<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
               
                xmlns:xlg="urn:xlg"
                version="1.0">
  <xsl:output method="html" />
  <xsl:template match="/xlgDoc">
    <HTML>
      <HEAD>
        <title>Security Application Editor</title>
        <LINK REL="StyleSheet" TYPE="text/css" HREF="/xlgSupport/css/Styles.css" />
      </HEAD>
      <body>
		    <H1>XLG Security Administrator</H1>
		    <H2>Application: <font color="blue"><xsl:value-of select="/xlgDoc/Application/@ApplicationName"/></font></H2>
        <H3><a class="buttonLink" href="applications.aspx">Change Security Applications</a> | <a class="buttonLink"><xsl:attribute name="href">editCookie.aspx?Application=<xsl:value-of select="/xlgDoc/Application/@ApplicationName"/></xsl:attribute> Set Security Cookie</a></H3>
        
			<p>
				<table border="1" cellpadding="3" cellspacing="1">
					<tr CLASS="HeaderContent">
						<td style="width: 188px">Security Groups</td>
					</tr>
					<tr>
						<td style="width: 188px">
              <table border="1" cellpadding="3" cellspacing="1">
              <xsl:for-each select="/xlgDoc/Groups/Group">
                <tr>
                  <td width="100">&amp;nbsp;</td>
                  <td nowrap="nowrap"><xsl:value-of select="@GroupName" /></td>
                  <td>
                    <a class="buttonLink"><xsl:attribute name="href">editMembers.aspx?Application=<xsl:value-of select="/xlgDoc/Application/@ApplicationName" />&amp;GroupName=<xsl:value-of select="@GroupName" /></xsl:attribute>Edit&amp;nbsp;Members</a>
                  </td>
                  <td>
                    <a class="buttonLink"><xsl:attribute name="href">editPermissions.aspx?Application=<xsl:value-of select="/xlgDoc/Application/@ApplicationName" />&amp;GroupName=<xsl:value-of select="@GroupName" /></xsl:attribute>Edit&amp;nbsp;Permissions</a>
                  </td>
                </tr>
              </xsl:for-each>
              </table>
            </td>
					</tr>
				</table>
			</p>
		<form method="post" action="editAppliation.aspx">
      <input type="hidden" name="PostAction" value="AddGroup" />
				<table border="1" cellpadding="3" cellspacing="1">
					<tr CLASS="subHeaderContent">
						<td>New Group Name:</td>
						<td><input type="text" style="width: 200px;" ID="NewGroupName" /></td>
						<td><input type="submit" value="Add New Group" /></td>
					</tr>
				</table>
		</form>
				<br />
		<form method="post" action="editAppliation.aspx">
      <input type="hidden" name="PostAction" value="UpdateApplication" />
				<table border="1" cellpadding="3" cellspacing="1">
					<tr CLASS="subHeaderContent">
						<td>Administrator's Email Addresss:</td>
						<td>
              <input type="text" name="Email" style="width: 400px;" maxlength="50">
                <xsl:attribute name="value"><xsl:value-of select="/xlgDoc/Application/@AdministratorEmail"/></xsl:attribute>
              </input>
            </td>
					</tr>
					<tr CLASS="subHeaderContent">
						<td>Profile Edit URL:</td>
						<td>
              <input type="text" name="ProfileURL" style="width: 400px;" maxlength="50">
                <xsl:attribute name="value"><xsl:value-of select="/xlgDoc/Application/@ProfileEditUrl"/></xsl:attribute>
              </input>
            </td>
					</tr>
					<tr CLASS="subHeaderContent">
						<td colspan="2">
              <input type="submit" value="Update Application" />
						</td>
					</tr>
				</table>
		</form>        
      </body>
    </HTML>
  </xsl:template>
</xsl:stylesheet>