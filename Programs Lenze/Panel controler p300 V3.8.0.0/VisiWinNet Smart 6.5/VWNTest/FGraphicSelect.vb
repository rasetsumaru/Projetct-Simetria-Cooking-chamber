Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms

Imports VisiWinNET.Services 
Imports VisiWinNET.Forms
Imports VisiWinNET.Forms.Trend
Imports VisiWinNET.Trend

Imports VisiWinNET.LanguageSwitching 

Public Class FGraphicSelect
    Inherits VisiWinNET.Forms.SmartForm

    #Region "Initialization"
	
    Public Sub New()

        InitializeComponent()
		InitListView()
		
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
	
	#Region "Events"

    Private Sub Me_FormChanged(ByVal sender As System.Object, ByVal e As VisiWinNET.Forms.FormChangedEventArgs) Handles Me.FormChanged
        VisiWinNET.Services.AppService.VWSet("__FORMS.LASTACTIVEFORM", VisiWinNET.Services.AppService.VWGet("__FORMS.ACTIVEFORM"))
	    VisiWinNET.Services.AppService.VWSet("__FORMS.ACTIVEFORM", Me.Name)
    End Sub

    #End Region
	
	 Public Sub InitListView()
        Dim i As Integer = 0
        For Each lvcitem As VisiWinNET.Forms.ListViewColumn In lvwSelectedTrends.Columns
            i = i + 1
        Next
        For j As Integer = 0 To i - 1
            Me.lvwSelectedTrends.Columns.Remove(lvwSelectedTrends.Columns.Item(0))
        Next
        Dim ListViewCheckBoxColumn1 As VisiWinNET.Forms.ListViewCheckBoxColumn = New VisiWinNET.Forms.ListViewCheckBoxColumn
        Dim ListViewImageColumn1 As VisiWinNET.Forms.ListViewImageColumn = New VisiWinNET.Forms.ListViewImageColumn
        Dim ListViewTextColumn1 As VisiWinNET.Forms.ListViewTextColumn = New VisiWinNET.Forms.ListViewTextColumn
        Dim ListViewTextColumn2 As VisiWinNET.Forms.ListViewTextColumn = New VisiWinNET.Forms.ListViewTextColumn
		
        ListViewCheckBoxColumn1.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        ListViewCheckBoxColumn1.CheckSize = CType(36, Short)
        ListViewCheckBoxColumn1.ColumnType = VisiWinNET.Forms.ColumnTypes.CheckBox
        ListViewCheckBoxColumn1.Editable = True
		
        ListViewCheckBoxColumn1.HeaderText = Localization.GetText("@LabelForms.FGraphicSelect.Label002")
		ListViewCheckBoxColumn1.LocalizedHeaderText.TextType = VisiWinNET.LanguageSwitching.TextTypes.ClientApplication
        ListViewCheckBoxColumn1.Name = "colScaleVisible"
        ListViewCheckBoxColumn1.Width = 67
				
		ListViewImageColumn1.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        ListViewImageColumn1.ColumnType = VisiWinNET.Forms.ColumnTypes.Image
		ListViewImageColumn1.Width = 56
       	ListViewImageColumn1.HeaderText = Localization.GetText("@LabelForms.FGraphicSelect.Label003")
		ListViewImageColumn1.LocalizedHeaderText.TextType = VisiWinNET.LanguageSwitching.TextTypes.ClientApplication
        ListViewImageColumn1.Name = "colColor"
        
		ListViewTextColumn1.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        ListViewTextColumn1.ColumnType = VisiWinNET.Forms.ColumnTypes.Text
		ListViewTextColumn1.Width = 111
        ListViewTextColumn1.HeaderText = Localization.GetText("@LabelForms.FGraphicSelect.Label004")
		ListViewTextColumn1.LocalizedHeaderText.TextType = VisiWinNET.LanguageSwitching.TextTypes.ClientApplication
        ListViewTextColumn1.Name = "colArchive"
        
		ListViewTextColumn2.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        ListViewTextColumn2.ColumnType = VisiWinNET.Forms.ColumnTypes.Text
		ListViewTextColumn2.Width = 117
        ListViewTextColumn2.HeaderText = Localization.GetText("@LabelForms.FGraphicSelect.Label005")
		ListViewTextColumn2.LocalizedHeaderText.TextType = VisiWinNET.LanguageSwitching.TextTypes.ClientApplication
        ListViewTextColumn2.Name = "colTrend"
		
		If System.Convert.ToInt32(AppService.VWGet("Controller.Controller.Application.SETTINGS.Enabled_chambers")) = 1 Then
			ListViewTextColumn1.Visible = False
			ListViewTextColumn2.Width = 228
		Else
			ListViewTextColumn1.Visible = True
			ListViewTextColumn1.Width = 111
			ListViewTextColumn2.Width = 117
		End If

		Me.lvwSelectedTrends.Columns.AddRange(New VisiWinNET.Forms.Internals.ListViewBaseColumn() {ListViewCheckBoxColumn1, ListViewImageColumn1, ListViewTextColumn1, ListViewTextColumn2})
        Me.lvwSelectedTrends.DateTimeFormat.Mode = VisiWinNET.LanguageSwitching.DateTimeMode.DateTime
		
        Me.lvwSelectedTrends.Header.Height = 30
       	Me.lvwSelectedTrends.ItemHeight = 20
       
    End Sub

	Public Overloads Shared Function ShowDialog(ByVal trendChart As TrendChart) As DialogResult
        Return ShowDialog(trendChart, "")
    End Function

    Public Overloads Shared Function ShowDialog(ByVal trendChart As TrendChart, ByVal archiveName As String) As DialogResult
        Dim frm As New FGraphicSelect
		If VisiWinNET.Trend.TrendManager.Archives.Count = 0 Then
        	Return frm.InitDialog(trendChart, archiveName)
		Else
			Return frm.InitDialog(trendChart, "")
		End If
    End Function

	Private Function InitDialog(ByVal trendChart As TrendChart, ByVal archiveName As String) As DialogResult
        Dim ret As DialogResult

		If TrendManager.Archives.Count <> 0 Then
			' List current trends from the TrendChart.
			For Each curve As TrendCurve In trendChart.Curves
				AddSelection(lvwSelectedTrends, curve.Archive, curve.Trend, curve.LineColor, curve.Scale.Visible)
			Next
		
			' List all defined archives.
			For Each arc As Archive In TrendManager.Archives
				cmbArchives.Items.Add(IIf(arc.Text.Length > 0, arc.Text, arc.Name), arc.Name)
			Next
			
			For i As Integer = System.Convert.ToInt32(AppService.VWGet("Controller.Controller.Application.SETTINGS.Enabled_chambers"))  To 3
				cmbArchives.Items.RemoveAt(i)
			Next
		
			If System.Convert.ToInt32(AppService.VWGet("Controller.Controller.Application.SETTINGS.Enabled_chambers")) = 1 Then
				Me.lblArchives.Visible = False
				Me.cmbArchives.Visible = False
			Else
				Me.lblArchives.Visible = True
				Me.cmbArchives.Visible = True
			End If
			
			If archiveName.Length = 0 Then
				' Enable archive selection
				cmbArchives.Enabled = True
				cmbArchives.SelectedIndex = 0
			Else
				' Archive specified => disable archive selection
				cmbArchives.Enabled = False
				For i As Integer = 0 To cmbArchives.Items.Count - 1
					If cmbArchives.Items(i).Tag = archiveName Then
						' and select specified archive
						cmbArchives.SelectedIndex = i
						Exit For
					End If
				Next
				
			End If

			ret = Me.ShowDialog()

			If ret = Windows.Forms.DialogResult.OK Then
				' Write selection back into the TrendChart.
				trendChart.BeginInit()
				trendChart.Curves.Clear()
				For Each itm As VisiWinNET.Forms.ListViewItem In lvwSelectedTrends.Items
					Dim curve As TrendCurve = New TrendCurve
					Dim trd As Trend
					Dim col As Color
					Dim astr() As String
					astr = Split(itm.Key, vbNullChar)
					trd = TrendManager.Archives(astr(0)).Trends(astr(1))
					curve.Archive = trd.Archive
					curve.Trend = trd.Name
					curve.Style = TrendCurveStyles.Interpolated
					'Dim max As Double = trd.DisplayParameters.MaxValue
					'Dim min As Double = trd.DisplayParameters.MinValue
					Dim max As Double = trd.Parameters.Maximum
					Dim min As Double = trd.Parameters.Minimum
					If max = min Then
						max = 100
						min = 0
					End If
					curve.MaxValue = max
					curve.MinValue = min
					col = itm.Tag
					curve.LineColor = col
					curve.LineWidth = 3
					curve.Scale.AutoAlignment.Enabled = True
					curve.Scale.AutoAlignment.RulerSpacing = 10
					curve.Scale.UnitLayout = TrendScaleUnitLayouts.AtMaxValue
					curve.Scale.Visible = itm.Values.Item("colScaleVisible")
					'curve.Scale.DecPoint = 1
					
					If VisiWinNET.Services.AppService.VWGet("TrendSystem.Zoom") = True Then
						Dim VwItemBinding1 As VisiWinNET.Forms.VWItemBinding = New VisiWinNET.Forms.VWItemBinding
						VwItemBinding1.PropertyName = "MaxValue"
						VwItemBinding1.VWItem = "TrendSystem.Max"
						curve.PropertyBindings.AddRange(New VisiWinNET.Forms.IPropertyBinding() {VwItemBinding1})           
						Dim VwItemBinding2 As VisiWinNET.Forms.VWItemBinding = New VisiWinNET.Forms.VWItemBinding
						VwItemBinding2.PropertyName = "MinValue"
						VwItemBinding2.VWItem = "TrendSystem.Min"
						curve.PropertyBindings.AddRange(New VisiWinNET.Forms.IPropertyBinding() {VwItemBinding2})  
					End If
					trendChart.Curves.Add(curve)
				Next
				trendChart.EndInit()
			End If

			Me.Dispose()
			Return ret
		End If
    End Function

    Private Sub cmbArchives_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbArchives.SelectedIndexChanged
        FillAvailableTrends()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        If lstAvailableTrends.SelectedIndex >= 0 And cmbArchives.SelectedIndex >= 0 Then
            AddSelection(lvwSelectedTrends, _
                         cmbArchives.Items(cmbArchives.SelectedIndex).Tag, _
                         lstAvailableTrends.Items(lstAvailableTrends.SelectedIndex).Tag, _
                         GetNextColor, _
                         True)
            lstAvailableTrends.Items.RemoveAt(lstAvailableTrends.SelectedIndex)
        End If
    End Sub

    Private Sub cmdAddAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddAll.Click
        Dim itm As ListItem
        For Each itm In lstAvailableTrends.Items
            AddSelection(lvwSelectedTrends, _
                         cmbArchives.Items(cmbArchives.SelectedIndex).Tag, _
                         itm.Tag, _
                         GetNextColor, _
                         True)
        Next itm
        lstAvailableTrends.Clear()
    End Sub

    Private Sub cmdRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemove.Click
        If lvwSelectedTrends.SelectedIndex >= 0 Then
            lvwSelectedTrends.Items.RemoveAt(lvwSelectedTrends.SelectedIndex)
            FillAvailableTrends()
        End If
    End Sub

    Private Sub cmdRemoveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemoveAll.Click
        lvwSelectedTrends.Items.Clear()
        FillAvailableTrends()
    End Sub

    Private Sub lstAvailableTrends_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstAvailableTrends.DoubleClick
        'cmdAdd.PerformClick()
		If cmbArchives.SelectedIndex <> -1 And lstAvailableTrends.Items.Count > 0 Then
            AddSelection(lvwSelectedTrends, _
                         cmbArchives.Items(cmbArchives.SelectedIndex).Tag, _
                         lstAvailableTrends.Items(lstAvailableTrends.SelectedIndex).Tag, _
                         GetNextColor, _
                         True)
            lstAvailableTrends.Items.RemoveAt(lstAvailableTrends.SelectedIndex)
        End If
    End Sub

    Private Sub lvwSelectedTrends_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwSelectedTrends.DoubleClick
        cmdChangeColor.PerformClick()
    End Sub

    Private Sub cmdChangeColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChangeColor.Click
        If lvwSelectedTrends.SelectedIndex >= 0 Then
            Dim col As Color = lvwSelectedTrends.SelectedItem.Tag
            If FColorSelect.ShowDialog(col) = Windows.Forms.DialogResult.OK Then
                Dim bmp As New Bitmap(lvwSelectedTrends.Columns.Item("colColor").Width, lvwSelectedTrends.ItemHeight)
                Dim grph As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(bmp)
                grph.FillRectangle(New SolidBrush(col), 0, 0, bmp.Width, bmp.Height)
                lvwSelectedTrends.SelectedItem.SubItems.Item("colColor") = bmp
                lvwSelectedTrends.SelectedItem.Tag = col
            End If
        End If
    End Sub

    Private Sub FillAvailableTrends()
        lstAvailableTrends.Items.Clear()
        If cmbArchives.SelectedIndex >= 0 Then
            Dim s As String = cmbArchives.Items(cmbArchives.SelectedIndex).Tag
            For Each trd As Trend In TrendManager.Archives(s).Trends
                Dim itm As VisiWinNET.Forms.ListViewItem = lvwSelectedTrends.Items(s & vbNullChar & trd.Name)
                If itm Is Nothing Then
                    lstAvailableTrends.Items.Add(trd.Text, trd.Name)
                End If
            Next
        End If
    End Sub

    Private Sub AddSelection(ByVal lvw As VisiWinNET.Forms.ListView, _
                             ByVal archiveName As String, _
                             ByVal trendName As String, _
                             ByVal color As Color, _
                             ByVal visible As Boolean)
        Dim itm As New VisiWinNET.Forms.ListViewItem
        Dim bmp As New Bitmap(lvw.Columns.Item("colColor").Width, lvw.ItemHeight)
        Dim archive As Archive = TrendManager.Archives(archiveName)
        If archive IsNot Nothing Then
            Dim trend As Trend = archive.Trends(trendName)
            If trend IsNot Nothing Then
                lvw.Items.Add(archiveName & vbNullChar & trendName, itm)
                itm.SubItems.Item("colArchive") = IIf(archive.Text.Length > 0, archive.Text, archive.Name)
                itm.SubItems.Item("colTrend") = trend.Text
                itm.SubItems.Item("colScaleVisible") = visible
                Using grph As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(bmp)
                    grph.FillRectangle(New SolidBrush(color), 0, 0, bmp.Width, bmp.Height)
                End Using
                itm.SubItems.Item("colColor") = bmp
                itm.Tag = color
            End If
        End If
    End Sub

    Private Function GetNextColor() As Color
        Dim colors() As Color = _
            {Color.FromArgb(230, 0, 48), _
             Color.FromArgb(255, 240, 80), _
             Color.FromArgb(242, 146, 71), _
             Color.FromArgb(51, 177, 231), _
             Color.FromArgb(255, 0, 255), _
             Color.FromArgb(255, 255, 0), _
             Color.FromArgb(0, 0, 128), _
             Color.FromArgb(0, 128, 0), _
             Color.FromArgb(128, 0, 0), _
             Color.FromArgb(0, 128, 128), _
             Color.FromArgb(128, 0, 128), _
             Color.FromArgb(128, 128, 0)}

        For Each itm As VisiWinNET.Forms.ListViewItem In lvwSelectedTrends.Items
            Dim i As Integer = Array.IndexOf(colors, CType(itm.Tag, Color))
            If i >= 0 Then
                colors(i) = Color.Empty
            End If
        Next

        For Each col As Color In colors
            If Not col.Equals(Color.Empty) Then
                Return col
            End If
        Next

        Return Color.Black
    End Function

	Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Me.DialogResult = DialogResult.OK
		Me.Close()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.DialogResult = DialogResult.Cancel
		Me.Close()
    End Sub

End Class
