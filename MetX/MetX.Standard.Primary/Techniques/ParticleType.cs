using System;

namespace MetX.Standard.Primary.Techniques
{
    [Serializable]
    public enum ParticleType
    {
        Unknown,
        RealFile,
        RealFolder,
        VirtualFile,
        VirtualFolder,
        Url,
        Database,
        Variable,
        Connetion,
        Provider,
    }
}