using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;

namespace MetX.SliceAndDice
{
    [MacroDefinition("MetX.SliceAndDice.ApplyRegexMacro",
      ShortDescription = "Replace string matching {#0:pattern} with {#1:string} in {#2:variable}", 
      LongDescription = "Applies regex to replace value in another template variable.")]
    public class ApplyRegexMacro : SimpleMacroDefinition
    {
    }

    [MacroImplementation(Definition = typeof(ApplyRegexMacro))]
    class ApplyRegexMacroImpl : QuickParameterlessMacro
    {
        public override string QuickEvaluate(string value)
        {
            throw new NotImplementedException();
        }

        public string EvaluateQuickResult(IHotspotContext context, IList<string> arguments)
        {
            if (arguments.Count != 3)
            {
                return null;
            }
            else
            {
                try
                {
                    var result = Regex.Replace(arguments[2], arguments[0], arguments[1], RegexOptions.IgnoreCase);

                    return result;
                }
                catch (Exception e)
                {
                    return "<" + e.Message + ">";
                }
            }
        }

        public HotspotItems GetLookupItems(IHotspotContext context, IList<string> arguments)
        {
            return null;
        }

        public string GetPlaceholder()
        {
            return "a";
        }

        public bool HandleExpansion(IHotspotContext context, IList<string> arguments)
        {
            return false;
        }

        public ParameterInfo[] Parameters
        {
            get
            {
                return new[] { new ParameterInfo(ParameterType.String), new ParameterInfo(ParameterType.String), new ParameterInfo(ParameterType.VariableReference) };           
            }
        }        
    }
}