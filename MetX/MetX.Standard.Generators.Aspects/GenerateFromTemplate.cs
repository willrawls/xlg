using System;

namespace MetX.Standard.Generators.Aspects
{
    [AttributeUsage(AttributeTargets.Class)]
    // ReSharper disable once UnusedType.Global
    public class GenerateFromTemplate : Attribute 
    {
        public readonly string TemplateFilePath;

        public GenerateFromTemplate(string templateFilePath)
        {
            TemplateFilePath = templateFilePath;
        }
    }
}