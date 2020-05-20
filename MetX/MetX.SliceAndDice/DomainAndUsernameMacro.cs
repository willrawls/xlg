using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;

namespace MetX.SliceAndDice
{
    [MacroDefinition("MetX.SliceAndDice.DomainAndUsername",
      ShortDescription = "Current username with domain",
      LongDescription = "Current username with domain on the format <Domain>\\<Username>")]
    public class DomainAndUsernameMacro : SimpleMacroDefinition
    {
    }
}