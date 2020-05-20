using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;

namespace MetX.SliceAndDice
{
    [MacroDefinition("ResharperExt.ToUpperConstant",
        ShortDescription = "Value of another variable to Upper Case and separated by _",
        LongDescription = "Value of another variable to Upper Case and separated by _")]
    public class ToUpperConstantMacroDefinition : IMacroDefinition
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

    [MacroImplementation(Definition = typeof (ToUpperConstantMacroDefinition))]
    public class ToUpperConstantMacroImplementation : SimpleMacroImplementation
    {
        private readonly MacroParameterValueCollection _paramCollection;

        public ToUpperConstantMacroImplementation([Optional] MacroParameterValueCollection paramCollection)
        {
            _paramCollection = paramCollection;
        }

        public override string EvaluateQuickResult(IHotspotContext context)
        {
            var myArg = _paramCollection.OptionalFirstOrDefault();
            if (myArg == null) return string.Empty;

            //let's build our custom test LongBoard == LONG_BOARD
            var value = myArg.GetValue();
            //detect capital letters and add them a low dash before them _S
            var newText = new StringBuilder();
            newText.Append(value[0]);
            for (var i = 1; i < value.Length; i++)
            {
                if (char.IsUpper(value[i]))
                {
                    newText.Append('_');
                } 
                newText.Append(value[i]);
            }
            return newText.ToString().ToUpper();
        }
    }
}