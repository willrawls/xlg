<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:xlg="urn:xlg" xmlns:exsl="http://exslt.org/common">
<xsl:output method="text" />
<xsl:template match="/xlgDoc">
<!-- 
The skeletal XML generated internally is:
  <xlgDoc>
    <Tables>
      <Table TableName="" ClassName="" PrimaryKeyColumnName="">
	      <Columns>
		      <Column ColumnName="" MaxLength="" PropertyName="" CSharpVariableType="" VBVariableType="" AuditField="" DbType="" AutoIncrement="" IsForiegnKey="" IsNullable="" IsPrimaryKey="" />
	      </Columns>
      </Table>
    </Tables>
    <StoredProcedures ClassName="">
      <StoredProcedure StoredProcedureName="" MethodName="">
	      <Parameter DataType="" CSharpVariableType="" VBVariableType="" ParameterName="" VariableName="" />
      </StoredProcedure>
    </StoredProcedures>
    <XslEndpoints>
	    <XslEndpoint Filename="" Classname="" />
    </XslEndpoints>
  </xlgData>
-->
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using System.IO;

using MetX;
using MetX.IO;
using MetX.Security;
using MetX.Standard.Data;

namespace <xsl:value-of select="@Namespace" />
{
<xsl:if test="/xlgDoc/XslEndpoints/XslEndpoint or /xlgDoc/XslEndpoints/XslEndpoints/XslEndpoint">
  <xsl:call-template name="XslEndpointsClass" />
</xsl:if>
<xsl:if test="Tables or StoredProcedures">
  public static class DB
  {
    private static <xsl:value-of select="/xlgDoc/@DatabaseProvider"/> InternalInstance;
    /// &lt;summary&gt;Get or sets the <xsl:value-of select="/xlgDoc/@ConnectionStringName"/> DataProvider. If one was not previously set (which is normal), a DataProvider will be created from the connection string named "<xsl:value-of select="/xlgDoc/@ConnectionStringName"/>" will be used&lt;/summary&gt;
	public static <xsl:value-of select="/xlgDoc/@DatabaseProvider"/> Instance
	{
		get
		{
			if(InternalInstance == null)
				InternalInstance = (<xsl:value-of select="/xlgDoc/@DatabaseProvider"/>) DataService.GetProvider("<xsl:value-of select="/xlgDoc/@ConnectionStringName"/>");
			return InternalInstance;
		}
		set
		{
			InternalInstance = value;
		}
	}
  }
	<xsl:apply-templates select="Tables/Table" mode="Class" />
	<xsl:apply-templates select="StoredProcedures" mode="Class" />
}
	<xsl:call-template name="CreateProjectFiles" />


<!--
// ================================
// Uber Indexing SQL (every column)
// ================================

	<xsl:apply-templates select="Tables/Table" mode="UberIndexing" />
-->
</xsl:if>
</xsl:template>

<!--
<xsl:template match="Table" mode="UberIndexing">
/* 
	<xsl:for-each select="Tables/Table[Columns/Column[@IsIndexed='False' and @DbType!='Binary' and @MaxLength&gt;-1 and @MaxLength&lt;512]]">
		<xsl:sort select="@TableName"/>
		<xsl:variable name="TableName" select="@TableName" />
		<xsl:variable name="ClassName" select="@ClassName" />

- - <xsl:value-of select="$TableName"/>
		<xsl:for-each select="Columns/Column[@IsIndexed='False' and @DbType!='Binary' and @MaxLength&gt;-1 and @MaxLength&lt;512]">
			<xsl:sort select="@ColumnName"/>
   CREATE NONCLUSTERED INDEX [<xsl:value-of select="$ClassName" />_<xsl:value-of select="@PropertyName" />] ON [dbo].[<xsl:value-of select="$TableName" />] ( [<xsl:value-of select="@ColumnName" />] ASC ) WITH (STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]</xsl:for-each>
GO
</xsl:for-each>
*/
</xsl:template>
-->

<xsl:template name="CreateProjectFiles">
  <xsl:variable name="Namespace" select="/xlgDoc/@Namespace" />
  <xsl:variable name="OutputFolder" select="/xlgDoc/@OutputFolder" />
	<xsl:variable name="ProjectFile"><xsl:value-of select="concat($OutputFolder,$Namespace)"/>.csproj</xsl:variable>
	<xsl:variable name="PropertiesFolder"><xsl:value-of select="$OutputFolder"/>Properties\</xsl:variable>
	<xsl:variable name="AssemblyInfoFile"><xsl:value-of select="$PropertiesFolder"/>AssemblyInfo.cs</xsl:variable>
	<xsl:value-of select="xlg:CreateDirectory($PropertiesFolder)" />
	<xsl:if test="not(xlg:FileExists($AssemblyInfoFile))">
<exsl:document href="{$AssemblyInfoFile}" method="text" omit-xml-declaration="yes"
>using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("<xsl:value-of select="$Namespace" />")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Your Company Here")]
[assembly: AssemblyProduct("<xsl:value-of select="$Namespace" />")]
[assembly: AssemblyCopyright("Copyright © Your Name Here")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("<xsl:value-of select="/xlgDoc/@XlgInstanceID" />")]
[assembly: AssemblyVersion("1.0.0.*")]
[assembly: AssemblyFileVersion("1.0.0.*")]
</exsl:document>
	</xsl:if>
	<xsl:if test="not(xlg:FileExists($ProjectFile))">
<exsl:document href="{$ProjectFile}" method="text" omit-xml-declaration="yes"
>&lt;Project DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003"&gt;
  &lt;PropertyGroup&gt;
    &lt;Configuration Condition=" '$(Configuration)' == '' "&gt;Debug&lt;/Configuration&gt;
    &lt;Platform Condition=" '$(Platform)' == '' "&gt;AnyCPU&lt;/Platform&gt;
    &lt;ProductVersion&gt;8.0.50727&lt;/ProductVersion&gt;
    &lt;SchemaVersion&gt;2.0&lt;/SchemaVersion&gt;
    &lt;ProjectGuid&gt;{<xsl:value-of select="/xlgDoc/@XlgInstanceID" />}&lt;/ProjectGuid&gt;
    &lt;OutputType&gt;Library&lt;/OutputType&gt;
    &lt;AppDesignerFolder&gt;Properties&lt;/AppDesignerFolder&gt;
    &lt;RootNamespace&gt;<xsl:value-of select="$Namespace" />&lt;/RootNamespace&gt;
    &lt;AssemblyName&gt;xlg.DAL.<xsl:value-of select="$Namespace" />&lt;/AssemblyName&gt;
  &lt;/PropertyGroup&gt;
  &lt;PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "&gt;
    &lt;DebugSymbols&gt;true&lt;/DebugSymbols&gt;
    &lt;DebugType&gt;full&lt;/DebugType&gt;
    &lt;Optimize&gt;false&lt;/Optimize&gt;
    &lt;OutputPath&gt;bin\Debug\&lt;/OutputPath&gt;
    &lt;DefineConstants&gt;DEBUG;TRACE&lt;/DefineConstants&gt;
    &lt;ErrorReport&gt;prompt&lt;/ErrorReport&gt;
    &lt;WarningLevel&gt;4&lt;/WarningLevel&gt;
  &lt;/PropertyGroup&gt;
  &lt;PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "&gt;
    &lt;DebugType&gt;pdbonly&lt;/DebugType&gt;
    &lt;Optimize&gt;true&lt;/Optimize&gt;
    &lt;OutputPath&gt;bin\Release\&lt;/OutputPath&gt;
    &lt;DefineConstants&gt;TRACE&lt;/DefineConstants&gt;
    &lt;ErrorReport&gt;prompt&lt;/ErrorReport&gt;
    &lt;WarningLevel&gt;4&lt;/WarningLevel&gt;
  &lt;/PropertyGroup&gt;
  &lt;ItemGroup&gt;
    &lt;Reference Include="<xsl:value-of select="/xlgDoc/@MetXAssemblyString" />, processorArchitecture=MSIL"&gt;
      &lt;SpecificVersion&gt;False&lt;/SpecificVersion&gt;
      &lt;HintPath&gt;..\..\MetX\MetX\bin\Debug\MetX.dll&lt;/HintPath&gt;
    &lt;/Reference&gt;
    &lt;Reference Include="<xsl:value-of select="/xlgDoc/@MetXProviderAssemblyString" />, processorArchitecture=MSIL"&gt;
      &lt;SpecificVersion&gt;False&lt;/SpecificVersion&gt;
      &lt;HintPath&gt;..\..\MetX\MetX.Glove.Console\bin\Debug\<xsl:value-of select="/xlgDoc/@MetXObjectName"/>.dll&lt;/HintPath&gt;
    &lt;/Reference&gt;
    &lt;Reference Include="System" /&gt;
    &lt;Reference Include="System.configuration" /&gt;
    &lt;Reference Include="System.Data" /&gt;
    &lt;Reference Include="System.Xml" /&gt;
    <xsl:choose>
    <xsl:when test="/xlgDoc/@DatabaseProvider='SybaseDataProvider'"
    >&lt;Reference Include="<xsl:value-of select="/xlgDoc/@ProviderAssemblyString" />, processorArchitecture=MSIL"&gt;
      &lt;SpecificVersion&gt;False&lt;/SpecificVersion&gt;
      &lt;HintPath&gt;..\..\MetX\MetX.Glove.Console\bin\Debug\Sybase.Data.AseClient.dll&lt;/HintPath&gt;
    &lt;/Reference&gt;
    </xsl:when>
    <xsl:when test="/xlgDoc/@DatabaseProvider='MySqlDataProvider'"
	>&lt;Reference Include="Mysql.Data" /&gt;
    </xsl:when>
    </xsl:choose>
  &lt;/ItemGroup&gt;
  &lt;ItemGroup&gt;
    &lt;Compile Include="<xsl:value-of select="$Namespace" />.Glove.cs" /&gt;
  <xsl:for-each select="Tables/Table">
    &lt;Compile Include="<xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" />.Glove.cs" /&gt;</xsl:for-each>
    &lt;Compile Include="Properties\AssemblyInfo.cs" /&gt;
  &lt;/ItemGroup&gt;
  &lt;Import Project="$(MSBuildBinPath)\Microsoft.CSharp.targets" /&gt;
  &lt;!-- To modify your build process, add your task inside one of the targets below and uncomment it. 
       Other similar extension points exist, see Microsoft.Common.targets.
  &lt;Target Name="BeforeBuild"&gt;
  &lt;/Target&gt;
  &lt;Target Name="AfterBuild"&gt;
  &lt;/Target&gt;
  --&gt;
&lt;/Project&gt;
</exsl:document>
	</xsl:if>
</xsl:template>
	
<xsl:template match="StoredProcedures" mode="Class">
// -----------------------------
// Stored Procedure classes
// -----------------------------
	public static class SP
	{
<xsl:for-each select="StoredProcedure">
			/// &lt;summary&gt;Calls the <xsl:value-of select="@MethodName" /> stored procedure returning a StoredProcedureResult which contains a DataReader and any returned value&lt;/summary&gt;
            public static StoredProcedureResult <xsl:value-of select="@MethodName" />(<xsl:for-each select="Parameters/Parameter"><xsl:if test="position()!=1">,</xsl:if><xsl:text> </xsl:text><xsl:if test="@IsOutput='True'">ref<xsl:text> </xsl:text></xsl:if><xsl:value-of select="@CSharpVariableType"/><xsl:text> </xsl:text><xsl:value-of select="@VariableName"/></xsl:for-each>)
            {
                QueryCommand cmd=new QueryCommand("<xsl:value-of select="@StoredProcedureName" />");
                cmd.CommandType=CommandType.StoredProcedure;
<xsl:for-each select="Parameters/Parameter">
                cmd.AddParameter("<xsl:value-of select="@ParameterName"/>",<xsl:value-of select="@VariableName"/>, DbType.<xsl:value-of select="@DataType" /><xsl:choose><xsl:when test="@IsOutput='True' and @IsInput!='True'">, ParameterDirection.Output</xsl:when><xsl:when test="@IsOutput='True' and @IsInput='True'">, ParameterDirection.InputOutput</xsl:when></xsl:choose>);</xsl:for-each>
                StoredProcedureResult __ret = <xsl:value-of select="/xlgDoc/@Namespace" />.DB.Instance.GetStoredProcedureResult(cmd);
				<xsl:for-each select="Parameters/Parameter[@IsOutput='True']">
				<xsl:value-of select="@VariableName"/> = <xsl:value-of select="@CovertToPart" />(((IDataParameter)__ret.Parameters["<xsl:value-of select="@ParameterName"/>"]).Value);</xsl:for-each>
                return __ret;
            }
</xsl:for-each>
	}
</xsl:template>
<xsl:template name="XslEndpointsClass">
<!--
	public class xlgGlobal : HttpApplicationErrorHandler
	{
		public xlgGlobal() : base("XslEndpointsClass") { }
	}

  public class HandlerFactory : xlgHandlerFactory
  {
		public override IHttpHandler GetXlgHandler(string url, string pagePath)
    {
        switch (pagePath)
        {
<xsl:for-each select="/xlgDoc/XslEndpoints">
  <xsl:call-template name="XslEndpointsCases">
    <xsl:with-param name="nodes" select="." />
  </xsl:call-template>
</xsl:for-each>
        }
        return null;
    }
	}
-->
</xsl:template>
<xsl:template name="XslEndpointsCases">
  <xsl:param name="nodes" />
  <xsl:param name="path" />
  <xsl:param name="namespace" />
<xsl:for-each select="$nodes/XslEndpoint">
                case "<xsl:value-of select="xlg:Lower($path)"/><xsl:value-of select="xlg:Lower(@Filepart)"/>": return new <xsl:if test="$namespace!=''"><xsl:value-of select="$namespace"/>.</xsl:if><xsl:value-of select="@ClassName"/>();</xsl:for-each>
<xsl:if test="$nodes/XslEndpoints">
  <xsl:for-each select="$nodes/XslEndpoints">
    <xsl:call-template name="XslEndpointsCases">
      <xsl:with-param name="nodes" select="." />
      <xsl:with-param name="path"><xsl:if test="$path!=''"><xsl:value-of select="$path" />/</xsl:if><xsl:value-of select="xlg:Lower(@Folder)"/>/</xsl:with-param>
      <xsl:with-param name="namespace"><xsl:if test="$namespace"><xsl:value-of select="$namespace" />.</xsl:if><xsl:value-of select="@Folder"/></xsl:with-param>
    </xsl:call-template>
  </xsl:for-each>
</xsl:if>
</xsl:template>
<xsl:template match="Table" mode="Class">
  <xsl:variable name="CurrTable" select="." />
  <xsl:variable name="Namespace" select="/xlgDoc/@Namespace" />
  <xsl:variable name="OutputFolder" select="/xlgDoc/@OutputFolder" />
  <xsl:variable name="ClassName" select="@ClassName" />
  <xsl:variable name="TableName" select="@TableName" />
  <xsl:variable name="PrimaryKeyColumnName" select="@PrimaryKeyColumnName" />
  <xsl:variable name="PrimaryKeyColumn" select="Columns/Column[@ColumnName=$PrimaryKeyColumnName or @PropertyName=$PrimaryKeyColumnName]" />
  <xsl:variable name="IdentifierName"><xsl:choose>
    <xsl:when test="/xlgDoc/@DatabaseProvider='SybaseDataProvider'"><xsl:value-of select="@TableName"/></xsl:when>
    <xsl:otherwise>[<xsl:value-of select="@TableName"/>]</xsl:otherwise>
  </xsl:choose></xsl:variable>
  <xsl:variable name="BaseItemClass"><xsl:choose>
    <xsl:when test="/xlgDoc/@BaseItemClass"><xsl:value-of select="/xlgDoc/@BaseItemClass"/></xsl:when>
    <xsl:otherwise>MetX.Standard.Data.ActiveRecord</xsl:otherwise>
  </xsl:choose></xsl:variable>
  <xsl:variable name="BaseListClass"><xsl:choose>
    <xsl:when test="/xlgDoc/@BaseListClass"><xsl:value-of select="/xlgDoc/@BaseListClass"/></xsl:when>
    <xsl:otherwise>MetX.Standard.Data.ActiveList&lt;<xsl:value-of select="$BaseItemClass"/>&gt;</xsl:otherwise>
  </xsl:choose></xsl:variable>
