using CommandLine;

namespace MetX.Generators
{
    public class CommandLineOptions
    {
        [Option('m', "mode", Required = false, HelpText = @"
    NOTE: When this parameter is missing, 'create' is assumed,

        create (generate a new generator, new attribute, and new client projects)
        gengen (generate a generator and attribute projects only)
        client (generate only a new client project)

        update (Update a previously created generator as needed),
            NOTE: Say when you've renamed classes, moved files, 
                  directories, or whenever something breaks 
                  the changes normally made

        inject (Add an existing generator to a project),
            NOTE: This will only modify an existing csproj

        check (Double check the setup of an existing generator and client),
            NOTE: Nothing changes here, just a report on the state / usability

        clean (remove any generated files in the client project folder)
            NOTE: Only from the 'Generated' folder this program 
                  adds to the client project

        remove (Delete the generator and aspect projects. Remove reference from client)
            NOTE: Will not delete the folders 
                  or any files that would not have been created
")]
        public GenGen.Operation Operation { get; set; }
        
        // Operation
        [Option('b', "build", Required = true, HelpText = "Build generator and client when done")]
        public bool Build { get; set; }
        
        [Option('s', "namespace", Required = true, HelpText = "Namespace of generator")]
        public string Namespace { get; set; }

        [Option('c', "class", Required = true, HelpText = "Class name of generator")]
        public string GeneratorName { get; set; }

        [Option('a', "attribute", Required = true, HelpText = "Name of generate attribute ()")]
        public string AttributeName { get; set; }

        [Option('f', "folder", Required = false, HelpText = "Root folder for generation (default is the current directory)")]
        public string RootFolder { get; set; }

        [Option('x', "client", Required = false, HelpText = "Path to a template for the client project (default is to use the built in template)")]
        public string ClientTemplate { get; set; }

        [Option('y', "generator", Required = false, HelpText = "Path to a template (or template folder) for the generator project (default is to use the built in template)")]
        public string GeneratorTemplate { get; set; }

        [Option('z', "aspects", Required = false, HelpText = "Path to a template for the aspects (attribute) project (default is to use the built in template)")]
        public string AspectsTemplate { get; set; }

        [Option('p', "client project", Required = false, HelpText = "Path to csproj the generator should be added as an analyzer (Default is the csproj of the same name as the folder parameter)")]
        public string AddTo { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }
        
    }
}