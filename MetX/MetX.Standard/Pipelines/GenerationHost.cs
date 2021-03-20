namespace MetX.Standard.Pipelines
{
    public class GenerationHost : IGenerationHost
    {
        public IMessageBox MessageBox { get; set; }
        public virtual MessageBoxResult InputBoxRef(string title, string description, ref string itemName)
        {
            return MessageBoxResult.Unknown;
        }
    }
}