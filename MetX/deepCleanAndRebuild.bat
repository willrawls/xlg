@ECHO OFF
ECHO This will blow away all the child /bin/ and /obj/folders and their contents.
ECHO Hit space to continue or Ctrl-C to exit
Pause

CLS
@ECHO ON

rmdir /s /q MetX\bin
rmdir /s /q MetX.Aspects\bin
rmdir /s /q MetX.Controls\bin
rmdir /s /q MetX.Generators\bin
rmdir /s /q MetX.Generators\Generated
rmdir /s /q MetX.Generators.Samples.Client\bin
rmdir /s /q MetX.Glove.Console\bin
rmdir /s /q MetX.QuickScripts\bin
rmdir /s /q MetX.Standard\bin
rmdir /s /q MetX.Techniques.Tests\bin
rmdir /s /q MetX.Tests\bin
rmdir /s /q MetX.Windows\bin

rmdir /s /q MetX\obj
rmdir /s /q MetX.Aspects\obj
rmdir /s /q MetX.Controls\obj
rmdir /s /q MetX.Generators\obj
rmdir /s /q MetX.Generators\Generated
rmdir /s /q MetX.Generators.Samples.Client\obj
rmdir /s /q MetX.Generators.Samples.Client\Generated
rmdir /s /q MetX.Glove.Console\obj
rmdir /s /q MetX.QuickScripts\obj
rmdir /s /q MetX.Standard\obj
rmdir /s /q MetX.Techniques.Tests\obj
rmdir /s /q MetX.Tests\obj
rmdir /s /q MetX.Windows\obj

devenv /clean

devenv /build