<!-- This next line (exsl:template) causes all xsl output from inside the exsl:document tag to be written to the named file. -->
<exsl:document href="{$OutputFolder}\{$Namespace}.{$ClassName}.Glove.cs" method="text" omit-xml-declaration="yes">
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using System.IO;

using MetX;
using MetX.IO;
using MetX.Security;
using MetX.Standard.Data;

namespace <xsl:value-of select="$Namespace" />
{
	[Serializable, XmlType(AnonymousType = true)]
    /// &lt;summary&gt;Represents a single record from <xsl:value-of select="$TableName" />. Follows some conventions of the Active Record Pattern and some from LINQ acting as a middle ground for organizations stuck in the .net 2.0 world.&lt;/summary&gt;
    public partial class <xsl:value-of select="@ClassName" /> : <xsl:value-of select="$BaseItemClass"/>
    {
      private static readonly object _SyncRoot = new object();
	  private static XmlSerializer xs<xsl:value-of select="@ClassName" />;
	  private static XmlSerializerNamespaces ns;

      /// &lt;summary&gt;The name of the table this object is designed to connect to&lt;/summary&gt;
      public const string FullTableName = "<xsl:value-of select="@TableName"/>";

      /// &lt;summary&gt;Returns this class' name: <xsl:value-of select="@ClassName" />&lt;/summary&gt;
      public override string _ClassName() { return "<xsl:value-of select="@ClassName" />"; }

      <xsl:for-each select="Columns/Column">
      <xsl:value-of select="@CSharpVariableType" /> mv_<xsl:value-of select="@PropertyName" />;
	  </xsl:for-each>

      <xsl:for-each select="Columns/Column">
      <xsl:choose><xsl:when test="@CSharpVariableType='string' and @MaxLength &gt; 0">
      /// &lt;summary&gt;Get or set the value of the field: <xsl:value-of select="@ColumnName" />&lt;/summary&gt;
      [XmlAttribute] public string <xsl:value-of select="@PropertyName" /> { get { return mv_<xsl:value-of select="@PropertyName" />; } 
	  set { 
		if(string.IsNullOrEmpty(value)) mv_<xsl:value-of select="@PropertyName" /> = null; 
		else if(value.Length &lt;= <xsl:value-of select="@MaxLength" />) mv_<xsl:value-of select="@PropertyName" /> = value; 
		else mv_<xsl:value-of select="@PropertyName" /> = value.Substring(0, <xsl:value-of select="@MaxLength" />); } }
	  </xsl:when><xsl:otherwise>
      /// &lt;summary&gt;Get or set the value of the field: <xsl:value-of select="@ColumnName" />&lt;/summary&gt;
      [XmlAttribute] public <xsl:value-of select="@CSharpVariableType" /><xsl:text> </xsl:text><xsl:value-of select="@PropertyName" /> { get { return mv_<xsl:value-of select="@PropertyName" />; } set { mv_<xsl:value-of select="@PropertyName" /> = value; } }
	  </xsl:otherwise></xsl:choose></xsl:for-each>

	  /// &lt;summary&gt;String representations of the fields in the <xsl:value-of select="$TableName" /> table&lt;/summary&gt;
      public static class Columns{<xsl:for-each select="Columns/Column">
        public const string <xsl:value-of select="@PropertyName" /> = "<xsl:value-of select="@ColumnName" />";</xsl:for-each>
      }

      /// &lt;summary&gt;A read-only representation of the schema of the table: <xsl:value-of select="$TableName" />&lt;/summary&gt;
      private static TableSchema.Table InternalTable;
      /// &lt;summary&gt;Returns a TableSchema.Table object representing the schema of the table including fields, their types, and lengths. Used by underlying classes to build queries.&lt;/summary&gt;
      public static TableSchema.Table Schema
      {
        get
        {
		  if (InternalTable == null)
		  {
			  lock(typeof(<xsl:value-of select="@ClassName" />))
			  {
				  if (InternalTable == null)
				  {
					  InternalTable = new TableSchema.Table(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName);
					  <xsl:for-each select="Columns/Column">
					  InternalTable.Columns.Add(new TableSchema.TableColumn(Columns.<xsl:value-of select="@PropertyName" />, System.Data.DbType.<xsl:value-of select="@DbType" />, <xsl:value-of select="xlg:Lower(@AutoIncrement)" />, <xsl:value-of select="@MaxLength" />, <xsl:value-of select="xlg:Lower(@IsNullable)" />, <xsl:value-of select="xlg:Lower(@IsPrimaryKey)" />, <xsl:value-of select="xlg:Lower(@IsForiegnKey)" />));</xsl:for-each>
					  InternalTable.Instance = <xsl:value-of select="$Namespace" />.DB.Instance;
					  InternalTable.INSERT_SQL = "INSERT INTO <xsl:value-of select="$IdentifierName" /> (<xsl:for-each select="Columns/Column[@IsIdentity='False']"><xsl:if test="position()!=1">,</xsl:if>[<xsl:value-of select="@ColumnName" />]</xsl:for-each>) VALUES (<xsl:for-each select="Columns/Column[@IsIdentity='False']"><xsl:if test="position()!=1">,</xsl:if>@p<xsl:value-of select="xlg:sHash(@PropertyName)" /></xsl:for-each>)\n SELECT @@IDENTITY";
					  InternalTable.UPDATE_SQL = "UPDATE <xsl:value-of select="$IdentifierName" /> SET <xsl:for-each select="Columns/Column"><xsl:if test="position()!=1">,</xsl:if><xsl:value-of select="@ColumnName" />=@p<xsl:value-of select="xlg:sHash(@PropertyName)" /></xsl:for-each> WHERE <xsl:for-each select="$CurrTable/Keys/Key[@IsPrimary='True']/Columns/Column"><xsl:if test="position()!=1"> and </xsl:if><xsl:value-of select="@Column"/>=@p<xsl:variable name="Column" select="@Column" /><xsl:value-of select="xlg:sHash($CurrTable/Columns/Column[@ColumnName=$Column]/@PropertyName)"/></xsl:for-each>";
					  InternalTable.FIELD_LIST = "<xsl:for-each select="Columns/Column"><xsl:if test="position()!=1">,</xsl:if><xsl:choose><xsl:when test="@DbType='DateTime' and @IsNullable='False' and /xlgDoc/@DatabaseProvider='SybaseDataProvider'">convert(datetime,[<xsl:value-of select="@ColumnName" />],103) as [<xsl:value-of select="@ColumnName" />]</xsl:when><xsl:otherwise>[<xsl:value-of select="@ColumnName"/>]</xsl:otherwise></xsl:choose></xsl:for-each>";
				  }
			  }
          }
          return InternalTable;
        }
      }

      /// &lt;summary&gt;Creates an empty instance with default state&lt;/summary&gt;
      public <xsl:value-of select="@ClassName" />() { }
      /// &lt;summary&gt;Fills this instance with the current row in the passed IDataReader&lt;/summary&gt;
      public <xsl:value-of select="@ClassName" />(IDataReader rdr) : base(rdr) { }
      /// &lt;summary&gt;Fills this instance with the current row in the passed DataRow&lt;/summary&gt;
      public <xsl:value-of select="@ClassName" />(DataRow dr) : base(dr) { }
      /// &lt;summary&gt;Fills this instance with a direct copy of the field values of another <xsl:value-of select="$ClassName" /> object. This effectively clones the passed object.&lt;/summary&gt;
	  public <xsl:value-of select="$ClassName" />(<xsl:value-of select="$ClassName" /> ToImport)
	  {
		Load(ToImport);
	  }
	  
	  /// &lt;summary&gt;Fills this instance with the current row in the passed IDataReader&lt;/summary&gt;
      public override void Load(IDataReader rdr)
      {
            <xsl:for-each select="Columns/Column">
			<xsl:choose>
			<xsl:when test="@CSharpVariableType='string'">
			<xsl:value-of select="@PropertyName" /> = (rdr[Columns.<xsl:value-of select="@PropertyName" />] == DBNull.Value ? null : <xsl:value-of select="@CovertToPart" />(rdr[Columns.<xsl:value-of select="@PropertyName" />]))</xsl:when>
			<xsl:otherwise><xsl:value-of select="@PropertyName" /> = <xsl:value-of select="@CovertToPart" />(rdr[Columns.<xsl:value-of select="@PropertyName" />])</xsl:otherwise>
			</xsl:choose>;
            </xsl:for-each>
            _IsLoaded = true;
            _IsNew = false;
			_RecordHashThen = RecordHashNow();
      }
      /// &lt;summary&gt;Fills this instance with the current row in the passed DataRow&lt;/summary&gt;
      public override void Load(DataRow dr)
      {
            <xsl:for-each select="Columns/Column">
            <xsl:value-of select="@PropertyName" /> = <xsl:value-of select="@CovertToPart" />(dr[Columns.<xsl:value-of select="@PropertyName" />]);
            </xsl:for-each>
            _IsLoaded = true;
            _IsNew = false;
			_RecordHashThen = RecordHashNow();
        }    
		<!--/// &lt;summary&gt;Returns false if there ARE differences between ToCompare and the current instance and Appends the differences to the supplied StringBuilder&lt;/summary&gt;
        public bool Diff(StringBuilder sb, <xsl:value-of select="$Namespace"/>.<xsl:value-of select="$ClassName"/> ToCompare)
        {
			bool ret = true;
            <xsl:for-each select="Columns/Column">
            if(<xsl:value-of select="@PropertyName" /> != ToCompare.<xsl:value-of select="@PropertyName" />) { sb.AppendLine(Columns.<xsl:value-of select="@PropertyName" /> + " : " + Worker.nzString(<xsl:value-of select="@PropertyName" />) + " != " + Worker.nzString(ToCompare.<xsl:value-of select="@PropertyName" />)); ret = false; }</xsl:for-each>
            return ret;
        }-->
        /// &lt;summary&gt;Returns the total number of records in the <xsl:value-of select="$IdentifierName"/> table&lt;/summary&gt;
        public static int Count() { return <xsl:value-of select="$Namespace" />.DB.Instance.RetrieveSingleIntegerValue("SELECT COUNT(*) FROM <xsl:value-of select="$IdentifierName"/>"); }
        /// &lt;summary&gt;Returns a basic QueryCommand for selecting records from <xsl:value-of select="$IdentifierName"/>&lt;/summary&gt;
        public QueryCommand GetSelectCommand() { return new QueryCommand(Schema.SELECT_SQL); }

