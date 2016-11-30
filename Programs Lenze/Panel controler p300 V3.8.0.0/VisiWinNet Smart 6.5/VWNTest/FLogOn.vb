Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Xml

Imports VisiWinNET.UserManagement
Imports VisiWinNET.LanguageSwitching

Public Class FLogOn
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
		InitializeVisiWinNETComponents()
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

    Private Sub Me_FormChanged(ByVal sender As System.Object, ByVal e As VisiWinNET.Forms.FormChangedEventArgs) Handles Me.FormChanged
        VisiWinNET.Services.AppService.VWSet("__FORMS.LASTACTIVEFORM", VisiWinNET.Services.AppService.VWGet("__FORMS.ACTIVEFORM"))
	    VisiWinNET.Services.AppService.VWSet("__FORMS.ACTIVEFORM", Me.Name)
    End Sub

    #End Region
    
	#Region "Declarations"

    Private Enum DialogType
        LogOn
        Verify
    End Enum

    Private mDialogType As DialogType

	#End Region

    Private Sub cmdLogOn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLogOn.Click
        Dim enmLogOnSuccess As VisiWinNET.UserManagement.LogOnSuccess

        Select Case mDialogType
            Case DialogType.LogOn
                enmLogOnSuccess = UserManager.LogOn(DirectCast(vinUserName.VWItem.EditableValue, String), DirectCast(vinPassword.VWItem.EditableValue, String))
                Exit Select
            Case Else
                enmLogOnSuccess = UserManager.VerifyUser(DirectCast(vinUserName.VWItem.EditableValue, String), DirectCast(vinPassword.VWItem.EditableValue, String))
                Exit Select
        End Select
        
		Select Case enmLogOnSuccess
			
            Case LogOnSuccess.Succeeded
				Me.vinUserName.Enabled = False
				Me.vinPassword.Enabled = False
				
				Dim UserMan As New UserManager 
				AddHandler UserMan.AutoLogOffQuery, AddressOf AutoLogoff 
			
				FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption000"), _
					Localization.GetText("@LabelForms.FMessageBox.Messages.Message000"), MessageBoxButtons.OK)
				
            Case LogOnSuccess.PasswordExpired, LogOnSuccess.RenewPassword
				FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption000"), _
					Localization.GetText("@LabelForms.FMessageBox.Messages.Message003"), MessageBoxButtons.OK)
				
				If FLogOnChangePassword.Show(DirectCast(vinUserName.VWItem.EditableValue, String)) = DialogResult.OK Then
                	vinPassword.Focus()
                Else
                    Cancel()
                End If
                Exit Select

            Case LogOnSuccess.InvalidPassword
				FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption001"), _
					Localization.GetText("@LabelForms.FMessageBox.Messages.Message001"), MessageBoxButtons.OK)
				
				Me.vinPassword.Text = String.Empty 
                Me.vinPassword.Focus()
                Exit Select

            Case LogOnSuccess.UserDeactivated, LogOnSuccess.UnknownUser
                FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption001"), _
					Localization.GetText("@LabelForms.FMessageBox.Messages.Message002"), MessageBoxButtons.OK)
				
				Me.vinUserName.Text = String.Empty
                Me.vinUserName.Focus()
                Exit Select

            Case LogOnSuccess.CantAccessDomainInfo, LogOnSuccess.TooManyFailedLogOns
                FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption001"), _
					Localization.GetText("@LabelForms.FMessageBox.Messages.Message004"), MessageBoxButtons.OK)
                Cancel()
				Me.vinUserName.Focus()
				Exit Select
        End Select
    End Sub

	#Region "Initialization of VisiWinNET controls"

    Private Sub InitializeVisiWinNETComponents()

        CType(vinPassword, System.ComponentModel.ISupportInitialize).EndInit()
        CType(vinUserName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdLogOn, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

	#End Region

	Private Sub vinUserName_Change(ByVal sender As System.Object, ByVal e As VisiWinNET.DataAccess.ChangeEventArgs) Handles vinUserName.Change
    
		If (Not String.IsNullOrEmpty(vinUserName.VWItem.Text)) Or (Not String.IsNullOrEmpty(vinPassword.VWItem.Text)) 
            cmdCancel.Enabled = True
		Else
        	cmdCancel.Enabled = False
		End If
	
    End Sub

	Private Sub vinPassword_Change(ByVal sender As System.Object, ByVal e As VisiWinNET.DataAccess.ChangeEventArgs) Handles vinPassword.Change
        
		If (Not String.IsNullOrEmpty(vinUserName.Text)) Or (Not String.IsNullOrEmpty(vinPassword.Text)) 
            cmdCancel.Enabled = True
		Else
        	cmdCancel.Enabled = False
		End If
	
    End Sub

	Private Sub Cancel()
		Me.vinUserName.VWItem.Value = String.Empty
		Me.vinPassword.VWItem.Value = String.Empty
		Me.vinUserName.text = String.Empty
		Me.vinPassword.text = String.Empty
		Me.cmdCancel.Enabled = False
		Me.vinUserName.Focus()
    End Sub

    Private Sub Me_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.vinUserName.VWItem.Value = String.Empty
		Me.vinPassword.VWItem.Value = String.Empty
		Me.cmdCancel.Enabled = False
    End Sub
	
    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Cancel()
    End Sub
	
    Private Sub cmdLogOff_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLogOff.Click
		UserManager.LogOff()
		Me.vinUserName.Enabled = True
		Me.vinPassword.Enabled = True
		Cancel()
    End Sub

    Private Sub cmdChangePassword_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChangePassword.Click
		FLogOnChangePassword.Show(vinUserName.VWItem.EditableValue)
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
		Me.Close()
		Me.Dispose()
    End Sub

End Class
