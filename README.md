xlg
===

XML Library Generator - A XML / XSLT based code generator

This project is going to be retooled over time.

--------------------------
What works well
--------------------------
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

