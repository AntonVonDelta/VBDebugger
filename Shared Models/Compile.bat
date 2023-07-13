@echo off

flatc --cpp --scoped-enums --gen-object-api -o Cpluplus DebugEvent.fbs
flatc --cpp --scoped-enums --gen-object-api -o Cpluplus DebuggerInfo.fbs
flatc --cpp --scoped-enums --gen-object-api -o Cpluplus DebuggerAttached.fbs

flatc --csharp --gen-object-api -o Csharp DebugEvent.fbs
flatc --csharp --gen-object-api -o Csharp DebuggerInfo.fbs
flatc --csharp --gen-object-api -o Csharp DebuggerAttached.fbs