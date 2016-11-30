Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Xml

Imports VisiWinNET.Forms
Imports VisiWinNET.UserManagement
Imports VisiWinNET.LanguageSwitching 

Public Class FUsers
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
        InitListView()
        InitDialog()
        
		' Add any initialization here:

		'--------------------
		'ScaleToScreenSize
		'--------------------
		
		'ChildForm Resolution
		Me.Height = Me.Height - m_iButtonLineHeight - m_iHeaderHeight	
		
    End Sub
    
	  Public Sub InitListView()
        Dim i As Integer = 0
        For Each lvcitem As VisiWinNET.Forms.ListViewColumn In lvwUsers.Columns
            i = i + 1
        Next
        For j As Integer = 0 To i - 1
            Me.lvwUsers.Columns.Remove(lvwUsers.Columns.Item(0))
        Next
        Dim ListViewTextColumn1 As VisiWinNET.Forms.ListViewTextColumn = New VisiWinNET.Forms.ListViewTextColumn
        Dim ListViewTextColumn2 As VisiWinNET.Forms.ListViewTextColumn = New VisiWinNET.Forms.ListViewTextColumn
        Dim ListViewTextColumn3 As VisiWinNET.Forms.ListViewTextColumn = New VisiWinNET.Forms.ListViewTextColumn
        Dim ListViewTextColumn4 As VisiWinNET.Forms.ListViewTextColumn = New VisiWinNET.Forms.ListViewTextColumn
        Dim ListViewTextColumn5 As VisiWinNET.Forms.ListViewTextColumn = New VisiWinNET.Forms.ListViewTextColumn
        Dim ListViewNumericalColumn1 As VisiWinNET.Forms.ListViewNumericalColumn = New VisiWinNET.Forms.ListViewNumericalColumn
        Dim ListViewNumericalColumn2 As VisiWinNET.Forms.ListViewNumericalColumn = New VisiWinNET.Forms.ListViewNumericalColumn
        Dim ListViewTextColumn6 As VisiWinNET.Forms.ListViewTextColumn = New VisiWinNET.Forms.ListViewTextColumn
        Dim ListViewTextColumn7 As VisiWinNET.Forms.ListViewTextColumn = New VisiWinNET.Forms.ListViewTextColumn
        Me.lvwUsers.Authorization.Mode = VisiWinNET.UserManagement.AuthorizationModes.Disabled
        Me.lvwUsers.Blink.Mode = VisiWinNET.Services.BlinkMode.EventOnly
		
        ListViewTextColumn1.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        ListViewTextColumn1.ColumnType = VisiWinNET.Forms.ColumnTypes.Text
        ListViewTextColumn1.HeaderText = Localization.GetText("@LabelForms.FUsers.ListView.Label000")
		ListViewTextColumn1.LocalizedHeaderText.TextType = VisiWinNET.LanguageSwitching.TextTypes.ClientSystem
        ListViewTextColumn1.Name = "Name"
		ListViewTextColumn1.Width = 80
        
		ListViewTextColumn2.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        ListViewTextColumn2.ColumnType = VisiWinNET.Forms.ColumnTypes.Text
        ListViewTextColumn2.HeaderText = Localization.GetText("@LabelForms.FUsers.ListView.Label001")
        ListViewTextColumn2.LocalizedHeaderText.TextType = VisiWinNET.LanguageSwitching.TextTypes.ClientSystem
        ListViewTextColumn2.Name = "FullName"
        ListViewTextColumn2.Width = 120

        ListViewTextColumn4.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        ListViewTextColumn4.ColumnType = VisiWinNET.Forms.ColumnTypes.Text
        ListViewTextColumn4.HeaderText = Localization.GetText("@LabelForms.FUsers.ListView.Label002")
        ListViewTextColumn4.LocalizedHeaderText.TextType = VisiWinNET.LanguageSwitching.TextTypes.ClientSystem
        ListViewTextColumn4.Name = "UserGroup"
        ListViewTextColumn4.Width = 120
		
        ListViewTextColumn5.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        ListViewTextColumn5.ColumnType = VisiWinNET.Forms.ColumnTypes.Text
        ListViewTextColumn5.HeaderText = Localization.GetText("@LabelForms.FUsers.ListView.Label003")
        ListViewTextColumn5.LocalizedHeaderText.TextType = VisiWinNET.LanguageSwitching.TextTypes.ClientSystem
        ListViewTextColumn5.Name = "State"
        ListViewTextColumn5.Width = 100
		
        ListViewTextColumn7.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        ListViewTextColumn7.ColumnType = VisiWinNET.Forms.ColumnTypes.Text
        ListViewTextColumn7.HeaderText = Localization.GetText("@LabelForms.FUsers.ListView.Label004")
        ListViewTextColumn7.LocalizedHeaderText.TextType = VisiWinNET.LanguageSwitching.TextTypes.ClientSystem
        ListViewTextColumn7.Name = "Comment"
        ListViewTextColumn7.Width = 140
		
        Me.lvwUsers.Columns.AddRange(New VisiWinNET.Forms.Internals.ListViewBaseColumn() { ListViewTextColumn1, ListViewTextColumn2, ListViewTextColumn4, ListViewTextColumn5, ListViewTextColumn7})
        Me.lvwUsers.DateTimeFormat.Mode = VisiWinNET.LanguageSwitching.DateTimeMode.DateTime
        Me.lvwUsers.FontClass = "Buttons"

        Me.lvwUsers.Header.FontClass = "Captions"
        Me.lvwUsers.Header.Height = 30
        Me.lvwUsers.ItemAutoHeight = False
        Me.lvwUsers.ItemHeight = 20
        Me.lvwUsers.Location = New System.Drawing.Point(20, 187)
        Me.lvwUsers.Name = "lvwUsers"
		
        Me.lvwUsers.ScrollBarProperties.Width = 30
        Me.lvwUsers.Size = New System.Drawing.Size(590, 360)
        Me.lvwUsers.TabIndex = 1
    End Sub

	#End Region
	
	#Region "Dispose"
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If components IsNot Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
    #End Region
    
	Private Sub InitDialog()
        'Show all user
		For Each user As VisiWinNET.UserManagement.User In UserManager.Users
			Dim item As New VisiWinNET.Forms.ListViewItem()
			lvwUsers.Items.Add(user.Name, item)
			UserToItem(user, item)
		Next
	End Sub

    Private Sub ListUsers(ByVal userGroup As String)
        If userGroup <> "" Then
            For Each user As VisiWinNET.UserManagement.User In UserManager.Groups(userGroup).Users
                Dim item As New VisiWinNET.Forms.ListViewItem()
                lvwUsers.Items.Add(user.Name, item)
                UserToItem(user, item)
            Next
        End If
    End Sub

    Private Sub UserToItem(ByVal user As User, ByVal item As VisiWinNET.Forms.ListViewItem)
        On Error Resume Next
        ' If the column has been removed from the list 'lvwGroups', an error occurs.
        ' This can be ignored, the information is not displayed then simply.
        item.SubItems.Item("Name") = user.Name
        item.SubItems.Item("FullName") = user.FullName
        item.SubItems.Item("Code") = user.Code
        item.SubItems.Item("UserGroup") = user.Group.Text
        item.SubItems.Item("Comment") = user.Comment
        item.SubItems.Item("State") = Localization.GetText(TextTypes.ClientSystem, "Dialogs.UserManagement.UserStates." & user.State.ToString)
        If user.Group.MaxFailedLogOns > 0 Then
            item.SubItems.Item("RemainingLogOns") = user.RemainingLogOns
        End If
        If user.Group.RenewPasswordInterval > 0 Then
            If user.RenewPassword <= 0 Then
                item.SubItems.Item("RenewPassword") = Localization.GetText(TextTypes.ClientSystem, "Dialogs.UserManagement.Expired")
            Else
                item.SubItems.Item("RenewPassword") = user.RenewPassword
            End If
        End If

        Dim dateTimeFormat As New DateTimeFormat
        dateTimeFormat.FormatDate = "@LongDate"
        dateTimeFormat.FormatTime = "@LongTime"
        dateTimeFormat.Mode = DateTimeMode.DateTime
        If user.DeactivationTime.ToOADate() <> 0 Then
            item.SubItems.Item("DeactivationTime") = dateTimeFormat.FormatDateTime(user.DeactivationTime)
        End If
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim user As User
        user = FUserChange.ShowAddDialog()
		
		If user IsNot Nothing Then
            Dim item As New VisiWinNET.Forms.ListViewItem
            lvwUsers.Items.Add(user.Name, item)
            UserToItem(user, item)
        End If
    End Sub

    Private Sub cmdChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChange.Click
        Dim user As User
        Dim item As VisiWinNET.Forms.ListViewItem
		
        item = lvwUsers.SelectedItem
        If item IsNot Nothing Then
            user = FUserChange.ShowChangeDialog(item.Key)
            If user IsNot Nothing Then
                user = UserManager.Users.Item(CType(item.Key, String))
                UserToItem(user, item)
            End If
        End If
    End Sub

    Private Sub cmdRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemove.Click
        Dim item As VisiWinNET.Forms.ListViewItem
        Dim user As User
		
        item = lvwUsers.SelectedItem
        If item IsNot Nothing Then
            user = UserManager.Users.Item(CType(item.Key, String))
            If user.Group.UsersRemovable Then
                If FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption002"), _
					Localization.GetText("@LabelForms.FMessageBox.Messages.Message011"), MessageBoxButtons.YesNo) = DialogResult.Yes Then
					
					If UserManager.Users.Remove(item.Key) Then
                        lvwUsers.Items.Remove(item)
					Else
						FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption003"), _
							Localization.GetText("@LabelForms.FMessageBox.Messages.Message014"), MessageBoxButtons.Ok)
                    End If
                End If
            Else
                If FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption002"), _
					Localization.GetText("@LabelForms.FMessageBox.Messages.Message012"), MessageBoxButtons.YesNo) = DialogResult.Yes Then
					Me.TimerDeactivated.Enabled = True
				End If
            End If
        End If
    End Sub

    Private Sub lvwUsers_SelectedIndexChanged(ByVal sender As Object, ByVal e As VisiWinNET.Forms.Internals.SelectedIndexChangedEventArgs) Handles lvwUsers.SelectedIndexChanged
        cmdRemove.Enabled = (e.Index <> -1)
        cmdChange.Enabled = (e.Index <> -1)
        cmdActivate.Enabled = (e.Index <> -1)
        cmdDeactivate.Enabled = (e.Index <> -1)
    End Sub

    Private Sub cmdActivate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdActivate.Click
        ChangeUserState(UserStates.Active)
    End Sub

    Private Sub cmdDeactivate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDeactivate.Click
        ChangeUserState(UserStates.Deactivated)
    End Sub

    Private Sub ChangeUserState(ByVal userState As UserStates)
        Dim user As User
        Dim item As VisiWinNET.Forms.ListViewItem
        Dim enmChangeUserSuccess As ChangeUserSuccess
		
        item = lvwUsers.SelectedItem
        If item IsNot Nothing Then
            user = UserManager.Users.Item(CType(item.Key, String))
            If user IsNot Nothing Then
                user.State = userState
                user.InitialPassword = vbNullString
                enmChangeUserSuccess = UserManager.Users.Change(user)
                If enmChangeUserSuccess = ChangeUserSuccess.Succeeded Then
                    user = UserManager.Users.Item(CType(item.Key, String))
                    UserToItem(user, item)
					Me.TimerChange.Enabled = True 	
				End If
            End If
        End If
    End Sub

	#Region "Initialization of VisiWinNET controls"

    Private Sub InitializeVisiWinNETComponents()

        'CType(cmdClose, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdDeactivate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdActivate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(lvwUsers, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdChange, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdAdd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdRemove, System.ComponentModel.ISupportInitialize).EndInit()

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

	
    Private Sub TimerDeactivated_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerDeactivated.Tick
        
		Me.TimerDeactivated.Enabled = False
		
		Dim item As VisiWinNET.Forms.ListViewItem
        item = lvwUsers.SelectedItem
		
		If UserManager.Users.Remove(item.Key) Then
        	ChangeUserState(UserStates.Deactivated)
			UserToItem(UserManager.Users.Item(CType(item.Key, String)), item)
    	End If
					
    End Sub

    Private Sub TimerChange_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerChange.Tick
		Me.TimerChange.Enabled = False
		FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption003"), _
			Localization.GetText("@LabelForms.FMessageBox.Messages.Message013"), MessageBoxButtons.Ok)
    End Sub

End Class