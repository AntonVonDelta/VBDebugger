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
    DebugInit
End Sub


Private Sub Command1_Click()
On Error GoTo debug_handler
    Sleep 2000
    DebugEnterProcedure "Form1.frm", "Command1_Click", 10


    DebugLog "Form1.frm", "Command1_Click", 13
    Test1 "test"
    
    DebugLeaveProcedure "Form1.frm", "Command1_Click", 16
    Exit Sub
debug_handler:
    DebugLeaveProcedure "Form1.frm", "Command1_Click", 19
    'Err.Raise Err.Number, Err.Source, Err.Description
End Sub


Sub Test1(ParamArray locals() As Variant)
On Error GoTo debug_handler
    DebugEnterProcedure "Form1.frm", "Test1", 26

    Dim a(0) As Integer
    Dim str As String
    
    str = "test 1454"
    
    DebugLog "Form1.frm", "Test1", 33, "str", str
    Debug.Print LBound(locals)
    str = "test2"
    
    DebugLog "Form1.frm", "Test1", 37, "str", str
    Debug.Print UBound(locals)

    DebugLog "Form1.frm", "Test1", 40
    Debug.Print LBound(a)
    
    DebugLog "Form1.frm", "Test1", 43
    Debug.Print UBound(a)

    DebugLog "Form1.frm", "Test1", 46
    Test2

    DebugLeaveProcedure "Form1.frm", "Test1", 49
    Exit Sub
debug_handler:
    DebugLeaveProcedure "Form1.frm", "Test1", 52
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub



Sub Test2()
On Error GoTo debug_handler
    DebugEnterProcedure "Form1.frm", "Test2", 60

    Dim a As Integer
    
    DebugLog "Form1.frm", "Test2", 64
    a = a / 0
    
    DebugLeaveProcedure "Form1.frm", "Test2", 67
    Exit Sub
debug_handler:
    DebugLeaveProcedure "Form1.frm", "Test2", 70
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub



Sub Test3()

End Sub



Sub Test4()
    
    MsgBox "test"

End Sub

Sub Test5()

On Error Resume Next

Dim a As Integer

a = 2 / 0

Test6
a = 1

Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Sub Test6()
On Error Resume Next
    Debug.Print Err.Number
    Debug.Print Err.Description
    
    Dim b As Integer
    
    b = 4 / 0
    
End Sub

