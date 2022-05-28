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
            var found = 0;
            foreach(var t in xlgDoc.Tables)
            {
                found = 0;
                if (string.Equals(t.Schema, relationship.LeftSchema, StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(t.TableName, relationship.LeftTable, StringComparison.InvariantCultureIgnoreCase))
                {
                    found++;
                }
                if (string.Equals(t.Schema, relationship.RightSchema, StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(t.TableName, relationship.RightTable, StringComparison.InvariantCultureIgnoreCase))
                {
                    found++;
                }

                if (found == 2)
                    break;
            }

            if (found < 2)
                return false;
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

