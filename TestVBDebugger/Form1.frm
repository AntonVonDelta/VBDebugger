VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   6600
   ClientLeft      =   120
   ClientTop       =   465
   ClientWidth     =   11685
   LinkTopic       =   "Form1"
   ScaleHeight     =   6600
   ScaleWidth      =   11685
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton Command1 
      Caption         =   "Command1"
      Height          =   615
      Left            =   480
      TabIndex        =   0
      Top             =   600
      Width           =   3255
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Private Declare Sub Sleep Lib "kernel32.dll" (ByVal dwMilliseconds As Long)

Private Sub Form_Load()
On Error GoTo debug_handler
    DebugEnterProcedure "Form1.frm", "Form_Load", 6

    DebugLog "Form1.frm", "Form_Load", 8
    Test8 "const", Test9
    DebugInit

    DebugLeaveProcedure "Form1.frm", "Form_Load", 12
    Exit Sub
debug_handler:
    DebugLeaveProcedure "Form1.frm", "Form_Load", 15
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub


Sub Test8(a As String, b As String)
On Error GoTo debug_handler
    DebugEnterProcedure "Form1.frm", "Test8", 22, "a", a, "b", b



    DebugLeaveProcedure "Form1.frm", "Test8", 26
    Exit Sub
debug_handler:
    DebugLeaveProcedure "Form1.frm", "Test8", 29
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Function Test9() As String
    Test9 = "test9"
End Function

Private Sub Command1_Click()
On Error GoTo debug_handler
    DebugEnterProcedure "Form1.frm", "Command1_Click", 39

    DebugLog "Form1.frm", "Command1_Click", 41
    Test1 "test"

    DebugLeaveProcedure "Form1.frm", "Command1_Click", 44
    Exit Sub
debug_handler:
    DebugLeaveProcedure "Form1.frm", "Command1_Click", 47
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub


Sub Test1(var As String, ParamArray locals() As Variant)
On Error GoTo debug_handler
    DebugEnterProcedure "Form1.frm", "Test1", 54, "var", var

    Dim a(0) As Integer
    Dim str As String
    
    str = "test 1454"
    
    Debug.Print LBound(locals)
    str = "test2"
    
    Debug.Print UBound(locals)

    Debug.Print LBound(a)
    
    Debug.Print UBound(a)

    Test2

    DebugLeaveProcedure "Form1.frm", "Test1", 72
    Exit Sub
debug_handler:
    DebugLeaveProcedure "Form1.frm", "Test1", 75
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub



Sub Test2()
On Error GoTo debug_handler
    DebugEnterProcedure "Form1.frm", "Test2", 83

    Dim a As Integer
    
    ' a = a / 0

    DebugLeaveProcedure "Form1.frm", "Test2", 89
    Exit Sub
debug_handler:
    DebugLeaveProcedure "Form1.frm", "Test2", 92
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub



Sub Test3()
On Error GoTo debug_handler
    DebugEnterProcedure "Form1.frm", "Test3", 100



    DebugLeaveProcedure "Form1.frm", "Test3", 104
    Exit Sub
debug_handler:
    DebugLeaveProcedure "Form1.frm", "Test3", 107
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub



Sub Test4()
On Error GoTo debug_handler
    DebugEnterProcedure "Form1.frm", "Test4", 115

    
    DebugLog "Form1.frm", "Test4", 118
    MsgBox "test"


    DebugLeaveProcedure "Form1.frm", "Test4", 122
    Exit Sub
debug_handler:
    DebugLeaveProcedure "Form1.frm", "Test4", 125
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Sub Test5()
On Error GoTo debug_handler
    DebugEnterProcedure "Form1.frm", "Test5", 131


On Error Resume Next

Dim a As Integer

a = 2 / 0

Test6
a = 1

Err.Raise Err.Number, Err.Source, Err.Description


    DebugLeaveProcedure "Form1.frm", "Test5", 146
    Exit Sub
debug_handler:
    DebugLeaveProcedure "Form1.frm", "Test5", 149
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Sub Test6()
On Error GoTo debug_handler
    DebugEnterProcedure "Form1.frm", "Test6", 155

On Error Resume Next
    Debug.Print Err.Number
    Debug.Print Err.Description
    
    Dim b As Integer
    
    b = 4 / 0
    

    DebugLeaveProcedure "Form1.frm", "Test6", 166
    Exit Sub
debug_handler:
    DebugLeaveProcedure "Form1.frm", "Test6", 169
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub


