&lt;Project DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003"&gt;
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
      &lt;HintPath&gt;..\..\MetX\MetX.Glove.Console\bin\Debug\MetX.Data.Factory.System_Data_SqlClient.dll&lt;/HintPath&gt;
    &lt;/Reference&gt;
    &lt;Reference Include="System" /&gt;
    &lt;Reference Include="System.configuration" /&gt;
    &lt;Reference Include="System.Data" /&gt;
    &lt;Reference Include="System.Xml" /&gt;
    <xsl:choose>
    <xsl:when test="/xlgDoc/@DatabaseProvider='SybaseDataProvider'">
    &lt;Reference Include="<xsl:value-of select="/xlgDoc/@ProviderAssemblyString" />, processorArchitecture=MSIL"&gt;
      &lt;SpecificVersion&gt;False&lt;/SpecificVersion&gt;
      &lt;HintPath&gt;..\..\MetX\MetX.Glove.Console\bin\Debug\Sybase.Data.AseClient.dll&lt;/HintPath&gt;
    &lt;/Reference&gt;
    </xsl:when>
    <xsl:when test="/xlgDoc/@DatabaseProvider='MySqlDataProvider'">
    &lt;Reference Include="Mysql.Data" /&gt;
    </xsl:when>
    </xsl:choose>
  &lt;/ItemGroup&gt;
  &lt;ItemGroup&gt;
  <xsl:for-each select="Tables/Table">
    &lt;Compile Include="<xsl:value-of select="$Namespace" />.<xsl:value-of select="@ClassName" />.Glove.cs" /&gt;
  </xsl:for-each>
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