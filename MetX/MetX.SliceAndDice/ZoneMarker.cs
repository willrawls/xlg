using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services;

namespace MetX.SliceAndDice
{
    [ZoneMarker]
    public class ZoneMarker : IRequire<ICodeEditingZone>
    {
         
    }
}