<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xlg="urn:xlg" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:ebl="urn:ebay:apis:eBLBaseComponents" exclude-result-prefixes="ebl">
	<!--====================================================================================
	Original version by : Holten Norris ( holtennorris at yahoo.com )
	Current version maintained  by: Alan Lewis (alanlewis at gmail.com)
	Thanks to Venu Reddy from eBay XSLT team for help with the array detection code
	Protected by CDDL open source license.  
	Transforms XML into JavaScript objects, using a JSON format.
	===================================================================================== -->
	<xsl:output method="html" encoding="utf-8" />

  <xsl:template match="/">
    <HTML><BODY><HEAD><SCRIPT language="javascript">
      <xsl:if test="xlg:GetVar('JsonVariableName')='debug'">
  			var <xsl:value-of select="xlg:GetVar('JsonVariableName')" /> =
      </xsl:if>
      <xsl:apply-templates select="*" mode="go" />
      <xsl:if test="xlg:GetVar('JsonVariableName')='debug'">
          var content = document.getElementById('content');
          outputNode(debug.xlgDoc);
          function outputNode(node) {
                content.innerHTML = 'node.ServerPath = ' + node.ServerPath;
          }
      </xsl:if>
    </SCRIPT></HEAD></BODY></HTML>    
  </xsl:template>

  <xsl:template match="*" mode="go">
		<xsl:param name="recursionCnt">0</xsl:param>
		<xsl:param name="isLast">1</xsl:param>
		<xsl:param name="inArray">0</xsl:param>
		<xsl:if test="$recursionCnt=0">{</xsl:if>
		<!-- test what type of data to output  -->
		<xsl:variable name="elementDataType">
			<xsl:value-of select="number(text())"/>
		</xsl:variable>
		<xsl:variable name="elementData">

			<!-- TEXT ( use quotes ) -->
			<xsl:if test="string($elementDataType) ='NaN'">
				<xsl:if test="boolean(text())">
				"<xsl:value-of select="text()"/>"
				</xsl:if>
			</xsl:if>
			<!-- NUMBER (no quotes ) -->
			<xsl:if test="string($elementDataType) !='NaN'">
				<xsl:value-of select="text()"/>

			</xsl:if>
			<!-- NULL -->
			<xsl:if test="not(*)">
				<xsl:if test="not(text())">
					null
				</xsl:if>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="hasRepeatElements">

			<xsl:for-each select="*">
				<xsl:if test="name() = name(preceding-sibling::*) or name() = name(following-sibling::*)">
					true
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:if test="not(count(@*) &gt; 0)">
		 "<xsl:value-of select="local-name()"/>": <xsl:value-of select="$elementData"/>

		</xsl:if>
		<xsl:if test="count(@*) &gt; 0">
		"<xsl:value-of select="local-name()"/>": {
		"content": "<xsl:value-of select="$elementData"/>"
			<xsl:for-each select="@*">
				<xsl:if test="position()=1">,</xsl:if>
				<!-- test what type of data to output  -->
				<xsl:variable name="dataType">

					<xsl:value-of select="number(.)"/>
				</xsl:variable>
				<xsl:variable name="data">
					<!-- TEXT ( use quotes ) -->
					<xsl:if test="string($dataType) ='NaN'">
				"<xsl:value-of select="current()"/>" </xsl:if>
					<!-- NUMBER (no quotes ) -->
					<xsl:if test="string($dataType) !='NaN'">

						<xsl:value-of select="current()"/>
					</xsl:if>
				</xsl:variable>
				<xsl:value-of select="local-name()"/>:<xsl:value-of select="$data"/>
				<xsl:if test="position() !=last()">, </xsl:if>
			</xsl:for-each>
		}
		</xsl:if>

		<xsl:if test="not($hasRepeatElements = '')">
					[{
				</xsl:if>
		<xsl:for-each select="*">
			<xsl:if test="position()=1">
				<xsl:if test="$hasRepeatElements = ''">
					<xsl:text> { </xsl:text>
				</xsl:if>

			</xsl:if>
			<xsl:apply-templates select="current()" mode="go">
				<xsl:with-param name="recursionCnt" select="$recursionCnt+1"/>
				<xsl:with-param name="isLast" select="position()=last()"/>
				<xsl:with-param name="inArray" select="not($hasRepeatElements = '')"/>
			</xsl:apply-templates>
			<xsl:if test="position()=last()">
				<xsl:if test="$hasRepeatElements = ''">
					<xsl:text> } </xsl:text>

				</xsl:if>
			</xsl:if>
		</xsl:for-each>
		<xsl:if test="not($hasRepeatElements = '')">
					}]
				</xsl:if>
		<xsl:if test="not( $isLast )">
			<xsl:if test="$inArray = 'true'">
				<xsl:text> } </xsl:text>

			</xsl:if>
			, 
			<xsl:if test="$inArray = 'true'">
				<xsl:text> { </xsl:text>
			</xsl:if>
		</xsl:if>
		<xsl:if test="$recursionCnt=0"> }; 
    </xsl:if>
	</xsl:template>
</xsl:stylesheet>
