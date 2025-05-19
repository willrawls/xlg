using MetX.Standard.Strings.Assoc;

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


        // ===== Values Property =====
        [TestMethod]
        public void Values_EmptyItems_ReturnsEmptyArray()
        {
            var aa = new AssocArray();
            CollectionAssert.AreEqual(Array.Empty<string>(), aa.Values);
        }

        [TestMethod]
        public void Values_SingleItem_ReturnsArrayWithValue()
        {
            var aa = new AssocArray();
            aa.Items.Add(new BasicAssocItem("k", "v"));
            CollectionAssert.AreEqual(new[] { "v" }, aa.Values);
        }

        [TestMethod]
        public void Values_MultipleItems_ReturnsAllValues()
        {
            var aa = new AssocArray();
            aa.Items.Add(new BasicAssocItem("k1", "v1"));
            aa.Items.Add(new BasicAssocItem("k2", "v2"));
            CollectionAssert.AreEqual(new[] { "v1", "v2" }, aa.Values);
        }

        [TestMethod]
        public void Values_ItemWithNullValue_ReturnsNullEntry()
        {
            var aa = new AssocArray();
            aa.Items.Add(new BasicAssocItem("k1", null));
            CollectionAssert.AreEqual(new[] { (string)null }, aa.Values);
        }

        [TestMethod]
        public void Values_ModifyingItems_ReflectsNewValues()
        {
            var aa = new AssocArray();
            aa.Items.Add(new BasicAssocItem("k1", "v1"));
            aa.Items[0].Value = "v1-mod";
            CollectionAssert.AreEqual(new[] { "v1-mod" }, aa.Values);
        }

        // ===== Numbers Property =====
        [TestMethod]
        public void Numbers_EmptyItems_ReturnsEmptyArray()
        {
            var aa = new AssocArray();
            CollectionAssert.AreEqual(Array.Empty<int>(), aa.Numbers);
        }

        [TestMethod]
        public void Numbers_SingleItem_ReturnsSingleNumber()
        {
            var aa = new AssocArray();
            var item = new BasicAssocItem("k", "v") { Number = 42 };
            aa.Items.Add(item);
            CollectionAssert.AreEqual(new[] { 42 }, aa.Numbers);
        }

        [TestMethod]
        public void Numbers_MultipleItems_ReturnsAllNumbers()
        {
            var aa = new AssocArray();
            aa.Items.Add(new BasicAssocItem("k1", "v1") { Number = 1 });
            aa.Items.Add(new BasicAssocItem("k2", "v2") { Number = 2 });
            CollectionAssert.AreEqual(new[] { 1, 2 }, aa.Numbers);
        }

        [TestMethod]
        public void Numbers_DefaultNumber_ReturnsZero()
        {
            var aa = new AssocArray();
            aa.Items.Add(new BasicAssocItem("k", "v"));
            CollectionAssert.AreEqual(new[] { 0 }, aa.Numbers);
        }

        [TestMethod]
        public void Numbers_ModifyingItems_ReflectsNewNumbers()
        {
            var aa = new AssocArray();
            var item = new BasicAssocItem("k", "v") { Number = 5 };
            aa.Items.Add(item);
            item.Number = 10;
            CollectionAssert.AreEqual(new[] { 10 }, aa.Numbers);
        }

        // ===== Names Property =====
        [TestMethod]
        public void Names_EmptyItems_ReturnsEmptyArray()
        {
            var aa = new AssocArray();
            CollectionAssert.AreEqual(Array.Empty<string>(), aa.Names);
        }

        [TestMethod]
        public void Names_SingleItem_ReturnsName()
        {
            var aa = new AssocArray();
            var item = new BasicAssocItem("k", "v", name: "Name1");
            aa.Items.Add(item);
            CollectionAssert.AreEqual(new[] { "Name1" }, aa.Names);
        }

        [TestMethod]
        public void Names_MultipleItems_ReturnsAllNames()
        {
            var aa = new AssocArray();
            aa.Items.Add(new BasicAssocItem("k1", "v1", name: "N1"));
            aa.Items.Add(new BasicAssocItem("k2", "v2", name: "N2"));
            CollectionAssert.AreEqual(new[] { "N1", "N2" }, aa.Names);
        }

        [TestMethod]
        public void Names_ItemWithNullName_ReturnsNullEntry()
        {
            var aa = new AssocArray();
            aa.Items.Add(new BasicAssocItem("k", "v", name: null));
            CollectionAssert.AreEqual(new string[] { null }, aa.Names);
        }

        [TestMethod]
        public void Names_ModifyingItems_ReflectsNewNames()
        {
            var aa = new AssocArray();
            var item = new BasicAssocItem("k", "v", name: "N1");
            aa.Items.Add(item);
            item.Name = "N1b";
            CollectionAssert.AreEqual(new[] { "N1b" }, aa.Names);
        }

        // ===== ToString =====
        [TestMethod]
        public void ToString_EmptyArray_ReturnsEmptyString()
        {
            var aa = new AssocArray();
            Assert.AreEqual(string.Empty, aa.ToString());
        }

        [TestMethod]
        public void ToString_SingleItem_ReturnsKeyValue()
        {
            var aa = new AssocArray();
            aa.Items.Add(new BasicAssocItem("k", "v"));
            var expected = "k=v" + Environment.NewLine;
            Assert.AreEqual(expected, aa.ToString());
        }

        [TestMethod]
        public void ToString_MultipleItems_ReturnsMultipleLines()
        {
            var aa = new AssocArray();
            aa.Items.Add(new BasicAssocItem("k1", "v1"));
            aa.Items.Add(new BasicAssocItem("k2", "v2"));
            var result = aa.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            CollectionAssert.AreEqual(new[] { "k1=v1", "k2=v2" }, result);
        }

        [TestMethod]
        public void ToString_FormatMatchesExpected()
        {
            var aa = new AssocArray();
            aa.Items.Add(new BasicAssocItem("key", "value"));
            var output = aa.ToString();
            StringAssert.Matches(output, new System.Text.RegularExpressions.Regex("^key=value\r?\n$"));
        }

        [TestMethod]
        public void ToString_ValueNull_ShowsKeyOnly()
        {
            var aa = new AssocArray();
            aa.Items.Add(new BasicAssocItem("k", null));
            var expected = "k=" + Environment.NewLine;
            Assert.AreEqual(expected, aa.ToString());
        }

        // ===== Resolve =====
        [TestMethod]
        public void Resolve_EmptyTarget_ReturnsEmpty()
        {
            var aa = new AssocArray();
            Assert.AreEqual(string.Empty, aa.Resolve(string.Empty));
        }

        [TestMethod]
        public void Resolve_NoPlaceholders_ReturnsSame()
        {
            var aa = new AssocArray();
            var input = "no placeholders here";
            Assert.AreEqual(input, aa.Resolve(input));
        }

        [TestMethod]
        public void Resolve_SinglePlaceholder_Replaces()
        {
            var aa = new AssocArray();
            aa.Items.Add(new BasicAssocItem("X", "Y"));
            var input = "start%X%end";
            Assert.AreEqual("startYend", aa.Resolve(input));
        }

        [TestMethod]
        public void Resolve_MultiplePlaceholders_ReplacesAll()
        {
            var aa = new AssocArray();
            aa.Items.Add(new BasicAssocItem("A", "1"));
            aa.Items.Add(new BasicAssocItem("B", "2"));
            var input = "%A%+%B%=%A%";
            Assert.AreEqual("1+2=1", aa.Resolve(input));
        }

        [TestMethod]
        public void Resolve_PlaceholderCaseInsensitive_NoReplace()
        {
            var aa = new AssocArray();
            aa.Items.Add(new BasicAssocItem("Key", "V"));
            var input = "Value of %key%?";
            var actual = aa.Resolve(input);
            string expected = "Value of V?";
            Assert.AreEqual(expected, actual);
        }
    }
}