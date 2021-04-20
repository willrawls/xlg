<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:xlg="urn:xlg" xmlns:exsl="http://exslt.org/common">
<xsl:output method="html" />
<xsl:template match="/xlgDoc">
<xsl:variable name="Namespace" select="/xlgDoc/@Namespace" />
<xsl:variable name="OutputFolder" select="/xlgDoc/@OutputFolder" />
<xsl:variable name="ProjectFile"><xsl:value-of select="concat($OutputFolder,$Namespace)"/>.csproj</xsl:variable>
<xsl:variable name="ASPXFolder"><xsl:value-of select="$OutputFolder"/>ASPX\</xsl:variable>
<xsl:value-of select="xlg:CreateDirectory($ASPXFolder)" />
<xsl:for-each select="Tables/Table">
	<xsl:sort select="@TableName"/>
	<xsl:variable name="TableName" select="@TableName" />
	<xsl:variable name="ClassName" select="@ClassName" />
<xsl:variable name="PageFile"><xsl:value-of select="$ASPXFolder"/><xsl:value-of select="$ClassName" />.aspx</xsl:variable>
<xsl:variable name="CodeBehindFile"><xsl:value-of select="$ASPXFolder"/><xsl:value-of select="$ClassName" />.aspx.cs</xsl:variable>
<exsl:document href="{$PageFile}" method="text" omit-xml-declaration="yes"
>&lt;%@ Page Language="C#" AutoEventWireup="true" CodeFile="<xsl:value-of select="$ClassName" />.aspx.cs" Inherits="<xsl:value-of select="$ClassName" />" %&gt;
&lt;!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"&gt;
&lt;html xmlns="http://www.w3.org/1999/xhtml" &gt;
&lt;head runat="server"&gt;
    &lt;title&gt;Simple <xsl:value-of select="$ClassName" /> Editor&lt;/title&gt;
&lt;/head&gt;
&lt;body&gt;
    &lt;form id="form1" runat="server"&gt;
    &lt;div&gt;
        &lt;asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            DataSourceID="<xsl:value-of select="$ClassName" />DataSource"&gt;
            &lt;Columns&gt;
                &lt;asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" /&gt;
	<xsl:for-each select="Columns/Column">
                &lt;asp:BoundField DataField="<xsl:value-of select="@ColumnName" />" HeaderText="<xsl:value-of select="@PropertyName" />" SortExpression="<xsl:value-of select="@ColumnName" />" /&gt;</xsl:for-each>
            &lt;/Columns&gt;
        &lt;/asp:GridView&gt;
        &lt;asp:ObjectDataSource ID="<xsl:value-of select="$ClassName" />DataSource" runat="server" 
			DataObjectTypeName="<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />"
            DeleteMethod="odsDelete" 
			InsertMethod="odsInsert" SelectMethod="SelectAll" TypeName="<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />"
            UpdateMethod="odsUpdate"&gt;
		&lt;/asp:ObjectDataSource&gt;
    &lt;/div&gt;
    &lt;/form&gt;
&lt;/body&gt;
&lt;/html&gt;
	</exsl:document>
<exsl:document href="{$CodeBehindFile}" method="text" omit-xml-declaration="yes">
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using MetX.Standard;
using MetX.Standard.Data;
using <xsl:value-of select="$Namespace" />;

public partial class <xsl:value-of select="$ClassName" /> : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
</exsl:document>
</xsl:for-each>
</xsl:template>
</xsl:stylesheet>