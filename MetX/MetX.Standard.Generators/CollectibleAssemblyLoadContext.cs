using System.Reflection;
using System.Runtime.Loader;

namespace MetX.Standard.Generators;

public class CollectibleAssemblyLoadContext : AssemblyLoadContext
{
    protected override Assembly Load(AssemblyName assemblyName)
    {
        return null;
    }
}