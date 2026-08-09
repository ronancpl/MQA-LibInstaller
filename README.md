# MQA-LibInstaller
A tool with emphasis on installing quickly the mandatory data for MapleQuestAdvisor.

## Head developer: Ronan C. P. Lana

### About
---
* This tool stands as a standalone from MapleQuestAdvisor, for MapleQuestAdvisor needs to be properly rested on the file system.
* It runs with a framework having CSharp as back-end, as it interacts with MapleLib to fetch information from the WZ files.
* The front-end is Lua, with C as the native code adapter.

Therefore, it is mandatory to install:
* A .NET framework;
* A C compiler;
* A Lua interpreter.

---
### Compilation process

* Firstly, __compile__ the C# class-file into a DLL by using the following command:
`dotnet publish -r win-x86 -c Release`

* Secondly, __copy/move the built DLL__ into the folder where MQA-LibInstaller is placed, now on referred as <MQA-LibInstaller-PATH>

* It's needed to have a C-produced DLL for the adapter, by inserting some commands such as:
`g++.exe -m32 -o2 -o ./WzBmp.o -c ./WzBmp.c -I ./lua/include`
`g++.exe -m32 -shared -o ./WzBmp.dll ./WzBmp.o -L. -l"lua5.1" -lMQA-PngInstaller`

* Now, define into `LUA_CPATH` environment variable a new value for the path to this created DLL:
`<MQA-LibInstaller-PATH>/WzBmp.dll`

---
### Using the program

Being a Lua script, execute the application by referring it into a Lua interpreter.
`lua5.1 main.lua`

The program reports the files being installed into the MapleQuestAdvisor folder. This spares the effort of handling with the PNG/XML files.