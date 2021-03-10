<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
               
                xmlns:xlg="urn:xlg"
                version="1.0">
  <xsl:output method="html" />
  <xsl:template match="/xlgDoc">
    <HTML>
      <HEAD>
        <title>Edit Permissions</title>
        <LINK REL="StyleSheet" TYPE="text/css" HREF="/xlgSupport/css/Styles.css" />
        <script language="javascript">
          function handleCategoryClick() {
            document.location = 'editPermissions.aspx?Application=<xsl:value-of select="/xlgDoc/Application/@ApplicationName"/>&amp;GroupName=<xsl:value-of select="/xlgDoc/Group/@GroupName"/>&amp;Category=' + document.getElementById('Category').value;
          }
        </script>
      </HEAD>
      <body>
        <H1>XLG Security Group Page Permission Editor</H1>
        <H2>Application: <font color="blue"><xsl:value-of select="/xlgDoc/Application/@ApplicationName"/> </font></H2>
        <H2>Security Group: <font color="blue"><xsl:value-of select="/xlgDoc/Group/@GroupName"/></font></H2>
        <H3><a class="buttonLink"><xsl:attribute name="href">editApplication.aspx?Application=<xsl:value-of select="/xlgDoc/Application/@ApplicationName"/></xsl:attribute>Change Security Groups</a></H3>

				<table border="0" cellpadding="1" cellspacing="1">
					<tr CLASS="headerContent">
              <td style="width: 10px; background-color: white;">&amp;nbsp;</td>
						<td>Resource Name</td>
						<td>Group Permissions</td>
              <td style="width: 10px; background-color: white;">&amp;nbsp;</td>
              <td>
		            <xsl:variable name="uppercase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
		            <xsl:variable name="lowercase">abcdefghijklmnopqrstuvwxyz</xsl:variable>
		            Category Overrides : <select size="1" name="Category" id="Category" onchange="handleCategoryClick();">
                  <xsl:if test="/xlgDoc/@Category=''">
                    <option value="" selected="on"></option>
                  </xsl:if>
			            <xsl:for-each select="/xlgDoc/Categorys/Category">
					            <option>
						            <xsl:attribute name="value"><xsl:value-of select="@Name" /></xsl:attribute>
						            <xsl:if test="normalize-space(translate(string(/xlgDoc/@Category),$lowercase,$uppercase))=normalize-space(translate(@Name,$lowercase,$uppercase))">
							            <xsl:attribute name="selected">on</xsl:attribute>
						            </xsl:if>
						            <xsl:value-of select="@Name" />
					            </option>
			            </xsl:for-each>
		            </select>
              </td>
            </tr>
          <xsl:for-each select="/xlgDoc/Permissions/Permission">
					<tr>
						<xsl:choose>
							<xsl:when test="position() mod 2 = 0"><xsl:attribute name="class">contentDataRow1</xsl:attribute></xsl:when>
							<xsl:otherwise><xsl:attribute name="class">contentDataRow2</xsl:attribute></xsl:otherwise>
						</xsl:choose>
              <td style="width: 10px; background-color: white;">&amp;nbsp;</td>
            <td><b><xsl:value-of select="@PageName"/></b></td>
            <xsl:apply-templates select="." />
            <xsl:if test="/xlgDoc/@Category!=''">
              <td style="width: 10px; background-color: white;">&amp;nbsp;</td>
              <xsl:variable name="pagename" select="@PageName" />
              <xsl:choose>
                <xsl:when test="/xlgDoc/Overrides/Permission[@PageName=$pagename]">
                  <xsl:apply-templates select="/xlgDoc/Overrides/Permission[@PageName=$pagename]" />
                </xsl:when>
                <xsl:otherwise>
                 <td>
                   <a class="buttonLinkLight" title="Click to override group permissions">
                     <xsl:attribute name="href">editPermissions.aspx?Application=<xsl:value-of select="/xlgDoc/Application/@ApplicationName"/>&amp;GroupName=<xsl:value-of select="/xlgDoc/Group/@GroupName"/>&amp;PageName=<xsl:value-of select="@PageName"/>&amp;Category=<xsl:value-of select="/xlgDoc/@Category"/>&amp;PostAction=add&amp;P1=<xsl:value-of select="@P1"/></xsl:attribute>
                     Override
                   </a>
                 </td>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:if>
					</tr>
          </xsl:for-each>
        </table>
        <hr />
        <table border="1" cellpadding="3" cellspacing="2">
          <tr>
            <td valign="top">
              Please use the following commands carefully<br />
              because they affect the entire group's permissions.
            </td>
            <td>
              <input type="submit" name="buttonNone" value=" None " id="buttonNone" /> <br />
              <input type="submit" name="buttonReadOnly" value=" Read " id="buttonReadOnly" /> <br />
              <input type="submit" name="buttonReadWrite" value=" Read &amp;amp; Write " id="buttonReadWrite" /> <br />
              <input type="submit" name="buttonReadWriteExecute" value=" Read, Write &amp;amp; Execute " id="buttonReadWriteExecute" /> <br />
              <input type="submit" name="buttonReadWriteExecuteSpecial" value=" Read, Write, Execute &amp;amp; Special " id="buttonReadWriteExecuteSpecial" /> <br />
            </td>
          </tr>
        </table>
      </body>
    </HTML>
  </xsl:template>
  
  <xsl:template match="Permission">
    <td>
      <table border="0">
        <tr>
      <xsl:call-template name="bit">
        <xsl:with-param name="name">Create</xsl:with-param>
        <xsl:with-param name="value" select="number(16)" />
        <xsl:with-param name="node" select="." />
      </xsl:call-template>
      <xsl:call-template name="bit">
        <xsl:with-param name="name">Read</xsl:with-param>
        <xsl:with-param name="value" select="number(1)" />
        <xsl:with-param name="node" select="." />
      </xsl:call-template>
      <xsl:call-template name="bit">
        <xsl:with-param name="name">Update</xsl:with-param>
        <xsl:with-param name="value" select="number(2)" />
        <xsl:with-param name="node" select="." />
      </xsl:call-template>
      <xsl:call-template name="bit">
        <xsl:with-param name="name">Delete</xsl:with-param>
        <xsl:with-param name="value" select="number(32)" />
        <xsl:with-param name="node" select="." />
      </xsl:call-template>
      <xsl:call-template name="bit">
        <xsl:with-param name="name">Execute</xsl:with-param>
        <xsl:with-param name="value" select="number(4)" />
        <xsl:with-param name="node" select="." />
      </xsl:call-template>
      <xsl:call-template name="bit">
        <xsl:with-param name="name">Special</xsl:with-param>
        <xsl:with-param name="value" select="number(8)" />
        <xsl:with-param name="node" select="." />
      </xsl:call-template>
        </tr>
      </table>
    </td>
  </xsl:template>
  <xsl:template name="bit">
    <xsl:param name="name" />
    <xsl:param name="value" />
    <xsl:param name="node" />
        <td style="width: 20px; height: 20px;">
            <xsl:choose>
              <xsl:when test="xlg:HasBit($node/@P1, $value)">
                <a class="buttonLinkDark" title="Click to Revoke"><xsl:attribute name="href">editPermissions.aspx?PostAction=add&amp;Category=<xsl:value-of select="/xlgDoc/@Category"/>&amp;id=<xsl:value-of select="@PermissionID"/>&amp;add=-<xsl:value-of select="$value" />&amp;Application=<xsl:value-of select="/xlgDoc/Application/@ApplicationName"/>&amp;GroupName=<xsl:value-of select="/xlgDoc/Group/@GroupName"/>&amp;PageName=<xsl:value-of select="@PageName"/></xsl:attribute><xsl:value-of select="$name"/></a>
              </xsl:when>
              <xsl:otherwise>
                <a class="buttonLinkLight" title="Click to Grant"><xsl:attribute name="href">editPermissions.aspx?PostAction=add&amp;Category=<xsl:value-of select="/xlgDoc/@Category"/>&amp;id=<xsl:value-of select="@PermissionID"/>&amp;add=<xsl:value-of select="$value" />&amp;Application=<xsl:value-of select="/xlgDoc/Application/@ApplicationName"/>&amp;GroupName=<xsl:value-of select="/xlgDoc/Group/@GroupName"/>&amp;PageName=<xsl:value-of select="@PageName"/></xsl:attribute><xsl:value-of select="$name"/></a>
              </xsl:otherwise>
            </xsl:choose>
        </td>
  </xsl:template>
</xsl:stylesheet>