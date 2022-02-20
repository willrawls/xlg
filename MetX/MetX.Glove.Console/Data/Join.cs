namespace XLG.Pipeliner.Data
{
    public class Join
    {
        public string FromColumn;
        public string ToColumn;
        public string JoinTable;
        public string JoinType = "INNER JOIN";
    }
}