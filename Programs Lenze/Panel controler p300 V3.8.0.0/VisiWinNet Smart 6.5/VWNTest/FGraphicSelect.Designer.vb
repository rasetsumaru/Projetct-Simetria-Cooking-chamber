'---------------------------------------------------------
' This file was generated by VisiWinNET
' 
' Attention:
' Changes to this file may cause incorrect behavior 
' and will be lost if the file is regenerated.
'
'---------------------------------------------------------

Partial Class FGraphicSelect

    Private Sub InitializeComponent()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
        Dim loader As New VisiWinNET.Forms.Internals.SFCLoader(Me)
        loader.Load(System.IO.Path.Combine(VisiWinNET.Project.ProjectInfo.Path, "FGraphicSelect.sfc"), False)
        lblArchives = loader.Find("lblArchives")
        cmdChangeColor = loader.Find("cmdChangeColor")
        cmdRemoveAll = loader.Find("cmdRemoveAll")
        cmdRemove = loader.Find("cmdRemove")
        cmdAddAll = loader.Find("cmdAddAll")
        cmdAdd = loader.Find("cmdAdd")
        lstAvailableTrends = loader.Find("lstAvailableTrends")
        cmbArchives = loader.Find("cmbArchives")
        lvwSelectedTrends = loader.Find("lvwSelectedTrends")
        Label1 = loader.Find("Label1")
        Label3 = loader.Find("Label3")
        Label4 = loader.Find("Label4")
        cmdOk = loader.Find("cmdOk")
        cmdCancel = loader.Find("cmdCancel")
        Shape1 = loader.Find("Shape1")
        Label2 = loader.Find("Label2")
        Shape2 = loader.Find("Shape2")
        CType(lblArchives, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdChangeColor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdRemoveAll, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdRemove, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdAddAll, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdAdd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(lstAvailableTrends, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmbArchives, System.ComponentModel.ISupportInitialize).EndInit()
        CType(lvwSelectedTrends, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Label1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Label3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Label4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdOk, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdCancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Label2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape2, System.ComponentModel.ISupportInitialize).EndInit()

        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
    End Sub
    
    Friend WithEvents lblArchives As VisiWinNET.Forms.Label
    Friend WithEvents cmdChangeColor As VisiWinNET.Forms.CommandButton
    Friend WithEvents cmdRemoveAll As VisiWinNET.Forms.CommandButton
    Friend WithEvents cmdRemove As VisiWinNET.Forms.CommandButton
    Friend WithEvents cmdAddAll As VisiWinNET.Forms.CommandButton
    Friend WithEvents cmdAdd As VisiWinNET.Forms.CommandButton
    Friend WithEvents lstAvailableTrends As VisiWinNET.Forms.ListBox
    Friend WithEvents cmbArchives As VisiWinNET.Forms.ComboBox
    Friend WithEvents lvwSelectedTrends As VisiWinNET.Forms.ListView
    Friend WithEvents Label1 As VisiWinNET.Forms.Label
    Friend WithEvents Label3 As VisiWinNET.Forms.Label
    Friend WithEvents Label4 As VisiWinNET.Forms.Label
    Friend WithEvents cmdOk As VisiWinNET.Forms.CommandButton
    Friend WithEvents cmdCancel As VisiWinNET.Forms.CommandButton
    Friend WithEvents Shape1 As VisiWinNET.Forms.Shape
    Friend WithEvents Label2 As VisiWinNET.Forms.Label
    Friend WithEvents Shape2 As VisiWinNET.Forms.Shape

End Class