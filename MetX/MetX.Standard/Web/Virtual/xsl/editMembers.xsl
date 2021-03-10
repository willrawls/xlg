<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
               
                xmlns:xlg="urn:xlg"
                version="1.0">
  <xsl:output method="html" />
  <xsl:template match="/xlgDoc">
    <HTML>
      <HEAD>
        <title>Security Group Membership Editor</title>
        <LINK REL="StyleSheet" TYPE="text/css" HREF="/xlgSupport/css/Styles.css" />
        <script language="javascript">
            function HandleAdd()
            {
            }
            
			      function DeleteMember()
			      {
				      var UserName = '';
				      document.forms[0].hDeleteUser.value = "false";
				      if(document.forms[0].listGroupMembers.value.length &gt; 0)
				      {
					      UserName = document.forms[0].listGroupMembers.value;
				      }
				      else if(document.forms[0].listNonGroupMembers.value.length &gt; 0)
				      {
					      UserName = document.forms[0].listNonGroupMembers.value;
				      }
				      if(UserName.length &gt; 0)
				      {
					      var message = "Permanently delete the '" + UserName + "' security profile ? (type 'yes' and press enter to continue)";
					      if(prompt(message, 'no') == 'yes')
					      {
						      document.forms[0].hDeleteUser.value = "true";
						      document.forms[0].submit();
					      }
				      }
			      }
		      </script>
      </HEAD>
      <body>
		    <H1>XLG Security Group Membership Editor</H1>
		    <H2>Application: <font color="blue"><xsl:value-of select="/xlgDoc/Application/@ApplicationName"/></font></H2>
		    <H2>Security Group: <font color="blue"><xsl:value-of select="/xlgDoc/Group/@GroupName"/></font></H2>
        <H3><a class="buttonLink"><xsl:attribute name="href">editApplication.aspx?Application=<xsl:value-of select="/xlgDoc/Application/@ApplicationName"/></xsl:attribute> Change Security Groups</a></H3>
			<p>
				<table border="0" cellpadding="3" cellspacing="1">
					<tr CLASS="HeaderContent">
						<td style="width: 188px">Security Group Members</td>
            <td style="width: 20px; background-color: white;"></td>
						<td style="width: 188px">Other Members</td>
            <td style="width: 20px; background-color: white;"></td>
            <td style="width: 188px;">Add a new user to this group</td>
					</tr>
					<tr>
						<td valign="top" style="width: 188px">
              <table border="1" cellpadding="3" cellspacing="1">
                 <tr class="subHeaderContent">
                  <td width="100">&amp;nbsp;</td>
                   <td>Remove from Security Group</td>
                   <td>User Name</td>
                   <td>Full Name</td>
                   <td>Category</td>
                 </tr>
              <xsl:for-each select="/xlgDoc/Members/User[@GroupName=/xlgDoc/Group/@GroupName]">
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
                    <a class="buttonLink"><xsl:attribute name="href">editMembers.aspx?UserName=<xsl:value-of select="@UserName" />&amp;Application=<xsl:value-of select="/xlgDoc/Application/@ApplicationName" />&amp;GroupName=<xsl:value-of select="/xlgDoc/Group/@GroupName" />&amp;&amp;PostAction=removemember</xsl:attribute>Remove</a>
                  </td>
                  <td nowrap="nowrap"><xsl:value-of select="@UserName" /></td>
                  <td nowrap="nowrap"><xsl:value-of select="@FullName" /></td>
                  <td nowrap="nowrap"><xsl:value-of select="@Category" /></td>
                </tr>
              </xsl:for-each>
              </table>
            </td>
            <td style="width: 20px; background-color: white;"></td>
						<td valign="top" style="width: 188px">
              <table border="1" cellpadding="3" cellspacing="1">
                 <tr class="subHeaderContent">
                  <td width="100">&amp;nbsp;</td>
                   <td>Add to Security Group</td>
                   <td>User Name</td>
                   <td>Full Name</td>
                   <td>Security Group</td>
                   <td>Category</td>
                 </tr>
              <xsl:for-each select="/xlgDoc/Members/User[@GroupName!=/xlgDoc/Group/@GroupName]">
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
                    <a class="buttonLink"><xsl:attribute name="href">editMembers.aspx?UserName=<xsl:value-of select="@UserName" />&amp;Application=<xsl:value-of select="/xlgDoc/Application/@ApplicationName" />&amp;GroupName=<xsl:value-of select="/xlgDoc/Group/@GroupName" />&amp;PostAction=addmember</xsl:attribute>Add</a>
                  </td>
                  <td nowrap="nowrap"><xsl:value-of select="@UserName" /></td>
                  <td nowrap="nowrap"><xsl:value-of select="@FullName" /></td>
                  <td nowrap="nowrap"><xsl:value-of select="@GroupName" /></td>
                  <td nowrap="nowrap"><xsl:value-of select="@Category" /></td>
                </tr>
              </xsl:for-each>
              </table>
            </td>
            <td style="width: 20px; background-color: white;"></td>
            <td valign="top" style="width: 188px">
	            <form method="post" action="editMember.aspx">
                <input type="hidden" name="PostAction" value="addnewuser" />
			            <table border="1" cellpadding="3" cellspacing="1">
				            <tr bgcolor="#ffcc00">
					            <td colspan="2" style="WIDTH: 349px">
                        <input type="text" ID="txtUserName" Width="365px" />
                      </td>
					            <td colspan="2" style="WIDTH: 349px">
                        <input type="button" value="Add New User" onclick="HandleAdd();" />
                      </td>
				            </tr>
			            </table>
	            </form>
            </td>
					</tr>
				</table>
			</p>
  
      </body>
    </HTML>
  </xsl:template>
</xsl:stylesheet>