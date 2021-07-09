using System;

namespace MetX.Standard.Generators.Aspects
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