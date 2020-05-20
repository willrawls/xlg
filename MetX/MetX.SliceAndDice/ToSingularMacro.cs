using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;

namespace MetX.SliceAndDice
{
    [MacroDefinition("ResharperExt.ToSingular",
        ShortDescription = "Value of another variable to singular.",
        LongDescription = "Value of another variable to singular.")]
    public class ToSingularMacroDefinition : IMacroDefinition
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

    [MacroImplementation(Definition = typeof (ToSingularMacroDefinition))]
    public class ToSingularMacroImplementation : SimpleMacroImplementation
    {
        private readonly MacroParameterValueCollection _paramCollection;

        public ToSingularMacroImplementation([Optional] MacroParameterValueCollection paramCollection)
        {
            _paramCollection = paramCollection;
        }

        public override string EvaluateQuickResult(IHotspotContext context)
        {
            var myArg = _paramCollection.OptionalFirstOrDefault();
            if (myArg == null) return string.Empty;

            var value = myArg.GetValue();
            if (value.EndsWith("s"))
            {
                value = value.Remove(value.Length - 1);
            }
            return value;
        }
    }
}