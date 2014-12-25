<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:xlg="urn:xlg" xmlns:exsl="http://exslt.org/common">
	<xsl:output method="html" />
	<xsl:template match="/xlgSqlToXml">
		<html>
			<body>
				<xsl:for-each select="Mail[@sent_status!='sent']">
					<ul>
						<li>
              <xsl:value-of select="xlg:GetToken(xlg:GetToken(xlg:GetToken(@body,2,'&lt;body&gt;'),2,'&lt;p&gt;'),1,'&lt;/p&gt;')" />
              <xsl:value-of select="@recipients"/><br/>
              <xsl:value-of select="@sent_status"/><xsl:text> </xsl:text><xsl:value-of select="xlg:sXmlDate(@sent_date)"/><br/>
							<ul>
								<xsl:for-each select="Log">
									<li>
										<xsl:value-of select="xlg:sXmlDate(@log_date)" /><br/>
										<xsl:value-of select="xlg:GetToken(@description,2,'Exception Message: ')" />
									</li>
								</xsl:for-each>
							</ul>
						</li>
					</ul>
				</xsl:for-each>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>