MetX.Standard.Strings 
===
* A library of turning strings into tokens at break neck speeds
Example: 
```
	var fred = "I (am a) string of text";
	var george = fred.TokenBetween("(", ")"); // Equals "am a"
	george = fred.FirstToken(); // Equals "I"
	george = fred.LastToken(); // Equals "text"
	george = fred.TokenAt(5) // Equals "of"
```
Many other string and tokenizing functions available.

* A set of associative array classes that automatically add to themselves and support multiple dimentions 
Example:
```
	var fred = new AssocArray();
	fred["george"].Value = "mary";
	fred["george"].Number = 12;
	var serializedXml = fred.ToXml();	
```
And much much more. More documentation needed.

--------------------------
MetX.Standard.Library
===
* A library of general extension methods for enumerations, double, int, list, long, tasks, encryption, randomness, automated serialization of classes/lists to/from xml, html, xml, xsl, a super simple in memory cache, a stream builder, and more.
* Please note that this is an aging library with functionality covered by later frameworks. Still a few gems in here though like:
* 
Example:
```
public class fred : SerializesToXml<fred>
{
	public string george;
}

var mary = new fred();
mary.george = "jill";
var xml = mary.ToXml();
var clone = fred.FromXml(xml);

fred.SaveXmlToFile("somefile.xml");
var henry = fred.LoadXmlFromFile("somefile.xml");

var bytes = clone.ToBytes();
var clone2 = bytes.FromBytes();
```

* Also ListSerializedToXml<Parent,Child> which works on List like objects (parent with child objects)
Example:
```
public class Mary : ListSerializesToXml<Mary, MaryItem>
{
    public Guid TestGuid { get; set; } = Guid.NewGuid();
}

public class MaryItem
{
    public string MaryItemName {get; set; }
    public Guid MaryItemTestGuid { get; set; } = Guid.NewGuid();
}
```
Which works pretty much the same as SerializesToXml<T>
--------------------------
XLG - The Xml Library Generator
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
```
    public string Fred {get; set;}
    public int George = 0;
```

And turns it into something like:
```
    Fred = "SomeValue",
    George = "SomeValue",
```

We would write a QuickScript that might look something like this:
```
if(line.Contains("public") || line.Contains("private") || line.Contains("protected") || line.Contains("internal"))
{
  string[] word = line.Trim().Split(' ');
  if(word.Length > 1)
  {
    word[2] = word[2].Trim();
    Output.WriteLine(word[2] + " = \"SomeValue\"";
  }
}
```

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
```
    Output.WriteLine("\t\t" + word[2] + " = \"SomeValue\"";
```

With the shorthand and gotten:
```
    ~~:\t\t%word[2]% = "SomeValue",
```
For something this simple, you'd probably just go with the normal code, but for very... intense writes 
you will likely find the shorthand helpful. For instance, the following two lines are equivalent:

```
  Output.AppendLine("\"Example\":\t" + number + " (" + word[0] + "): \"" + line + "\"");
 
  ~~:"Example":\t%number% (%word[0]%): "%line%"
```

It's up to you which you prefer. Note that if you want to acutally output a % or if you want a complex reference
inside the %variable%, you may find you can't use the ~~: shorthand on that line.

XLG QuickScripts: Code areas
===
	
Ther's a number of other QuickScript commands available which include the ability to break out of the single
line processing method and run code before the first line is processed and after the last line is processed.

You deliniate the areas of code by adding commands.
```
~~Start:
   Any C# lines you put here run befor the first line
~~Line:
   Runs for each line in the input
~~Finish:
   Runs after the last line
```

These areas actually expand to functions that return bool. If you return false; at any point, it will cancel the
quick script execution.

There's actually one more code area which let's you define class variables (Members) as follows:
```
~~Members:
  Dictionary<string, string> KeyValuePairs = new Dictionary<string, string>();
```

The KeyValuePairs variable can then be used throughout the script.

Here's an example that brings it all together. This script asks the user for an 
```
~~Members:
	string arrayName;

~~Start:	
	arrayName = Ask("What is the array name?", "myArray");
	if(string.IsNullOrEmpty(arrayName)) return false;
	Output.Append("string[] " + arrayName + " = {" + Environment.NewLine + "\t");
	
~~Line:
	if((number+1) % 50 == 0)
	{
		Output.AppendLine();
		Output.Append("\t");
	}
	Output.Append("\"" + line.Trim() + "\", ");
	
~~Finish:
	Output.AppendLine(Environment.NewLine + "\t};");
	Output.AppendLine(" // " + arrayName);

```

XLG QuickScripts: Ask()
===
	
You'll notice a call to an Ask() function. This is automatically included in your script. It's defined as:
````
  string Ask(string title, string promptText, string defaultValue)
or
  string Ask(string promptText, string defaultValue = "")
