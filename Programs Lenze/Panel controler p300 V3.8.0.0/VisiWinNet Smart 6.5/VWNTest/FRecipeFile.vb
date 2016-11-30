Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms
Imports VisiWinNET.Forms.Recipe
Imports VisiWinNET.LanguageSwitching
Imports System.Xml

Public Class FRecipeFile
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
	
	#Region "Enumerations"
		Public Enum RecipeFileModes
			Load
			Save
		End Enum
	#End Region
	
	#Region "Variables"
		Dim _mode As RecipeFileModes = RecipeFileModes.Load
		Dim _fileNameBackup As String = ""
		Dim _fileDescriptionBackup As String = ""
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
		
		'ChildForm Resolution
		Me.Height = Me.Height - m_iButtonLineHeight - m_iHeaderHeight	
		
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

	Public Sub New(recipeClassHandler As RecipeClassHandler)

        InitializeComponent()

		_fileNameBackup = RecipeClassHandler.RecipeFile.FileName
		_fileDescriptionBackup = RecipeClassHandler.RecipeFile.FileDescription

        Me.RecipeClassHandler = RecipeClassHandler
        lvFiles.RecipeClassHandler = RecipeClassHandler
        lvFiles.UpdateFileView()

    End Sub
	
	Public Property DialogMode As RecipeFileModes
		Get
			Return _mode
		End Get
		Set
			_mode = value
			SetupDialog()
		End Set
	End Property
	
	Public Shared Function ShowLoadRecipeDialog(recipeClassHandler As VisiWinNET.Forms.Recipe.RecipeClassHandler) As DialogResult 
    
		Dim dlg As FRecipeFile = New FRecipeFile(recipeClassHandler)
		dlg.DialogMode = FRecipeFile.RecipeFileModes.Load

		Dim res As DialogResult = dlg.ShowDialog()
		dlg.Dispose()
		

		Return res
	End Function

	Public Shared Function ShowSaveRecipeDialog(recipeClassHandler As VisiWinNET.Forms.Recipe.RecipeClassHandler) As DialogResult 
    
		Dim dlg As FRecipeFile = New FRecipeFile(recipeClassHandler)
		dlg.DialogMode = FRecipeFile.RecipeFileModes.Save
		
		Dim res As DialogResult = dlg.ShowDialog()
		dlg.Dispose()

		Return res
	End Function
	
	Private Sub UndoChanges()
            
		If (recipeClassHandler Is Nothing)
			Return
		End If

		RecipeClassHandler.RecipeFile.FileName = _fileNameBackup
		RecipeClassHandler.RecipeFile.FileDescription = _fileDescriptionBackup
		
	End Sub
	
	Private Sub SetupDialog()

		If (_mode = RecipeFileModes.Load)
			vinFileName.Enabled = False
			cmdSaveOK.Enabled = False
            cmdLoadOK.Enabled = False
			cmdDelete.Enabled = False
			vinFileDescription.Enabled = False
		Else
			vinFileName.Enabled = True
           	cmdSaveOK.Enabled = False
            cmdLoadOK.Enabled = False
			cmdDelete.Enabled = False
			vinFileDescription.Enabled = True
		End If

        If (lvFiles.SelectedIndex > -1)
        	lvFiles_SelectedIndexChanged(Me, New VisiWinNET.Forms.Internals.SelectedIndexChangedEventArgs(lvFiles.SelectedIndex))
		End If
	
	End Sub
	
    Private Sub lvFiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As VisiWinNET.Forms.Internals.SelectedIndexChangedEventArgs) Handles lvFiles.SelectedIndexChanged
        If (lvFiles.SelectedRecipeFile IsNot Nothing)
			cmdDelete.Enabled = True
			vinFileName.Text = lvFiles.SelectedRecipeFile.FileName
			vinFileDescription.Text = lvFiles.SelectedRecipeFile.Description
		Else
			cmdDelete.Enabled = False
			vinFileName.Text = ""
			vinFileDescription.Text = ""
		End If
    End Sub

    Private Sub vinFileName_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vinFileName.KeyUp
        
		If (RecipeClassHandler Is Nothing)
        	Return
		End If

		RecipeClassHandler.RecipeFile.FileName = vinFileName.Text

		If (Not String.IsNullOrEmpty(RecipeClassHandler.RecipeFile.FileName))
            If (_mode = RecipeFileModes.Load)
        		cmdLoadOK.Enabled = True
			Else
				cmdSaveOK.Enabled = True
			End If
		Else
        	cmdLoadOK.Enabled = False
			cmdSaveOK.Enabled = False
		End If
	
    End Sub

	Private Sub cmdSaveOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveOK.Click
    	
		If (RecipeClassHandler Is Nothing)
			Return
		End If
	     
		If System.IO.File.Exists(VisiWinNET.Project.ProjectInfo.Path + "\Recipes\" + Me.vinFileName.text + ".R") Then
			
			If FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption002"), _
				Localization.GetText("@LabelForms.FMessageBox.Messages.Message021") + RecipeClassHandler.RecipeFile.FileName + _
				Localization.GetText("@LabelForms.FMessageBox.Messages.Message022"), MessageBoxButtons.YesNo) = DialogResult.Yes Then
		
				RecipeClassHandler.Save()
				DialogResult = DialogResult.OK
				Return
			End If
		
		Else
			RecipeClassHandler.Save()
			DialogResult = DialogResult.OK
			Return
		End If
	
    End Sub

	Private Sub cmdLoadOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLoadOK.Click
    	
		If (RecipeClassHandler Is Nothing)
			Return
		End If

			RecipeClassHandler.Load()

			DialogResult = DialogResult.OK
	
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        DialogResult = DialogResult.Cancel
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        
		If (RecipeClassHandler Is Nothing)
			Return
		End If
		
		If FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption002"), _
			Localization.GetText("@LabelForms.FMessageBox.Messages.Message020") + RecipeClassHandler.RecipeFile.FileName + "'?", MessageBoxButtons.YesNo) = DialogResult.Yes Then
    
			RecipeClassHandler.Delete()
			lvFiles.UpdateFileView()
			
		End If
		
		End Sub

    Private Sub Me_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        If(Not DialogResult = DialogResult.OK)
			UndoChanges()
		End If
    End Sub

    Private Sub vinFileName_Change(ByVal sender As System.Object, ByVal e As VisiWinNET.DataAccess.ChangeEventArgs) Handles vinFileName.Change
        
		If (RecipeClassHandler Is Nothing)
			Return
		End If

        If (e.Value IsNot Nothing And Not String.IsNullOrEmpty(e.Value.ToString()))
        	RecipeClassHandler.RecipeFile.FileName = e.Value.ToString()
		End If

        If (Not String.IsNullOrEmpty(recipeClassHandler.RecipeFile.FileName))
			If (_mode = RecipeFileModes.Load)
        		cmdLoadOK.Enabled = True
			Else
				cmdSaveOK.Enabled = True
			End If
		Else
        	cmdLoadOK.Enabled = False
			cmdSaveOK.Enabled = False
		End If
    End Sub

    Private Sub vinFileName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles vinFileName.TextChanged
        
		If (RecipeClassHandler Is Nothing)
			Return
		End If

        If (Not RecipeClassHandler.RecipeFile.FileName.Equals(vinFileName.Text))
		    RecipeClassHandler.RecipeFile.FileName = vinFileName.Text
		End If
	
        If (Not String.IsNullOrEmpty(RecipeClassHandler.RecipeFile.FileName))
            If (_mode = RecipeFileModes.Load)
        		cmdLoadOK.Enabled = True
			Else
				cmdSaveOK.Enabled = True
			End If
		Else
        	cmdLoadOK.Enabled = False
			cmdSaveOK.Enabled = False
		End If
	
    End Sub

    Private Sub vinFileDescription_Change(ByVal sender As System.Object, ByVal e As VisiWinNET.DataAccess.ChangeEventArgs) Handles vinFileDescription.Change
        
		If (RecipeClassHandler Is Nothing)
			Return
		End If
	
		If (e.Value IsNot Nothing And Not String.IsNullOrEmpty(e.Value.ToString()))
                RecipeClassHandler.RecipeFile.FileDescription = e.Value.ToString()
		End If
	
    End Sub

    Private Sub vinFileDescription_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles vinFileDescription.TextChanged
        
		If (RecipeClassHandler Is Nothing)
			Return
		End If
	
		RecipeClassHandler.RecipeFile.FileDescription = vinFileDescription.Text
	
    End Sub
	
End Class
