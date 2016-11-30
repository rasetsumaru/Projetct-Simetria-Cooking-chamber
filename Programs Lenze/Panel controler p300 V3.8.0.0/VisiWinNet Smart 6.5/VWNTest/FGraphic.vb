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
Imports VisiWinNET.Forms
Imports VisiWinNET.Forms.Trend
Imports VisiWinNET.Trend

Public Class FGraphic
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

    Public Sub New()

        InitializeComponent()
		InitializeForm()

		Me.Height = Me.Height - m_iButtonLineHeight - m_iHeaderHeight	
		
    End Sub

    #End Region
    
    #Region "Dispose"
    
	 Protected Overrides Sub Dispose(ByVal disposing As Boolean)
      
        MyBase.Dispose(disposing)
        
    End Sub
    
    #End Region
    
    #Region "Events"

    Private Sub Me_FormChanged(ByVal sender As System.Object, ByVal e As VisiWinNET.Forms.FormChangedEventArgs) Handles Me.FormChanged
        VisiWinNET.Services.AppService.VWSet("__FORMS.LASTACTIVEFORM", VisiWinNET.Services.AppService.VWGet("__FORMS.ACTIVEFORM"))
	    VisiWinNET.Services.AppService.VWSet("__FORMS.ACTIVEFORM", Me.Name)
    End Sub

    #End Region

	Public indexChamber As Integer 
	
	Public Sub InitializeForm()
		
		VisiWinNET.Services.AppService.VWSet("TrendSystem.Min", 0)
		VisiWinNET.Services.AppService.VWSet("TrendSystem.Max", 250)
		VisiWinNET.Services.AppService.VWSet("TrendSystem.Zoom", True)
		VisiWinNET.Services.AppService.VWSet("TrendSystem.Ticks", 60)
		
		If System.Convert.ToInt32(AppService.VWGet("Controller.Controller.Application.SETTINGS.Enabled_chambers")) > 1 Then
			indexChamber = System.Convert.ToInt32(AppService.VWGet("ProcessSystem.ChamberIndex"))
		
		Else
			indexChamber = 0
			
			Me.TrendLegend.Columns.Item(0).Visible = False
			Me.TrendLegend.Columns.Item(1).Width = 290
			
		End If
		
		AppService.VWSet("TrendSystem.ShowSettings", False)
		showSettings()
	
		
	End Sub

    Private Sub TrendChart_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrendChart.DoubleClick
        
		FGraphicSelect.ShowDialog(TrendChart, "DataFunctions")
    
	End Sub

 	Private Sub cmd1Day_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd1Day.Click
		VisiWinNET.Services.AppService.VWSet("TrendSystem.Ticks", 60)
		Dim day As New TimeSpan(1, 0, 0, 0)
        TrendChart.TimeScale.Resolution = day	
	End Sub

    Private Sub cmd1Hour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd1Hour.Click
		VisiWinNET.Services.AppService.VWSet("TrendSystem.Ticks", 60)
        Dim hour As New TimeSpan(1, 0, 0)
        TrendChart.TimeScale.Resolution = hour		
    End Sub

    Private Sub cmd1Minute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd1Minute.Click
		VisiWinNET.Services.AppService.VWSet("TrendSystem.Ticks", 60)
        Dim minute As New TimeSpan(0, 1, 0)
		TrendChart.TimeScale.Resolution = minute	
    End Sub

    Private Sub cmd5Minutes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd5Minutes.Click
		VisiWinNET.Services.AppService.VWSet("TrendSystem.Ticks", 60)
        Dim fiveminutes As New TimeSpan(0, 5, 0)
		TrendChart.TimeScale.Resolution = fiveminutes	
    End Sub


	Private HelpTime As Date = System.DateTime.Now
	
	Private SmallChange As System.TimeSpan = New System.TimeSpan(0, 0, 5)
	Private LargeChange As System.TimeSpan = New System.TimeSpan(0, 1, 0)

    Private Sub ItemServerTrendTime_Change(ByVal sender As System.Object, ByVal e As VisiWinNET.DataAccess.ChangeEventArgs) Handles ItemServerTrendTime.Change
        
		If e.Source = VisiWinNET.DataAccess.ChangeSources.Application Or e.Source = VisiWinNET.DataAccess.ChangeSources.ValueProperty Then
		
			With TrendChart.TimeScale
				
				Select Case ItemServerTrendTime.VWItem.Value 
					Case 34 
						.Online = False
						HelpTime = HelpTime - LargeChange
						.To = HelpTime 
					Case 36
						.Online = False
						HelpTime = HelpTime - SmallChange
						.To = HelpTime
					Case 40 
						.Online = False
						HelpTime = HelpTime + SmallChange
						.To = HelpTime
					Case 48
						.Online = False
						HelpTime = HelpTime + LargeChange
						.To = HelpTime
					Case 32
						.Online = False
					Case 0 
						.Online = True
						HelpTime  = System.DateTime.Now
						.To = HelpTime
					Case Else
				
				End Select
			
			End With
		
		End If
    
	End Sub


	
    Private Sub ExtendedSliderResolution_Change(ByVal sender As System.Object, ByVal e As VisiWinNET.DataAccess.AnalogChangeEventArgs) Handles ExtendedSliderResolution.Change
        
		If e.Cause = VisiWinNET.DataAccess.ChangedBy.Process Or e.Cause = VisiWinNET.DataAccess.ChangedBy.ValueProperty Then
		
			Dim calculatedValue As Double
			Dim success As Boolean = ExtendedSliderResolution.VWItem.CalculateValue(ExtendedSliderResolution.VWItem.Value, calculatedValue)
            Dim resolution As New TimeSpan(calculatedValue * 10000000)
            
			TrendChart.TimeScale.Resolution = resolution
		
		End If
    
	End Sub

	Private Archives() As String

    Private Sub Me_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
		
		VisiWinNET.Services.AppService.VWSet("TrendSystem.ActivateTrend", True)
		VisiWinNET.Services.AppService.VWSet("TrendSystem.Min", 0)
		VisiWinNET.Services.AppService.VWSet("TrendSystem.Max", 250)
   
	 	ReDim Archives(VisiWinNET.Trend.TrendManager.Archives.Count - 1)
        
		Dim j As Integer = 0
        Dim trd As VisiWinNET.Trend.Archive
        
		For Each trd In VisiWinNET.Trend.TrendManager.Archives
            Archives(j) = trd.Text
            j = j + 1
        Next	
	
	End Sub

	   Private Sub cmdOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOptions.Click
        
		If Convert.ToBoolean(AppService.VWGet("TrendSystem.ShowSettings")) = True Then
			AppService.VWSet("TrendSystem.ShowSettings", False)
		Else
			AppService.VWSet("TrendSystem.ShowSettings", True)
		End If
	
		showSettings()
		
    End Sub

	Private Sub showSettings()
		
		Dim state As Boolean = Convert.ToBoolean(AppService.VWGet("TrendSystem.ShowSettings"))
		
		Me.cmd1Minute.Visible = state
		Me.cmd5Minutes.Visible = state
		Me.cmd1Hour.Visible = state
		Me.cmd1Day.Visible = state
		Me.ExtendedSliderResolution.Visible = state
		Me.ExtendedSliderMax.Visible = state
		Me.ExtendedSliderMin.Visible = state
		
	End Sub	

End Class
