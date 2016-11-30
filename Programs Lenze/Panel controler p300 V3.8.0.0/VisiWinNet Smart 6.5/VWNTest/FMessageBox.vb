Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Xml

Imports VisiWinNET.LanguageSwitching

Public Class FMessageBox
    Inherits VisiWinNET.Forms.SmartForm

	#Region "Scale To Screen Size"
	' This region is required for the scaling of forms.
	' Do not remove this lines.
	
	#Region "Enumerations"	
	Public Enum m_eDisplayResolution
		UNKOWN = 0
		QVGA = 1  '320x240
		WQVGA = 2 '480x272
		VGA = 3   '640x480
		WVGA = 4  '800x480
		SVGA = 5  '800x600
		XGA = 6   '1024x768
		SXGA = 7  '1280x1024
	End Enum
	#End Region

	#Region "Variables"
	'Use of Scaling
	Public m_bActivateScaleToScreenSize As Boolean = True
	
	'Display Resolution
	Public m_iWidth As Integer = ReadResolution("width")
	Public m_iHeight As Integer = ReadResolution("height")
	Public m_iDisplayResolution As m_eDisplayResolution = m_eDisplayResolution.UNKOWN
	
	'ButtonLine
	Public m_iNumberOfButtons As Integer = 8
	Public m_iButtonWidth As Integer = 128
	Public m_iButtonLineHeight As Integer = 80
	
	'Header
	Public m_iHeaderHeight As Integer = 80
	Public m_iPictureBoxHeaderWidth As Integer = 120
	
	'Emulator
	Public m_iEmulatorFrame As Integer = 75
	Public m_iFreeSpaceHeight As Integer = 40
	#End Region
	
	#Region "Functions"
	Public  Function SetFontClass() As String	
			If ((m_iWidth = 320) And (m_iHeight = 240)) Then
				m_iDisplayResolution = m_eDisplayResolution.QVGA
				m_iButtonLineHeight = 40
				Return "FontTahomaSmall"
			ElseIf ((m_iWidth = 480) And (m_iHeight = 272)) Then
				m_iDisplayResolution = m_eDisplayResolution.WQVGA
				m_iButtonLineHeight = 60
				Return "FontTahomaSmall"
			ElseIf ((m_iWidth = 640) And (m_iHeight = 480)) Then
				m_iDisplayResolution = m_eDisplayResolution.VGA
				m_iButtonLineHeight = 60
				Return "FontTahomaSmall"
			ElseIf ((m_iWidth = 800) And (m_iHeight = 480)) Then
				m_iDisplayResolution = m_eDisplayResolution.WVGA
				Return "FontTahomaMedium"
			ElseIf ((m_iWidth = 800) And (m_iHeight = 600)) Then
				m_iDisplayResolution = m_eDisplayResolution.SVGA
				Return "FontTahomaMedium"
			ElseIf ((m_iWidth = 1024) And (m_iHeight = 768)) Then			
				m_iDisplayResolution = m_eDisplayResolution.XGA
				Return "FontTahomaMedium"
			ElseIf ((m_iWidth = 1280) And (m_iHeight = 1024)) Then	
				m_iDisplayResolution = m_eDisplayResolution.SXGA
				Return "FontTahomaMedium"
			Else
				m_iDisplayResolution = m_eDisplayResolution.UNKOWN
				'?!Emulator
				Return "FontTahomaMedium"
			End If	
	End Function
		
	Public  Function ReadResolution(ByVal name As String) As Integer
		If (System.Environment.OSVersion.Platform.ToString() = "WinCE")
			'Runtime under Windows CE
			If name = "width" Then
				Return Screen.PrimaryScreen.Bounds.Width 
			Else If name = "height" Then
				Return Screen.PrimaryScreen.Bounds.Height 
			End If
		Else
			'Running project in emulator under standard OS
			Try		
				Dim nIndex As Integer
				Dim sPath As String = Trim(System.Reflection.Assembly.GetExecutingAssembly.ManifestModule.FullyQualifiedName)
				Dim sProjectName As String = sPath.Substring(sPath.LastIndexOf("\"), sPath.Length - sPath.LastIndexOf("\") - 4)
				
				For i As Integer = 0 To 2
					nIndex = sPath.LastIndexOf("\")
					sPath = sPath.Substring(0, nIndex)
				Next
			
				Dim m_xmld As XmlDocument
				Dim m_nodelist As XmlNodeList
				Dim m_node As XmlNode
				Dim m_iValue As Integer = 0
				
				m_xmld = New XmlDocument()
				m_xmld.Load(sPath + sProjectName + ".Device.config")
				m_nodelist = m_xmld.SelectNodes("/DeviceSettings/Device/Display")
				
				For Each m_node In m_nodelist
					m_iValue = m_node.Attributes.GetNamedItem(name).Value
				Next
			
				Return m_iValue			
			Catch ErrorException As Exception
				Return 0	
			End Try
		End If
    End Function

	Public Sub ScaleButtonLineControls (ByVal m_GroupBox As VisiWinNET.Forms.GroupBox)
		Dim iIndex As Integer = 0
		'Calculation the number of button controls in button line (m_GroupBox.Controls.Count)
		For Each ctrl As Control In m_GroupBox.Controls
			If TypeOf ctrl Is VisiWinNET.Forms.CommandButton Or TypeOf ctrl Is VisiWinNET.Forms.Key Or TypeOf ctrl Is VisiWinNET.Forms.Switch Then
				iIndex = iIndex + 1
			End If
		Next
		'Recalculation the properties of the buttonline controls (width, height, fontclass)
		m_iNumberOfButtons = iIndex 
		m_GroupBox.FontClass = SetFontClass()
		m_GroupBox.Height = m_iButtonLineHeight
		m_iButtonWidth = m_iWidth / m_iNumberOfButtons  
		For Each ctrl As Control In m_GroupBox.Controls	
			If TypeOf ctrl Is VisiWinNET.Forms.CommandButton Or TypeOf ctrl Is VisiWinNET.Forms.Key Or TypeOf ctrl Is VisiWinNET.Forms.Switch Then
				ctrl.Width = m_iButtonWidth
			End If
		Next
	End Sub

	#End Region
	
	#End Region
	
    #Region "Initialization"
	
    ''' <summary>
    ''' This method is called, when the Form is initialized.
    ''' </summary>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        ' Do not remove this line.
        InitializeComponent()
		
        ' Add any initialization here:
		
		'--------------------
		'ScaleToScreenSize
		'--------------------
		
		'Display Resolution
		'Me.Height = m_iHeight - m_iHeaderHeight - m_iButtonLineHeight
		'Me.Width = m_iWidth
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

    'This method is called by a form switching.
    'Saving the name of the last activated form and setting the name of the current form ('Me.Name')
    'Private Sub Me_FormChanged(ByVal sender As System.Object, ByVal e As VisiWinNET.Forms.FormChangedEventArgs) Handles Me.FormChanged
        'VisiWinNET.Services.AppService.VWSet("__FORMS.LASTACTIVEFORM", VisiWinNET.Services.AppService.VWGet("__FORMS.ACTIVEFORM"))
		'VisiWinNET.Services.AppService.VWSet("__FORMS.ACTIVEFORM", Me.Name)
    'End Sub

    #End Region
		
	Public Overloads Shared Function Show( _
        ByVal caption As String, _
		ByVal text As String, _
        ByVal buttons As MessageBoxButtons _
		) As DialogResult

        Return ShowMessageBox(caption, text, buttons)
    End Function
	
	Private Shared Function ShowMessageBox( _
		ByVal caption As String, _
        ByVal text As String, _
		ByVal buttons As MessageBoxButtons _
        ) As DialogResult

        Dim frm As New FMessageBox
        Dim ret As DialogResult

		frm.SetCaption(caption)
        frm.SetText(text)
		frm.SetButtonTexts(buttons)
		
        ret = frm.ShowDialog()
        frm.Dispose()
        Return ret
    End Function

	Private Sub SetCaption(ByVal caption As String)
        lblCaption.text = caption
    End Sub

	Private Sub SetText(ByVal text As String)
        lblMessage.text = text
    End Sub

	Private Sub SetButtonTexts(ByVal buttons As System.Windows.Forms.MessageBoxButtons)
        Select Case buttons
            Case MessageBoxButtons.AbortRetryIgnore
                cmdButton000.text = Localization.GetText("@LabelForms.FMessageBox.ButtonTexts.cmdAbort")
				cmdButton000.DialogResult = DialogResult.Abort
                cmdButton001.text = Localization.GetText("@LabelForms.FMessageBox.ButtonTexts.cmdRetry")
				cmdButton001.DialogResult = DialogResult.Retry
                cmdButton002.text = Localization.GetText("@LabelForms.FMessageBox.ButtonTexts.cmdIgnore")
				cmdButton002.DialogResult = DialogResult.Ignore

            Case MessageBoxButtons.OK
                cmdButton000.text = Localization.GetText("@LabelForms.FMessageBox.ButtonTexts.cmdOK")
				cmdButton000.DialogResult = DialogResult.OK
				
            Case MessageBoxButtons.OKCancel
                cmdButton000.text = Localization.GetText("@LabelForms.FMessageBox.ButtonTexts.cmdOK")
				cmdButton000.DialogResult = DialogResult.OK
                cmdButton001.text = Localization.GetText("@LabelForms.FMessageBox.ButtonTexts.cmdCancel")
				cmdButton001.DialogResult = DialogResult.Cancel
				
            Case MessageBoxButtons.RetryCancel
                cmdButton000.text = Localization.GetText("@LabelForms.FMessageBox.ButtonTexts.cmdRetry")
				cmdButton000.DialogResult = DialogResult.Retry
                cmdButton001.text = Localization.GetText("@LabelForms.FMessageBox.ButtonTexts.cmdCancel")
				cmdButton001.DialogResult = DialogResult.Cancel
				
            Case MessageBoxButtons.YesNo
                cmdButton000.text = Localization.GetText("@LabelForms.FMessageBox.ButtonTexts.cmdYes")
				cmdButton000.DialogResult = DialogResult.Yes
                cmdButton001.text = Localization.GetText("@LabelForms.FMessageBox.ButtonTexts.cmdNo")
				cmdButton001.DialogResult = DialogResult.No
				
            Case MessageBoxButtons.YesNoCancel
                cmdButton000.text = Localization.GetText("@LabelForms.FMessageBox.ButtonTexts.cmdYes")
				cmdButton000.DialogResult = DialogResult.Yes
                cmdButton001.text = Localization.GetText("@LabelForms.FMessageBox.ButtonTexts.cmdNo")
				cmdButton001.DialogResult = DialogResult.No
                cmdButton002.text = Localization.GetText("@LabelForms.FMessageBox.ButtonTexts.cmdCancel")
				cmdButton002.DialogResult = DialogResult.Cancel
				
        End Select

        cmdButton000.Visible = (cmdButton000.Text.Length > 0)
        cmdButton001.Visible = (cmdButton001.Text.Length > 0)
        cmdButton002.Visible = (cmdButton002.Text.Length > 0)
    End Sub

    Private Sub TimerClose_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerClose.Tick
        Me.TimerClose.Enabled = False
		Me.Close()
		Me.Dispose()
    End Sub

    Private Sub cmdButton000_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdButton000.Click
        Me.TimerClose.Enabled=True
    End Sub

	Private Sub cmdButton001_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdButton001.Click
        Me.TimerClose.Enabled=True
    End Sub

	Private Sub cmdButton002_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdButton002.Click
        Me.TimerClose.Enabled=True
    End Sub
End Class
