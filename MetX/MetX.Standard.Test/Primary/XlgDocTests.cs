using System.Collections.Generic;
using MetX.Standard.Primary.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.Primary;

[TestClass]
public class XlgDocTests
{
    [TestMethod]
    public void Relationships_Simple_1_Confirmed()
    {
        var data = new xlgDoc().MakeViable();
        data.Tables.Add(_bobPersonTable);
        data.Tables.Add(_dboEmployeeTable);
        data.Relationships.Add(_bobPersonDboEmployeeRelationship);

        var actual = data.ConfirmRelationships("", out var confirmed, out var bad);
        Assert.IsTrue(actual);

        Assert.AreEqual(1, confirmed.Count);
        Assert.AreEqual(0, bad.Count);
    }

    [TestMethod]
    public void Relationships_Simple_1_Bad()
    {
        var data = new xlgDoc().MakeViable();
        data.Tables.Add(_bobPersonTable);
        data.Tables.Add(_dboEmployeeTable);

        _bobPersonDboEmployeeRelationship.Fields[0].Right = "Paul";
        data.Relationships.Add(_bobPersonDboEmployeeRelationship);

        var actual = data.ConfirmRelationships("", out var confirmed, out var bad);
        Assert.IsFalse(actual);

        Assert.AreEqual(0, confirmed.Count);
        Assert.AreEqual(1, bad.Count);
    }

    Relationship _bobPersonDboEmployeeRelationship = new()
    {
        Name = "FK_PersonID_Person_PersonID",
        Type = "OneToMany",
        LeftSchema = "Bob",
        LeftTable = "Person",
        RightSchema = "dbo",
        RightTable = "Employee",
        Fields = new List<RelationshipField>
        {
            new()
            {
                Left = "PersonID",
                Right = "PersonID",
                LeftPosition = 1,
                RightPosition = 1,
            },
        },
        Tags = new List<string>
        {
            "explicit",
        }
    };

    private Table _bobPersonTable = new()
    {
        Schema = "Bob",
        TableName = "Person",
        ClassName = "Bob_Person",
        Columns = new List<Column>
        {
            new()
            {
                ColumnName = "PersonID",
                AutoIncrement = "true",
            },
        },
        Keys = new List<Key>
        {
            new()
            {
                Columns = new List<KeyColumn>
                {
                    new()
                    {
                        Column = "PersonID",
                        Location = "1",
                    }
                }
            }
        }
    };

    private Table _dboEmployeeTable = new()
    {
        Schema = "dbo",
        TableName = "Employee",
        ClassName = "dbo_Employee",
        Columns = new List<Column>
        {
            new()
            {
                ColumnName = "EmployeeID",
                AutoIncrement = "true",
            },
            new()
            {
                ColumnName = "PersonID",
            },
        },
        Keys = new List<Key>
        {
            new()
            {
                Columns = new List<KeyColumn>
                {
                    new()
                    {
                        Column = "EmployeeID",
                        Location = "1",
                    }
                }
            }
        }
    };
}
