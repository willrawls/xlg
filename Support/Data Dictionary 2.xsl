<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:xlg="urn:xlg" xmlns:exsl="http://exslt.org/common">
	<xsl:output method="html" />
	
<xsl:template match="/xlgDoc">
<style>

th td {
  font-weight: bold;
}

table, th, td {
  border: 1px solid black;
  margin: 5px;
  padding: 5px;
}

table {
  border-collapse: collapse;
}

</style>
<body>
<h1>Tables</h1>
	<xsl:for-each select="Tables/Table[not(@RowCount = 0)]">
		<xsl:sort select="@TableName"/>
		<xsl:variable name="TableName" select="@TableName" />

<h3 id="table{$TableName}">
	Table: <xsl:value-of select="$TableName"/>
	<span style="padding: 10px; font-size: 14px;">
	
		: with <xsl:value-of select='format-number(@RowCount, "###,###,###")' /> rows
	</span>
</h3>

	<table>
		<thead>
			<th>Name</th>
			<th>Type</th>
			<th>Length</th>
			<th>Nullable</th>
			<th>Increments</th>
			<th>Foreign key</th>
			<th>Indexed</th>
			<th>Description</th>
		</thead>
		<xsl:for-each select="Columns/Column[not(
				   @ColumnName='CreatedBy' 
				or @ColumnName='CreatedDate' 
				or @ColumnName='CreatedDt' 
				or @ColumnName='CreateDate' 
				or @ColumnName='UpdatedBy' 
				or @ColumnName='UpdatedDate'
				or @ColumnName='UpdatedDt'
				or @ColumnName='UpdateDate'
				)]">
			<xsl:sort select="@Location" data-type="number"/>
			<xsl:variable name="ColumnName" select="@ColumnName" />
		<tr>
			<td>
				<xsl:if test="@IsPrimaryKey='True'"><b style='color: blue;' /></xsl:if>
				<xsl:value-of select="$ColumnName" />
			</td>
			<td><xsl:value-of select="@SourceType" /></td>
			<td>
				<xsl:choose>
				<xsl:when test="@MaxLength='-1'">Max</xsl:when>
				<xsl:when test="not(@MaxLength='0')"><xsl:value-of select="@MaxLength" /></xsl:when>
				</xsl:choose>
			</td>
			<td><xsl:if test="@IsNullable='True'"><xsl:value-of select="@IsNullable" /></xsl:if></td>
			<td><xsl:if test="@AutoIncrement='True'"><xsl:value-of select="@AutoIncrement" /></xsl:if></td>
		
			<td>
				<xsl:if test="@IsForeignKey='True'">
					<xsl:variable name='rawFKT' select='../../Keys/Key[Columns/Column/@Column=$ColumnName]/@Name' />
					<xsl:choose>
					<xsl:when test="contains($rawFKT, 'FK_')">
						<xsl:variable name='minorFKT' select='xlg:SReplace($rawFKT, "FK_","")' />
						<xsl:variable name='FKT' select='xlg:SReplace($minorFKT, concat($TableName, "_"), "")' />
						<xsl:variable name='anchorName' select='concat("table", $FKT)' />
						<a href="#{$anchorName}" title="{$rawFKT}">
							<b style='color: blue;' />
							<xsl:value-of select='$FKT' />	
						</a>
					</xsl:when>
					<xsl:otherwise>
						<b style='color: blue;' />
						<xsl:value-of select='$rawFKT' />	
					</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</td>
			<td><xsl:if test="@IsIndexed='True'"><xsl:value-of select="@IsIndexed" /></xsl:if></td>
			<td><xsl:if test="not(@Description='')"><xsl:value-of select="@Description" /></xsl:if></td>		
		</tr>
		</xsl:for-each>
	</table>
</xsl:for-each>

<hr />

<h1>Stored Procedures</h1>

	<xsl:for-each select="StoredProcedures/StoredProcedure">
		<xsl:sort select="@StoredProcedureName"/>
		<xsl:variable name="StoredProcedureName" select="@StoredProcedureName" />

<h3>Procedure: <xsl:value-of select="$StoredProcedureName"/></h3>
	<xsl:choose>
	<xsl:when test='not(Parameters/Parameter)'>No parameters</xsl:when>
	<xsl:otherwise>
	<table>
		<thead>
			<th>Name</th>
			<th>Type</th>
			<th>Output</th>
		</thead>
		<xsl:for-each select="Parameters/Parameter">
			<xsl:sort select="@Location" data-type="number"/>
		<tr>
			<td><xsl:value-of select="@ParameterName" /></td>
			<td><xsl:value-of select="@CSharpVariableType" /></td>
			<td><xsl:if test="@Output='True'"><xsl:value-of select="@IsOutput" /></xsl:if></td>
		</tr>
		</xsl:for-each>
	</table>
	</xsl:otherwise>
	</xsl:choose>

</xsl:for-each>

</body>
</xsl:template>
</xsl:stylesheet>