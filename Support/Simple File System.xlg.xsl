<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:xlg="urn:xlg" xmlns:exsl="http://exslt.org/common">
	<xsl:output method="html" />
	<xsl:template match="/xlgFolder">
		<html>
			<head>
				<xsl:value-of select="/xlgFolder/@ConnectionStringName"/>
			</head>
			<body>
        <xsl:apply-templates select="Folders/Folder" />
			</body>
		</html>
	</xsl:template>
	<xsl:template match="Folder">
					<ul>
						<li>
							<a>
                <xsl:value-of select="@Name"/>
							</a>
							<ul>
								<xsl:for-each select="Files/File">
									<li>
										<xsl:value-of select="@Name" /> - <xsl:value-of select="@Size" />
									</li>
								</xsl:for-each>
							</ul>
							<xsl:apply-templates select="Folders/Folder" />
						</li>
					</ul>
	</xsl:template>
</xsl:stylesheet>