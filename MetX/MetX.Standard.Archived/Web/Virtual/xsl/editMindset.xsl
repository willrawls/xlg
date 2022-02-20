<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xlg="urn:xlg" version="1.0">
	<xsl:output method="html" omit-xml-declaration="yes" indent="yes" />
	<xsl:template match="/">
    <HTML>
      <HEAD>
        <!--<META HTTP-EQUIV="EXPIRES" CONTENT="2/5/99 11:30PM" />-->
        <LINK REL="StyleSheet" TYPE="text/css"><xsl:attribute name="href"><xsl:value-of select="/xlgDoc/@ServerPath"/>xlgSupport/css/Styles.css</xsl:attribute></LINK>
        <link rel="shortcut icon" href="/xlgSupport/images/favicon.ico" />
        <SCRIPT language="JavaScript" src="/xlgSupport/scripts/dhtmlgoodies_menu.js" type="text/javascript"></SCRIPT>
        <SCRIPT language="JavaScript" src="/xlgSupport/scripts/prototype.js" type="text/javascript"></SCRIPT>
        <SCRIPT language="JavaScript" src="/xlgSupport/scripts/prototype-extensions.js" type="text/javascript"></SCRIPT>
        <SCRIPT language="JavaScript">
          function initDhtmlGoodiesMenuContinued()
          {
            $('docarea').src='<xsl:value-of select="xlg:sReplace(xlg:sReplace(/xlgDoc/Mindset/@StartPage, '[UserName]', /xlgDoc/SecureUserPage/@UserName),'[SecurityToken]',/xlgDoc/@SecurityToken)"/>';
          }
        </SCRIPT>
      </HEAD>
      <BODY LEFTMARGIN="0" RIGHTMARGIN="0" TOPMARGIN="0" marginheight="0" marginwidth="0" SCROLL="NO" class="container">
        <div id="mainContainer">
          <div id="dhtmlgoodies_menu">
            <ul>
              <li><a href="/trm" title="Click to refresh entire page (including menu)"><img src="/xlgSupport/images/star.bmp" /></a></li>
              <xsl:apply-templates select="/xlgDoc/Mindset/MindsetMenu" mode="MainMenu" />
              <xsl:apply-templates select="/xlgDoc/Mindset/MindsetMenu" mode="MainSubmenus" />
            </ul>
          </div>
        </div>
        <div id="docareaDIV">
        <IFRAME ID="docarea" NAME="docarea" marginwidth="0" marginheight="0" FRAMEBORDER="1" SCROLLING="YES">
          Hello <xsl:value-of select="/xlgDoc/SecureUserPage/@FullName"/> (Loading...)
        </IFRAME>
        </div>
      </BODY>
    </HTML>
	</xsl:template>
	
	<xsl:template match="MindsetMenu" mode="MainMenu">
		<xsl:variable name="MenuID" select="@MenuID" />
		<xsl:apply-templates select="/xlgDoc/Menus/Menu[@MenuID=$MenuID and @Title='Main']/MenuItem" mode="MainSubmenus" />
	</xsl:template>
	
	<xsl:template match="MindsetMenu" mode="MainSubmenus">
		<xsl:variable name="MenuID" select="@MenuID" />
		<xsl:if test="count(/xlgDoc/Menus/Menu[@MenuID=$MenuID and @Title!='Main' and ((@Requires='1') or (@Requires='2' and /xlgDoc/SecureUserPage/@Update='True') or (@Requires='4' and /xlgDoc/SecureUserPage/@Execute='True') or (@Requires='8' and /xlgDoc/SecureUserPage/@Special='True'))]) &gt; 0">
			<ul>
				<xsl:apply-templates select="/xlgDoc/Menus/Menu[@MenuID=$MenuID and @Title!='Main']" mode="MainSubmenus" />
			</ul>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="Menu" mode="MainSubmenus">
		<xsl:variable name="MenuID" select="@MenuID" />
		<xsl:if test="(@Requires='1') or (@Requires='2' and /xlgDoc/SecureUserPage/@Update='True') or (@Requires='4' and /xlgDoc/SecureUserPage/@Execute='True') or (@Requires='8' and /xlgDoc/SecureUserPage/@Special='True')">
			<xsl:choose>
				<xsl:when test="count(/xlgDoc/Menus/Menu[@MenuID=$MenuID]/MenuItem) &gt; 0">
					<li>
						<a target="docarea">
							<xsl:attribute name="title"><xsl:value-of select="@Description" /></xsl:attribute>
							<xsl:value-of select="@Title" />
						</a>
						<ul>
						<xsl:apply-templates select="/xlgDoc/Menus/Menu[@MenuID=$MenuID]/MenuItem" mode="MainSubmenus" />
						</ul>
					</li>
				</xsl:when>
				<xsl:otherwise>
					<xsl:variable name="ItemID" select="/xlgDoc/Menus/Menu[@MenuID=$MenuID]/MenuItem/@ItemID" />
					<xsl:apply-templates select="/xlgDoc/Urls/Url[@UrlID=$ItemID]" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="MenuItem" mode="MainSubmenus">
		<xsl:variable name="ItemID" select="@ItemID" />
		<xsl:choose>
			<xsl:when test="@IsSubMenu='1'">
						<xsl:apply-templates select="/xlgDoc/Menus/Menu[@MenuID=$ItemID]" mode="MainSubmenus" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="/xlgDoc/Urls/Url[@UrlID=$ItemID]" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="Url">
		<xsl:if test="(@Requires='1') or (@Requires='2' and /xlgDoc/SecureUserPage/@Update='True') or (@Requires='4' and /xlgDoc/SecureUserPage/@Execute='True') or (@Requires='8' and /xlgDoc/SecureUserPage/@Special='True')">
			<li>
				<a target="docarea">
					<xsl:attribute name="href"><xsl:value-of select="xlg:sReplace(xlg:sReplace(xlg:sReplace(@Uri, '[UserName]', /xlgDoc/SecureUserPage/@UserName),'[UserID]', /xlgDoc/SecureUserPage/@UserID), '[SecurityToken]', /xlgDoc/@SecurityToken)" /></xsl:attribute>
					<xsl:attribute name="title"><xsl:value-of select="@Description" /></xsl:attribute>
					<xsl:value-of select="@Title" />
				</a>
			</li>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>