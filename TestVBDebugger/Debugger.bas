Attribute VB_Name = "Module2"
Option Explicit

Private Declare Sub Init Lib "C:\Users\VM\Documents\Projects\VBDebugger\Debug\VBRuntime.dll" ()

Private Declare Sub EnterProcedure Lib "C:\Users\VM\Documents\Projects\VBDebugger\Debug\VBRuntime.dll" (ByVal filename As String, ByVal scopeName As String, ByVal lineNumber As Long, ByVal arguments As String)
Private Declare Sub Log Lib "C:\Users\VM\Documents\Projects\VBDebugger\Debug\VBRuntime.dll" (ByVal filename As String, ByVal scopeName As String, ByVal lineNumber As Long, ByVal arguments As String)
Private Declare Sub LeaveProcedure Lib "C:\Users\VM\Documents\Projects\VBDebugger\Debug\VBRuntime.dll" (ByVal filename As String, ByVal scopeName As String, ByVal lineNumber As Long, ByVal arguments As String)


'' Each local is composed of two pairs: first string representing the name of the local, a string which stores the value

Public Sub DebugInit()
    Init
End Sub

Public Sub DebugEnterProcedure(filename As String, scopeName As String, lineNumber As Long, ParamArray locals() As Variant)
    EnterProcedure filename, scopeName, lineNumber, Serialize(locals)
End Sub

Public Sub DebugLog(filename As String, scopeName As String, lineNumber As Long, ParamArray locals() As Variant)
   Log filename, scopeName, lineNumber, AddSerializedErr(Serialize(locals))
End Sub

Public Sub DebugLeaveProcedure(filename As String, scopeName As String, lineNumber As Long)
    LeaveProcedure filename, scopeName, lineNumber, AddSerializedErr("")
End Sub



Private Function AddSerializedErr(data As String) As String
    '' We must always include the exception information even when its empty
    '' because the debugging runtime adds all the locals in the scope and keeps them
    '' during the entire scope
    '' Setting once the Err local will keep it set if we do not resync it every time
    
    AddSerializedErr = "ErrNumber," & Err.Number & ","
    AddSerializedErr = AddSerializedErr & "ErrSource," & EscapeContent(Err.Source) & ","
    AddSerializedErr = AddSerializedErr & "ErrDescription," & EscapeContent(Err.Description)
    
    If Len(data) <> 0 Then AddSerializedErr = AddSerializedErr & "," & data
End Function

Private Function Serialize(ParamArray paramContainingParam() As Variant) As String
    Dim result As String
    Dim i As Integer
    Dim countItems As Integer
    Dim locals() As Variant
    
    locals = paramContainingParam(0)
    Serialize = ""
    
    countItems = UBound(locals) + 1
    
    If countItems Mod 2 <> 0 Then
        Exit Function
    End If
    
    If countItems = 0 Then
        Exit Function
    End If
    
    For i = 0 To countItems - 2 Step 2
        Dim localName As String
        Dim localValue As String
        
        If i > 0 Then
            Serialize = Serialize & ","
        End If
        
        localName = EscapeContent(locals(i))
        
        localValue = EscapeContent(locals(i + 1))
        Serialize = Serialize & localName & "," & localValue
    Next
    
End Function


Private Function EscapeContent(value As Variant) As String
    Dim result As String
    
    result = Replace(value, "\", "\\")
    result = Replace(value, ",", "\,")
    
    EscapeContent = result
End Function