        private QueryCommand AddParameters(string userName, QueryCommand cmd, bool IncludeIdentity)
        {
          <xsl:for-each select="Columns/Column">
          <xsl:if test="@IsIdentity='True'">if(IncludeIdentity)</xsl:if>
          <xsl:choose>
            <xsl:when test="(xlg:Lower(@ColumnName)='createdby' or xlg:Lower(@ColumnName)='modifiedby') and @DbType='String'">
            cmd.Parameters.Add(new QueryParameter("@p<xsl:value-of select="xlg:sHash(@PropertyName)"/>", (userName != null ? userName : <xsl:value-of select="@PropertyName" />), DbType.<xsl:value-of select="@DbType"/>));</xsl:when>
            <xsl:when test="@CSharpVariableType='bool'">
            cmd.Parameters.Add(new QueryParameter("@p<xsl:value-of select="xlg:sHash(@PropertyName)"/>", (<xsl:value-of select="@PropertyName" /> ? 1 : 0), DbType.<xsl:value-of select="@DbType"/>));</xsl:when>
            <xsl:when test="@CSharpVariableType='int' or @CSharpVariableType='long' or @CSharpVariableType='short' or @CSharpVariableType='decimal' or @CSharpVariableType='double'">
            cmd.Parameters.Add(new QueryParameter("@p<xsl:value-of select="xlg:sHash(@PropertyName)"/>", (<xsl:value-of select="@PropertyName" /> == <xsl:value-of select="@CSharpVariableType" />.MinValue ? (object) DBNull.Value : (object) <xsl:value-of select="@PropertyName" />), DbType.<xsl:value-of select="@DbType"/>));</xsl:when>
            <xsl:when test="@CSharpVariableType='DateTime'">
            cmd.Parameters.Add(new QueryParameter("@p<xsl:value-of select="xlg:sHash(@PropertyName)"/>", (<xsl:value-of select="@PropertyName" /> == DateTime.MinValue ? (object) DBNull.Value : (object) <xsl:value-of select="@PropertyName" />), DbType.<xsl:value-of select="@DbType"/>));</xsl:when>
            <xsl:when test="@DbType='Guid'">
            cmd.Parameters.Add(new QueryParameter("@p<xsl:value-of select="xlg:sHash(@PropertyName)"/>", (<xsl:value-of select="@PropertyName" /> == null || <xsl:value-of select="@PropertyName" /> == Guid.Empty ? (object) DBNull.Value : <xsl:value-of select="@PropertyName" />), DbType.<xsl:value-of select="@DbType"/>));</xsl:when>
            <xsl:otherwise>
            cmd.Parameters.Add(new QueryParameter("@p<xsl:value-of select="xlg:sHash(@PropertyName)"/>", (<xsl:value-of select="@PropertyName" /> == null ? (object) DBNull.Value : (object) <xsl:value-of select="@PropertyName" />), DbType.<xsl:value-of select="@DbType"/>));</xsl:otherwise>
          </xsl:choose>
        </xsl:for-each>
            return cmd;
        }
        /// &lt;summary&gt;Calculates and returns the integer hash from the current field values. This will be different than GetHash which returns the hash for the entire object. This can be used to determine if the object values have changed since the fields were loaded from the database.&lt;/summary&gt;
        public override int RecordHashNow()
        {
			int ret = 0;
			<xsl:for-each select="Columns/Column">
			<xsl:choose>
			<xsl:when test="@IsDotNetObject='True'">if(<xsl:value-of select="@PropertyName"/> != null) ret ^= <xsl:value-of select="@PropertyName"/>.GetHashCode()</xsl:when>
			<xsl:otherwise>ret ^= <xsl:value-of select="@PropertyName"/>.GetHashCode()</xsl:otherwise>
			</xsl:choose>;
			</xsl:for-each>
            return ret;
        }
        /// &lt;summary&gt;Returns a QueryCommand object tailored toward performing an INSERT INTO sql statement from the current settings and parameter values&lt;/summary&gt;
        public override QueryCommand GetInsertCommand(string userName)  {  return AddParameters(userName, new QueryCommand(Schema.INSERT_SQL), false);  }
        /// &lt;summary&gt;Returns a QueryCommand object tailored toward performing an UPDATE sql statement from the current settings and parameter values&lt;/summary&gt;
        public override QueryCommand GetUpdateCommand(string userName)  {  return AddParameters(userName, new QueryCommand(Schema.UPDATE_SQL), true);   }
		/// &lt;summary&gt;Saves the current property/field state of the object to the database, overwriting the existing record of appending a new record, as apporopriate&lt;/summary&gt;
        public override void Save(string userName)
        {
			<xsl:choose>
			<xsl:when test="/xlgDoc/@LockOnSave='Object'">lock(_SyncRoot)
			{</xsl:when>
			<xsl:when test="/xlgDoc/@LockOnSave='Type'">lock(typeof(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />))
			{</xsl:when>
			<xsl:when test="/xlgDoc/@LockOnSave='Global'">lock(typeof(<xsl:value-of select="$BaseItemClass"/>))
			{</xsl:when>
			</xsl:choose>
				QueryCommand cmd = null;
				if (_IsNew)
				{
				  <xsl:if test="Columns/Column[@ColumnName='CreatedOn']">CreatedOn = DateTime.Now;</xsl:if>
				  <xsl:if test="Columns/Column[@ColumnName='DateCreated']">DateCreated = DateTime.Now;</xsl:if>
				  <xsl:if test="Columns/Column[@ColumnName='ModifiedOn']">ModifiedOn = DateTime.Now;</xsl:if>
				  <xsl:if test="Columns/Column[@ColumnName='DateModified']">DateModified = DateTime.Now;</xsl:if>
				  cmd = GetInsertCommand(userName);
				}
				else       
				{
				  <xsl:if test="Columns/Column[@ColumnName='ModifiedOn']">ModifiedOn = DateTime.Now;</xsl:if>
				  <xsl:if test="Columns/Column[@ColumnName='DateModified']">DateModified = DateTime.Now;</xsl:if>
				  cmd = GetUpdateCommand(userName);
				}
				<xsl:choose>
				  <xsl:when test="Columns/Column[@IsIdentity='True']">object pkVal = <xsl:value-of select="$Namespace" />.DB.Instance.ExecuteScalar(cmd);
				  <xsl:variable name="IdentityColumn" select="Columns/Column[@IsIdentity='True']" />
				if(pkVal != null &amp;&amp; pkVal != DBNull.Value) <xsl:value-of select="$IdentityColumn/@PropertyName" /> = <xsl:value-of select="$IdentityColumn/@CovertToPart" />(pkVal);</xsl:when>
				  <xsl:when test="$PrimaryKeyColumnName='' or not($PrimaryKeyColumn)"><xsl:value-of select="$Namespace" />.DB.Instance.ExecuteScalar(cmd);</xsl:when>
				  <xsl:otherwise>
					<xsl:choose>
					<xsl:when test="$PrimaryKeyColumn/@CSharpVariableType='DateTime'">
				<xsl:value-of select="$Namespace" />.DB.Instance.ExecuteScalar(cmd);
					</xsl:when>
					<xsl:when test="$PrimaryKeyColumn/@CSharpVariableType='Guid'">
				object pkVal = <xsl:value-of select="$Namespace" />.DB.Instance.ExecuteScalar(cmd);
				if(_IsNew &amp;&amp; pkVal != null &amp;&amp; pkVal != DBNull.Value &amp;&amp; <xsl:value-of select="$PrimaryKeyColumn/@CovertToPart" />(pkVal) != Guid.Empty) <xsl:value-of select="$PrimaryKeyColumn/@PropertyName" /> = <xsl:value-of select="$PrimaryKeyColumn/@CovertToPart" />(pkVal);
					</xsl:when>
					<xsl:when test="$PrimaryKeyColumn/@CSharpVariableType='string'">
				object pkVal = <xsl:value-of select="$Namespace" />.DB.Instance.ExecuteScalar(cmd);
				if(_IsNew &amp;&amp; pkVal != null &amp;&amp; pkVal != DBNull.Value &amp;&amp; <xsl:value-of select="$PrimaryKeyColumn/@CovertToPart" />(pkVal).Length &gt; 0 &amp;&amp; <xsl:value-of select="$PrimaryKeyColumn/@CovertToPart" />(<xsl:value-of select="$PrimaryKeyColumn/@PropertyName" />).Length &gt; 0) <xsl:value-of select="$PrimaryKeyColumn/@PropertyName" /> = <xsl:value-of select="$PrimaryKeyColumn/@CovertToPart" />(pkVal);
					</xsl:when>
					<xsl:otherwise>
				object pkVal = <xsl:value-of select="$Namespace" />.DB.Instance.ExecuteScalar(cmd);
				if(_IsNew &amp;&amp; pkVal != null &amp;&amp; pkVal != DBNull.Value &amp;&amp; <xsl:value-of select="$PrimaryKeyColumn/@CovertToPart" />(pkVal) != 0 &amp;&amp; <xsl:value-of select="$PrimaryKeyColumn/@PropertyName" /> == 0) <xsl:value-of select="$PrimaryKeyColumn/@PropertyName" /> = <xsl:value-of select="$PrimaryKeyColumn/@CovertToPart" />(pkVal);
					</xsl:otherwise>
					</xsl:choose>
				  </xsl:otherwise>
				</xsl:choose>
				_IsNew = false;
				_RecordHashThen = RecordHashNow();
			<xsl:if test="/xlgDoc/@LockOnSave!='None'">}</xsl:if>
        }
		/// &lt;summary&gt;Updates all property values to the values supplied and calls SaveIfChanged()&lt;/summary&gt;
		public void UpdateAndSave(<xsl:for-each select="Columns/Column"><xsl:if test="position()&gt;1">, </xsl:if><xsl:value-of select="@CSharpVariableType" /><xsl:text> </xsl:text><xsl:value-of select="@PropertyName" /></xsl:for-each>)
		{ <xsl:for-each select="Columns/Column">
		  this.<xsl:value-of select="@PropertyName" />=<xsl:value-of select="@PropertyName" />;</xsl:for-each>
		  SaveIfChanged();
		}
		/// &lt;summary&gt;Causes any record with the exact values for all fields to be deleted from the table. That is to say, a DELETE sql statement will be run that targets every field with a value equal to the property in this instance&lt;/summary&gt;
		public void Delete() 
		{ 
				Where<xsl:for-each select="Columns/Column">
					.<xsl:value-of select="@PropertyName" />(<xsl:value-of select="@PropertyName" />)</xsl:for-each>
				.Delete();
		}
		/// &lt;summary&gt;Sets IsNew = true and forces a call to Item.Save(). This is useful when using the ObjectDataSource (ods) and can be used for the INSERT method when configuring the ObjectDataSource&lt;/summary&gt;
		public static void odsInsert(<xsl:value-of select="@ClassName" /> Item) { if(Item != null) { Item._IsNew = true; Item.Save(); } }
		/// &lt;summary&gt;Sets IsNew = false and forces a call to Item.SaveIfChanged(). This is useful when using the ObjectDataSource (ods) and can be used for the UPDATE method when configuring the ObjectDataSource&lt;/summary&gt;
		public static void odsUpdate(<xsl:value-of select="@ClassName" /> Item) { if(Item != null) { Item._IsNew = false; Item.SaveIfChanged(); } }
		/// &lt;summary&gt;Forces a call to Item.Delete(). This is useful when using the ObjectDataSource (ods) and can be used for the DELETE method when configuring the ObjectDataSource&lt;/summary&gt;
		public static void odsDelete(<xsl:value-of select="@ClassName" /> Item) { if(Item != null) Item.Delete(); }
		/// &lt;summary&gt;Returns a string containing a single xml node representation of the current instance using standard .net xml object serialization&lt;/summary&gt;
        public override string OuterXml()
        {
			StringBuilder sb = new StringBuilder();
			ToXml(sb);
			return sb.ToString();
        }
		
		/// &lt;summary&gt;Appends to a XmlWriter the contents of the  as an xml node, one tag per object wrapped in a node named by OuterTagName&lt;/summary&gt;
		public static void ToXml(XmlWriter xw, <xsl:value-of select="@ClassName" /> Target)
		{
			if(xs<xsl:value-of select="@ClassName" /> == null) { xs<xsl:value-of select="@ClassName" /> = new XmlSerializer(typeof(<xsl:value-of select="@ClassName" />)); ns = new XmlSerializerNamespaces(); ns.Add(string.Empty, string.Empty); }
			xs<xsl:value-of select="@ClassName" />.Serialize(xw, Target, ns);
		}
		/// &lt;summary&gt;Reads from the XmlReader the contents of the , one tag per object wrapped in a node named by OuterTagName&lt;/summary&gt;
		public static <xsl:value-of select="@ClassName" /> FromXml(XmlReader xr)
		{
			if(xs<xsl:value-of select="@ClassName" /> == null) { xs<xsl:value-of select="@ClassName" /> = new XmlSerializer(typeof(<xsl:value-of select="@ClassName" />)); ns = new XmlSerializerNamespaces(); ns.Add(string.Empty, string.Empty); }
			return (<xsl:value-of select="@ClassName" />) xs<xsl:value-of select="@ClassName" />.Deserialize(xr);
		}
		/// &lt;summary&gt;Appends to a TextWriter the contents of the  as an xml node, one tag per object wrapped in a node named by OuterTagName&lt;/summary&gt;
		public static void ToXml(TextWriter Output, <xsl:value-of select="@ClassName" /> Target)
		{
			if(xs<xsl:value-of select="@ClassName" /> == null) { xs<xsl:value-of select="@ClassName" /> = new XmlSerializer(typeof(<xsl:value-of select="@ClassName" />)); ns = new XmlSerializerNamespaces(); ns.Add(string.Empty, string.Empty); }
			xs<xsl:value-of select="@ClassName" />.Serialize(Output, Target);
		}
		/// &lt;summary&gt;Reads from the TextReader the contents of the , one tag per object wrapped in a node named by OuterTagName&lt;/summary&gt;
		public static <xsl:value-of select="@ClassName" /> FromXml(TextReader Input)
		{
			if(xs<xsl:value-of select="@ClassName" /> == null) { xs<xsl:value-of select="@ClassName" /> = new XmlSerializer(typeof(<xsl:value-of select="@ClassName" />)); ns = new XmlSerializerNamespaces(); ns.Add(string.Empty, string.Empty); }
			return (<xsl:value-of select="@ClassName" />) xs<xsl:value-of select="@ClassName" />.Deserialize(Input);
		}

		/// &lt;summary&gt;Appends to a XmlWriter the contents of the xml node, one tag per object wrapped in a node named by OuterTagName&lt;/summary&gt;
        public override void ToXml(TextWriter Output)
        {
			ToXml(Output, this);
        }
		/// &lt;summary&gt;Appends to a XmlWriter the contents of the xml node, one tag per object wrapped in a node named by OuterTagName&lt;/summary&gt;
        public override void ToXml(XmlWriter xw)
        {
            ToXml(xw, this);
        }
		/// &lt;summary&gt;Appends to a StringBuilder the contents of the object as an xml node, one tag per object wrapped in a node named by OuterTagName&lt;/summary&gt;
        public override void ToXml(StringBuilder sb)
        {
            using (XmlWriter xw = xml.Writer(sb))
				ToXml(xw, this);
            <!--XmlWriterSettings settings = new XmlWriterSettings(); settings.OmitXmlDeclaration = true; settings.Indent = true;
            XmlWriter xw = XmlTextWriter.Create(sb, settings);
            ToXml(xw, this); xw.Close();
			sb.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);
            //sb.Replace("&lt;<xsl:value-of select="@ClassName" />&gt;", string.Empty);
            //sb.Replace("&lt;/<xsl:value-of select="@ClassName" />&gt;", string.Empty);
            sb.AppendLine();-->
        }
		/// &lt;summary&gt;Appends to a StringBuilder the contents of the object as an xml node, one tag per object wrapped in a node named by OuterTagName&lt;/summary&gt;
        public override void ToXml(StringBuilder sb, string OuterTagName)
        {
			sb.AppendLine("&lt;" + OuterTagName + "&gt;");
            using (XmlWriter xw = xml.Writer(sb))
				ToXml(xw, this);
 			sb.AppendLine("&lt;/" + OuterTagName + "&gt;");
           <!--XmlWriterSettings settings = new XmlWriterSettings(); settings.OmitXmlDeclaration = true; settings.Indent = true;
            XmlWriter xw = XmlTextWriter.Create(sb, settings);
			sb.AppendLine("&lt;" + OuterTagName + "&gt;");
            ToXml(xw, this); xw.Close();
			sb.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);
            //sb.Replace("&lt;<xsl:value-of select="@ClassName" />&gt;", "&lt;" + OuterTagName + "&gt;");
            //sb.Replace("&lt;/<xsl:value-of select="@ClassName" />&gt;", "&lt;/" + OuterTagName + "&gt;");
			sb.AppendLine("&lt;/" + OuterTagName + "&gt;");
            sb.AppendLine();-->
        }
		/// &lt;summary&gt;Takes a string containing a single xml node representation of a <xsl:value-of select="@ClassName" /> object and returns the equivalent object using standard .net xml object deserialization&lt;/summary&gt;
        public static <xsl:value-of select="@ClassName" /> FromXml(string xmlNode)
        {
            <xsl:value-of select="@ClassName" /> oOut = null;
            if (!string.IsNullOrEmpty(xmlNode))
            {
				XmlReader xr = null;
				if (xmlNode.IndexOf("&lt;?xml") > -1)
					xr = XmlReader.Create(new StringReader(xmlNode));
				else
				{
					StringBuilder sb = new StringBuilder();
					sb.AppendLine(xml.Declaration);
					sb.Append(xmlNode);
					xr = XmlReader.Create(new StringReader(sb.ToString()));
				}
				if (xr != null)
				{
					oOut = (<xsl:value-of select="@ClassName" />) FromXml(xr);
					xr.Close();
				}
			}
            return oOut;
        }
		/// &lt;summary&gt;Creates an instance with values set to each of the supplied properties&lt;/summary&gt;
        public <xsl:value-of select="@ClassName" />(<xsl:for-each select="Columns/Column"><xsl:if test="position()&gt;1">, </xsl:if><xsl:value-of select="@CSharpVariableType" /><xsl:text> </xsl:text><xsl:value-of select="@PropertyName" /></xsl:for-each>)
        {
            <xsl:for-each select="Columns/Column">
            this.<xsl:value-of select="@PropertyName" /> = <xsl:value-of select="@PropertyName" />;</xsl:for-each>
        }
	  /// &lt;summary&gt;Creates an instance with property values set to the value (or null) supplied in the NameValueCollection such as when they are passed in an HTTP request (Form or QueryString). If AutoSave is true, then Save() is automatically called creating a new record.&lt;/summary&gt;
      public <xsl:value-of select="@ClassName" />(NameValueCollection Set, bool AutoSave) { Load(Set, AutoSave); }
      /// &lt;summary&gt;Creates an instance with property values set to the value (or null) supplied in the NameValueCollection such as when they are passed in an HTTP request (Form or QueryString). The object is not automatically saved.&lt;/summary&gt;
      public <xsl:value-of select="@ClassName" />(NameValueCollection Set) { Load(Set, false); }
      /// &lt;summary&gt;Loads the current instance with property values set to the value (or null) supplied in the NameValueCollection such as when they are passed in an HTTP request (Form or QueryString). The object is not automat&lt;/summary&gt;
      public void Load(NameValueCollection Set) { Load(Set, false); }
	  /// &lt;summary&gt;Loads the current instance with property values set to the value (or null) supplied in the NameValueCollection such as when they are passed in an HTTP request (Form or QueryString). If AutoSave is true, then Save() is automatically called creating a new record.&lt;/summary&gt;
      public void Load(NameValueCollection Set, bool AutoSave)
      {
        bool AtLeastOneSet = false;
        foreach(string CurrKey in Set.AllKeys)
          switch(CurrKey)
          {
            <xsl:for-each select="Columns/Column">
              case <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />: this.<xsl:value-of select="@PropertyName" /> = <xsl:value-of select="@CovertToPart" />(Set[CurrKey]); AtLeastOneSet = true; break;</xsl:for-each>
          }
        if(AtLeastOneSet &amp;&amp; AutoSave) Save();
      }

