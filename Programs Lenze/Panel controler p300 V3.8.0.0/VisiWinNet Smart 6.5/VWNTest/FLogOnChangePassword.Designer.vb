'---------------------------------------------------------
' This file was generated by VisiWinNET
' 
' Attention:
' Changes to this file may cause incorrect behavior 
' and will be lost if the file is regenerated.
'
'---------------------------------------------------------

Partial Class FLogOnChangePassword

    Private Sub InitializeComponent()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
        Dim loader As New VisiWinNET.Forms.Internals.SFCLoader(Me)
        loader.Load(System.IO.Path.Combine(VisiWinNET.Project.ProjectInfo.Path, "FLogOnChangePassword.sfc"), False)
        cmdOk = loader.Find("cmdOk")
        cmdCancel = loader.Find("cmdCancel")
        vinPassword = loader.Find("vinPassword")
        vinUserName = loader.Find("vinUserName")
        Shape1 = loader.Find("Shape1")
        Label2 = loader.Find("Label2")
        Shape2 = loader.Find("Shape2")
        vinNewPassword = loader.Find("vinNewPassword")
        vinRepeatNewPassword = loader.Find("vinRepeatNewPassword")
        CType(cmdOk, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdCancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(vinPassword, System.ComponentModel.ISupportInitialize).EndInit()
        CType(vinUserName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Label2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(vinNewPassword, System.ComponentModel.ISupportInitialize).EndInit()
        CType(vinRepeatNewPassword, System.ComponentModel.ISupportInitialize).EndInit()

        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
    End Sub
    
    Friend WithEvents cmdOk As VisiWinNET.Forms.CommandButton
    Friend WithEvents cmdCancel As VisiWinNET.Forms.CommandButton
    Friend WithEvents vinPassword As VisiWinNET.Forms.VarIn
    Friend WithEvents vinUserName As VisiWinNET.Forms.VarIn
    Friend WithEvents Shape1 As VisiWinNET.Forms.Shape
    Friend WithEvents Label2 As VisiWinNET.Forms.Label
    Friend WithEvents Shape2 As VisiWinNET.Forms.Shape
    Friend WithEvents vinNewPassword As VisiWinNET.Forms.VarIn
    Friend WithEvents vinRepeatNewPassword As VisiWinNET.Forms.VarIn

End Class