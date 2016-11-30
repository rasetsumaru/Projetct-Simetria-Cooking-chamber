Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms

Imports System.Xml
Imports VisiWinNET.Forms

Public Class FColorSelect
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
	
    Private Sub New()

        InitializeComponent()

    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If components IsNot Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private _cmdSelection As CommandButton = Nothing

    Public Overloads Shared Function ShowDialog(ByRef color As Color) As DialogResult
        Dim frm As New FColorSelect
        Dim ret As DialogResult

        If color.IsEmpty Then
            color = Drawing.Color.Black
            frm._cmdSelection = frm.cmdIndividualColor
        End If

        frm.vinRed.VWItem.Value = color.R
        frm.vinGreen.VWItem.Value = color.G
        frm.vinBlue.VWItem.Value = color.B
        frm.cmdIndividualColor.BackColor = color
        For Each ctr As Control In frm.grp.Controls
            If TypeOf ctr Is VisiWinNET.Forms.CommandButton Then
                If ctr.BackColor.ToArgb() = color.ToArgb() And frm._cmdSelection Is Nothing Then
                    frm._cmdSelection = ctr
                End If
            End If
        Next
        frm.SelectCommandButton(frm._cmdSelection)

        ret = frm.ShowDialog()
        If ret = System.Windows.Forms.DialogResult.OK Then
            color = frm._cmdSelection.BackColor
        End If

        frm.Dispose()
        Return ret
    End Function

    Private Sub SelectCommandButton(ByVal commandButton As CommandButton)
        ' Remember the selected button
        _cmdSelection = commandButton
        ' and mark with a selection border.
        pbxSelection.Left = _cmdSelection.Left - 4
        pbxSelection.Top = _cmdSelection.Top - 4
    End Sub

    Private Sub cmd_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles _
        cmdIndividualColor.Click, _
        cmdColor1.Click, cmdColor2.Click, cmdColor3.Click, cmdColor4.Click, cmdColor5.Click, cmdColor6.Click, cmdColor7.Click, cmdColor8.Click, _
        cmdColor9.Click, cmdColor10.Click, cmdColor11.Click, cmdColor12.Click, cmdColor13.Click, cmdColor14.Click, cmdColor15.Click, cmdColor16.Click, _
        cmdColor17.Click, cmdColor18.Click, cmdColor19.Click, cmdColor20.Click, cmdColor21.Click, cmdColor22.Click, cmdColor23.Click, cmdColor24.Click, _
        cmdColor25.Click, cmdColor26.Click, cmdColor27.Click, cmdColor28.Click, cmdColor29.Click, cmdColor30.Click, cmdColor31.Click, cmdColor32.Click, _
        cmdColor33.Click, cmdColor34.Click, cmdColor35.Click, cmdColor36.Click, cmdColor37.Click, cmdColor38.Click, cmdColor39.Click, cmdColor40.Click, _
        cmdColor41.Click, cmdColor42.Click, cmdColor43.Click, cmdColor44.Click, cmdColor45.Click, cmdColor46.Click, cmdColor47.Click, cmdColor48.Click, _
        cmdColor49.Click, cmdColor50.Click, cmdColor51.Click, cmdColor52.Click, cmdColor53.Click, cmdColor54.Click, cmdColor55.Click, cmdColor56.Click, _
        cmdColor57.Click, cmdColor58.Click, cmdColor59.Click, cmdColor60.Click, cmdColor61.Click, cmdColor62.Click, cmdColor63.Click, cmdColor64.Click, _
        cmdColor65.Click, cmdColor66.Click, cmdColor67.Click, cmdColor68.Click, cmdColor69.Click, cmdColor70.Click, cmdColor71.Click, cmdColor72.Click, _
        cmdColor73.Click, cmdColor74.Click, cmdColor75.Click

        SelectCommandButton(sender)
    End Sub

    Private Sub vin_Change(ByVal sender As System.Object, ByVal e As VisiWinNET.DataAccess.ChangeEventArgs) _
        Handles vinRed.Change, vinGreen.Change, vinBlue.Change

        cmdIndividualColor.BackColor = Color.FromArgb(CByte(vinRed.VWItem.Value), CByte(vinGreen.VWItem.Value), CByte(vinBlue.VWItem.Value))
        If e.Source = VisiWinNET.DataAccess.ChangeSources.StopEdit Then
            SelectCommandButton(cmdIndividualColor)
        End If
    End Sub

End Class

