using System;
using System.Xml.Serialization;

namespace MetX.Techniques
{
    [Serializable]
    public class FolderParticle : Particle
    {
        [XmlArray(ElementName = "Files")]
        [XmlArrayItem(typeof(FileParticle), ElementName = "File")]
        public ParticleList<FileParticle> Files;

        [XmlArray(ElementName = "Folders")]
        [XmlArrayItem(typeof(FolderParticle), ElementName = "Folder")]
        public ParticleList<FolderParticle> Folders;

        public FolderParticle() { }

        public FolderParticle(ParticleList<FileParticle> files, ParticleList<FolderParticle> folders = null)
        {
            Files = files;
            Folders = folders;
        }
    }
}