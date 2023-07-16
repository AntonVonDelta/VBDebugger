Attribute VB_Name = "Module2"
Option Explicit

Private Declare Sub Init Lib "C:\Users\VM\Documents\Projects\VBDebugger\Debug\VBRuntime.dll" ()

Private Declare Sub EnterProcedure Lib "C:\Users\VM\Documents\Projects\VBDebugger\Debug\VBRuntime.dll" (ByVal filename As String, ByVal scopeName As String, ByVal lineNumber As Long, ByVal arguments As String)
Private Declare Sub Log Lib "C:\Users\VM\Documents\Projects\VBDebugger\Debug\VBRuntime.dll" (ByVal filename As String, ByVal scopeName As String, ByVal lineNumber As Long, ByVal arguments As String)
Private Declare Sub LeaveProcedure Lib "C:\Users\VM\Documents\Projects\VBDebugger\Debug\VBRuntime.dll" (ByVal filename As String, ByVal scopeName As String, ByVal lineNumber As Long)


'' Each local is composed of two pairs: first string representing the name of the local, a string which stores the value

Public Sub DebugInit()
    Init
End Sub

Public Sub DebugEnterProcedure(filename As String, scopeName As String, lineNumber As Long, ParamArray locals() As Variant)
    EnterProcedure filename, scopeName, lineNumber, Serialize(locals)
End Sub

Public Sub DebugLog(filename As String, scopeName As String, lineNumber As Long, ParamArray locals() As Variant)
   Log filename, scopeName, lineNumber, Serialize(locals)
End Sub

Public Sub DebugLeaveProcedure(filename As String, scopeName As String, lineNumber As Long)
    LeaveProcedure filename, scopeName, lineNumber
End Sub



Private Function Serialize(ParamArray locals() As Variant) As String
    Dim result As String
    Dim i As Integer
    Dim countItems As Integer
    
    Serialize = ""
    
    countItems = UBound(locals(0)) + 1
    
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
        
        localName = SerializeContent(locals(0)(i))
        localValue = SerializeContent(locals(0)(i + 1))
        Serialize = Serialize & localName & "," & localValue
    Next
    
End Function


Private Function SerializeContent(value As Variant) As String
    Dim result As String
    
    result = Replace(value, "\", "\\")
    result = Replace(value, ",", "\,")
    
    SerializeContent = result
End Function