	  /// &lt;summary&gt;Copies the field state of the ToImport object into the current instance. WARNING: State value like RecordHashThen and IsNew are not copied.&lt;/summary&gt;
	  public void Load(<xsl:value-of select="$ClassName" /> ToImport)
	  {
		<xsl:for-each select="Columns/Column">
		<xsl:value-of select="@PropertyName"/> = ToImport.<xsl:value-of select="@PropertyName"/>;
		</xsl:for-each>
	  }
      /// &lt;summary&gt;Returns a new <xsl:value-of select="@ClassName" />.SQL object. Useful for constructing in line queries.&lt;/summary&gt;
	  public static SQL Where { get { return new SQL(); } }

<!--
		Details Sub Class
	  [Serializable, XmlElement("<xsl:value-of select="@ClassName" />Details", Namespace="", IsNullable=false)]
	  public class Details : <xsl:value-of select="$ClassName" />
	  {
		public Details() { }
		public Details(<xsl:value-of select="$ClassName" /> ToImport) : base(ToImport) { }
		public Details(<xsl:value-of select="$ClassName" /> ToImport, bool LoadParentRecords, bool LoadChildrenRecords)
		{
			Load(ToImport);
			if(LoadParentRecords)	LoadParents( LoadParentRecords, LoadChildrenRecords);
			if(LoadChildrenRecords) LoadChildren(LoadParentRecords, LoadChildrenRecords);
		}
		
		<xsl:if test="Indexes/Index[@SingleColumnIndex='True']">
		public static List&lt;Details&gt; Loaded<xsl:value-of select="$ClassName" />s;</xsl:if>
		<xsl:for-each select="Columns/Column">
		<xsl:variable name="PropertyName" select="@PropertyName" />
		<xsl:variable name="SubTable" select="/xlgDoc/Tables/Table[Indexes/Index[@SingleColumnIndex='True' and @PropertyName=$PropertyName]]" />
		<xsl:if test="$SubTable and not($SubTable/@ClassName=$ClassName)">
		[XmlAttribute("<xsl:value-of select="$SubTable/@ClassName" />")]
		public <xsl:value-of select="$SubTable/@ClassName" />.Details <xsl:value-of select="$SubTable/@ClassName" />;</xsl:if>
		</xsl:for-each>

