Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms
Imports VisiWinNET.Services
Imports System.IO
Imports System.Xml

Imports VisiWinNET.LanguageSwitching 

Public Class FRecipes
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
		CheckRecipeFileCount()
		RecipeClassHandler.StartEdit()
			
        Dim recipeClass As VisiWinNET.Recipe.RecipeClass = RecipeClassHandler.GetRecipeClassObject()
        If recipeClass IsNot Nothing Then
            AddHandler recipeClass.LoadDone, AddressOf OnLoadDone
        End If
        
		If AppService.VWget("RecipeSystem.RecipesInit") = False Then
			AppService.VWSet("RecipeSystem.RecipesInit", True)
			RecipeClassHandler.Get()
		End If
	
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

    Public Sub OnLoadDone(ByVal sender As Object, ByVal e As VisiWinNET.Recipe.LoadRecipeEventArgs)

		AppService.VWSet("RecipeSystem.RecipesChange", True)
		Me.cmdSetRecipe.Enabled = True
		Me.cmdChange.Enabled = True
		ChangeControl()
		StepsControl()
        
		If e.Result = VisiWinNET.Recipe.LoadRecipeResults.Succeeded Then
            
			FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption000"), _
				Localization.GetText("@LabelForms.FMessageBox.Messages.Message023"), MessageBoxButtons.OK)
			
            AppService.VWSet("RecipeSystem.LoadRecipeName", RecipeClassHandler.RecipeFile.FileName)
			AppService.VWSet("RecipeSystem.LoadRecipeDescription", RecipeClassHandler.RecipeFile.FileDescription)
		
		ElseIf e.Result = VisiWinNET.Recipe.LoadRecipeResults.LoadedWithErrors Then
            
			FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption000"), _
				Localization.GetText("@LabelForms.FMessageBox.Messages.Message033"), MessageBoxButtons.OK)
			
            AppService.VWSet("RecipeSystem.LoadRecipeName", RecipeClassHandler.RecipeFile.FileName)
			AppService.VWSet("RecipeSystem.LoadRecipeDescription", RecipeClassHandler.RecipeFile.FileDescription)

		End If
    End Sub

	Private Sub RecipeClassHandler_LoadDoneFailed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecipeClassHandler.LoadDoneFailed
		
		FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption000"), _
			Localization.GetText("@LabelForms.FMessageBox.Messages.Message027"), MessageBoxButtons.OK)
		
    End Sub

	Private Sub RecipeClassHandler_SaveDoneFailed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecipeClassHandler.SaveDoneFailed
        
		FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption000"), _
			Localization.GetText("@LabelForms.FMessageBox.Messages.Message029"), MessageBoxButtons.OK)

    End Sub

    Private Sub RecipeClassHandler_SaveDoneSucceeded(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecipeClassHandler.SaveDoneSucceeded

		AppService.VWSet("RecipeSystem.RecipesChange", True)
		Me.cmdSetRecipe.Enabled = True
		Me.cmdChange.Enabled = True
		ChangeControl()
		StepsControl()
		
		FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption000"), _
			Localization.GetText("@LabelForms.FMessageBox.Messages.Message025"), MessageBoxButtons.OK)

		AppService.VWSet("RecipeSystem.LoadRecipeName", RecipeClassHandler.RecipeFile.FileName)
		AppService.VWSet("RecipeSystem.LoadRecipeDescription", RecipeClassHandler.RecipeFile.FileDescription)

		CheckRecipeFileCount()
    End Sub

    Private Sub RecipeClassHandler_FileDeleted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecipeClassHandler.FileDeleted
		CheckRecipeFileCount()
    End Sub

    Private Sub RecipeClassHandler_SetDoneSucceeded(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecipeClassHandler.SetDoneSucceeded

		AppService.VWSet("Controller.Controller.Application.GLOBAL.Program_recipe_chamber", System.Convert.ToInt32(AppService.VWGet("ProcessSystem.ChamberIndex")) + 1)
		
		If System.Convert.ToInt32(AppService.VWGet("Controller.Controller.Application.SETTINGS.Enabled_chambers")) = 1 Then
			FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption000"), _
				Localization.GetText("@LabelForms.FMessageBox.Messages.Message026") + ".", MessageBoxButtons.OK)
		Else
			FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption000"), _
				Localization.GetText("@LabelForms.FMessageBox.Messages.Message026") + " 0" + (Convert.ToInt32(AppService.VWGet("ProcessSystem.ChamberIndex")) + 1).ToString() + ".", MessageBoxButtons.OK)
		End If

		AppService.VWSet("ProcessSystem.ChamberIndex", 0)
		
    End Sub

	Private Sub RecipeClassHandler_SetDoneFailed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecipeClassHandler.SetDoneFailed

		FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption000"), _
			Localization.GetText("@LabelForms.FMessageBox.Messages.Message030"), MessageBoxButtons.OK)

    End Sub

	Private Sub RecipeClassHandler_GetDoneSucceeded(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecipeClassHandler.GetDoneSucceeded
		
		Me.cmdChange.Enabled = False
		Me.cmdSetRecipe.Enabled = False
		AppService.VWset("RecipeSystem.RecipesChange", False)
		ChangeControl()
		StepsControl()

    End Sub

    Private Sub RecipeClassHandler_GetDoneFailed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecipeClassHandler.GetDoneFailed
		
		FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption000"), _
			Localization.GetText("@LabelForms.FMessageBox.Messages.Message028"), MessageBoxButtons.OK)

    End Sub
	
    Private Sub cmdLoadRecipe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLoadRecipe.Click
		FRecipeFile.ShowLoadRecipeDialog(Me.RecipeClassHandler)
    End Sub

	Private Sub cmdSaveRecipeAs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveRecipeAs.Click
		FRecipeFile.ShowSaveRecipeDialog(Me.RecipeClassHandler)
    End Sub

    Private Sub cmdSetRecipe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSetRecipe.Click
		
		Dim Chamber As String = AppService.VWGet("ProcessSystem.ChamberIndex").toString()
	
		If System.Convert.ToInt32(AppService.VWGet("Controller.Controller.Application.SETTINGS.Enabled_chambers")) > 1 Then
			If FSelectBox.Show(Localization.GetText("@LabelForms.FSelectBox.Labels.Label000")) = DialogResult.ok Then
				
				If System.Convert.ToInt32(AppService.VWGet("Controller.Controller.Application.GLOBAL.Program_chamber_status<[" + Chamber + "]>")) > 0 Then
				
					FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption003"), _
						Localization.GetText("@LabelForms.FMessageBox.Messages.Message039") + " 0" + Chamber + ".", MessageBoxButtons.OK)
						
				Else
					
					RecipeClassHandler.Set()
					
					Select Case System.Convert.ToInt32(AppService.VWGet("ProcessSystem.ChamberIndex"))
						Case Is = 0
							AppService.VWSet("Controller.Controller.Application.GLOBAL.Program_chamber<[0]>", True)
						Case Is = 1
							AppService.VWSet("Controller.Controller.Application.GLOBAL.Program_chamber<[1]>", True)
						Case Is = 2
							AppService.VWSet("Controller.Controller.Application.GLOBAL.Program_chamber<[2]>", True)
					End Select 
			
					AppService.VWSet("Controller.Controller.Application.RECIPES.Transfer_name", Me.varRecipename.Text)
					AppService.VWSet("Controller.Controller.Application.RECIPES.Transfer_description", Me.VarDescription.Text)
					
				End If
			End If
		Else
			
			If System.Convert.ToInt32(AppService.VWGet("Controller.Controller.Application.GLOBAL.Program_chamber_status<[" + Chamber + "]>")) > 0 Then
				
					FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption003"), _
						Localization.GetText("@LabelForms.FMessageBox.Messages.Message039") + ".", MessageBoxButtons.OK)
						
				Else
				
					RecipeClassHandler.Set()
					AppService.VWSet("Controller.Controller.Application.GLOBAL.Program_chamber<[0]>", True)
					AppService.VWSet("Controller.Controller.Application.RECIPES.Transfer_name", Me.varRecipename.Text)
					AppService.VWSet("Controller.Controller.Application.RECIPES.Transfer_description", Me.VarDescription.Text)
					
				End If
		End If
	
    End Sub
	
	Private Sub CheckRecipeFileCount()
		
		Dim recipeClass As VisiWinNET.Recipe.RecipeClass = RecipeClassHandler.GetRecipeClassObject()
		
		If Not recipeClass Is Nothing Then 
			If recipeClass.MaxFiles > 0 And recipeClass.Files.Count >= recipeClass.MaxFiles Then

				FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption000"), _
					Localization.GetText("@LabelForms.FMessageBox.Messages.Message032"), MessageBoxButtons.OK)

				cmdSaveRecipeAs.Enabled = False
			Else
				cmdSaveRecipeAs.Enabled = True
			End If
		End If	
	End Sub
	
    Private Sub VarInSteps_ChangedByEditing(ByVal sender As System.Object, ByVal e As VisiWinNET.DataAccess.ChangeEventArgs) Handles VarInSteps.ChangedByEditing
        StepsControl()
    End Sub

	Private Sub ChangeControl()
		
		Dim listOfTemperature() As VisiWinNET.Forms.VarIn = {Me.VarInCycle00Temperature, Me.VarInCycle01Temperature, Me.VarInCycle02Temperature, Me.VarInCycle03Temperature, Me.VarInCycle04Temperature, _ 
											Me.VarInCycle05Temperature, Me.VarInCycle06Temperature, Me.VarInCycle07Temperature, Me.VarInCycle08Temperature, Me.VarInCycle09Temperature}
		
		Dim listOfTimer() As VisiWinNET.Forms.VarIn = {Me.VarInCycle00Timer, Me.VarInCycle01Timer, Me.VarInCycle02Timer, Me.VarInCycle03Timer, Me.VarInCycle04Timer, _
											Me.VarInCycle05Timer, Me.VarInCycle06Timer, Me.VarInCycle07Timer, Me.VarInCycle08Timer, Me.VarInCycle09Timer}
		
									
		Dim listOfHumidity() As VisiWinNET.Forms.VarIn = {Me.VarInCycle00Humidity, Me.VarInCycle01Humidity, Me.VarInCycle02Humidity, Me.VarInCycle03Humidity, Me.VarInCycle04Humidity, _
											Me.VarInCycle05Humidity, Me.VarInCycle06Humidity, Me.VarInCycle07Humidity, Me.VarInCycle08Humidity, Me.VarInCycle09Humidity}
											
		Dim listOfChimney() As VisiWinNET.Forms.Switch ={Me.switchCycle00Chimney, Me.switchCycle01Chimney, Me.switchCycle02Chimney, Me.switchCycle03Chimney, Me.switchCycle04Chimney, _
											Me.switchCycle05Chimney, Me.switchCycle06Chimney, Me.switchCycle07Chimney, Me.switchCycle08Chimney, Me.switchCycle09Chimney}
		
		Dim listOfSmokehouse() As VisiWinNET.Forms.Switch = {Me.SwitchCycle00Smokehouse, Me.SwitchCycle01Smokehouse, Me.SwitchCycle02Smokehouse, Me.SwitchCycle03Smokehouse, Me.SwitchCycle04Smokehouse, _
											Me.SwitchCycle05Smokehouse, Me.SwitchCycle06Smokehouse, Me.SwitchCycle07Smokehouse, Me.SwitchCycle08Smokehouse, Me.SwitchCycle09Smokehouse}
	
		
		Dim change As Boolean = Not AppService.VWget("RecipeSystem.RecipesChange")

		Me.VarInCycles.Enabled = change
		Me.VarInSteps.Enabled=change
		Me.VarInCycleCoreTemperature.Enabled = change
		
		For i As Integer = 0 To 9
			listOfTemperature(i).Enabled = change
			listOfTimer(i).Enabled = change
			listOfHumidity(i).Enabled = change
			listOfChimney(i).Enabled = change
			listOfSmokehouse(i).Enabled = change
		Next

		Me.VarRecipename.Visible = Not change
		Me.VarDescription.Visible = Not change

	End Sub

	Private Sub StepsControl()
		
		Dim listOfTemperature() As VisiWinNET.Forms.VarIn = {Me.VarInCycle00Temperature, Me.VarInCycle01Temperature, Me.VarInCycle02Temperature, Me.VarInCycle03Temperature, Me.VarInCycle04Temperature, _ 
											Me.VarInCycle05Temperature, Me.VarInCycle06Temperature, Me.VarInCycle07Temperature, Me.VarInCycle08Temperature, Me.VarInCycle09Temperature}
		
		Dim listOfTimer() As VisiWinNET.Forms.VarIn = {Me.VarInCycle00Timer, Me.VarInCycle01Timer, Me.VarInCycle02Timer, Me.VarInCycle03Timer, Me.VarInCycle04Timer, _
											Me.VarInCycle05Timer, Me.VarInCycle06Timer, Me.VarInCycle07Timer, Me.VarInCycle08Timer, Me.VarInCycle09Timer}
		
									
		Dim listOfHumidity() As VisiWinNET.Forms.VarIn = {Me.VarInCycle00Humidity, Me.VarInCycle01Humidity, Me.VarInCycle02Humidity, Me.VarInCycle03Humidity, Me.VarInCycle04Humidity, _
											Me.VarInCycle05Humidity, Me.VarInCycle06Humidity, Me.VarInCycle07Humidity, Me.VarInCycle08Humidity, Me.VarInCycle09Humidity}
											
		Dim listOfChimney() As VisiWinNET.Forms.Switch ={Me.switchCycle00Chimney, Me.switchCycle01Chimney, Me.switchCycle02Chimney, Me.switchCycle03Chimney, Me.switchCycle04Chimney, _
											Me.switchCycle05Chimney, Me.switchCycle06Chimney, Me.switchCycle07Chimney, Me.switchCycle08Chimney, Me.switchCycle09Chimney}
		
		Dim listOfSmokehouse() As VisiWinNET.Forms.Switch = {Me.SwitchCycle00Smokehouse, Me.SwitchCycle01Smokehouse, Me.SwitchCycle02Smokehouse, Me.SwitchCycle03Smokehouse, Me.SwitchCycle04Smokehouse, _
											Me.SwitchCycle05Smokehouse, Me.SwitchCycle06Smokehouse, Me.SwitchCycle07Smokehouse, Me.SwitchCycle08Smokehouse, Me.SwitchCycle09Smokehouse}
	
			
		If AppService.VWGet("Controller.Controller.Application.SETTINGS.Temperature_control_system") = False Then
			For i As Integer = 0 To 9
				listOfHumidity(i).Visible = True
				listOfTemperature(i).location = New Point(23, listOfTemperature(i).location.y)
				listOfTimer(i).Location = New Point (220, listOfTimer(i).Location.Y)
			Next
			
			LblHumidity.Visible = False
			LblTemperature.Location = New Point(23, LblTemperature.Location.Y)
			LblTime.Location = New Point(206, LblTime.Location.Y)	
		End If
	
		If Convert.ToInt32(Me.VarInSteps.text) > 0 Then
			For i As Integer = 1 To 9
				listOfTemperature(i).Visible = False
				listOfTimer(i).Visible = False
				listOfHumidity(i).Visible = False
				listOfChimney(i).Visible = False
				listOfSmokehouse(i).Visible = False
			Next

			For i As Integer = 1 To Convert.ToInt32(Me.VarInSteps.text) -1
				listOfTemperature(i).Visible = True
				listOfTimer(i).Visible = True
				listOfHumidity(i).Visible = True
				listOfChimney(i).Visible = True
				listOfSmokehouse(i).Visible = True
			Next
		End If
	
		If AppService.VWGet("Controller.Controller.Application.SETTINGS.Temperature_control_system") = True Then
			For i As Integer = 0 To 9
				listOfHumidity(i).Visible = False
				listOfTemperature(i).location = New Point(122, listOfTemperature(i).location.y)
				listOfTimer(i).Location = New Point (320, listOfTimer(i).Location.Y)
			Next
			
			LblHumidity.Visible = False
			LblTemperature.Location = New Point(122, LblTemperature.Location.Y)
			LblTime.Location = New Point(306, LblTime.Location.Y)	
		End If
	
	End Sub

    Private Sub cmdChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChange.Click
        
		If FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption002"), _
			Localization.GetText("@LabelForms.FMessageBox.Messages.Message024"), MessageBoxButtons.YesNo) = DialogResult.Yes
		
			AppService.VWSet("RecipeSystem.RecipesChange", False)
			Me.cmdChange.Enabled = False
			Me.cmdSetRecipe.Enabled = False
			ChangeControl()
		
		End If
	
    End Sub

End Class
