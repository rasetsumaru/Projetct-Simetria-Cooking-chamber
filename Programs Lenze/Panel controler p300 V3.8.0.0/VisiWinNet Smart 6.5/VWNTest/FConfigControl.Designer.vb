'---------------------------------------------------------
' This file was generated by VisiWinNET
' 
' Attention:
' Changes to this file may cause incorrect behavior 
' and will be lost if the file is regenerated.
'
'---------------------------------------------------------

Partial Class FConfigControl

    Private Sub InitializeComponent()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
        Dim loader As New VisiWinNET.Forms.Internals.SFCLoader(Me)
        loader.Load(System.IO.Path.Combine(VisiWinNET.Project.ProjectInfo.Path, "FConfigControl.sfc"), False)
        GroupBox = loader.Find("GroupBox")
        Shape1 = loader.Find("Shape1")
        Shape2 = loader.Find("Shape2")
        Shape3 = loader.Find("Shape3")
        cmdReturn = loader.Find("cmdReturn")
        Shape4 = loader.Find("Shape4")
        VarIn1 = loader.Find("VarIn1")
        VarIn2 = loader.Find("VarIn2")
        VarIn3 = loader.Find("VarIn3")
        VarIn4 = loader.Find("VarIn4")
        VarIn5 = loader.Find("VarIn5")
        CType(GroupBox, System.ComponentModel.ISupportInitialize).EndInit()
        GroupBox.ResumeLayout(False)
        CType(Shape1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdReturn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(VarIn1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(VarIn2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(VarIn3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(VarIn4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(VarIn5, System.ComponentModel.ISupportInitialize).EndInit()

        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
    End Sub
    
    Friend WithEvents GroupBox As VisiWinNET.Forms.GroupBox
    Friend WithEvents Shape1 As VisiWinNET.Forms.Shape
    Friend WithEvents Shape2 As VisiWinNET.Forms.Shape
    Friend WithEvents Shape3 As VisiWinNET.Forms.Shape
    Friend WithEvents cmdReturn As VisiWinNET.Forms.CommandButton
    Friend WithEvents Shape4 As VisiWinNET.Forms.Shape
    Friend WithEvents VarIn1 As VisiWinNET.Forms.VarIn
    Friend WithEvents VarIn2 As VisiWinNET.Forms.VarIn
    Friend WithEvents VarIn3 As VisiWinNET.Forms.VarIn
    Friend WithEvents VarIn4 As VisiWinNET.Forms.VarIn
    Friend WithEvents VarIn5 As VisiWinNET.Forms.VarIn

End Class