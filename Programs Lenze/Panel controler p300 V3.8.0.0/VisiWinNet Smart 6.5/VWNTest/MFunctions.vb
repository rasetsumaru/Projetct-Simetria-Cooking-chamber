Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic

Imports VisiWinNET.UserManagement 
Imports VisiWinNET.LanguageSwitching 
Imports VisiWinNET.Services

Public Module Functions

	Public WithEvents LogofTimer As New System.Windows.Forms.Timer
	Public WithEvents ChamberTimer As New System.Windows.Forms.Timer
	
	Public Sub AutoLogOff(ByVal sender As System.Object, ByVal e As VisiWinNET.UserManagement.AutoLogOffEventArgs)
		
		UserManager.LogOff()
		
		If FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption000"), _
			Localization.GetText("@LabelForms.FMessageBox.Messages.Message010"), MessageBoxButtons.OK) = DialogResult.OK Then

			Dim UserMan As New UserManager 
			RemoveHandler  UserMan.AutoLogOffQuery, AddressOf AutoLogoff 
		
			LogofTimer.Enabled = True
			
		End If
	
	End Sub

	Public Sub LogofTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogofTimer.Tick
		LogofTimer.Enabled = False
		UserManager.LogOff()
	
		Dim frm As New FLogOn()
		frm.ShowDialog()
	End Sub

	Public Sub ChabmerTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChamberTimer.Tick
	
		Dim chambers As Integer = System.Convert.ToInt32(AppService.VWGet("Controller.Controller.Application.SETTINGS.Enabled_chambers")) - 1
		
		For i As Integer = 0 To chambers
			Dim status As String = "Controller.Controller.Application.GLOBAL.Program_chamber_status<[" + i.ToString() + "]>"
			Dim value As Integer = AppService.VWGet(status)
			
			If value > 3 And value < 8 Then
				ChamberTimer.Enabled = False
				
				Dim message As String = ""
				
				If chambers > 1 Then
					message = "@LabelForms.FMessageBox.Messages.Message04" + (value - 2).toString()
				Else
					message = "@LabelForms.FMessageBox.Messages.Message05" + (value - 4).toString()
				End If
				
				If value = 7 Then
					AppService.VWSet(status, 8)
				End If
			
				If chambers > 1 Then
					If FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption001"), _
						Localization.GetText("@LabelForms.FMessageBox.Messages.Message041") + (i + 1).toString() + Localization.GetText(message), MessageBoxButtons.Ok) = DialogResult.Ok Then
				
						ChamberTimer.Enabled = True
						AppService.VWSet(status, 0)
					End If
				Else
					If FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption001"), Localization.GetText(message), MessageBoxButtons.Ok) = DialogResult.Ok Then
						ChamberTimer.Enabled = True
						AppService.VWSet(status, 0)
					End If
				End If
			End If
			
		Next
	
	End Sub
	
End Module
