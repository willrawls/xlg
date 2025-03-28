﻿using System;
using System.Drawing;
using MetX.Standard.Primary;
using MetX.Standard.Primary.Host;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Console.Tests.Fimm;

public class TestHost : IGenerationHost
{
    public TestHost(string testTextToProcess)
    {
        Context = new TestContext(this);
        Context.Host = this;

        GetTextForProcessing = () => testTextToProcess ?? "";
        MessageBox = new DoNothingMessageBoxHost(this);

    }

    public IMessageBox MessageBox { get; set; }
    public MessageBoxResult InputBox(string title, string description, ref string itemName)
    {
        return MessageBoxResult.No;
    }

    public Func<string> GetTextForProcessing { get; set; }
    public ContextBase Context { get; set; }
    public void WaitFor(Action action)
    {
    }

    public Rectangle Boundary => new Rectangle(0, 0, 1024, 1024);

    public static TestHost Factory(string text = "") => new(text);
}