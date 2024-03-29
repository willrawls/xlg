; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "qkScrptr"
#define MyAppVersion "5.1"
#define MyAppPublisher "William M. Rawls"
#define MyAppURL "https://www.github.com/willrawls/xlg"
#define MyAppExeName "qkScrptr.exe"
#define MyAppAssocName MyAppName + " File"
#define MyAppAssocExt ".xlgs"
#define MyAppAssocKey StringChange(MyAppAssocName, " ", "") + MyAppAssocExt

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{A59C107A-3FC4-4E46-A5E5-6255DAF281F2}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
ChangesAssociations=yes
DisableProgramGroupPage=yes
LicenseFile=I:\OneDrive\Data\code\GitHub\xlg\LICENSE.txt
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
OutputDir=I:\OneDrive\Data\code\GitHub\xlg\Setups\qkScrptr
OutputBaseFilename=qkScrptrSetup510
SetupIconFile=I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.Glove.Console\Resources\AnnotateDefault.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\ICSharpCode.TextEditorEx.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\MetX.Controls.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\MetX.Controls.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\MetX.Standard.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\MetX.Standard.Generators.Aspects.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\MetX.Standard.Generators.Aspects.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\MetX.Standard.Library.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\MetX.Standard.Library.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\MetX.Standard.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\MetX.Windows.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\MetX.Windows.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\Microsoft.CodeAnalysis.CSharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\Microsoft.CodeAnalysis.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\mvp.xml.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\NHotkey.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\NHotkey.WindowsForms.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\NHotPhrase.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\NHotPhrase.WindowsForms.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\System.Data.SqlClient.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\System.Drawing.Common.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\xlgQuickScripts.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\xlgQuickScripts.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\xlgQuickScripts.dll.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\xlgQuickScripts.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\xlgQuickScripts.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\xlgQuickScripts.runtimeconfig.dev.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\OneDrive\Data\code\GitHub\xlg\MetX\MetX.QuickScripts\bin\Debug\net5.0-windows\xlgQuickScripts.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Registry]
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocExt}\OpenWithProgids"; ValueType: string; ValueName: "{#MyAppAssocKey}"; ValueData: ""; Flags: uninsdeletevalue
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}"; ValueType: string; ValueName: ""; ValueData: "{#MyAppAssocName}"; Flags: uninsdeletekey
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppExeName},0"
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""
Root: HKA; Subkey: "Software\Classes\Applications\{#MyAppExeName}\SupportedTypes"; ValueType: string; ValueName: ".myp"; ValueData: ""

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

