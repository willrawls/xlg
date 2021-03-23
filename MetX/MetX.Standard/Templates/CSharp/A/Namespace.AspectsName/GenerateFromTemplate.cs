using System;

namespace ~~Namespace~~.~~AspectsName~~
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GenerateFromTemplate : Attribute 
    {
        public readonly string TemplateFilePath;

        public GenerateFromTemplate(string templateFilePath)
        {
            TemplateFilePath = templateFilePath;
        }
    }
}