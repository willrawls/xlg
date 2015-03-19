xlg
===

XML Library Generator - A XML / XSLT based code generator

This project is going to be retooled over time.

--------------------------
What works well
--------------------------
XLG QuickScripts:
This is an entirely new line of thought for code generation. 
The idea is to quickly and easily generate entire line processing programs that run from the QuickScript GUI or that generate into a fully independent command line exe. 
Inputs supported include clipboard and file (excel or any plain text). 
Outputs include clipboard, text box (with file watch and auto update), and (text) file.
In its simplest use, a QuickScript is called for every line in the input (linux or windows). 
You have two inpu variables, line and number. line is the string content of the line, and number is the line number.
You have one output. A StringBuilder called Output.
You write just the C# lines that would appear inside such a function.

That's in its simplest form. There's actually a number of other features that allow you to get more control of the prcessing and to simplify writing Output.WriteLine() calls.

For instance, a shorthand for Output.WriteLine() is ~~:
Any line starting with ~~:

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

