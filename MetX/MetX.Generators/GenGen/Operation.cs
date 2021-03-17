namespace MetX.Generators.GenGen
{
    public enum Operation
    {
        Unknown,
        
        Create, // Create Generator, attribute and client projects
        GenGen, // Create Generator, attribute projects
        Client, // Create client project only
        Update, // Update Generator, attribute and client projects
        Inject, // Add existing generator to existing client project
        Check,  // Diagnostic. Like update but report only
        Remove, // Remove generator reference from client
                // Delete generator and attribute projects
        Clean,  // Delete any generated files/folder
        
    }
}