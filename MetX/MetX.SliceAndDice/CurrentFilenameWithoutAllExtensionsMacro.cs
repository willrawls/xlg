using System;
using System.Collections.Generic;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;
using JetBrains.ReSharper.Psi;
using JetBrains.Util;

namespace MetX.SliceAndDice
{
    [MacroDefinition("ResharperExt.CurrentFilenameWithoutAllExtensionsMacro",
    ShortDescription = "Current file name without any extension",
    LongDescription = "Current file name without any extension. If several extensions applied, then all will be removed.")]
    public class CurrentFilenameWithoutAllExtensionsMacroDefinition : IMacroDefinition
    {
        public string GetPlaceholder(IDocument document, IEnumerable<IMacroParameterValue> parameters)
        {
            return "A";
        }

        public ParameterInfo[] Parameters
        {
            get { return EmptyArray<ParameterInfo>.Instance; }
        }
    }

    [MacroImplementation(Definition = typeof(CurrentFilenameWithoutAllExtensionsMacroDefinition))]
    public class CurrentFilenameWithoutAllExtensionsMacroImplementation : SimpleMacroImplementation
    {
       
        public override string EvaluateQuickResult(IHotspotContext context)
        {
            var sourceFile = context.ExpressionRange.Document.GetPsiSourceFile(context.SessionContext.Solution);
            var filename = sourceFile != null ? sourceFile.Name : null;
            if(string.IsNullOrEmpty(filename))
                return "failed";
            var pos = filename.IndexOf(".",StringComparison.InvariantCultureIgnoreCase);
            if (pos == -1)
                return filename;
            var result = filename.Substring(0, pos);
            return result;
        }
    }
}