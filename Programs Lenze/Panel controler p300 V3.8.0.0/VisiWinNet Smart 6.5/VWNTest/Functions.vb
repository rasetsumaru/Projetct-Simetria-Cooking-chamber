Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic

Imports System.Windows.Forms 

Imports VisiWinNET.UserManagement 

Public Module Functions

	Public Function AutomatLofOff () As System.Windows.Forms.DialogResult 
		If UserManager.SecondsToAutoLogOff = 0 Then
		
			Dim frm As New FCentral()
			Dim ref As System.Windows.Forms.DialogResult 
			
			ref = frm.ShowDialog()
			frm.Dispose()
			
			Return ref
		End If
	End Function

End Module
