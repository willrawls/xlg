using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;

namespace MetX.SliceAndDice
{
    [MacroImplementation(Definition = typeof(ValidIdentifierMacro))]
    public class ValidIdentifierMacroImpl : QuickParameterlessMacro
    {
        public override string QuickEvaluate(string value)
        {
            var identifierBuilder = new IdentifierBuilder(value);
            identifierBuilder.TransformToValidIdentifier();
            return identifierBuilder.ToString();
        }

    }
}