using System;
using System.Xml.Serialization;

namespace MetX.Standard.Primary.Metadata;

[Serializable]
public class RelationshipField
{
    [XmlAttribute] public string Left { get; set; }
    [XmlAttribute] public string Right { get; set; }
    [XmlAttribute] public int LeftPosition { get; set; }
    [XmlAttribute] public int RightPosition { get; set; }
}


public static class Extensions
{

    public static bool ConfirmRelationships(this xlgDoc xlgDoc)
    {
        foreach (var relationship in xlgDoc.Relationships)
        {
            if (xlgDoc
                .Tables
                .TrueForAll(t =>
                    string.Equals(t.Schema, relationship.LeftSchema, StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(t.TableName, relationship.LeftTable, StringComparison.InvariantCultureIgnoreCase)

                ))
            {

            }
        }

        return true;
    }

    /*
    public static bool ContainsColumn(this Table table, string columnName)
    {
        return table.Columns.Contains(c => string.Equals(c, columnName));
    }
    */

}

