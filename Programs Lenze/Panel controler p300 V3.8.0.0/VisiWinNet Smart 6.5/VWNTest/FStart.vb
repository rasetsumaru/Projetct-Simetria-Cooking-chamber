''' <summary>
''' Eröffnungsbild der Applikation.
''' Wird angezeigt, solange die VisiWinNET-Laufzeit und die Applikation initialisiert werden.
''' </summary>
Public Class FStart

    ''' <summary>
    ''' Erzeugt und initialisiert eine neue Instanz der Klasse.
    ''' </summary>
    Public Sub New(ByVal codeBase As String)
        ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        InitializeComponent()

		'System.Windows.Forms.Cursor.Hide()	
		
        ' SmartForm Content für FStart ermitteln
        Dim directory As String = System.IO.Path.GetDirectoryName(codeBase)
        Dim smartStartForm As String = System.IO.Path.Combine(directory, "FStart.sfc")

        If (smartStartForm.StartsWith("file:\")) Then
            smartStartForm = smartStartForm.Substring(6)
        End If

        If (System.IO.File.Exists(smartStartForm)) Then
            Dim sfcLoader As New VisiWinNET.Forms.Internals.SFCLoader(Me)
            sfcLoader.Load(smartStartForm)
        End If
	
		If (System.Environment.OSVersion.Platform.ToString() = "WinCE") Then
			'Runtime under Windows CE
			If (Screen.PrimaryScreen.Bounds.Width = 320) Then
				Me.Width = 360
				Me.Height = 240
			End If
		End If
	
    End Sub

    ''' <summary>
    ''' Verwendete Ressourcen bereinigen.
    ''' </summary>
    ''' <param name="disposing"> true, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls false.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If components IsNot Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    ''' <summary>
    ''' Wird aufgerufen, wenn die VisiWinNET-Laufzeit initialisiert wurde.
    ''' </summary>
    ''' <param name="sender"> Quelle des Ereignisses.</param>
    ''' <param name="e"> Stellt Informationen für das Ereignis bereit.</param>
    Protected Overrides Sub OnInitReady(ByVal sender As Object, ByVal e As VisiWinNET.Project.ProjectManagerEventArgs)
        VisiWinNET.Forms.GuiConfiguration.Initialize(System.Reflection.Assembly.GetExecutingAssembly())
		If (System.Environment.OSVersion.Platform.ToString() = "WinCE")
		    For Each strDefindedForm As String In VisiWinNET.Forms.ProjectForms.DefinedForms
				If (strDefindedForm = "FWaitCursor") Then
					VisiWinNET.Forms.ProjectForms.Show("FWaitCursor")
					VisiWinNET.Forms.ProjectForms.Hide("FWaitCursor")
				End If	
				If (strDefindedForm = "FNumPad") Then
					VisiWinNET.Forms.ProjectForms.Show("FNumPad")
					VisiWinNET.Forms.ProjectForms.Hide("FNumPad")
				End If	
				If (strDefindedForm = "FAlphaPad") Then
					VisiWinNET.Forms.ProjectForms.Show("FAlphaPad")
					VisiWinNET.Forms.ProjectForms.Hide("FAlphaPad")
				End If	
				If (strDefindedForm = "FHexPad") Then
					VisiWinNET.Forms.ProjectForms.Show("FHexPad")
					VisiWinNET.Forms.ProjectForms.Hide("FHexPad")
				End If	
				If (strDefindedForm = "FBinPad") Then
					VisiWinNET.Forms.ProjectForms.Show("FBinPad")
					VisiWinNET.Forms.ProjectForms.Hide("FBinPad")
				End If	
			Next    
		End If
		MyBase.OnInitReady(sender, e)
    End Sub

    ''' <summary>
    ''' Wird aufgerufen, wenn die VisiWinNET-Laufzeit beendet wurde.
    ''' </summary>
    ''' <param name="sender"> Quelle des Ereignisses.</param>
    ''' <param name="e"> Stellt Informationen für das Ereignis bereit.</param>
    Protected Overrides Sub OnProjectShutdownFinished(ByVal sender As Object, ByVal e As System.EventArgs)
        MyBase.OnProjectShutdownFinished(sender, e)
        Application.Exit()
    End Sub

End Class
