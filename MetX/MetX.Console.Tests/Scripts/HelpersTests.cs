﻿using System;
using MetX.Standard.Primary.Scripts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Scripts;

[TestClass()]
public class HelpersTests
{
    [TestMethod()]
    public void ExpandScriptLineVariablesTest()
    {
        var data = "a%temp%b%%c";
        var actual = data.ExpandScriptLineVariables();
        var temp = Environment.GetEnvironmentVariable("TEMP");
        var expected = $"a{temp}bc";
        Assert.AreEqual(expected, actual);
    }
}