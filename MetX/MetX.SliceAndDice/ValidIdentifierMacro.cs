using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;

namespace MetX.SliceAndDice
{
    [MacroDefinition("MetX.SliceAndDice.ValidIdentifier",
      ShortDescription = "Ensures that given variable is a valid C# identifier",
      LongDescription = "Replaces invalid characters in a C# identifier with underscores")]
    public class ValidIdentifierMacro : SimpleMacroDefinition
    {
    }
}