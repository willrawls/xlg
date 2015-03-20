xlg
===

XML Library Generator - A XML / XSLT based code generator

This project is going to be retooled over time.

--------------------------
XLG QuickScripts: The basics
--------------------------
This is an entirely new line of thought for code generation. 
The idea is to quickly and easily generate entire line processing programs that run from the QuickScript GUI or that generate into a fully independent command line exe. 
Inputs supported include clipboard and file (excel or any plain text). 
Outputs include clipboard, text box (with file watch and auto update), and (text) file.
In its simplest use, a QuickScript is called for every line in the input (linux or windows). 
You have two inpu variables, line and number. line is the string content of the line, and number is the line number.
You have one output. A StringBuilder called Output.
You write just the C# lines that would appear inside such a function.

So to write a line processor that takes something like this:
    public string Fred {get; set;}
    public int George = 0;
    
And turns it into something like:
    Fred = "SomeValue",
    George = "SomeValue",

We would write a QuickScript that might look something like this:

if(line.Contains("public") || line.Contains("private") || line.Contains("protected") || line.Contains("internal"))
{
  string[] word = line.Trim().Split(' ');
  if(word.Length > 1)
  {
    word[2] = word[2].Trim();
    Output.WriteLine(word[2] + " = \"SomeValue\"";
  }
}

XLG QuickScripts: The shorthand command
--------------------------
That's in its simplest form. There's actually a number of other features that allow you 
to get more control of the processing and to simplify writing complex Output.WriteLine() calls.

In QuickScript, the shorthand for an "inverted" Output.WriteLine() is ~~: (two tildes followed by a colon).
In fact all special QUickScript commands begin with ~~ and end with :

So, any line starting with ~~: goes through a special decoding process that lets you simply use a " (double quote)
where you would normally have to use the string "\"". It also transforms a variable name surrounded by % (percent)
into an actual variable reference outside the generated strings. 

For example, in our previous QuickScript we could have written:

    Output.WriteLine("\t\t" + word[2] + " = \"SomeValue\"";

With the shorthand and gotten:

    ~~:\t\t%word[2]% = "SomeValue",

For something this simple, you'd probably just go with the normal code, but for very... intense writes 
you will likely find the shorthand helpful. For instance, the following two lines are equivalent:

  Output.AppendLine("\"Example\":\t" + number + " (" + word[0] + "): \"" + line + "\"");
  
  ~~:"Example":\t%number% (%word[0]%): "%line%"
  
It's up to you which you prefer. Note that if you want to acutally output a % or if you want a complex reference
inside the %variable%, you may find you can't use the ~~: shorthand.



XLG Pipeliner:
Generating the metadata XML from a neutral data source
Generating from XLST + xlg:Urn
Regenerate
Auto-regenerate

SQL Server support
File system support
xlg:Urn (The new xsl namespace with added xls funtionality)

--------------------------
What works
--------------------------
Plug in support. Although this could be redone with modern IoC and DI. I may refactor the discoverable addins by naming convention as discoverable addins with NinjectModules or some such.

--------------------------
What may work
--------------------------
MySQL support
SyBase support
SQL to XML support

--------------------------
What doesn't work
--------------------------
There are some annoying and some serious GUI flaws that need to be addressed.
GUI could probably use a serious overhall... maybe reduce it to a multi pathed wizard or some such. My last attempt to do that did not end well but hopefully I can learn from those mistakes.
Documentation is sorely lacking
xlg:Urn functions need to be renamed for consistancy and usability
xlg:Urn functions need to be tested to see if any can be replaced with pure XSLT

