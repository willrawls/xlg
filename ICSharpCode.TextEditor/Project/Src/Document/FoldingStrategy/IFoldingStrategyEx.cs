using System.Collections.Generic;

namespace ICSharpCode.TextEditor.Document.FoldingStrategy
{
    public interface IFoldingStrategyEx : IFoldingStrategy
    {
        List<string> GetFoldingErrors();
    }
}