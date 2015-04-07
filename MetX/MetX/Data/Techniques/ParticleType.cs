using System;

namespace MetX.Data.Techniques
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