		<xsl:for-each select="Indexes/Index[@SingleColumnIndex='True']">
		<xsl:variable name="IndexColumnPropertyName" select="IndexColumns/IndexColumn/@PropertyName" />
		<xsl:variable name="IndexedColumn" select="$CurrTable/Columns/Column[@PropertyName=$IndexColumnPropertyName]" />
		public static Details Get<xsl:value-of select="$IndexColumnPropertyName"/>(<xsl:value-of select="$IndexedColumn/@CSharpVariableType"/><xsl:text> </xsl:text><xsl:value-of select="$IndexColumnPropertyName"/>, bool LoadParentRecords, bool LoadChildrenRecords)
		{
		    if(Loaded<xsl:value-of select="$ClassName" />s == null) Loaded<xsl:value-of select="$ClassName" />s = new List&lt;Details&gt;();
			Details rf = Loaded<xsl:value-of select="$ClassName" />s.Find(delegate(Details x) { return x.<xsl:value-of select="$IndexColumnPropertyName"/> == <xsl:value-of select="$IndexColumnPropertyName"/>; });
			if(rf == null) 
			{
				<xsl:value-of select="$ClassName" /> item = <xsl:value-of select="$Namespace" />.<xsl:value-of select="$CurrTable/@ClassName" />.I.<xsl:value-of select="$IndexColumnPropertyName"/>.SelectOne(<xsl:value-of select="$IndexColumnPropertyName"/>);
				if(item != null) rf = new Details(item, LoadParentRecords, LoadChildrenRecords);
			}
			return rf;
		}
		</xsl:for-each>
		public void LoadParents(bool LoadParentRecords, bool LoadChildrenRecords)
		{
		  <xsl:for-each select="Columns/Column">
		  <xsl:variable name="PropertyName" select="@PropertyName" />
		  <xsl:variable name="SubTable" select="/xlgDoc/Tables/Table[Indexes/Index[@SingleColumnIndex='True' and @PropertyName=$PropertyName]]" />
	      <xsl:variable name="SubColumn" select="$SubTable/Columns/Column[@PropertyName=$PropertyName]" />
		  <xsl:if test="$SubTable and not($SubTable/@ClassName=$ClassName)">
		  <xsl:value-of select="$SubTable/@ClassName" /> = <xsl:value-of select="$Namespace"/>.<xsl:value-of select="$SubTable/@ClassName" />.Details.Get<xsl:value-of select="$PropertyName"/>(<xsl:choose><xsl:when test="$SubColumn/@CSharpVariableType!=@CSharpVariableType"><xsl:value-of select="$SubColumn/@CovertToPart"/>(<xsl:value-of select="$PropertyName"/>)</xsl:when><xsl:otherwise><xsl:value-of select="$PropertyName"/></xsl:otherwise></xsl:choose>, LoadParentRecords, LoadChildrenRecords);
		  </xsl:if>
		  </xsl:for-each>
		}
		public void LoadChildren(bool LoadParentRecords, bool LoadChildrenRecords)
		{
		}
	  }  
-->
// Static methods 
	  /// &lt;summary&gt;Constructs a Query object with a single Where clause.&lt;/summary&gt;
      public static SQL ConstructQuery(Where where)
      {
          SQL qry = new SQL();
          where.TableName = <xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" />.FullTableName;
          qry.AddWhere(where);
          return qry;
      }
	  /// &lt;summary&gt;Constructs a Query object with a single Where clause attached to a specific column with a specific value.&lt;/summary&gt;
      public static SQL ConstructQuery(string ColumnName, object value)
      {
          SQL qry = new SQL();
          <xsl:choose>
          <xsl:when test="@CSharpVariableType='Guid'">
          if(value == Guid.Empty)
          	qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, ColumnName, Comparison.Is, null));
          else
          	qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, ColumnName, Comparison.Equals, value));
          </xsl:when>
          <xsl:when test="@IsDotNetObject='False'">
          if(value == <xsl:value-of select="@CSharpVariableType" />.MinValue)
          	qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, ColumnName, Comparison.Is, null));
          else
          	qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, ColumnName, Comparison.Equals, value));
          </xsl:when>
          <xsl:otherwise>
          if(value == null)
          	qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, ColumnName, Comparison.Is, null));
          else
          	qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, ColumnName, Comparison.Equals, value));
          </xsl:otherwise>
          </xsl:choose>
          return qry;
      }
	  /// &lt;summary&gt;Constructs a Query object with a two Where clauses attached to two specific columns with two specific values.&lt;/summary&gt;
      public static SQL ConstructQuery(string ColumnName1, object Value1, string ColumnName2, object Value2)
      {
          SQL qry = new SQL();
          qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" />.FullTableName, ColumnName1, Comparison.Equals, Value1));
          qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" />.FullTableName, ColumnName2, Comparison.Equals, Value2));
          return qry;
      }
	  /// &lt;summary&gt;Constructs a Query object with a single Where clause attached to a specific column with a value that matches the supplied comparison&lt;/summary&gt;
      public static SQL ConstructQuery(string ColumnName1, Comparison Comparison1, object Value1)
      {
          SQL qry = new SQL();
          qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" />.FullTableName, ColumnName1, Comparison1, Value1));
          return qry;
      }
	  /// &lt;summary&gt;Constructs a Query object with two Where clauses attached to two columns with values that matche the supplied comparisons&lt;/summary&gt;
      public static SQL ConstructQuery(string ColumnName1, Comparison Comparison1, object Value1, string ColumnName2, Comparison Comparison2, object Value2)
      {
          SQL qry = new SQL();
          qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" />.FullTableName, ColumnName1, Comparison1, Value1));
          qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" />.FullTableName, ColumnName2, Comparison2, Value2));
          return qry;
      }
	  /// &lt;summary&gt;Constructs a Query object with an arbitrary number of Where clauses&lt;/summary&gt;
      public static SQL ConstructQuery(Where[] whereParts)
      {
          SQL qry = new SQL();
          foreach(Where CurrWhere in whereParts)
          {
            CurrWhere.TableName = <xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" />.FullTableName;
            qry.AddWhere(CurrWhere);
          }
          return qry;
      }
	  /// &lt;summary&gt;Runs the passed query iterating through the records constructing an object for the current record and calls the Predicate for that object, then repeating until it runs out of objects or until the Predicate returns false&lt;/summary&gt;
      public static void ForEach(Query qry, Predicate&lt;<xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" />&gt; func)
      {
			bool cont = true;
			using(IDataReader idr = qry.ExecuteReader())
				while (idr.Read() &amp;&amp; cont)
					cont = func(new <xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" />(idr));
      }
	  /// &lt;summary&gt;Executes the supplied select query and constructs a collection object containing the results. Returns an empty collection object if the query returns no records.&lt;/summary&gt;
      public static <xsl:value-of select="@ClassName" />Collection Select(Query qry)
      {
          using(IDataReader idr = qry.ExecuteReader())
			return new <xsl:value-of select="@ClassName" />Collection(idr);
      }
	  /// &lt;summary&gt;Executes the supplied QueryCommand and constructs a collection object containing the results. Returns an empty collection object if the query returns no records.&lt;/summary&gt;
      public static <xsl:value-of select="@ClassName" />Collection Select(QueryCommand cmd)
      {
          using(IDataReader idr = <xsl:value-of select="$Namespace" />.DB.Instance.GetReader(cmd))
			return new <xsl:value-of select="@ClassName" />Collection(idr);
      }

	  /// &lt;summary&gt;Returns a collection containing all records in the table: <xsl:value-of select="$TableName" />&lt;/summary&gt;
      public static <xsl:value-of select="@ClassName" />Collection SelectAll() { return Select(new Query(<xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" />.Schema)); }
	  /// &lt;summary&gt;Executes the supplied SQL statement and constructs a collection object containing the results. Returns an empty collection object if the query returns no records or no records of the correct type.&lt;/summary&gt;
      public static <xsl:value-of select="@ClassName" />Collection Select(string qry) { return Select(new QueryCommand(qry)); }
	  /// &lt;summary&gt;Executes a select query with the single Where condition and constructs a collection object containing the results. Returns an empty collection object if the query returns no records or no records of the correct type.&lt;/summary&gt;
      public static <xsl:value-of select="@ClassName" />Collection Select(Where where) { return Select(ConstructQuery(where)); }
	  /// &lt;summary&gt;Executes a select query with the single ColumnName = Value condition and constructs a collection object containing the results. Returns an empty collection object if the query returns no records or no records of the correct type.&lt;/summary&gt;
      public static <xsl:value-of select="@ClassName" />Collection Select(string ColumnName, object Value) { return Select(ConstructQuery(ColumnName, Value)); }
	  /// &lt;summary&gt;Executes a select query with the double ColumnName = Value conditions and constructs a collection object containing the results. Returns an empty collection object if the query returns no records or no records of the correct type.&lt;/summary&gt;
      public static <xsl:value-of select="@ClassName" />Collection Select(string ColumnName1, object Value1, string ColumnName2, object Value2) { return Select(ConstructQuery(ColumnName1, Value1, ColumnName2, Value2)); }
	  /// &lt;summary&gt;Executes a select query with the single ColumnName comparison Value condition and constructs a collection object containing the results. Returns an empty collection object if the query returns no records or no records of the correct type.&lt;/summary&gt;
      public static <xsl:value-of select="@ClassName" />Collection Select(string ColumnName1, Comparison Comparison1, object Value1) { return Select(ConstructQuery(ColumnName1, Comparison1, Value1)); }
	  /// &lt;summary&gt;Executes a select query with the double ColumnName comparison Value conditions and constructs a collection object containing the results. Returns an empty collection object if the query returns no records or no records of the correct type.&lt;/summary&gt;
      public static <xsl:value-of select="@ClassName" />Collection Select(string ColumnName1, Comparison Comparison1, object Value1, string ColumnName2, Comparison Comparison2, object Value2) { return Select(ConstructQuery(ColumnName1, Comparison1, Value1, ColumnName2, Comparison2, Value2)); }
	  /// &lt;summary&gt;Executes a select query with Where conditions as passed and constructs a collection object containing the results. Returns an empty collection object if the query returns no records or no records of the correct type.&lt;/summary&gt;
      public static <xsl:value-of select="@ClassName" />Collection Select(Where[] whereParts) { return Select(ConstructQuery(whereParts)); }
      <!--public static <xsl:value-of select="@ClassName" />Collection Select(DataTable dataTable) { return (<xsl:value-of select="@ClassName" />Collection)new <xsl:value-of select="@ClassName" />Collection(dataTable); }-->
	  /// &lt;summary&gt;Executes a select query with the single Where condition and constructs a single object containing the result. Returns null if the query returned no results.&lt;/summary&gt;
      public static <xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" /> SelectOne(Where where) { return SelectOne(ConstructQuery(where)); }
	  /// &lt;summary&gt;Executes a select query with the single ColumnName = Value condition and constructs a single object containing the result. Returns null if the query returned no results.&lt;/summary&gt;
      public static <xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" /> SelectOne(string ColumnName, object Value) { return SelectOne(ConstructQuery(ColumnName, Value)); }
	  /// &lt;summary&gt;Executes a select query with the two ColumnName = Value conditions supplied and constructs a single object containing the result. Returns null if the query returned no results.&lt;/summary&gt;
      public static <xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" /> SelectOne(string ColumnName1, object Value1, string ColumnName2, object Value2) { return SelectOne(ConstructQuery(ColumnName1, Value1, ColumnName2, Value2)); }
	  /// &lt;summary&gt;Executes a select query with the single ColumnName comparison Value condition and constructs a single object containing the result. Returns null if the query returned no results.&lt;/summary&gt;
      public static <xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" /> SelectOne(string ColumnName1, Comparison Comparison1, object Value1) { return SelectOne(ConstructQuery(ColumnName1, Comparison1, Value1)); }
	  /// &lt;summary&gt;Executes a select query with the two ColumnName comparison Value condition and constructs a single object containing the result. Returns null if the query returned no results.&lt;/summary&gt;
      public static <xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" /> SelectOne(string ColumnName1, Comparison Comparison1, object Value1, string ColumnName2, Comparison Comparison2, object Value2) { return SelectOne(ConstructQuery(ColumnName1, Comparison1, Value1, ColumnName2, Comparison2, Value2)); }
	  /// &lt;summary&gt;Executes a select query with the supplied list of conditions and constructs a single object containing the result. Returns null if the query returned no results.&lt;/summary&gt;
      public static <xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" /> SelectOne(Where[] whereParts) { return SelectOne(ConstructQuery(whereParts)); }

	  /// &lt;summary&gt;Executes the supplied SQL statement and constructs a single object containing the result. Returns null if the query returned no results.&lt;/summary&gt;
      public static <xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" /> SelectOne(Query qry)
      {
          using(IDataReader idr = qry.ExecuteReader())
			if (idr.Read())
              return new <xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" />(idr); 
          return null;
      }

	  /// &lt;summary&gt;Takes the supplied values for each field and constructs an object then calls the Save() method on that object and returns the object. This effectively creates both a new record and an object to represent it&lt;/summary&gt;
      public static <xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" /> CreateAndSave(<xsl:for-each select="Columns/Column"><xsl:if test="position()&gt;1">, </xsl:if><xsl:value-of select="@CSharpVariableType" /><xsl:text> </xsl:text><xsl:value-of select="@PropertyName" /></xsl:for-each>)
      {
          <xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" /> ret = new <xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" />(<xsl:for-each select="Columns/Column"><xsl:if test="position()&gt;1">, </xsl:if><xsl:value-of select="@PropertyName" /></xsl:for-each>);
          ret.Save();
          return ret;
      }
	  /// &lt;summary&gt;Takes values from the NameValueCollection that match the names of the fields and constructs an object to represent it. If AutoSave is true, then a call to the Save() method is made. Very useful for creating a record from an HTTP report form POST.&lt;/summary&gt;
      public static <xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" /> Create(NameValueCollection Set, bool AutoSave)
      {
        <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" /> ret = new <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />(Set, false);
        <xsl:if test="Columns/Column[@ColumnName='DateCreated']">ret.DateCreated = DateTime.Now;</xsl:if>
        <xsl:if test="$PrimaryKeyColumn/@DbType='Guid'">
        ret.<xsl:value-of select="$PrimaryKeyColumn/@PropertyName"/> = Guid.NewGuid();</xsl:if>
        if(AutoSave) ret.Save();
        return ret;
      }
	  /// &lt;summary&gt;Takes values from the NameValueCollection that match the names of the fields and constructs an object to represent it. The object is then returned without saving to the database. Very useful for quickly creating an object from an HTTP report form POST.&lt;/summary&gt;
      public static <xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" /> Create(NameValueCollection Set) { return Create(Set, false); }
	  /// &lt;summary&gt;Walks through each record in the IDataReader and creates a collection, then serializes the collection to Xml via the collection's OuterXml property. NOTE: Do not use this with very large collections.&lt;/summary&gt;
      public static string ToXml(IDataReader rdr)
      {
        <xsl:value-of select="@ClassName" />Collection t = new <xsl:value-of select="@ClassName" />Collection(rdr);
        return t.OuterXml();
      }
	  /// &lt;summary&gt;Walks through each record in the IDataReader and creates a collection, then appends a serialized version of the collection's Xml to the supplied StringBuilder. NOTE: Do not use this with very large collections.&lt;/summary&gt;
      public static string ToXml(string OuterTagName, IDataReader rdr)
      {
        <xsl:value-of select="@ClassName" />Collection t = new <xsl:value-of select="@ClassName" />Collection(rdr);
        StringBuilder sb = new StringBuilder();
        t.ToXml(sb, OuterTagName);
        return sb.ToString();
      }      
	  /// &lt;summary&gt;Executes a SELECT MAX(...) query for the stated column across the entire table&lt;/summary&gt;
      public static object Max(string ColumnName) { return <xsl:value-of select="$Namespace" />.DB.Instance.ExecuteScalar("SELECT MAX(" + ColumnName + ") FROM <xsl:value-of select="$IdentifierName" />"); }
	  /// &lt;summary&gt;Executes a SELECT MIN(...) query for the stated column across the entire table&lt;/summary&gt;
      public static object Min(string ColumnName) { return <xsl:value-of select="$Namespace" />.DB.Instance.ExecuteScalar("SELECT MIN(" + ColumnName + ") FROM <xsl:value-of select="$IdentifierName" />"); }

  <xsl:choose>
    <xsl:when test="/xlgDoc/@DatabaseProvider='SqlDataProvider'">
      /// &lt;summary&gt;Retrieves the entire table as an Xml document. NOTE: For SQL Server this is more efficient than using the Collections's ToXml() method&lt;/summary&gt;
      public static string ToXml() { return <xsl:value-of select="$Namespace" />.DB.Instance.ToXml("<xsl:value-of select="@TableName" />s", string.Empty, Schema.SELECT_SQL); }
      /// &lt;summary&gt;Retrieves the table limited by a SQL WHERE clause as an Xml document. NOTE: For SQL Server this is more efficient than using the Collections's ToXml() method&lt;/summary&gt;
      public static string ToXml(string WhereClause) { return <xsl:value-of select="$Namespace" />.DB.Instance.ToXml("<xsl:value-of select="@TableName" />s", string.Empty, Schema.SELECT_SQL + " WHERE " + WhereClause); }
    </xsl:when>
    <xsl:otherwise>
      /// &lt;summary&gt;Retrieves the entire table as an Xml document.&lt;/summary&gt;
      public static string ToXml() { return Select(new Query(<xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" />.Schema)).OuterXml(); }
    </xsl:otherwise>
  </xsl:choose>
  
<!-- Query Sub Class -->
    /// &lt;summary&gt;An implementation of the Query class tailored to the <xsl:value-of select="$ClassName" /> class&lt;/summary&gt;
    public class SQL : Query
    {
		/// &lt;summary&gt;Establishes a And connection between two Where conditions, the one immediately before and after the call to this method. Returns itself&lt;/summary&gt;
        public SQL And
        {
            get
            {
                LastOption = WhereOptions.And;
                return this;
            }
        }
		/// &lt;summary&gt;Establishes an Or connection between two Where conditions, the one immediately before and after the call to this method. Returns itself&lt;/summary&gt;
        public SQL Or
        {
            get
            {
                LastOption = WhereOptions.Or; 
                return this;
            }
        }
		/// &lt;summary&gt;Begins a grouping of Where conditions. Returns itself. NOTE: Currently this does NOT support multiple simultaneous groups such as (x and (y or z))&lt;/summary&gt;
        public SQL Paren
        {
            get 
            {
                BeginParen();
                return this;
            }
        }
		/// &lt;summary&gt;Ends a grouping of Where conditions. Returns itself. NOTE: Currently this does NOT support multiple simultaneous groups such as (x and (y or z))&lt;/summary&gt;
        public SQL CloseParen
        {
            get
            {
                EndParen();
                return this;
            }
        }
		/// &lt;summary&gt;Limits the number of records returned by the query to a maximum of MaxRecordCount records. Returns itself&lt;/summary&gt;
        public SQL Limit(int MaximumRecordCount)
        {
            Top = MaximumRecordCount.ToString();
            return this;
        }
		/// &lt;summary&gt;Used with PageNumber. Limits the number of records returned by the query to a maximum of PageSize records. Returns itself&lt;/summary&gt;
        public SQL PageSize(int PageSize)
        {
            Top = PageSize.ToString();
            return this;
        }
		/// &lt;summary&gt;Used with PageSize. This determines which page of records to return. So a PageSize of 10 with PageNumber of 2 would return records 11 through 20. Returns itself&lt;/summary&gt;
        public SQL PageNumber(int PageNumber)
        {
            base.Page = PageNumber;
            return this;
        }

	<xsl:for-each select="Columns/Column">
		/// &lt;summary&gt;Defines the next (or first) ORDER BY column for the query. Returns itself&lt;/summary&gt;
		public SQL Ascending<xsl:value-of select="@PropertyName" /> { 
			get { 
				if(OrderBy == null) OrderBy = new OrderBy();
				if(string.IsNullOrEmpty(OrderBy.OrderString)) 
					OrderBy.OrderString = " ORDER BY " + <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />; 
				else
					OrderBy.OrderString += "," + <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />; 
				return this;
			} }
		/// &lt;summary&gt;Defines the next (or first) ORDER BY DESC column for the query. Returns itself&lt;/summary&gt;
		public SQL Descending<xsl:value-of select="@PropertyName" /> { 
			get { 
				if(OrderBy == null) OrderBy = new OrderBy();
				if(string.IsNullOrEmpty(OrderBy.OrderString)) 
					OrderBy.OrderString = " ORDER BY " + <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" /> + " DESC"; 
				else
					OrderBy.OrderString += "," + <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" /> + " DESC"; 
				return this;
			} }
	</xsl:for-each>

// Quick access class
		/// &lt;summary&gt;The basic constructor creates a SQL Query object tailored to the table <xsl:value-of select="$TableName" />&lt;/summary&gt;
		public SQL() : base(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Schema) 
		{ 
			wheres = new List&lt;Where&gt;();
		}
		<xsl:for-each select="Columns/Column">
		/// &lt;summary&gt;Defines the next (or first) WHERE claused based on <xsl:value-of select="@ColumnName"/>. Records matching value are returned. Returns itself&lt;/summary&gt;
		public SQL <xsl:value-of select="@PropertyName"/>(<xsl:value-of select="@CSharpVariableType" /> value)
		{
			<xsl:choose>
			<xsl:when test="@CSharpVariableType='Guid'">
			if(value == Guid.Empty)
				AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName"/>, Comparison.Is, null));
			else
				AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName"/>, Comparison.Equals, value));
			</xsl:when>
			<xsl:when test="@CSharpVariableType='bool'">
			AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName"/>, Comparison.Equals, value));
			</xsl:when>
			<xsl:when test="@IsDotNetObject='False'">
			if(value == <xsl:value-of select="@CSharpVariableType" />.MinValue)
				AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName"/>, Comparison.Is, null));
			else
				AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName"/>, Comparison.Equals, value));
			</xsl:when>
			<xsl:otherwise>
			if(value == null)
				AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName"/>, Comparison.Is, null));
			else
				AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName"/>, Comparison.Equals, value));
			</xsl:otherwise>
			</xsl:choose>
			return this;
		}
		/// &lt;summary&gt;Defines the next (or first) WHERE claused for any comparison operator based on <xsl:value-of select="@ColumnName"/>. Records matching the comparison and value are returned. Returns itself&lt;/summary&gt;
		public SQL <xsl:value-of select="@PropertyName"/>(Comparison Comparison1, <xsl:value-of select="@CSharpVariableType" /> value)
		{
			AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName"/>, Comparison1, value));
			return this;
		}
		/// &lt;summary&gt;Defines the next (or first) WHERE claused for any comparison operator based on <xsl:value-of select="@ColumnName"/>. Records matching the comparison and value are returned. Normally used with Comparison.In or to check for a null value on a non-object type field (such as int or decimal). Returns itself&lt;/summary&gt;
		public SQL <xsl:value-of select="@PropertyName"/>(Comparison Comparison1, object value)
		{
			AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName"/>, Comparison1, value));
			return this;
		}
		/// &lt;summary&gt;Defines the next (or first) WHERE claused for any comparison operator based on <xsl:value-of select="@ColumnName"/>. Records matching the comparison and value are returned. Normally used with Comparison.In or to check for a null value on a non-object type field (such as int or decimal). WhereOptions define special grouping. Normally you'd want to use the Paren and CloseParen convenience properties. Returns itself&lt;/summary&gt;
		public SQL <xsl:value-of select="@PropertyName"/>(Comparison Comparison1, object value, WhereOptions options)
		{
			AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName"/>, Comparison1, value, options));
			return this;
		}
		</xsl:for-each>
		
		/// &lt;summary&gt;Causes a DELETE sql statement to execute based on the WHERE conditions added. NOTE: Only one command that modifies data executes against any one table per process. That is to say, any DELETE, INSERT INTO, or UPDATE SQL statements against the <xsl:value-of select="$TableName" /> table are executed one at a time to prevent deadlocks. Reads occur in parallel.&lt;/summary&gt;
        public int Delete()
        {
            QueryCommand cmd = BuildDeleteCommand();
			<xsl:if test="/xlgDoc/@LockOnDelete!='False'">lock(_SyncRoot)</xsl:if>
				return <xsl:value-of select="$Namespace" />.DB.Instance.ExecuteQuery(cmd);
        }

		/// &lt;summary&gt;Causes a SELECT sql statement to execute based on the WHERE, ORDER BY, and JOIN conditions added. Returns a collection of <xsl:value-of select="@ClassName" /> objects&lt;/summary&gt;
        public <xsl:value-of select="@ClassName" />Collection Select()
        {
          using(IDataReader idr = ExecuteReader())
			return new <xsl:value-of select="@ClassName" />Collection(idr);
        }

		/// &lt;summary&gt;Causes a SELECT sql statement to execute based on the WHERE, ORDER BY, and JOIN conditions added. Returns the first record as a <xsl:value-of select="@ClassName" /> object or null if the result set was empty&lt;/summary&gt;
        public <xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" /> SelectOne()
        {
          using(IDataReader idr = ExecuteReader())
			if (idr.Read())
              return new <xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" />(idr);
          return null;
        }

		/// &lt;summary&gt;Causes a SELECT EXISTS() sql statement to execute based on the WHERE, ORDER BY, and JOIN conditions added. Returns -1 if the statement failed to execute, 0 if the statement returned zero records (No records exist) or 1 if at least one record exists&lt;/summary&gt;
        public int Exists()
        {
			int ret = -1;
			QueryType temp = QueryType; QueryType = QueryType.Exists;
			using(IDataReader idr = ExecuteReader())
				if (idr.Read()) 
					ret = Worker.nzInteger(idr[0]);
			QueryType = temp;
			return ret;
        }

		/// &lt;summary&gt;Causes a SELECT COUNT(*) sql statement to execute based on the WHERE, ORDER BY, and JOIN conditions added. Returns -1 if the statement failed to execute or the total number of records the selection contains&lt;/summary&gt;
        public int Count()
        {
			int ret = -1;
			QueryType temp = QueryType; QueryType = QueryType.Count;
			using(IDataReader idr = ExecuteReader())
				if (idr.Read()) 
					ret = Worker.nzInteger(idr[0]);
			QueryType = temp;
			return ret;
        }

		/// &lt;summary&gt;Causes a SELECT MIN(FieldName) sql statement to execute based on the WHERE, ORDER BY, and JOIN conditions added. Returns -1 if the statement failed to execute or the total number of records the selection contains&lt;/summary&gt;
        public string Min(string FieldName)
        {
			string ret = null;
			QueryType temp = QueryType;
			QueryType = QueryType.Min;
			SelectList = FieldName;
			using(IDataReader idr = ExecuteReader())
				if (idr.Read())
					ret = Worker.nzString(idr[0]);
			QueryType = temp;
			return ret;
        }

		/// &lt;summary&gt;Causes a SELECT MAX(FieldName) sql statement to execute based on the WHERE, ORDER BY, and JOIN conditions added. Returns -1 if the statement failed to execute or the total number of records the selection contains&lt;/summary&gt;
        public string Max(string FieldName)
        {
			string ret = null;
			QueryType temp = QueryType;
			QueryType = QueryType.Max;
			SelectList = FieldName;
			using(IDataReader idr = ExecuteReader())
				if (idr.Read())
					ret = Worker.nzString(idr[0]);
			QueryType = temp;
			return ret;
        }

		/// &lt;summary&gt;PageSize must be set. Returns -1 if the statement failed to execute or the total number of pages (rounded up) the SELECT statement could return. So if there are 25 records in a result set and if PageSize = 10, then this function would return 3 (10 in the first page, 10 in the second page, 5 in the 3rd page)&lt;/summary&gt;
        public int PageCount()
        {
			int ret = -1;
			QueryType temp = QueryType; QueryType = QueryType.Count;
			using(IDataReader idr = ExecuteReader())
				if (idr.Read()) 
					ret = Worker.nzInteger(idr[0]);
			QueryType = temp;
			if(ret &gt; 0) {
				int PageSize;
				if(int.TryParse(Top, out PageSize) &amp;&amp; PageSize &gt; 0)
					ret = (int) Math.Ceiling((double) ret / PageSize);
			}
			return ret;
        }
     }

	 /// &lt;summary&gt;Static classes providing quick access to defined indexes on the <xsl:value-of select="$TableName" /> table&lt;/summary&gt;
	 public static class Index
	 {
        <xsl:for-each select="Columns/Column[@IsIndexed='True' or @IsPrimaryKey='True']">
		  /// &lt;summary&gt;
		  /// Static class providing quick access to the indexed column: <xsl:value-of select="@ColumnName"/><xsl:if test="@IsPrimaryKey='True'">.&lt;br /&gt;This is the PRIMARY KEY of <xsl:value-of select="$TableName"/></xsl:if>
		  /// &lt;/summary&gt;
          public static class <xsl:value-of select="@PropertyName" />
          {
        <xsl:choose>
          <xsl:when test="/xlgDoc/@DatabaseProvider='SqlDataProvider'">
		    <xsl:choose>
            <xsl:when test="@DbType='Guid'">
			/// &lt;summary&gt;Retrieves all records with <xsl:value-of select="@ColumnName"/> = Value to a string containing <xsl:value-of select="$TableName" /> XML nodes wrapped in a <xsl:value-of select="$TableName" />s node&lt;/summary&gt;
            public static string SelectToXml(Guid Value) { return <xsl:value-of select="$Namespace" />.DB.Instance.ToXml("<xsl:value-of select="$TableName" />s", string.Empty, Schema.SELECT_SQL + " WHERE <xsl:value-of select="@ColumnName" />=" + Worker.s2db(Value)); }
			/// &lt;summary&gt;Retrieves all records with <xsl:value-of select="@ColumnName"/> = Value to a string containing <xsl:value-of select="$TableName" /> XML nodes wrapped in a node named after OuterTagName with attributes equal to OuterTagAttriutes&lt;/summary&gt;
            public static string SelectToXml(string OuterTagName, string OuterTagAttributes, Guid Value) { return <xsl:value-of select="$Namespace" />.DB.Instance.ToXml(OuterTagName, OuterTagAttributes, Schema.SELECT_SQL + " WHERE <xsl:value-of select="@ColumnName" />=" + Worker.s2db(Value)); }
			/// &lt;summary&gt;Retrieves the first record with <xsl:value-of select="@ColumnName"/> = Value to a string containing a single <xsl:value-of select="$TableName" /> XML node&lt;/summary&gt;
            public static string SelectOneToXml(Guid Value) { return <xsl:value-of select="$Namespace" />.DB.Instance.ToXml(Schema.SELECT_SQL + " WHERE <xsl:value-of select="@ColumnName"/>=" + Worker.s2db(Value)); }

			/// &lt;summary&gt;Retrieves all records with <xsl:value-of select="@ColumnName"/> = Value to a string containing <xsl:value-of select="$TableName" /> XML nodes wrapped in a <xsl:value-of select="$TableName" />s node&lt;/summary&gt;
            public static void SelectToXml(StringBuilder sb, Guid Value) { sb.AppendLine(<xsl:value-of select="$Namespace" />.DB.Instance.ToXml("<xsl:value-of select="$TableName" />s", string.Empty, Schema.SELECT_SQL + " WHERE <xsl:value-of select="@ColumnName" />=" + Worker.s2db(Value))); }
			/// &lt;summary&gt;Retrieves all records with <xsl:value-of select="@ColumnName"/> = Value to a string containing <xsl:value-of select="$TableName" /> XML nodes wrapped in a node named after OuterTagName with attributes equal to OuterTagAttriutes&lt;/summary&gt;
            public static void SelectToXml(StringBuilder sb, string OuterTagName, string OuterTagAttributes, Guid Value) { sb.AppendLine(<xsl:value-of select="$Namespace" />.DB.Instance.ToXml(OuterTagName, OuterTagAttributes, Schema.SELECT_SQL + " WHERE <xsl:value-of select="@ColumnName" />=" + Worker.s2db(Value))); }
			/// &lt;summary&gt;Retrieves the first record with <xsl:value-of select="@ColumnName"/> = Value to a string containing a single <xsl:value-of select="$TableName" /> XML node&lt;/summary&gt;
            public static void SelectOneToXml(StringBuilder sb, Guid Value) { sb.AppendLine(<xsl:value-of select="$Namespace" />.DB.Instance.ToXml(Schema.SELECT_SQL + " WHERE <xsl:value-of select="@ColumnName"/>=" + Worker.s2db(Value))); }

			/// &lt;summary&gt;Retrieves all records with <xsl:value-of select="@ColumnName"/> = Value to a string containing <xsl:value-of select="$TableName" /> XML nodes wrapped in a <xsl:value-of select="$TableName" />s node&lt;/summary&gt;
            public static void SelectToXml(TextWriter Output, Guid Value) { Output.WriteLine(<xsl:value-of select="$Namespace" />.DB.Instance.ToXml("<xsl:value-of select="$TableName" />s", string.Empty, Schema.SELECT_SQL + " WHERE <xsl:value-of select="@ColumnName" />=" + Worker.s2db(Value))); }
			/// &lt;summary&gt;Retrieves all records with <xsl:value-of select="@ColumnName"/> = Value to a string containing <xsl:value-of select="$TableName" /> XML nodes wrapped in a node named after OuterTagName with attributes equal to OuterTagAttriutes&lt;/summary&gt;
            public static void SelectToXml(TextWriter Output, string OuterTagName, string OuterTagAttributes, Guid Value) { Output.WriteLine(<xsl:value-of select="$Namespace" />.DB.Instance.ToXml(OuterTagName, OuterTagAttributes, Schema.SELECT_SQL + " WHERE <xsl:value-of select="@ColumnName" />=" + Worker.s2db(Value))); }
			/// &lt;summary&gt;Retrieves the first record with <xsl:value-of select="@ColumnName"/> = Value to a string containing a single <xsl:value-of select="$TableName" /> XML node&lt;/summary&gt;
            public static void SelectOneToXml(TextWriter Output, Guid Value) { Output.WriteLine(<xsl:value-of select="$Namespace" />.DB.Instance.ToXml(Schema.SELECT_SQL + " WHERE <xsl:value-of select="@ColumnName"/>=" + Worker.s2db(Value))); }

			/// &lt;summary&gt;Retrieves all records with <xsl:value-of select="@ColumnName"/> = Value to a collection&lt;/summary&gt;
            public static <xsl:value-of select="$ClassName" />Collection Select(Guid Value) { return <xsl:value-of select="$Namespace"/>.<xsl:value-of select="$ClassName" />.Select(<xsl:value-of select="$Namespace"/>.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName"/>, Comparison.Equals, Value); }
			/// &lt;summary&gt;Retrieves all records with <xsl:value-of select="@ColumnName"/> comparison Value to a collection&lt;/summary&gt;
            public static <xsl:value-of select="$ClassName" />Collection Select(Comparison ComparisonOperator, Guid Value) { return <xsl:value-of select="$Namespace"/>.<xsl:value-of select="$ClassName" />.Select(<xsl:value-of select="$Namespace"/>.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName"/>, ComparisonOperator, Value); }
			/// &lt;summary&gt;Retrieves the first record with <xsl:value-of select="@ColumnName"/> = Value as a <xsl:value-of select="$ClassName" /> object&lt;/summary&gt;
            public static <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" /> SelectOne(Guid Value) { return <xsl:value-of select="$Namespace"/>.<xsl:value-of select="$ClassName" />.SelectOne(<xsl:value-of select="$Namespace"/>.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName"/>, Value); }
            </xsl:when>
			<xsl:otherwise>
			/// &lt;summary&gt;Retrieves all records with <xsl:value-of select="@ColumnName"/> = Value to a string containing <xsl:value-of select="$TableName" /> XML nodes wrapped in a <xsl:value-of select="$TableName" />s node&lt;/summary&gt;
            public static void SelectToXml(StringBuilder sb, <xsl:value-of select="@CSharpVariableType" /> Value) { sb.AppendLine(<xsl:value-of select="$Namespace" />.DB.Instance.ToXml("<xsl:value-of select="$TableName" />s", string.Empty, Schema.SELECT_SQL + " WHERE <xsl:value-of select="@ColumnName" />=" + Worker.s2db(Value))); }
			/// &lt;summary&gt;Retrieves all records with <xsl:value-of select="@ColumnName"/> = Value to a string containing <xsl:value-of select="$TableName" /> XML nodes wrapped in a node named after OuterTagName with attributes equal to OuterTagAttriutes&lt;/summary&gt;
            public static void SelectToXml(StringBuilder sb, string OuterTagName, string OuterTagAttributes, <xsl:value-of select="@CSharpVariableType" /> Value) { sb.AppendLine(<xsl:value-of select="$Namespace" />.DB.Instance.ToXml(OuterTagName, OuterTagAttributes, Schema.SELECT_SQL + " WHERE <xsl:value-of select="@ColumnName" />=" + Worker.s2db(Value))); }
			/// &lt;summary&gt;Retrieves the first record with <xsl:value-of select="@ColumnName"/> = Value to a string containing a single <xsl:value-of select="$TableName" /> XML node&lt;/summary&gt;
            public static void SelectOneToXml(StringBuilder sb, <xsl:value-of select="@CSharpVariableType" /> Value) { sb.AppendLine(<xsl:value-of select="$Namespace" />.DB.Instance.ToXml(Schema.SELECT_SQL + " WHERE <xsl:value-of select="@ColumnName"/>=" + Worker.s2db(Value))); }
			/// &lt;summary&gt;Retrieves all non-null distinct values of <xsl:value-of select="@ColumnName"/> to a xml string containing <xsl:value-of select="$IdentifierName" />XML nodes with a Name attribute wrapped in a <xsl:value-of select="@PropertyName" />s node&lt;/summary&gt;
            public static void SelectDistinctToXml(StringBuilder sb) { sb.AppendLine(<xsl:value-of select="$Namespace" />.DB.Instance.ToXml("<xsl:value-of select="@PropertyName" />s", string.Empty, "SELECT DISTINCT <xsl:value-of select="@ColumnName" /> Name FROM <xsl:value-of select="$IdentifierName" /> <xsl:value-of select="@PropertyName" /> WHERE <xsl:value-of select="@ColumnName" /> IS NOT NULL")); }
			/// &lt;summary&gt;Retrieves matching records with non-null distinct values of <xsl:value-of select="@ColumnName"/> to a xml string containing <xsl:value-of select="$IdentifierName" />XML nodes with a Name attribute wrapped in a <xsl:value-of select="@PropertyName" />s node&lt;/summary&gt;
            public static void SelectDistinctToXml(StringBuilder sb, string WhereClause) { sb.AppendLine(<xsl:value-of select="$Namespace" />.DB.Instance.ToXml("<xsl:value-of select="@PropertyName" />s", string.Empty, "SELECT DISTINCT <xsl:value-of select="@ColumnName" /> Name FROM <xsl:value-of select="$IdentifierName" /> <xsl:value-of select="@PropertyName" /> WHERE <xsl:value-of select="@ColumnName" /> IS NOT NULL AND " + WhereClause)); }

			/// &lt;summary&gt;Retrieves all records with <xsl:value-of select="@ColumnName"/> = Value to a string containing <xsl:value-of select="$TableName" /> XML nodes wrapped in a <xsl:value-of select="$TableName" />s node&lt;/summary&gt;
            public static void SelectToXml(TextWriter Output, <xsl:value-of select="@CSharpVariableType" /> Value) { Output.WriteLine(<xsl:value-of select="$Namespace" />.DB.Instance.ToXml("<xsl:value-of select="$TableName" />s", string.Empty, Schema.SELECT_SQL + " WHERE <xsl:value-of select="@ColumnName" />=" + Worker.s2db(Value))); }
			/// &lt;summary&gt;Retrieves all records with <xsl:value-of select="@ColumnName"/> = Value to a string containing <xsl:value-of select="$TableName" /> XML nodes wrapped in a node named after OuterTagName with attributes equal to OuterTagAttriutes&lt;/summary&gt;
            public static void SelectToXml(TextWriter Output, string OuterTagName, string OuterTagAttributes, <xsl:value-of select="@CSharpVariableType" /> Value) { Output.WriteLine(<xsl:value-of select="$Namespace" />.DB.Instance.ToXml(OuterTagName, OuterTagAttributes, Schema.SELECT_SQL + " WHERE <xsl:value-of select="@ColumnName" />=" + Worker.s2db(Value))); }
			/// &lt;summary&gt;Retrieves the first record with <xsl:value-of select="@ColumnName"/> = Value to a string containing a single <xsl:value-of select="$TableName" /> XML node&lt;/summary&gt;
            public static void SelectOneToXml(TextWriter Output, <xsl:value-of select="@CSharpVariableType" /> Value) { Output.WriteLine(<xsl:value-of select="$Namespace" />.DB.Instance.ToXml(Schema.SELECT_SQL + " WHERE <xsl:value-of select="@ColumnName"/>=" + Worker.s2db(Value))); }
			/// &lt;summary&gt;Retrieves all non-null distinct values of <xsl:value-of select="@ColumnName"/> to a xml string containing <xsl:value-of select="$IdentifierName" />XML nodes with a Name attribute wrapped in a <xsl:value-of select="@PropertyName" />s node&lt;/summary&gt;
            public static void SelectDistinctToXml(TextWriter Output) { Output.WriteLine(<xsl:value-of select="$Namespace" />.DB.Instance.ToXml("<xsl:value-of select="@PropertyName" />s", string.Empty, "SELECT DISTINCT <xsl:value-of select="@ColumnName" /> Name FROM <xsl:value-of select="$IdentifierName" /> <xsl:value-of select="@PropertyName" /> WHERE <xsl:value-of select="@ColumnName" /> IS NOT NULL")); }
			/// &lt;summary&gt;Retrieves matching records with non-null distinct values of <xsl:value-of select="@ColumnName"/> to a xml string containing <xsl:value-of select="$IdentifierName" />XML nodes with a Name attribute wrapped in a <xsl:value-of select="@PropertyName" />s node&lt;/summary&gt;
            public static void SelectDistinctToXml(TextWriter Output, string WhereClause) { Output.WriteLine(<xsl:value-of select="$Namespace" />.DB.Instance.ToXml("<xsl:value-of select="@PropertyName" />s", string.Empty, "SELECT DISTINCT <xsl:value-of select="@ColumnName" /> Name FROM <xsl:value-of select="$IdentifierName" /> <xsl:value-of select="@PropertyName" /> WHERE <xsl:value-of select="@ColumnName" /> IS NOT NULL AND " + WhereClause)); }

			/// &lt;summary&gt;Retrieves all records with <xsl:value-of select="@ColumnName"/> = Value to a string containing <xsl:value-of select="$TableName" /> XML nodes wrapped in a <xsl:value-of select="$TableName" />s node&lt;/summary&gt;
            public static string SelectToXml(<xsl:value-of select="@CSharpVariableType" /> Value) { return <xsl:value-of select="$Namespace" />.DB.Instance.ToXml("<xsl:value-of select="$TableName" />s", string.Empty, Schema.SELECT_SQL + " WHERE <xsl:value-of select="@ColumnName" />=" + Worker.s2db(Value)); }
			/// &lt;summary&gt;Retrieves all records with <xsl:value-of select="@ColumnName"/> = Value to a string containing <xsl:value-of select="$TableName" /> XML nodes wrapped in a node named after OuterTagName with attributes equal to OuterTagAttriutes&lt;/summary&gt;
            public static string SelectToXml(string OuterTagName, string OuterTagAttributes, <xsl:value-of select="@CSharpVariableType" /> Value) { return <xsl:value-of select="$Namespace" />.DB.Instance.ToXml(OuterTagName, OuterTagAttributes, Schema.SELECT_SQL + " WHERE <xsl:value-of select="@ColumnName" />=" + Worker.s2db(Value)); }
			/// &lt;summary&gt;Retrieves the first record with <xsl:value-of select="@ColumnName"/> = Value to a string containing a single <xsl:value-of select="$TableName" /> XML node&lt;/summary&gt;
            public static string SelectOneToXml(<xsl:value-of select="@CSharpVariableType" /> Value) { return <xsl:value-of select="$Namespace" />.DB.Instance.ToXml(Schema.SELECT_SQL + " WHERE <xsl:value-of select="@ColumnName"/>=" + Worker.s2db(Value)); }
			/// &lt;summary&gt;Retrieves all non-null distinct values of <xsl:value-of select="@ColumnName"/> to a xml string containing <xsl:value-of select="$IdentifierName" />XML nodes with a Name attribute wrapped in a <xsl:value-of select="@PropertyName" />s node&lt;/summary&gt;
            public static string SelectDistinctToXml() { return <xsl:value-of select="$Namespace" />.DB.Instance.ToXml("<xsl:value-of select="@PropertyName" />s", string.Empty, "SELECT DISTINCT <xsl:value-of select="@ColumnName" /> Name FROM <xsl:value-of select="$IdentifierName" /> <xsl:value-of select="@PropertyName" /> WHERE <xsl:value-of select="@ColumnName" /> IS NOT NULL"); }
			/// &lt;summary&gt;Retrieves matching records with non-null distinct values of <xsl:value-of select="@ColumnName"/> to a xml string containing <xsl:value-of select="$IdentifierName" />XML nodes with a Name attribute wrapped in a <xsl:value-of select="@PropertyName" />s node&lt;/summary&gt;
            public static string SelectDistinctToXml(string WhereClause) { return <xsl:value-of select="$Namespace" />.DB.Instance.ToXml("<xsl:value-of select="@PropertyName" />s", string.Empty, "SELECT DISTINCT <xsl:value-of select="@ColumnName" /> Name FROM <xsl:value-of select="$IdentifierName" /> <xsl:value-of select="@PropertyName" /> WHERE <xsl:value-of select="@ColumnName" /> IS NOT NULL AND " + WhereClause); }
			</xsl:otherwise>
          </xsl:choose>
          </xsl:when>
          <xsl:otherwise></xsl:otherwise>
        </xsl:choose>
			/// &lt;summary&gt;Deletes all records where <xsl:value-of select="@ColumnName" /> is any one of the supplied Values&lt;/summary&gt;
            public static int DeleteIn(string[] Values)
            {
              if(Values != null &amp;&amp; Values.Length > 0) {
                for(int i = 0; i &lt; Values.Length; i++) Values[i] = Worker.s2db(Values[i]);
                QueryCommand cmd = new QueryCommand("DELETE FROM <xsl:value-of select="$IdentifierName" /> WHERE <xsl:value-of select="@ColumnName" /> IN (" + string.Join(",", Values) + ")");
                return <xsl:value-of select="$Namespace" />.DB.Instance.ExecuteQuery(cmd);
              }
              return 0;
            }
			/// &lt;summary&gt;Deletes all records where <xsl:value-of select="@ColumnName" /> = Value&lt;/summary&gt;
            public static int Delete(<xsl:value-of select="@CSharpVariableType" /> Value)
            {
                QueryCommand cmd = new QueryCommand("DELETE FROM <xsl:value-of select="$IdentifierName" /> WHERE <xsl:value-of select="@ColumnName" />=@p<xsl:value-of select="xlg:sHash(@PropertyName)" />");
                cmd.AddParameter("@p<xsl:value-of select="xlg:sHash(@PropertyName)" />", Value, Query.ConvertToDbType(Value.GetType()));
                return <xsl:value-of select="$Namespace" />.DB.Instance.ExecuteQuery(cmd);
            }
			/// &lt;summary&gt;Constructs a Query object with a single Where condition on <xsl:value-of select="@ColumnName" /> comparison Value&lt;/summary&gt;
            public static Query ConstructQuery(Comparison Comparison, object Value)
            {
                Query qry = new Query(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Schema);
                qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />, Comparison, Value));
                return qry;
            }
			/// &lt;summary&gt;Constructs a Query object with a single Where condition on <xsl:value-of select="@ColumnName" /> = Value&lt;/summary&gt;
            public static Query ConstructQuery(object Value1) { return ConstructQuery(Comparison.Equals, Value1); }

			/// &lt;summary&gt;Returns a count of records where <xsl:value-of select="@ColumnName" /> = Value&lt;/summary&gt;
            public static int Count(<xsl:value-of select="@CSharpVariableType" /> Value) 
            { 
				<xsl:choose>
				<xsl:when test="@CSharpVariableType='int' or @CSharpVariableType='long' or @CSharpVariableType='short' or @CSharpVariableType='decimal' or @CSharpVariableType='double' or @CSharpVariableType='bool'">
				return <xsl:value-of select="$Namespace" />.DB.Instance.RetrieveSingleIntegerValue("SELECT COUNT(*) FROM <xsl:value-of select="$IdentifierName" /> WHERE <xsl:value-of select="@ColumnName" />=" + Worker.nzString(Value));
				</xsl:when>
				<xsl:otherwise>
				return <xsl:value-of select="$Namespace" />.DB.Instance.RetrieveSingleIntegerValue("SELECT COUNT(*) FROM <xsl:value-of select="$IdentifierName" /> WHERE <xsl:value-of select="@ColumnName" />=" + Worker.s2db(Value));
				</xsl:otherwise>
				</xsl:choose>
            }
            
			<xsl:if test="@DbType!='Guid'">            
			/// &lt;summary&gt;Returns a collection of <xsl:value-of select="$ClassName" /> objects where <xsl:value-of select="@ColumnName" /> = Value&lt;/summary&gt;
            public static <xsl:value-of select="$ClassName" />Collection Select(<xsl:value-of select="@CSharpVariableType" /> value)
            {
                Query qry = new Query(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Schema);
				<xsl:choose>
				<xsl:when test="@CSharpVariableType='Guid'">
				if(value == Guid.Empty)
					qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />, Comparison.Is, null));
				else
					qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />, Comparison.Equals, value));
				</xsl:when>
				<xsl:when test="@IsDotNetObject='False'">
				if(value == <xsl:value-of select="@CSharpVariableType" />.MinValue)
					qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />, Comparison.Is, null));
				else
					qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />, Comparison.Equals, value));
				</xsl:when>
				<xsl:otherwise>
				if(value == null)
					qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />, Comparison.Is, null));
				else
					qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />, Comparison.Equals, value));
				</xsl:otherwise>
				</xsl:choose>
                using(IDataReader idr = qry.ExecuteReader())
					return new <xsl:value-of select="$ClassName" />Collection(idr);
            }
			/// &lt;summary&gt;Returns a collection of <xsl:value-of select="$ClassName" /> objects where <xsl:value-of select="@ColumnName" /> comparison Value&lt;/summary&gt;
            public static <xsl:value-of select="$ClassName" />Collection Select(Comparison ComparisonOperator, <xsl:value-of select="@CSharpVariableType" /> Value)
            {
                Query qry = new Query(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Schema);
                qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />, ComparisonOperator, Value));
                using(IDataReader idr = qry.ExecuteReader())
					return new <xsl:value-of select="$ClassName" />Collection(idr);
            }
			/// &lt;summary&gt;Returns the first <xsl:value-of select="$ClassName" /> object where <xsl:value-of select="@ColumnName" /> = Value&lt;/summary&gt;
            public static <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" /> SelectOne(<xsl:value-of select="@CSharpVariableType" /> value)
            {
                Query qry = new Query(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Schema);
				<xsl:choose>
				<xsl:when test="@CSharpVariableType='Guid'">
				if(value == Guid.Empty)
					qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />, Comparison.Is, null));
				else
					qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />, Comparison.Equals, value));
				</xsl:when>
				<xsl:when test="@IsDotNetObject='False'">
				if(value == <xsl:value-of select="@CSharpVariableType" />.MinValue)
					qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />, Comparison.Is, null));
				else
					qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />, Comparison.Equals, value));
				</xsl:when>
				<xsl:otherwise>
				if(value == null)
					qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />, Comparison.Is, null));
				else
					qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />, Comparison.Equals, value));
				</xsl:otherwise>
				</xsl:choose>
				using(IDataReader idr = qry.ExecuteReader())
					if (idr.Read())
						return new <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />(idr);
				return null;
            }
            </xsl:if>
			/// &lt;summary&gt;Returns a collection of <xsl:value-of select="$ClassName" /> objects where <xsl:value-of select="@ColumnName" /> = any Value passed in the array&lt;/summary&gt;
            public static <xsl:value-of select="$ClassName" />Collection SelectIn(<xsl:value-of select="@CSharpVariableType" />[] Value)
            {
				if(Value != null &amp;&amp; Value.Length &gt; 0)
				{
					Query qry = new Query(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Schema);
					qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />, Comparison.In, Value));
					using(IDataReader idr = qry.ExecuteReader())
						return new <xsl:value-of select="$ClassName" />Collection(idr);
				}
				return null;
            }
			/// &lt;summary&gt;Returns a collection of <xsl:value-of select="$ClassName" /> objects where <xsl:value-of select="@ColumnName" /> is in the In claused pased such as ('1','2','3') or (select x from y where z)&lt;/summary&gt;
            public static <xsl:value-of select="$ClassName" />Collection SelectIn(string InClause)
            {
				if(!string.IsNullOrEmpty(InClause))
				{
	                Query qry = new Query(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Schema);
		            qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />, Comparison.In, InClause));
			        using(IDataReader idr = qry.ExecuteReader())
						return new <xsl:value-of select="$ClassName" />Collection(idr);
				}
				return null;
            }
			/// &lt;summary&gt;Returns the first <xsl:value-of select="$ClassName" /> object where <xsl:value-of select="@ColumnName" /> = Value&lt;/summary&gt;
            public static <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" /> SelectOne(Comparison ComparisonOperator, <xsl:value-of select="@CSharpVariableType" /> Value)
            {
                Query qry = new Query(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Schema);
                qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="@PropertyName" />, ComparisonOperator, Value));
                <!--<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" /> ret = null;-->
                using(IDataReader idr = qry.ExecuteReader())
					if (idr.Read())
						return new <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />(idr);
				return null;
            }

			/// &lt;summary&gt;Returns an array containing distinct values of <xsl:value-of select="@ColumnName" />&lt;/summary&gt;
            public static <xsl:value-of select="@CSharpVariableType" />[] SelectDistinct()
            {
				DataSet ds = <xsl:value-of select="$Namespace" />.DB.Instance.ToDataSet(new QueryCommand("SELECT DISTINCT <xsl:value-of select="@ColumnName" /> FROM <xsl:value-of select="$IdentifierName" /> WHERE <xsl:value-of select="@ColumnName" /> IS NOT NULL"));
				if(ds != null &amp;&amp; ds.Tables.Count > 1)
				{
					DataRowCollection rows = ds.Tables[0].Rows;
					if(rows != null &amp;&amp; rows.Count &gt; 0)
					{
					  <xsl:value-of select="@CSharpVariableType" />[] ret = new <xsl:value-of select="@CSharpVariableType" />[rows.Count];
					  int i = 0;
					  foreach(DataRow CurrRow in rows)
						ret[i++] = (<xsl:value-of select="@CSharpVariableType" />) CurrRow[0];
					  return ret;
					}
				}
                return null;
            }
			/// &lt;summary&gt;Retrieves all non-null distinct values of <xsl:value-of select="@ColumnName"/> to an array. Selection of records is limited by the supplied WHEER clause&lt;/summary&gt;
            public static <xsl:value-of select="@CSharpVariableType" />[] SelectDistinct(string WherePhrase)
            {
                DataRowCollection rows = <xsl:value-of select="$Namespace" />.DB.Instance.ToDataRows("SELECT DISTINCT <xsl:value-of select="@ColumnName" /> FROM <xsl:value-of select="$IdentifierName" /> WHERE <xsl:value-of select="@ColumnName" /> IS NOT NULL AND " + WherePhrase);
                if(rows != null &amp;&amp; rows.Count &gt; 0)
                {
                  <xsl:value-of select="@CSharpVariableType" />[] ret = new <xsl:value-of select="@CSharpVariableType" />[rows.Count];
                  int i = 0;
                  foreach(DataRow CurrRow in rows)
                    ret[i++] = <xsl:value-of select="@CovertToPart" />(CurrRow[0]);
                  return ret;
                }
                return null;
            }
          }
		</xsl:for-each>
	<xsl:if test="count(Indexes/Index) &gt; 0">
		<xsl:variable name="TableColumns" select="Columns/Column" />
        <xsl:for-each select="Indexes/Index[@SingleColumnIndex!='True']">
		  /// &lt;summary&gt;Allows selection of records by the database index <xsl:value-of select="@IndexName" />&lt;/summary&gt;
          public static class <xsl:value-of select="@IndexName" />
          {
		    /// &lt;summary&gt;Returns a collection of <xsl:value-of select="$ClassName" /> objects matching the index values supplied&lt;/summary&gt;
            public static <xsl:value-of select="$ClassName" />Collection Select(<xsl:for-each select="IndexColumns/IndexColumn"><xsl:variable name="IndexColumnName" select="@IndexColumnName" /><xsl:if test="position()!=1">,</xsl:if><xsl:value-of select="$TableColumns[@ColumnName=$IndexColumnName]/@CSharpVariableType"/><xsl:text> </xsl:text><xsl:value-of select="$TableColumns[@ColumnName=$IndexColumnName]/@PropertyName"/></xsl:for-each>)
            {
                Query qry = new Query(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Schema);<xsl:for-each select="IndexColumns/IndexColumn"><xsl:variable name="IndexColumnName" select="@IndexColumnName"  />
				qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="$TableColumns[@ColumnName=$IndexColumnName]/@PropertyName"/>, Comparison.Equals, <xsl:value-of select="$TableColumns[@ColumnName=$IndexColumnName]/@PropertyName"/>));</xsl:for-each>
                using(IDataReader idr = qry.ExecuteReader())
					return new <xsl:value-of select="$ClassName" />Collection(idr);
            }
		    /// &lt;summary&gt;Returns the first <xsl:value-of select="$ClassName" /> object matching the index values supplied&lt;/summary&gt;
            public static <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" /> SelectOne(<xsl:for-each select="IndexColumns/IndexColumn"><xsl:variable name="IndexColumnName" select="@IndexColumnName" /><xsl:if test="position()!=1">,</xsl:if><xsl:value-of select="$TableColumns[@ColumnName=$IndexColumnName]/@CSharpVariableType"/><xsl:text> </xsl:text><xsl:value-of select="$TableColumns[@ColumnName=$IndexColumnName]/@PropertyName"/></xsl:for-each>)
            {
                Query qry = new Query(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Schema);<xsl:for-each select="IndexColumns/IndexColumn"><xsl:variable name="IndexColumnName" select="@IndexColumnName"  />
                qry.AddWhere(new Where(<xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.FullTableName, <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />.Columns.<xsl:value-of select="$TableColumns[@ColumnName=$IndexColumnName]/@PropertyName"/>, Comparison.Equals, <xsl:value-of select="$TableColumns[@ColumnName=$IndexColumnName]/@PropertyName"/>));</xsl:for-each>
                using(IDataReader idr = qry.ExecuteReader())
                  if (idr.Read())
                    return new <xsl:value-of select="$Namespace" />.<xsl:value-of select="$ClassName" />(idr);
                return null;
            }
          }
		</xsl:for-each>
	</xsl:if>
     }
    }
	
    [Serializable, XmlType(AnonymousType = true)]
    /// &lt;summary&gt;Represents a list of records from the <xsl:value-of select="$TableName" /> table&lt;/summary&gt;
    public partial class <xsl:value-of select="@ClassName" />Collection : ActiveList&lt;<xsl:value-of select="@ClassName" />&gt;
    {
        public <xsl:value-of select="@ClassName" />Collection() : base()                    { }
        /// &lt;summary&gt;Creates a collection from an appropriate SQL statement, connecting to the database and populating the collection from the results. This should rarely, if ever be used.&lt;/summary&gt;
        public <xsl:value-of select="@ClassName" />Collection(string SQL) : base()          { Load(<xsl:value-of select="$Namespace" />.DB.Instance.GetReader(new QueryCommand(SQL))); }
        /// &lt;summary&gt;Creates a collection from an appropriate QueryCommand object&lt;/summary&gt;
        public <xsl:value-of select="@ClassName" />Collection(QueryCommand Query) : base()  { Load(<xsl:value-of select="$Namespace" />.DB.Instance.GetReader(Query)); }
        /// &lt;summary&gt;Creates a collection iterating through the provided IDataReader to populate the collection.&lt;/summary&gt;
        public <xsl:value-of select="@ClassName" />Collection(IDataReader rdr) : base()     { Load(rdr); }
        /// &lt;summary&gt;Creates a collection from an appropriately filled DataTable object&lt;/summary&gt;
        public <xsl:value-of select="@ClassName" />Collection(DataTable dataTable) : base() { Load(dataTable); }
        /// &lt;summary&gt;Create a collection and populates it from a pre existing List of <xsl:value-of select="$ClassName" /> objects&lt;/summary&gt;
        public <xsl:value-of select="@ClassName" />Collection(List&lt;<xsl:value-of select="$ClassName" />&gt; list) : base() { AddRange(list); }

		/// &lt;summary&gt;Iterates through the collection calling Save() on each object. All records are saved in this way, even if they don't need to be.&lt;/summary&gt;
        public virtual void Save()
        {
            foreach(<xsl:value-of select="@ClassName" /> CurrItem in this)
                CurrItem.Save(null);
        }
        /// &lt;summary&gt;Iterates through the collection calling SaveIfChanged() on each object. The effect is only records that have changed since they were read will be saved.&lt;/summary&gt;
        public virtual void SaveIfChanged()
        {
            foreach (<xsl:value-of select="@ClassName" /> CurrItem in this)
                CurrItem.SaveIfChanged();
        }
		/// &lt;summary&gt;Iterates through the provided IDataReader and adds those records to the existing collection. NOTE: The collection is not cleared first.&lt;/summary&gt;
        public void Load(IDataReader rdr)
        {
            while (rdr.Read())
                Add(new <xsl:value-of select="@ClassName" />(rdr));
            rdr.Close();
            rdr.Dispose();
        }
		/// &lt;summary&gt;Iterates through the DataTable adding those records to the existing collection. NOTE: The collection is not cleared first.&lt;/summary&gt;
        public void Load(DataTable tbl)
        {
            foreach (DataRow dr in tbl.Rows)
                Add(new <xsl:value-of select="@ClassName" />(dr));
        }
  <xsl:for-each select="Columns/Column">
		<xsl:if test="@CSharpVariableType!='byte[]'">
		/// &lt;summary&gt;Sorts the collection in ascending order by <xsl:value-of select="@PropertyName" />&lt;/summary&gt;
        public void Sort<xsl:value-of select="@PropertyName" />() { Sort(Compare<xsl:value-of select="@PropertyName" />); }
        /// &lt;summary&gt;Compares two <xsl:value-of select="$ClassName" /> objects by <xsl:value-of select="@PropertyName" />&lt;/summary&gt;
        public static int Compare<xsl:value-of select="@PropertyName" />(<xsl:value-of select="$ClassName" /> x, <xsl:value-of select="$ClassName" /> y) { 
          if (x == null)  if (y == null)  return  0;  else return -1; else if (y == null) return  1; 
          <xsl:choose><xsl:when test="@CSharpVariableType='int' or @CSharpVariableType='long' or @CSharpVariableType='short' or @CSharpVariableType='decimal' or @CSharpVariableType='double' or @CSharpVariableType='bool' or @CSharpVariableType='DateTime'"></xsl:when>
            <xsl:when test="@CSharpVariableType='Guid'">if (x.<xsl:value-of select="@PropertyName" /> == Guid.Empty)  if (y.<xsl:value-of select="@PropertyName" /> == Guid.Empty)  return  0;  else return -1; else if (y.<xsl:value-of select="@PropertyName" /> == Guid.Empty) return  1;</xsl:when>
            <xsl:otherwise>if (x.<xsl:value-of select="@PropertyName" /> == null)  if (y.<xsl:value-of select="@PropertyName" /> == null)  return  0;  else return -1; else if (y.<xsl:value-of select="@PropertyName" /> == null) return  1;</xsl:otherwise></xsl:choose>
          <xsl:choose><xsl:when test="@CSharpVariableType='int' or @CSharpVariableType='long' or @CSharpVariableType='double' or @CSharpVariableType='decimal' or @CSharpVariableType='short'">return (int) (x.<xsl:value-of select="@PropertyName" /> - y.<xsl:value-of select="@PropertyName" />);</xsl:when>
            <xsl:when test="@CSharpVariableType='bool'">return (x.<xsl:value-of select="@PropertyName" /> &amp;&amp; y.<xsl:value-of select="@PropertyName" /> ? 0 : (x.<xsl:value-of select="@PropertyName" /> ? 1 : -1));</xsl:when>
            <xsl:when test="@CSharpVariableType='Guid'">return x.<xsl:value-of select="@PropertyName" />.CompareTo(y.<xsl:value-of select="@PropertyName" />); </xsl:when>
            <xsl:otherwise>return <xsl:value-of select="@CSharpVariableType" />.Compare(x.<xsl:value-of select="@PropertyName" />, y.<xsl:value-of select="@PropertyName" />); </xsl:otherwise></xsl:choose>
        }
        /// &lt;summary&gt;Searches the collection for the first object matching the supplied <xsl:value-of select="@PropertyName" />&lt;/summary&gt;
        public <xsl:value-of select="$ClassName" /> Find<xsl:value-of select="@PropertyName" />(<xsl:value-of select="@CSharpVariableType" /> ToFind)  { <xsl:if test="@CSharpVariableType='string'">if(ToFind != null) ToFind = ToFind.ToLower(); </xsl:if>return Find(delegate(<xsl:value-of select="$ClassName" /> x) { return x.<xsl:value-of select="@PropertyName" /> == ToFind<xsl:if test="@CSharpVariableType='string'"> || (x.<xsl:value-of select="@PropertyName" /> != null &amp;&amp; x.<xsl:value-of select="@PropertyName" />.ToLower() == ToFind)</xsl:if>; }); }
        /// &lt;summary&gt;Searches the collection for all objects matching the supplied <xsl:value-of select="@PropertyName" />&lt;/summary&gt;
        public <xsl:value-of select="$ClassName" />Collection FindAll<xsl:value-of select="@PropertyName" />(<xsl:value-of select="@CSharpVariableType" /> ToFind) { return new <xsl:value-of select="$ClassName" />Collection(FindAll(delegate(<xsl:value-of select="$ClassName" /> x) { return x.<xsl:value-of select="@PropertyName" /> == ToFind; })); }
		</xsl:if>
		/// &lt;summary&gt;Searches the collection for each distinct value of <xsl:value-of select="@PropertyName" /> and returns each distinct value in an array&lt;/summary&gt;
		public <xsl:value-of select="@CSharpVariableType" />[] Distinct<xsl:value-of select="@PropertyName" />
		{
			get
			{
				List&lt;<xsl:value-of select="@CSharpVariableType" />&gt; ret = new List&lt;<xsl:value-of select="@CSharpVariableType" />&gt;();
				foreach(<xsl:value-of select="$ClassName" /> CurrItem in this)
					if(!ret.Contains(CurrItem.<xsl:value-of select="@PropertyName" />))
						ret.Add(CurrItem.<xsl:value-of select="@PropertyName" />);
				ret.Sort();
				return ret.ToArray();
			}
		}
  </xsl:for-each>
  
		/// &lt;summary&gt;Returns a string containing an xml node of the contents of the collection, one tag per object wrapped in a "<xsl:value-of select="@ClassName" />s" node&lt;/summary&gt;
        public override string OuterXml()
        {
			StringBuilder sb = new StringBuilder();
			ToXml(sb, "<xsl:value-of select="@ClassName" />s");
			return sb.ToString();
        }
		/// &lt;summary&gt;Returns a string containing an xml node of the contents of the collection, one tag per object wrapped in a "<xsl:value-of select="@ClassName" />s" node&lt;/summary&gt;
        public override string InnerXml()
        {
			StringBuilder sb = new StringBuilder();
			ToXml(sb);
			return sb.ToString();
        }
		
		private static XmlSerializer xs<xsl:value-of select="@ClassName" />Collection;
		private static XmlSerializerNamespaces ns;
		
		/// &lt;summary&gt;Appends to a XmlWriter the contents of the collection as an xml node, one tag per object wrapped in a node named by OuterTagName&lt;/summary&gt;
		public static void ToXml(XmlWriter xw, <xsl:value-of select="@ClassName" />Collection Target)
		{
			if(xs<xsl:value-of select="@ClassName" />Collection == null) { xs<xsl:value-of select="@ClassName" />Collection = new XmlSerializer(typeof(<xsl:value-of select="@ClassName" />Collection)); ns = new XmlSerializerNamespaces(); ns.Add(string.Empty, string.Empty); }
			xs<xsl:value-of select="@ClassName" />Collection.Serialize(xw, Target, ns);
		}
		/// &lt;summary&gt;Reads from the XmlReader the contents of the collection, one tag per object wrapped in a node named by OuterTagName&lt;/summary&gt;
		public static <xsl:value-of select="@ClassName" />Collection FromXml(XmlReader xr)
		{
			if(xs<xsl:value-of select="@ClassName" />Collection == null) { xs<xsl:value-of select="@ClassName" />Collection = new XmlSerializer(typeof(<xsl:value-of select="@ClassName" />Collection)); ns = new XmlSerializerNamespaces(); ns.Add(string.Empty, string.Empty); }
			return (<xsl:value-of select="@ClassName" />Collection) xs<xsl:value-of select="@ClassName" />Collection.Deserialize(xr);
		}

		/// &lt;summary&gt;Appends to a XmlWriter the contents of the collection as an xml node, one tag per object&lt;/summary&gt;
        public override void ToXml(XmlWriter xw)
        {
            ToXml(xw, this);
        }
		/// &lt;summary&gt;Appends to a TextWriter the contents of the collection as an xml node, one tag per object&lt;/summary&gt;
		public static void ToXml(TextWriter Output, <xsl:value-of select="@ClassName" />Collection Target)
		{
			if(xs<xsl:value-of select="@ClassName" />Collection == null) { xs<xsl:value-of select="@ClassName" />Collection = new XmlSerializer(typeof(<xsl:value-of select="@ClassName" />Collection)); ns = new XmlSerializerNamespaces(); ns.Add(string.Empty, string.Empty); }
			xs<xsl:value-of select="@ClassName" />Collection.Serialize(Output, Target, ns);
		}
		/// &lt;summary&gt;Reads from the TextReader the contents of the collection, one tag per object wrapped in a node named by OuterTagName&lt;/summary&gt;
		public static <xsl:value-of select="@ClassName" />Collection FromXml(TextReader Input)
		{
			if(xs<xsl:value-of select="@ClassName" />Collection == null) { xs<xsl:value-of select="@ClassName" />Collection = new XmlSerializer(typeof(<xsl:value-of select="@ClassName" />Collection)); ns = new XmlSerializerNamespaces(); ns.Add(string.Empty, string.Empty); }
			return (<xsl:value-of select="@ClassName" />Collection ) xs<xsl:value-of select="@ClassName" />Collection.Deserialize(Input);
		}

		/// &lt;summary&gt;Appends to a TextWriter the contents of the collection as an xml node, one tag per object wrapped in a node named by OuterTagName&lt;/summary&gt;
        public override void ToXml(TextWriter Output)
        {
			ToXml(Output, this);
        }

		/// &lt;summary&gt;Appends to a StringBuilder the contents of the collection as an xml node, one tag per object wrapped in a node named by OuterTagName&lt;/summary&gt;
        public override void ToXml(StringBuilder sb)
        {
			using (XmlWriter xw = xml.Writer(sb))
				ToXml(xw, this);
            <!--XmlWriterSettings settings = new XmlWriterSettings(); settings.OmitXmlDeclaration = true; settings.Indent = true;
            XmlWriter xw = XmlTextWriter.Create(sb, settings);
            ToXml(xw, this); xw.Close();
			sb.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);
            sb.Replace("&lt;ArrayOf<xsl:value-of select="@ClassName" />&gt;", string.Empty);
            sb.Replace("&lt;/ArrayOf<xsl:value-of select="@ClassName" />&gt;", string.Empty);
            sb.AppendLine();-->
        }
		/// &lt;summary&gt;Appends to a StringBuilder the contents of the collection as an xml node, one tag per object wrapped in a node named by OuterTagName&lt;/summary&gt;
        public override void ToXml(StringBuilder sb, string OuterTagName)
        {
            using (XmlWriter xw = xml.Writer(sb))
				ToXml(xw, this);
            <!--XmlWriterSettings settings = new XmlWriterSettings(); settings.OmitXmlDeclaration = true; settings.Indent = true;
            XmlWriter xw = XmlTextWriter.Create(sb, settings);
            ToXml(xw, this); xw.Close();
			sb.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);-->
            sb.Replace("&lt;ArrayOf<xsl:value-of select="@ClassName" />&gt;", "&lt;" + OuterTagName + "&gt;");
            sb.Replace("&lt;/ArrayOf<xsl:value-of select="@ClassName" />&gt;", "&lt;/" + OuterTagName + "&gt;");
            <!--sb.AppendLine();-->
        }
		/// &lt;summary&gt;Deserializes a series of <xsl:value-of select="@ClassName" /> tags back into a collection. Nodes should be in the same format as generated by OuterXml&lt;/summary&gt;
        public static <xsl:value-of select="@ClassName" />Collection FromXml(string xmlNode)
        {
            <xsl:value-of select="@ClassName" />Collection oOut = null;
            if (!string.IsNullOrEmpty(xmlNode))
            {
				XmlReader xr = null;
				if (xmlNode.IndexOf("&lt;?xml") > -1)
					xr = XmlReader.Create(new StringReader(xmlNode));
				else
				{
					StringBuilder sb = new StringBuilder(xmlNode.Length + xml.Declaration.Length + 10);
					sb.AppendLine(xml.Declaration);
					sb.Append(xmlNode);
					xr = XmlReader.Create(new StringReader(sb.ToString()));
				}
				if (xr != null)
				{
					oOut = (<xsl:value-of select="@ClassName" />Collection) FromXml(xr);
					xr.Close();
				}
			}
            return oOut;
        }
    }
}

