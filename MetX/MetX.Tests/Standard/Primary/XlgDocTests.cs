using System.Collections.Generic;
using MetX.Standard.Primary.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Primary;

[TestClass]
public class XlgDocTests
{
    [TestMethod]
    public void Relationships_Simple_1()
    {
        var data = new xlgDoc();

        data.Tables.Add(new Table
        {
            ClassName = "Bob_Person",
            Columns = new List<Column>
            {
                new Column
                {
                    ColumnName = "PersonID",
                    AutoIncrement = "true",
                },
            },
            TableName = "Person",
            Schema = "Bob",
            Keys = new List<Key>
            {
                new Key
                {
                    Columns = new List<KeyColumn>
                    {
                        new KeyColumn
                        {
                            Column = "PersonID",
                            Location = "1",
                        }
                    }
                }
            }
        });

        var relationship = new Relationship
        {
            Name = "FK_PersonID_Person_PersonID",
            Type = "OneToMany",
            Local = "Bob.Person",
            Foreign = "dbo.Employee",
            Fields = new List<RelationshipField>
            {
                new RelationshipField
                {
                    Local = "PersonID",
                    Foreign = "PersonID",
                },
            },
            Tags = new List<string>
            {
                "hidden",
                "implied",
                "explicit",
                "added"
            }
        };
        data.Relationships.Add(relationship);

        bool actual = data.ConfirmRelationships();
        Assert.IsTrue(actual);

    }

}
