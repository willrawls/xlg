using System.Collections.Generic;

namespace MetX.Five.Setup;

public class ArgumentSettings
{
    public ArgumentSettings()
    {
    }

    public ArgumentNoun Noun;
    public ArgumentVerb Verb;

    public string Name{ get; set; }
    public string Path{ get; set; }
    public string ConnectionString{ get; set; }
    public List<string> AdditionalArguments { get; set; }
}