Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms

Imports VisiWinNET.Services
Imports VisiWinNET.LanguageSwitching 
Imports VisiWinNET.UserManagement 

Public Class FSystem
    Inherits VisiWinNET.Forms.SmartForm

    #Region "Initialization"
	
    ''' <summary>
    ''' This method is called, when the Form is initialized.
    ''' </summary>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        ' Do not remove this line.
        InitializeComponent()
		
        ' Add any initialization here:
	
		'internal item for language switching
		VisiWinNET.Services.AppService.VWSet("__LANGUAGE.LANGUAGE_CLID",VisiWinNET.LanguageSwitching.LanguageManager.CurrentLCID)
		
		'set the user defined touchkeyboards to the touchkeyboard control
		If (System.Environment.OSVersion.Platform.ToString() = "WinCE")
		    For Each strDefindedForm As String In VisiWinNET.Forms.ProjectForms.DefinedForms
				If (strDefindedForm = "FNumPad") Then
					TouchKeyboard.NumericalPad.ClassName = "FNumPad"
				End If	
				If (strDefindedForm = "FAlphaPad") Then
					TouchKeyboard.AlphaNumericalPad.ClassName = "FAlphaPad"
				End If	
				If (strDefindedForm = "FHexPad") Then
					TouchKeyboard.HexadecimalPad.ClassName = "FHexPad"
				End If	
				If (strDefindedForm = "FBinPad") Then
					TouchKeyboard.BinaryPad.ClassName = "FBinPad"	
				End If	
			Next    
		End If
		
		End Sub

    #End Region
    
    #Region "Dispose"
    
    ''' <summary>
    ''' This method is called, when the Form is unloaded.
    ''' Release the resources used by the Form inside this method.
    ''' </summary>
    ''' <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        
        ' Release the resources used by the form here:
        
        ' Call the Dispose Method of the base class.
        ' Do not remove this line.
        MyBase.Dispose(disposing)
        
    End Sub
    
    #End Region
	
	#Region "Events"

    ''' <summary>
    ''' This method is called by a form swichting.
    ''' Saving the name of the last activated form and setting the name of the current form ('Me.Name')
    ''' </summary>
    Private Sub Me_FormChanged(ByVal sender As System.Object, ByVal e As VisiWinNET.Forms.FormChangedEventArgs) Handles Me.FormChanged
        VisiWinNET.Services.AppService.VWSet("__FORMS.LASTACTIVEFORM", VisiWinNET.Services.AppService.VWGet("__FORMS.ACTIVEFORM"))
	    VisiWinNET.Services.AppService.VWSet("__FORMS.ACTIVEFORM", Me.Name)
    End Sub

    #End Region
	
	
    Private Sub cmdLogOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLogOff.Click
		Me.TimerLogOff.Enabled = True
    End Sub

    Public Sub TimerLogOff_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerLogOff.Tick
        Me.TimerLogOff.Enabled = False
		UserManager.LogOff()
	
		Dim frm As New FLogOn()
		frm.ShowDialog()
    End Sub

    Private Sub Me_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        LogofTimer.Enabled = False
        LogofTimer.Interval = 500
		
		ChamberTimer.Enabled = True
        ChamberTimer.Interval = 500
    End Sub

    Private Sub cmdProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdProcess.Click
	
		If System.Convert.ToInt32(AppService.VWGet("Controller.Controller.Application.SETTINGS.Enabled_chambers")) > 1 Then
			
			If FSelectBox.Show(Localization.GetText("@LabelForms.FSelectBox.Labels.Label001")) = DialogResult.ok Then
				
				Dim j() As Boolean = {False, False, False}
				
				For i As Integer = 0 To System.Convert.ToInt32(AppService.VWGet("Controller.Controller.Application.SETTINGS.Enabled_chambers")) -1
					Dim address As String = "Controller.Controller.Application.GLOBAL.Program_chamber<[" + i.toString() + "]>"
					j(i) = Convert.ToBoolean(AppService.VWGet(address))
				Next
					
				If j(Convert.ToInt32(AppService.VWGet("ProcessSystem.ChamberIndex"))) = True Then
					VisiWinNET.Forms.ProjectForms.ShowChildForm("FProcess", "FSystem",False, VisiWinNET.Forms.FormChangeModes.ShowNewCloseActive)
				Else
					FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption001"), _
						Localization.GetText("@LabelForms.FMessageBox.Messages.Message035"), MessageBoxButtons.Ok)
				End If
			End If
		
		Else
		
			If System.Convert.ToBoolean(AppService.VWGet("Controller.Controller.Application.GLOBAL.Program_chamber<[0]>")) = True Then
				AppService.VWSet("ProcessSystem.ChamberIndex", 1)
				VisiWinNET.Forms.ProjectForms.ShowChildForm("FProcess", "FSystem",False, VisiWinNET.Forms.FormChangeModes.ShowNewCloseActive)
			Else
				FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption001"), _
					Localization.GetText("@LabelForms.FMessageBox.Messages.Message034"), MessageBoxButtons.Ok)
			End If
			
		End If
	
    End Sub

    Private Sub cmdSynoptic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSynoptic.Click
        
		If System.Convert.ToInt32(AppService.VWGet("Controller.Controller.Application.SETTINGS.Enabled_chambers")) > 1 Then
			
			If FSelectBox.Show(Localization.GetText("@LabelForms.FSelectBox.Labels.Label002")) = DialogResult.ok Then
				
				VisiWinNET.Forms.ProjectForms.ShowChildForm("FSynoptic", "FSystem", False, VisiWinNET.Forms.FormChangeModes.ShowNewCloseActive)
				
			End If
		
		Else
		
			AppService.VWSet("ProcessSystem.ChamberIndex", 1)
			VisiWinNET.Forms.ProjectForms.ShowChildForm("FSynoptic", "FSystem",False, VisiWinNET.Forms.FormChangeModes.ShowNewCloseActive)
			
		End If
	
    End Sub

End Class
