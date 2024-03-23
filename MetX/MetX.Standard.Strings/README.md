XLG - MetX.Standard.Strings
===
Library of string classes implementing various forms of associative arrays plus their generics in multiple dimentions (1d-4d included).
```c#
	var aa = new AssociativeArray();
	aa["key"] = "value";
	var key2 = aa["key2"] = "value2";
	key2.Value = "any value you want";
	aa["key3"] = new BasicAssocItem("Yellow", "#FFFF00", Guid.NewGuid(), "Color");
	var xml = aa.ToXml(true, true);
	// And so much more including inheritable BasicAssocItem with extra types
```

Additionally a token parsing extension library that allows for simple, fast parsing of strings into tokens.
For instance:

```c#
	string s = "This is a test string";
	sting firstToken = s.FirstToken(" "); // returns "This"
	firstToken = s.FirstToken("a"); // returns "This is "

	string tokenBetween = s.TokenBetween("is", "string"); // returns " a test "
	string tokens = s.Tokens(); // returns "This", "is", "a", "test" with default space separator	
```
And many more. See Tokenizer.cs and StringExtensions.cs for more details.


