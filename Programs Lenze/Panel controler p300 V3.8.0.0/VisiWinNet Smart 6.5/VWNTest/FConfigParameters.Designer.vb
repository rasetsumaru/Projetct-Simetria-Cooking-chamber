'---------------------------------------------------------
' This file was generated by VisiWinNET
' 
' Attention:
' Changes to this file may cause incorrect behavior 
' and will be lost if the file is regenerated.
'
'---------------------------------------------------------

Partial Class FConfigParameters

    Private Sub InitializeComponent()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
        Dim loader As New VisiWinNET.Forms.Internals.SFCLoader(Me)
        loader.Load(System.IO.Path.Combine(VisiWinNET.Project.ProjectInfo.Path, "FConfigParameters.sfc"), False)
        GroupBox = loader.Find("GroupBox")
        VarInActivationEngines = loader.Find("VarInActivationEngines")
        VarInAvailableCars = loader.Find("VarInAvailableCars")
        VarInAvailableChambers = loader.Find("VarInAvailableChambers")
        Label1 = loader.Find("Label1")
        cboControl = loader.Find("cboControl")
        VarInUpperHumidity = loader.Find("VarInUpperHumidity")
        VarInLowerHumidity = loader.Find("VarInLowerHumidity")
        VarInUpperTimer = loader.Find("VarInUpperTimer")
        VarInLowerTimer = loader.Find("VarInLowerTimer")
        VarInUpperTemperature = loader.Find("VarInUpperTemperature")
        VarInLowerTemperature = loader.Find("VarInLowerTemperature")
        Shape1 = loader.Find("Shape1")
        Shape2 = loader.Find("Shape2")
        Shape3 = loader.Find("Shape3")
        cmdReturn = loader.Find("cmdReturn")
        Shape4 = loader.Find("Shape4")
        CType(GroupBox, System.ComponentModel.ISupportInitialize).EndInit()
        GroupBox.ResumeLayout(False)
        CType(VarInActivationEngines, System.ComponentModel.ISupportInitialize).EndInit()
        CType(VarInAvailableCars, System.ComponentModel.ISupportInitialize).EndInit()
        CType(VarInAvailableChambers, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Label1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cboControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(VarInUpperHumidity, System.ComponentModel.ISupportInitialize).EndInit()
        CType(VarInLowerHumidity, System.ComponentModel.ISupportInitialize).EndInit()
        CType(VarInUpperTimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(VarInLowerTimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(VarInUpperTemperature, System.ComponentModel.ISupportInitialize).EndInit()
        CType(VarInLowerTemperature, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdReturn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Shape4, System.ComponentModel.ISupportInitialize).EndInit()

        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
    End Sub
    
    Friend WithEvents GroupBox As VisiWinNET.Forms.GroupBox
    Friend WithEvents VarInActivationEngines As VisiWinNET.Forms.VarIn
    Friend WithEvents VarInAvailableCars As VisiWinNET.Forms.VarIn
    Friend WithEvents VarInAvailableChambers As VisiWinNET.Forms.VarIn
    Friend WithEvents Label1 As VisiWinNET.Forms.Label
    Friend WithEvents cboControl As VisiWinNET.Forms.ComboBox
    Friend WithEvents VarInUpperHumidity As VisiWinNET.Forms.VarIn
    Friend WithEvents VarInLowerHumidity As VisiWinNET.Forms.VarIn
    Friend WithEvents VarInUpperTimer As VisiWinNET.Forms.VarIn
    Friend WithEvents VarInLowerTimer As VisiWinNET.Forms.VarIn
    Friend WithEvents VarInUpperTemperature As VisiWinNET.Forms.VarIn
    Friend WithEvents VarInLowerTemperature As VisiWinNET.Forms.VarIn
    Friend WithEvents Shape1 As VisiWinNET.Forms.Shape
    Friend WithEvents Shape2 As VisiWinNET.Forms.Shape
    Friend WithEvents Shape3 As VisiWinNET.Forms.Shape
    Friend WithEvents cmdReturn As VisiWinNET.Forms.CommandButton
    Friend WithEvents Shape4 As VisiWinNET.Forms.Shape

End Class