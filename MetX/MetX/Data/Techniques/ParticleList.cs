using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MetX.Data.Techniques
{
    [Serializable]
    public class ParticleList<T> : List<T> where T : Particle, new()
    {
        [XmlArray(ElementName = "Files")]
        [XmlArrayItem(typeof(FileParticle), ElementName = "File")]
        public ParticleList<FileParticle> Files;

        [XmlArray(ElementName = "Folders")]
        [XmlArrayItem(typeof(FolderParticle), ElementName = "Folder")]
        public ParticleList<FolderParticle> Folders;

        [XmlAttribute]
        public Guid Id;

        [XmlIgnore]
        public ParticleList Parent { get; set; }

        public ParticleList()
        {
        }

        public ParticleList(ParticleList parent)
        {
            Parent = parent;
        }

        public ParticleList(ParticleList parent, ParticleList files, ParticleList folders, Guid id)
        {
            Files = files;
            Folders = folders;
            Id = id;
            Parent = parent;
        }
    }

    public class FolderParticle : Particle
    {
    }

    public class FileParticle : Particle
    {
    }
}