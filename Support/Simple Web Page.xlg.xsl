<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:xlg="urn:xlg" xmlns:exsl="http://exslt.org/common">
	<xsl:output method="html" />
	<xsl:template match="/xlgDoc">
		<html>
			<head>
				<xsl:value-of select="/xlgDoc/@ConnectionStringName"/>
			</head>
			<body>
				<xsl:for-each select="Tables/Table">
					<xsl:sort select="@TableName"/>
					<xsl:variable name="TableName" select="@TableName" />
					<xsl:variable name="ClassName" select="@ClassName" />
					<ul>
						<li>
							<a>
                <xsl:attribute name="href"><xsl:value-of select="$TableName"/>.aspx</xsl:attribute>
                <xsl:value-of select="$TableName"/>
							</a>
							<ul>
								<xsl:for-each select="Columns/Column[@IsIndexed='False' and @DbType!='Binary' and @MaxLength&gt;-1 and @MaxLength&lt;512]">
									<xsl:sort select="@ColumnName"/>
									<li>
										<xsl:value-of select="@ColumnName" />
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