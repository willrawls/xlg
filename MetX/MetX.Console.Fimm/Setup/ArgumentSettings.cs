using System.Collections.Generic;
using System.Linq;
using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Strings;

namespace MetX.Fimm.Setup;

public class ArgumentSettings
{
    public ArgumentNoun Noun;
    public ArgumentVerb Verb;

    public string Name{ get; set; }
    public string Path{ get; set; }
    public string ConnectionString{ get; set; }
    public List<string> AdditionalArguments { get; set; } = new();
    public IGenerationHost Host;

    public bool IsValid
    {
        get
        {
            var valid = Name.IsNotEmpty() 
                        && Noun != ArgumentNoun.Unknown
                        && Verb != ArgumentVerb.Unknown
                        && AdditionalArguments != null
                ;
            return valid;
        }
    }

    public override string ToString()
    {
        var result = $@"
 Argument Settings
    Name: {Name}
    Noun: {Noun}
    Verb: {Verb}
    Path: {Path}
    Conn: {ConnectionString}
";
        if (AdditionalArguments.IsEmpty())
            return result;

        result += "Additional Arguments:\n";

        return AdditionalArguments.Aggregate(result, (current, argument) => current + $"{argument}\n");  
    }
}