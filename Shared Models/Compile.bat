@echo off

flatc --cpp --scoped-enums --gen-object-api -o Cpluplus DebugEvent.fbs
flatc --cpp --scoped-enums --gen-object-api -o Cpluplus DebuggerInfo.fbs

flatc --csharp --gen-object-api -o Csharp DebugEvent.fbs
flatc --csharp --gen-object-api -o Csharp DebuggerInfo.fbs