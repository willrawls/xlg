using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;

namespace MetX.SliceAndDice
{
    [MacroDefinition("ResharperExt.ToUpperMacro",
        ShortDescription = "Value of another variable to Upper Case",
        LongDescription = "Value of another variable to Upper Case")]
    public class ToUpperMacroDefinition : IMacroDefinition
    {
        public string GetPlaceholder(IDocument document, IEnumerable<IMacroParameterValue> parameters)
        {
            return "A";
        }

        public ParameterInfo[] Parameters
        {
            get { return new[] {new ParameterInfo(ParameterType.VariableReference)}; }
        }
    }

    [MacroImplementation(Definition = typeof (ToUpperMacroDefinition))]
    public class ToUpperMacroImplementation : SimpleMacroImplementation
    {
        private readonly MacroParameterValueCollection _paramCollection;

        public ToUpperMacroImplementation([Optional] MacroParameterValueCollection paramCollection)
        {
            _paramCollection = paramCollection;
        }

        public override string EvaluateQuickResult(IHotspotContext context)
        {
            var myArg = _paramCollection.OptionalFirstOrDefault();
            return myArg == null
                ? null
                : myArg.GetValue().ToUpperInvariant();
        }
    }
}