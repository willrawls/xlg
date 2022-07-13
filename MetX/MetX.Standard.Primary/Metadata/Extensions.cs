using System;
using System.Collections.Generic;
using System.Linq;
using MetX.Standard.Library.Extensions;

namespace MetX.Standard.Primary.Metadata;

public static class Extensions
{

    public static bool ConfirmRelationships(this xlgDoc xlgDoc, string requiredTag, out List<Relationship> confirmed, out List<Relationship> bad)
    {
        confirmed = new List<Relationship>();
        bad = new List<Relationship>();

        foreach (var relationship in xlgDoc.Relationships)
        {
            if(xlgDoc.ConfirmRelationship(relationship, requiredTag))
                confirmed.Add(relationship);
            else
                bad.Add(relationship);
        }

        return bad.Count == 0;
    }

    public static bool ConfirmRelationships(this xlgDoc xlgDoc, string requiredTag = "")
    {
        if(requiredTag.IsEmpty())
            return xlgDoc.Relationships.All(r => xlgDoc.ConfirmRelationship(r, requiredTag));

        return xlgDoc
            .Relationships
            .Where(r => r
                .Tags
                .Any(tag => string
                    .Equals(tag, requiredTag, StringComparison
                        .InvariantCultureIgnoreCase)))
            .All(r => xlgDoc.ConfirmRelationship(r, requiredTag));
    }

    public static bool ConfirmRelationship(this xlgDoc xlgDoc, Relationship relationship, string requiredTag = "")
    {
        var leftTable = xlgDoc.FindTable(relationship.LeftSchema, relationship.LeftTable);
        if (leftTable == null) return false;

        var rightTable = xlgDoc.FindTable(relationship.RightSchema, relationship.RightTable);
        if (rightTable == null) return false;

        foreach (var field in relationship.Fields)
        {
            var leftColumn = leftTable.FindColumn(field.Left);
            if (leftColumn == null) return false;

            var rightColumn = rightTable.FindColumn(field.Right);
            if (rightColumn == null) return false;
        }

        if (requiredTag.IsEmpty())
            return true;
        
        return relationship.Tags.IsEmpty() 
               || !relationship.Tags
                   .Any(t => string
                       .Equals(requiredTag, t));
    }

    public static Table FindTable(this xlgDoc xlgDoc, string schemaName, string tableName)
    {
        if (xlgDoc == null || xlgDoc.Tables.IsEmpty())
            return null;

        foreach (var table in xlgDoc.Tables)
        {
            if (string.Equals(table.Schema.AsString("dbo"), schemaName.AsString("dbo"), StringComparison.InvariantCultureIgnoreCase) 
                && string.Equals(table.TableName, tableName, StringComparison.InvariantCultureIgnoreCase)) 
                return table;
        }

        return null;
    }
    
    public static Column FindColumn(this xlgDoc xlgDoc, string schemaName, string tableName, string columnName)
    {
        if (xlgDoc == null || xlgDoc.Tables.IsEmpty())
            return null;

        return xlgDoc
            .Tables
            .Where(table => string.Equals(table.Schema, schemaName, StringComparison.InvariantCultureIgnoreCase) 
                            && string.Equals(table.TableName, tableName, StringComparison.InvariantCultureIgnoreCase))
            .Select(table => table
                .Columns.FirstOrDefault(column => string
                    .Equals(column.ColumnName, columnName, StringComparison.InvariantCultureIgnoreCase)))
            .FirstOrDefault();
    }
    
    public static Column FindColumn(this Table table, string columnName)
    {
        if (table == null || table.Columns.IsEmpty())
            return null;

        return table.Columns
            .FirstOrDefault(column => 
                string.Equals(column.ColumnName, columnName, StringComparison.InvariantCultureIgnoreCase));
    }
    

}