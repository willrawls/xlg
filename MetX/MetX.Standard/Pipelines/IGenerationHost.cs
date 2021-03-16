namespace MetX.Standard.Pipelines
{
    public interface IGenerationHost
    {
        IMessageBox MessageBox { get; set; }
        MessageBoxResult InputBoxRef(string title, string description, ref string itemName);
    }
}