namespace MetX.Standard.Strings.Tests
{
    [TestClass]
    public class AssocArrayTests2
    {
        [TestMethod]
        public void AddAndRetrieveItem_ByKey()
        {
            var aa = new AssocArray();
            aa["TestKey"].Value = "TestValue";
            Assert.AreEqual("TestValue", aa["TestKey"].Value);
        }

        [TestMethod]
        public void ContainsKey_ReturnsTrueIfExists()
        {
            var aa = new AssocArray();
            aa["Key1"].Value = "Value1";
            Assert.IsTrue(aa.ContainsKey("Key1"));
            Assert.IsFalse(aa.ContainsKey("Key2"));
        }

        [TestMethod]
        public void Values_ReturnsAllValues()
        {
            var aa = new AssocArray();
            aa["A"].Value = "1";
            aa["B"].Value = "2";
            var values = aa.Values;
            CollectionAssert.Contains(values, "1");
            CollectionAssert.Contains(values, "2");
        }

        [TestMethod]
        public void Keys_ReturnsAllKeys()
        {
            var aa = new AssocArray();
            aa["A"].Value = "1";
            aa["B"].Value = "2";
            var keys = aa.Keys;
            CollectionAssert.Contains(keys, "A");
            CollectionAssert.Contains(keys, "B");
        }

        [TestMethod]
        public void Numbers_ReturnsAllNumbers()
        {
            var aa = new AssocArray();
            aa["A"].Number = 5;
            aa["B"].Number = 10;
            var numbers = aa.Numbers;
            CollectionAssert.Contains(numbers, 5);
            CollectionAssert.Contains(numbers, 10);
        }

        [TestMethod]
        public void Names_ReturnsAllNames()
        {
            var aa = new AssocArray();
            aa["A"].Name = "Alpha";
            aa["B"].Name = "Beta";
            var names = aa.Names;
            CollectionAssert.Contains(names, "Alpha");
            CollectionAssert.Contains(names, "Beta");
        }

        [TestMethod]
        public void Ids_ReturnsAllIds()
        {
            var aa = new AssocArray();
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            aa["A"].ID = id1;
            aa["B"].ID = id2;
            var ids = aa.Ids;
            CollectionAssert.Contains(ids, id1);
            CollectionAssert.Contains(ids, id2);
        }

        [TestMethod]
        public void FirstKeyContaining_FindsCorrectItem()
        {
            var aa = new AssocArray();
            aa["Alpha"].Value = "1";
            aa["Beta"].Value = "2";
            var found = aa.FirstKeyContaining("alp");
            Assert.IsNotNull(found);
            Assert.AreEqual("Alpha", found.Key);
        }

        [TestMethod]
        public void ToString_OutputsKeyValuePairs()
        {
            var aa = new AssocArray();
            aa["A"].Value = "1";
            aa["B"].Value = "2";
            var str = aa.ToString();
            Assert.IsTrue(str.Contains("A=1"));
            Assert.IsTrue(str.Contains("B=2"));
        }

        [TestMethod]
        public void Resolve_ReplacesPlaceholders()
        {
            var aa = new AssocArray();
            aa["X"].Value = "foo";
            aa["Y"].Value = "bar";
            var result = aa.Resolve("Value is %X% and %Y%.");
            Assert.AreEqual("Value is foo and bar.", result);
        }

        [TestMethod]
        public void SaveAndLoad_PersistsData()
        {
            var tempFile = Path.GetTempFileName();
            try
            {
                var aa = new AssocArray { FilePath = tempFile };
                aa["A"].Value = "1";
                aa.Save();

                var loaded = AssocArray.Load(tempFile);
                Assert.AreEqual("1", loaded["A"].Value);
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        [TestMethod]
        public void HandleAutoPersist_SavesWhenEnabled()
        {
            var tempFile = Path.GetTempFileName();
            try
            {
                var aa = new AssocArray { FilePath = tempFile, AutoPersist = true };
                aa["A"].Value = "1";
                aa.HandleAutoPersist();
                Assert.IsTrue(File.Exists(tempFile));
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        [TestMethod]
        public void DefaultKeyComparer_WorksAsExpected()
        {
            var item = new TimeTrackingAssocItem("key", "val", null, "name");
            Assert.IsTrue(AssocArray.DefaultKeyComparer("key", item));
            Assert.IsTrue(AssocArray.DefaultKeyComparer("name", item));
            Assert.IsFalse(AssocArray.DefaultKeyComparer("other", item));
        }
    }
}
