'---------------------------------------------------------
' This file was generated by VisiWinNET
' 
' Attention:
' Changes to this file may cause incorrect behavior 
' and will be lost if the file is regenerated.
'
'---------------------------------------------------------

Partial Class ChildForm

    Private Sub InitializeComponent()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
        Dim loader As New VisiWinNET.Forms.Internals.SFCLoader(Me)
        loader.Load(System.IO.Path.Combine(VisiWinNET.Project.ProjectInfo.Path, "ChildForm.sfc"), False)
        GroupBox1 = loader.Find("GroupBox1")
        Label1 = loader.Find("Label1")
        Label2 = loader.Find("Label2")
        Label3 = loader.Find("Label3")
        CType(GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		GroupBox1.ResumeLayout(False)
        CType(Label1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Label2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Label3, System.ComponentModel.ISupportInitialize).EndInit()

        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
    End Sub
    
    Friend WithEvents GroupBox1 As VisiWinNET.Forms.GroupBox
    Friend WithEvents Label1 As VisiWinNET.Forms.Label
    Friend WithEvents Label2 As VisiWinNET.Forms.Label
    Friend WithEvents Label3 As VisiWinNET.Forms.Label

End Class