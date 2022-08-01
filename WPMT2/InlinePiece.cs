using MetX.Standard.Strings;

namespace WilliamPersonalMultiTool
{
    public class InlinePiece
    {
        public string Contents { get; set; }
        public string Command { get; set; }
        public string Arguments { get; set; }

        public InlinePiece(string contents, bool isCommand)
        {
            Contents = contents;
            if (!isCommand) return;

            Command = contents.FirstToken().ToLower();
            Arguments = contents.TokensAfterFirst();
        }

        public override string ToString()
        {
            return Contents;
        }
    }
}