<!--
/*
&lt;!- - <xsl:value-of select="$ClassName" /> Long - -&gt;
        &lt;xsl:for-each select="<xsl:value-of select="$ClassName" />"&gt;
	      &lt;table&gt;
	      &lt;tr&gt;&lt;th colspan="2"&gt;<xsl:value-of select="$TableName" />&lt;/th&gt;&lt;/tr&gt;
	      &lt;tr&gt;&lt;th&gt;Name&lt;/th&gt;&lt;th&gt;Value&lt;/th&gt;&lt;/tr&gt;<xsl:for-each select="Columns/Column">
	      &lt;tr&gt;&lt;td&gt;<xsl:value-of select="@ColumnName" />&lt;/td&gt;&lt;td&gt;&lt;xsl:call-template name="gv"&gt;&lt;xsl:with-param name="v" select="@<xsl:value-of select="@PropertyName" />" /&gt;&lt;xsl:with-param name="t"&gt;<xsl:value-of select="@CSharpVariableType" />&lt;/xsl:with-param&gt;&lt;/xsl:call-template&gt;&lt;/td&gt;&lt;/tr&gt;</xsl:for-each>
	      &lt;/table&gt;
        &lt;/xsl:for-each&gt;

&lt;!- - <xsl:value-of select="$ClassName" /> Wide - -&gt;
      &lt;table&gt;
	    &lt;tr&gt;&lt;th colspan="<xsl:value-of select="count(Columns/Column)"/>"&gt;<xsl:value-of select="$Namespace" />&lt;/th&gt;&lt;/tr&gt;
	    &lt;tr&gt;&lt;th colspan="<xsl:value-of select="count(Columns/Column)"/>"&gt;<xsl:value-of select="$TableName" />&lt;/th&gt;&lt;/tr&gt;
	    &lt;tr&gt;<xsl:for-each select="Columns/Column">
	    &lt;th&gt;<xsl:value-of select="@ColumnName" />&lt;/th&gt;</xsl:for-each>&lt;/tr&gt;
        &lt;xsl:for-each select="<xsl:value-of select="$ClassName" />"&gt;
        &lt;tr&gt;<xsl:for-each select="Columns/Column">
	    &lt;td&gt;&lt;xsl:call-template name="gv"&gt;&lt;xsl:with-param name="v" select="@<xsl:value-of select="@PropertyName" />" /&gt;&lt;xsl:with-param name="t"&gt;<xsl:value-of select="@CSharpVariableType" />&lt;/xsl:with-param&gt;&lt;/xsl:call-template&gt;&lt;/td&gt;</xsl:for-each>&lt;/tr&gt;
        &lt;/xsl:for-each&gt;
      &lt;/table&gt;

*/
-->
    </exsl:document>
</xsl:template>
</xsl:stylesheet>
