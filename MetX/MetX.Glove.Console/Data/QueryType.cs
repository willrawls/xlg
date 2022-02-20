namespace XLG.Pipeliner.Data
{
    /// <summary>C#CD: </summary>
    public enum QueryType
    {
        /// <summary>SELECT FROM query</summary>
        Select,
        /// <summary>UPDATE query</summary>
        Update,
        /// <summary>INSERT INTO query</summary>
        Insert,
        /// <summary>DELETE FROM query</summary>
        Delete,
        /// <summary>IF EXISTS(query) SELECT 1 ELSE SELECT 0</summary>
        Exists,
        /// <summary>SELECT COUNT(*) FROM query</summary>
        Count,
        /// <summary>SELECT MAX(*) FROM query</summary>
        Max,
        /// <summary>SELECT MIN(*) FROM query</summary>
        Min

    }
}