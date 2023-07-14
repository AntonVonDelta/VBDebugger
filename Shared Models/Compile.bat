@echo off

flatc --cpp --scoped-enums --gen-object-api -o Cpluplus DebuggerInfo.fbs
flatc --cpp --scoped-enums --gen-object-api -o Cpluplus DebuggerAttached.fbs
flatc --cpp --scoped-enums --gen-object-api -o Cpluplus DebugCommand.fbs
flatc --cpp --scoped-enums --gen-object-api -o Cpluplus StackDump.fbs


flatc --csharp --gen-object-api -o Csharp DebuggerInfo.fbs
flatc --csharp --gen-object-api -o Csharp DebuggerAttached.fbs
flatc --csharp --gen-object-api -o Csharp DebugCommand.fbs
flatc --csharp --gen-object-api -o Csharp StackDump.fbs