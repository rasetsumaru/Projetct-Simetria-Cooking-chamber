'---------------------------------------------------------
' This file was generated by VisiWinNET
' 
' Attention:
' Changes to this file may cause incorrect behavior 
' and will be lost if the file is regenerated.
'
'---------------------------------------------------------

Partial Class FLogOn

    Private Sub InitializeComponent()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
        Dim loader As New VisiWinNET.Forms.Internals.SFCLoader(Me)
        loader.Load(System.IO.Path.Combine(VisiWinNET.Project.ProjectInfo.Path, "FLogOn.sfc"), False)
        UserManagementDialogs = loader.Find("UserManagementDialogs")
        cmdChangePassword = loader.Find("cmdChangePassword")
        cmdClose = loader.Find("cmdClose")
        cmdLogOff = loader.Find("cmdLogOff")
        cmdCancel = loader.Find("cmdCancel")
        cmdLogOn = loader.Find("cmdLogOn")
        vinPassword = loader.Find("vinPassword")
        vinUserName = loader.Find("vinUserName")
        Shape1 = loader.Find("Shape1")
        Label2 = loader.Find("Label2")
        Shape2 = loader.Find("Shape2")
        CType(cmdChangePassword, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdClose, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdLogOff, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdCancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdLogOn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(vinPassword, System.ComponentModel.ISupportInitialize).EndInit()
        CType(vinUserName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Label2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape2, System.ComponentModel.ISupportInitialize).EndInit()

        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
    End Sub
    
    Friend WithEvents UserManagementDialogs As VisiWinNET.Forms.UserManagementDialogs
    Friend WithEvents cmdChangePassword As VisiWinNET.Forms.CommandButton
    Friend WithEvents cmdClose As VisiWinNET.Forms.CommandButton
    Friend WithEvents cmdLogOff As VisiWinNET.Forms.CommandButton
    Friend WithEvents cmdCancel As VisiWinNET.Forms.CommandButton
    Friend WithEvents cmdLogOn As VisiWinNET.Forms.CommandButton
    Friend WithEvents vinPassword As VisiWinNET.Forms.VarIn
    Friend WithEvents vinUserName As VisiWinNET.Forms.VarIn
    Friend WithEvents Shape1 As VisiWinNET.Forms.Shape
    Friend WithEvents Label2 As VisiWinNET.Forms.Label
    Friend WithEvents Shape2 As VisiWinNET.Forms.Shape

End Class