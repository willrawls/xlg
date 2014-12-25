using System;
using System.Web;
using System.Data.SqlClient;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("MetX Library")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("William M. Rawls")]
[assembly: AssemblyProduct("XLG")]
[assembly: AssemblyCopyright("William M. Rawls 2009")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("2.6.0.0")]
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyName("")]

[assembly: AssemblyFileVersion("2.6.0.0")]
[assembly: ComVisibleAttribute(false)]
[assembly: CLSCompliant(true)]
[assembly: Guid("C4F77248-1E18-4A63-86EF-26AD836085B3")]

[assembly: IsolatedStorageFilePermission(SecurityAction.RequestMinimum, UserQuota = 1048576)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, UnmanagedCode = true)]
[assembly: FileIOPermission(SecurityAction.RequestOptional, Unrestricted = true)]
[assembly: AspNetHostingPermission(SecurityAction.RequestMinimum, Unrestricted = true)]
[assembly: ReflectionPermission(SecurityAction.RequestMinimum, Unrestricted = true)]
[assembly: SqlClientPermission(SecurityAction.RequestMinimum, Unrestricted = true)]
[assembly: AllowPartiallyTrustedCallersAttribute()]
[assembly: System.Net.WebPermission(SecurityAction.RequestMinimum, Unrestricted = true)]
[assembly: PermissionSet(SecurityAction.RequestMinimum, Unrestricted=true)]
[assembly: AssemblyDescriptionAttribute("XLG Core Functionality Library")]