```

I decided not to make this a command like ~~Ask: because the syntax just wasn't better but asking you for
a piece of information is fairly common. 

XLG QuickScripts: Namespaces
===
	
Scripts have access to the following namespaces:
```
  System;
  System.Collections.Generic;
  System.Drawing;
  System.IO;
  System.Text;
  System.Windows.Forms;
```

When the script is run using the "Run" button it also has access to:
```
  MetX;
  MetX.Library;
```

XLG QuickScripts: GUI Buttons
===
	
This brings up the point of the buttons that execute the script. "Run" and "Gen Exe". 
```
"Run" compiles and executes the script in side the QuickScript GUI. If there are any errors in the code, you'll 
be given a list of the errors with line numbers and then notepad will open with the complete code. 

"Gen" will generate the same c# code that "Run" would but it only displays the code in notepad.

"Gen Exe" generates and compiles a .cs file into an .exe and then gives you the option of running. You can then 
use the command line tool to later take file input and output. The exe takes two optional parameters, an input file or
an input file and an output file.
```

I recommend using notepad2 or notepad++. I use notepad2 because it actually replaces the notepad.exe file which I find extremely useful.

```
  The "New" button creates a new script with the option to clone the current script.
  "Save" saves all scripts. Closing the window will also save the script file. 
  "Delete" deletes the current script with confirmation.
```

XLG QuickScripts: The XLGQ file format
===
	
NOTE: I'm aware my file format is... unusual. Why didn't I just put it into an XML file like I do with everything
else... I probably will. I went back and forth and for now the half of my brain that's saying it's a format that 
is more easily human readable is winning. I will likely support an xml format as well as I love XML (obviously).

UPDATE: No on the XML file format but I have another format to implement which is much easier to read and understand. There will also soon be a new file type: x.fimm . Fimm script files have only a single script in them. Fimm.exe can run scripts from the command line generating entire programs with a simple command.
	
With that said, the current file format is itself a QuickScript command marked up list of quick scripts. 

You can associate xlgq with MetX.QuickScripts.exe and any xlgq file you double click on will be opened by the editor.

Support exists for saving/loading  xlgq files whereever you like.  By default everything will be under "C:\Users\Username\Documents\XLG\". This folder is evolving and now contains many things which I will document at a later time including the csproj and cs templates that get combined with a script to make a fully compilable c# program. More template sets are going to follow with a template manager.

XLG QuickScripts: Script fields
===
	
```
  "Script:"   - The name of the script you're editing. There's way to directly rename (yet).
  "Input:"    - You can set this to Clipboard (the default) or file. If file, you must set "Input File Path"
  "Output:"   - You can set this to Clipboard, Text box, Notepad or file. If file, you must set "Output File Path"
  "Slice at:" - Future use. The line processor will soon support different end of line delimiters
  "Input File Path:" - When "Input" is "File", this is the file where lines are loaded from.
  "Output File Path:" - When "Output" is "File, this is the file that all output will be written to or overwritten.
  
  Next to each file path can click "Edit" to open the file in notpad, or the "..." button to browse for a file.
```

XLG Pipeliner:
===
This guy is going to get overhauled. It's a really amazing tool, but the interface is just too clunkly
and there's some edges that need to be smoothed out. This guy is my pride and joy in many ways as it allows you
to take a meta data source (such as the structure of a database or the structure of a folder) and turn that into
a standardized XML file. Then it takes the metadata XML and performs one or more XSLT transformations on that
metadata. The XSLT engine has been hotwired with the excellent Microsoft EXSLT implementation and further
enhanced with a URN tied to a custom C# object, MetX.Library.xlgUrn. MetX is my name for the mini framework that
the XLG tools draw from. More on the library later

One day soon I will be writing documentation and examples for exactly how to use this guy... It makes for some 
serious time savings when there's several sets of code to be written. 

Some features working well.
```
Generating the metadata XML from a neutral data source.
Generating from XLST + xlg:Urn.
Regenerate.
Auto-regenerate.

SQL Server support
File system support
xlg:Urn (The new xsl namespace with added xls funtionality)
```

--------------------------
What works
--------------------------
Plug in support. This could be redone with modern IoC and DI. I may refactor the discoverable 
addins DLLs by naming convention as discoverable addins with NinjectModules or some such. 
Or I might eliminate that part all together and bring everything under one roof... Probably not though.

What probably doesn't work
--------------------------
```
MySQL support
SyBase support
SQL to XML support
```
These are guys I had working well but I just don't have a need for. I want to support MySql and Oracle again
and the SQL to XML support is something I'd like to put some time into as well as it's got some exciting 
potential.

What needs to be reworked (A growing list feel free to send me your thoughts)
--------------------------
There are some annoying and some serious GUI flaws that need to be addressed.
GUI could probably use a serious overhall... maybe reduce it to a multi pathed wizard or some such. My last attempt to do that did not end well but hopefully I can learn from those mistakes.
Documentation is sorely lacking
xlg:Urn functions need to be renamed for consistancy and usability
xlg:Urn functions need to be tested to see if any can be replaced with pure XSLT

