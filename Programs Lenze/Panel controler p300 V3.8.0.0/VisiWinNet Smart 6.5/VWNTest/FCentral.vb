Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms

Public Class FCentral
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

End Class
