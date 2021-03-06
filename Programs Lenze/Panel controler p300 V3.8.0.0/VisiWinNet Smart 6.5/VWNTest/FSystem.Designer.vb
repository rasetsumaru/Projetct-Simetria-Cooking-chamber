'---------------------------------------------------------
' This file was generated by VisiWinNET
' 
' Attention:
' Changes to this file may cause incorrect behavior 
' and will be lost if the file is regenerated.
'
'---------------------------------------------------------

Partial Class FSystem

    Private Sub InitializeComponent()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
        Dim loader As New VisiWinNET.Forms.Internals.SFCLoader(Me)
        loader.Load(System.IO.Path.Combine(VisiWinNET.Project.ProjectInfo.Path, "FSystem.sfc"), False)
        TouchKeyboard = loader.Find("TouchKeyboard")
        TimerLogOff = loader.Find("TimerLogOff")
        Shape6 = loader.Find("Shape6")
        Shape5 = loader.Find("Shape5")
        Shape4 = loader.Find("Shape4")
        Shape3 = loader.Find("Shape3")
        Label1 = loader.Find("Label1")
        cmdConfigurations = loader.Find("cmdConfigurations")
        cmdHistoric = loader.Find("cmdHistoric")
        cmdUsers = loader.Find("cmdUsers")
        cmdGrafics = loader.Find("cmdGrafics")
        cmdSynoptic = loader.Find("cmdSynoptic")
        cmdProcess = loader.Find("cmdProcess")
        cmdRecipes = loader.Find("cmdRecipes")
        cmdAlarms = loader.Find("cmdAlarms")
        cmdLogOff = loader.Find("cmdLogOff")
        LblFormName = loader.Find("LblFormName")
        Shape2 = loader.Find("Shape2")
        GroupBox1 = loader.Find("GroupBox1")
        VarOut1 = loader.Find("VarOut1")
        GroupBox2 = loader.Find("GroupBox2")
        PictureBox1 = loader.Find("PictureBox1")
        Label2 = loader.Find("Label2")
        VarOut2 = loader.Find("VarOut2")
        VarOut3 = loader.Find("VarOut3")
        AlarmLine = loader.Find("AlarmLine")
        GroupBox3 = loader.Find("GroupBox3")
        DateTimeOut1 = loader.Find("DateTimeOut1")
        Label4 = loader.Find("Label4")
        Shape1 = loader.Find("Shape1")
        CType(TouchKeyboard, System.ComponentModel.ISupportInitialize).EndInit()
        CType(TimerLogOff, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Label1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdConfigurations, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdHistoric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdUsers, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdGrafics, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdSynoptic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdProcess, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdRecipes, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdAlarms, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdLogOff, System.ComponentModel.ISupportInitialize).EndInit()
        CType(LblFormName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        GroupBox1.ResumeLayout(False)
        CType(VarOut1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(GroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
        GroupBox2.ResumeLayout(False)
        CType(PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Label2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(VarOut2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(VarOut3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(AlarmLine, System.ComponentModel.ISupportInitialize).EndInit()
        CType(GroupBox3, System.ComponentModel.ISupportInitialize).EndInit()
        GroupBox3.ResumeLayout(False)
        CType(DateTimeOut1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Label4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape1, System.ComponentModel.ISupportInitialize).EndInit()

        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
    End Sub
    
    Friend WithEvents TouchKeyboard As VisiWinNET.Forms.TouchKeyboard
    Friend WithEvents TimerLogOff As VisiWinNET.Forms.Timer
    Friend WithEvents Shape6 As VisiWinNET.Forms.Shape
    Friend WithEvents Shape5 As VisiWinNET.Forms.Shape
    Friend WithEvents Shape4 As VisiWinNET.Forms.Shape
    Friend WithEvents Shape3 As VisiWinNET.Forms.Shape
    Friend WithEvents Label1 As VisiWinNET.Forms.Label
    Friend WithEvents cmdConfigurations As VisiWinNET.Forms.CommandButton
    Friend WithEvents cmdHistoric As VisiWinNET.Forms.CommandButton
    Friend WithEvents cmdUsers As VisiWinNET.Forms.CommandButton
    Friend WithEvents cmdGrafics As VisiWinNET.Forms.CommandButton
    Friend WithEvents cmdSynoptic As VisiWinNET.Forms.CommandButton
    Friend WithEvents cmdProcess As VisiWinNET.Forms.CommandButton
    Friend WithEvents cmdRecipes As VisiWinNET.Forms.CommandButton
    Friend WithEvents cmdAlarms As VisiWinNET.Forms.CommandButton
    Friend WithEvents cmdLogOff As VisiWinNET.Forms.CommandButton
    Friend WithEvents LblFormName As VisiWinNET.Forms.Label
    Friend WithEvents Shape2 As VisiWinNET.Forms.Shape
    Friend WithEvents GroupBox1 As VisiWinNET.Forms.GroupBox
    Friend WithEvents VarOut1 As VisiWinNET.Forms.VarOut
    Friend WithEvents GroupBox2 As VisiWinNET.Forms.GroupBox
    Friend WithEvents PictureBox1 As VisiWinNET.Forms.PictureBox
    Friend WithEvents Label2 As VisiWinNET.Forms.Label
    Friend WithEvents VarOut2 As VisiWinNET.Forms.VarOut
    Friend WithEvents VarOut3 As VisiWinNET.Forms.VarOut
    Friend WithEvents AlarmLine As VisiWinNET.Forms.Alarm.AlarmLine
    Friend WithEvents GroupBox3 As VisiWinNET.Forms.GroupBox
    Friend WithEvents DateTimeOut1 As VisiWinNET.Forms.DateTimeOut
    Friend WithEvents Label4 As VisiWinNET.Forms.Label
    Friend WithEvents Shape1 As VisiWinNET.Forms.Shape

End Class