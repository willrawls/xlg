<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:xlg="urn:xlg" xmlns:exsl="http://exslt.org/common">
	<xsl:output method="text" />
	<xsl:template match="/xlgDoc">
-- ----------------------------------------------------------------
-- Uber Indexing SQL (every column)
-- ----------------------------------------------------------------
		<xsl:for-each select="Tables/Table[Columns/Column[@IsIndexed='False' and @DbType!='Binary' and @MaxLength&gt;-1 and @MaxLength&lt;512]]">
		<xsl:sort select="@TableName"/>
		<xsl:variable name="TableName" select="@TableName" />
		<xsl:variable name="ClassName" select="@ClassName" />

-- ----------------------------------------------------------------
-- <xsl:value-of select="$TableName"/>
		<xsl:for-each select="Columns/Column[@IsIndexed='False' and @DbType!='Binary' and @MaxLength&gt;-1 and @MaxLength&lt;512]">
			<xsl:sort select="@ColumnName"/>
   CREATE NONCLUSTERED INDEX [<xsl:value-of select="$ClassName" />_<xsl:value-of select="@PropertyName" />] ON [dbo].[<xsl:value-of select="$TableName" />] ( [<xsl:value-of select="@ColumnName" />] ASC ) WITH (STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]</xsl:for-each>
GO
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>