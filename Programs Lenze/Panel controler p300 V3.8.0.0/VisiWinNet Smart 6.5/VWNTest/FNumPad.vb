
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Globalization
Imports VisiWinNET.Forms
Imports VisiWinNET.DataAccess
Imports VisiWinNET.LanguageSwitching

Partial Public Class FNumPad
    Inherits VisiWinNET.Forms.SmartForm
    Implements IInputPad

#Region "Konstruktor, Dispose"

    Public Sub New()
        ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        InitializeComponent()
        InitializeVisiWinNETComponents()
        'Me.Size = grpBorder.Size
		' VWN 6.5.3
		Me.ClientSize = grpBorder.Size

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    End Sub

    ' <summary>
    ' Verwendete Ressourcen bereinigen.
    ' </summary>
    ' <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If components IsNot Nothing Then
                components.Dispose()
            End If
            _Control = Nothing
        End If
        MyBase.Dispose(disposing)
    End Sub

#End Region

#Region "Lokale Variablen"

    Private _bModal As Boolean = False
    Private _Control As Control = Nothing

#End Region

#Region "Überschreibungen der Basisklasse"

    Protected Overloads Overrides Sub OnDeactivate(ByVal e As EventArgs)
        MyBase.OnDeactivate(e)

        If Not _bModal Then
            Me.Hide()
        Else
			Me.DialogResult = DialogResult.OK
        End If

        Dim varIn As VarIn = TryCast(_Control, VarIn)
        If varIn IsNot Nothing AndAlso varIn.WriteOnReturn AndAlso varIn.VWItem.EditState = EditStates.Editing Then
            varIn.StopEdit(False)
        End If

        _Control = Nothing
    End Sub

#End Region

#Region "Implementierung IInputPad Schnittstelle"

    Public Overloads Sub Show(ByVal editControl As Control, ByVal position As TouchKeyboardPosition) Implements IInputPad.Show
        _bModal = False
        Initialize(editControl, position)
        Me.Show()
    End Sub

    Public Overloads Sub ShowDialog(ByVal editControl As Control, ByVal position As TouchKeyboardPosition) Implements IInputPad.ShowDialog
        _bModal = True
        Initialize(editControl, position)
        Me.ShowDialog()
    End Sub

    Public Overloads Sub Scale(ByVal ratio As Single) Implements IInputPad.Scale
        ' Bei einem Skalierungsfaktor != 1 müssen das Formular und alle Steuerelemente skaliert weden
        If ratio <> 1.0F Then
            ScaleControl(Me, ratio)
        End If
    End Sub

    Public Overloads Sub Dispose() Implements IInputPad.Dispose
        MyBase.Dispose()
    End Sub

    Public Overloads Property Location() As System.Drawing.Point Implements IInputPad.Location
        Get
            Return MyBase.Location
        End Get
        Set(ByVal Value As System.Drawing.Point)
            MyBase.Location = Value
        End Set
    End Property

#End Region

#Region "numEdit Ereignisse"

    Private Sub numEdit_Limit(ByVal sender As Object, ByVal e As VisiWinNET.Forms.Internals.NumEditLimitEventArgs) Handles numEdit.Limit
        cmdEnter.Enabled = False

        If e.Direction < 0 Then
            'vouMaxValue.DataBackColor = System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(200, Byte), Integer))
            'vouMinValue.DataBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(80, Byte), Integer))
        
			vouMaxValue.ForeColor = System.Drawing.Color.SteelBlue 
			vouMinValue.ForeColor = System.Drawing.Color.Red
			
			Else
            'vouMinValue.DataBackColor = System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(200, Byte), Integer))
            'vouMaxValue.DataBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(80, Byte), Integer))
        
			vouMinValue.ForeColor = System.Drawing.Color.SteelBlue 
			vouMaxValue.ForeColor = System.Drawing.Color.Red 
		End If
    End Sub

    Private Sub numEdit_ValidInput(ByVal sender As Object, ByVal e As System.EventArgs) Handles numEdit.ValidInput
        'vouMinValue.DataBackColor = System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(200, Byte), Integer))
        'vouMaxValue.DataBackColor = System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(200, Byte), Integer))
        
		vouMaxValue.ForeColor = System.Drawing.Color.SteelBlue 
		vouMinValue.ForeColor = System.Drawing.Color.SteelBlue 
		
		cmdEnter.Enabled = True
    End Sub

    Private Sub numEdit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles numEdit.KeyDown
        Dim varIn As VarIn = TryCast(_Control, VarIn)

		If (_Control IsNot Nothing) Then
			If e.KeyData = Keys.Enter Then
				_Control.Text = numEdit.Text
				If varIn IsNot Nothing AndAlso varIn.WriteOnReturn Then
					varIn.StopEdit(True)
				End If

				If _bModal Then
					Me.DialogResult = DialogResult.OK
				Else
					Me.Hide()
				End If
			End If

			If e.KeyData = Keys.Escape Then
				If _bModal Then
					Me.DialogResult = DialogResult.Cancel
				Else
					Me.Hide()
				End If
			End If
		End If
    End Sub

    Private Sub numEdit_EditStateChange(ByVal sender As Object, ByVal e As VisiWinNET.DataAccess.EditStateChangeEventArgs) Handles numEdit.EditStateChange
        Dim varIn As VarIn = TryCast(_Control, VarIn)
        If varIn IsNot Nothing AndAlso numEdit.IsEditing() AndAlso varIn.VWItem.EditState <> EditStates.Editing Then
            varIn.VWItem.StartEdit()
        End If
    End Sub

#End Region

#Region "Tasten-Ereignisse"

    Private Sub cmdNumber_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
    cmd0.Click, cmd1.Click, cmd2.Click, cmd3.Click, cmd4.Click, cmd5.Click, cmd6.Click, cmd7.Click, cmd8.Click, cmd9.Click
        numEdit.SendKey(Convert.ToChar(DirectCast(sender, CommandButton).Text))
    End Sub

    Private Sub cmdSign_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSign.Click
        numEdit.SendKey("-"c)
    End Sub

    Private Sub cmdDecSep_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDecSep.Click
        numEdit.SendKey(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator(0))
    End Sub

    Private Sub cmdEnter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdEnter.Click
        numEdit.SendKey(Keys.Enter)
    End Sub

    Private Sub cmdEsc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdEsc.Click
        If _bModal Then
            Me.DialogResult = DialogResult.Cancel
        Else
            Me.Hide()
        End If
    End Sub

    Private Sub cmdExponent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdExponent.Click
        numEdit.SendKey("E"c)
    End Sub

    Private Sub cmdDel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDel.Click
        numEdit.SendKey(Keys.Delete)
    End Sub

    Private Sub cmdEnd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdEnd.Click
        numEdit.SendKey(Keys.[End])
    End Sub

    Private Sub cmdHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdHome.Click
        numEdit.SendKey(Keys.Home)
    End Sub

    Private Sub cmdBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        numEdit.SendKey(Chr(8))
    End Sub

    Private Sub cmdLeft_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLeft.Click
        numEdit.SendKey(Keys.Left)
    End Sub

    Private Sub cmdRight_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRight.Click
        numEdit.SendKey(Keys.Right)
    End Sub

    Private Sub cmdClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdClear.Click
        numEdit.Text = ""
    End Sub

#End Region

#Region "Hilfsmethoden"

    Private Sub Initialize(ByVal editControl As Control, ByVal position As TouchKeyboardPosition)
        ' Übergebenes Steuerelement übernehmen
        _Control = editControl
        Dim varIn As VarIn = TryCast(_Control, VisiWinNET.Forms.VarIn)

        Dim format As Format = format.Number
        Dim decPoint As Integer = 2
        Dim limitCheck As LimitCheckMode = LimitCheckMode.None
        Dim dblMin As Double = 0
        Dim dblMax As Double = 0
        Dim value As Object = 0
        Dim dataType As DataTypes = DataTypes.VT_R8
        Dim caption As String = Localization.GetText("@TouchKeyboards.NumPad.Caption")
        Dim unit As String = ""

        ' Falls das editierte Steuerelement ein VarIn ist, können sämtliche Einstellungen
        ' für das NumPad aus dem VarIn übernommen werden.
        If varIn IsNot Nothing Then
            format = varIn.Format
            decPoint = varIn.VWItem.CalculatedDecPoint
            limitCheck = varIn.LimitCheck

            If Not String.IsNullOrEmpty(varIn.Label.DisplayText) Then
                caption = varIn.Label.DisplayText
            ElseIf Not String.IsNullOrEmpty(varIn.VWItem.Text) Then
                caption = varIn.VWItem.Text
            End If

            unit = varIn.Unit.Text.DisplayText
            dataType = varIn.VWItem.DataType
            value = varIn.VWItem.EditableState

            ' Die Grenzwerte müssen für das NumPad allerdings neu berechnet werden,
            ' da das NumPad intern mit einem NumEdit Control arbeitet, das fest auf den
            ' Datentyp VT_R8 eingestellt ist
            If varIn.LimitCheck = LimitCheckMode.None Then
                ' Die Datentypgrenzen des VarIn-Datentyps gelten auf jeden Fall
                dblMin = GetDataTypeMinValue(dataType)
                dblMax = GetDataTypeMaxValue(dataType)
            Else
                ' Falls jemand die Grenzen vorgegeben hat, 
                ' müssen die projektierten Werte aus dem VarIn übernommen werden
                dblMin = varIn.LimitMin.Value
                dblMax = varIn.LimitMax.Value
            End If

            ' Einheitenumschaltung berücksichtigen
            If varIn.VWItem.UnitConversion Then
                Dim min As Object = 0
                Try
                    varIn.VWItem.CalculateValue(dblMin, min)
                Catch
                    min = GetDataTypeMinValue(dataType)
                End Try
                dblMin = Convert.ToDouble(min)
                If Double.IsInfinity(dblMin) Then
                    dblMin = Double.MinValue
                End If

                Dim max As Object = 0
                Try
                    varIn.VWItem.CalculateValue(dblMax, max)
                Catch
                    max = GetDataTypeMaxValue(dataType)
                End Try
                dblMax = Convert.ToDouble(max)
                If Double.IsInfinity(dblMax) Then
                    dblMax = Double.MaxValue
                End If
            End If

            ' DecPoint bei Festpunktzahlen berücksichtigen
            If decPoint > 0 AndAlso dataType <> DataTypes.VT_R4 AndAlso dataType <> DataTypes.VT_R8 Then
                dblMin *= Math.Pow(10, -decPoint)
                dblMax *= Math.Pow(10, -decPoint)
            End If
        Else
            value = _Control.Text
        End If

        cmdDecSep.Text = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
        'vouMaxValue.DataBackColor = System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(200, Byte), Integer))
        'vouMinValue.DataBackColor = System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(200, Byte), Integer))
        
		vouMaxValue.ForeColor = System.Drawing.Color.SteelBlue 
		vouMinValue.ForeColor = System.Drawing.Color.SteelBlue 
		
		If numEdit.IsEditing() Then
            numEdit.StopEdit(False)
        End If

        lblCaption.Text = caption

        If limitCheck = LimitCheckMode.None Then
            limitCheck = LimitCheckMode.OnInput
        End If

        ' Das NumPad ist auf numerische Datentypen beschränkt!
        If IsValidDataType(dataType) Then
            ' Eingabe freischalten
            numEdit.Enabled = True
            cmdEnter.Enabled = True

            ' Intern arbeitet das NumPad immer mit Fließkommavariablen!
            If dataType = DataTypes.VT_R4 Then
                numEdit.VWItem.Name = "__UNLINKED_R4"
            Else
                numEdit.VWItem.Name = "__UNLINKED_R8"
            End If

            numEdit.VWItem.DecPoint = decPoint
            numEdit.VWItem.Value = value
            numEdit.Format = format
            numEdit.LimitCheck = limitCheck
            numEdit.LimitMin.Value = dblMin
            numEdit.LimitMax.Value = dblMax

            ' Handelt es sich um einen DatenTyp mit Vorzeichen?
            cmdSign.Enabled = IsSignedDataType(dataType)

            ' Wird der Dezimalseparator benötigt?
            cmdDecSep.Enabled = (decPoint > 0 OrElse format = format.AutoFloat OrElse format = format.Scientific)

            ' Bei Exponentialdarstellung wird die "E"-Taste benötigt
            cmdExponent.Enabled = (format = format.AutoFloat OrElse format = format.Scientific)

            ' Die Label für die Anzeige der Limits beschriften
            vouMaxValue.Format = format
            vouMinValue.Format = format
            vouMaxValue.VWItem.DecPoint = decPoint
            vouMinValue.VWItem.DecPoint = decPoint
            vouMaxValue.VWItem.Value = dblMax
            vouMinValue.VWItem.Value = dblMin

            lblUnit.Text = unit

            numEdit.Focus()
        Else
            ' Der übergebene Datentyp wird vom NumPad nicht unterstützt.
            ' Eingabe sperren!
            numEdit.Text = "INVALID ITEM"
            numEdit.VWItem.Value = 0
            numEdit.Enabled = False
            cmdEnter.Enabled = False
            vouMaxValue.Text = ""
            vouMinValue.Text = ""
            lblUnit.Text = ""
        End If
        numEdit.Top = lbInputBack.Top + (lbInputBack.Height - numEdit.Height) / 2
        Me.Location = CalcLocation(editControl, position)
    End Sub

    Private Function IsValidDataType(ByVal dataType As DataTypes) As Boolean
        Select Case dataType
            Case DataTypes.VT_BOOL, DataTypes.VT_I1, DataTypes.VT_I2, DataTypes.VT_I4, DataTypes.VT_INT, DataTypes.VT_R4, _
             DataTypes.VT_R8, DataTypes.VT_UI1, DataTypes.VT_UI2, DataTypes.VT_UI4, DataTypes.VT_UINT
                Return True
            Case Else
                Return False
        End Select
    End Function

    Private Function IsSignedDataType(ByVal dataType As DataTypes) As Boolean
        Select Case dataType
            Case DataTypes.VT_BOOL, DataTypes.VT_UI1, DataTypes.VT_UI2, DataTypes.VT_UI4, DataTypes.VT_UINT
                Return False
            Case Else
                Return True
        End Select
    End Function

    Private Function GetDataTypeMinValue(ByVal dataType As DataTypes) As Double
        Select Case dataType
            Case DataTypes.VT_I1
                Return SByte.MinValue
            Case DataTypes.VT_I2
                Return Short.MinValue
            Case DataTypes.VT_I4, DataTypes.VT_INT
                Return Integer.MinValue
            Case DataTypes.VT_R4
                Return Single.MinValue
            Case DataTypes.VT_R8
                Return Double.MinValue
            Case DataTypes.VT_UI1
                Return Byte.MinValue
            Case DataTypes.VT_UI2
                Return UShort.MinValue
            Case DataTypes.VT_UI4, DataTypes.VT_UINT
                Return UInteger.MinValue
            Case DataTypes.VT_BOOL
                Return 0
            Case Else
                Return 0
        End Select
    End Function

    Private Function GetDataTypeMaxValue(ByVal dataType As DataTypes) As Double
        Select Case dataType
            Case DataTypes.VT_I1
                Return SByte.MaxValue
            Case DataTypes.VT_I2
                Return Short.MaxValue
            Case DataTypes.VT_I4, DataTypes.VT_INT
                Return Integer.MaxValue
            Case DataTypes.VT_R4
                Return Single.MaxValue
            Case DataTypes.VT_R8
                Return Double.MaxValue
            Case DataTypes.VT_UI1
                Return Byte.MaxValue
            Case DataTypes.VT_UI2
                Return UShort.MaxValue
            Case DataTypes.VT_UI4, DataTypes.VT_UINT
                Return UInteger.MaxValue
            Case DataTypes.VT_BOOL
                Return 1
            Case Else
                Return 0
        End Select
    End Function

    Private Function ScaleFont(ByVal font As Font, ByVal ratio As Single) As Font
        Dim fontSize As Single = font.Size * (ratio * 0.5F + 0.5F)
        Dim newFont As New Font(font.Name, fontSize, font.Style)
        If newFont IsNot Nothing Then
            Return newFont
        Else
            Return font
        End If
    End Function

    Private Function CalcLocation(ByVal editControl As Control, ByVal position As TouchKeyboardPosition) As Point
        Dim currentScreen As Screen = Screen.PrimaryScreen

        Select Case position
            Case TouchKeyboardPosition.AtControl
                Dim location As New Point(0, 0)
                Dim parent As Control = editControl.Parent
                Dim controlLocation As Point = parent.PointToScreen(editControl.Location)

                If controlLocation.X + Me.Width < currentScreen.Bounds.Right Then
                    location.X = controlLocation.X
                Else
                    location.X = currentScreen.Bounds.Right - Me.Width
                End If

                If controlLocation.Y + editControl.Height + Me.Height < currentScreen.Bounds.Bottom Then
                    location.Y = controlLocation.Y + editControl.Height
                ElseIf (controlLocation.Y - Me.Height) > currentScreen.Bounds.Top Then
                    location.Y = controlLocation.Y - Me.Height
                Else
                    location.Y = currentScreen.Bounds.Top
                End If

                Return location
            Case TouchKeyboardPosition.CenterParent
                Dim parent As Control = GetParentForm(editControl)
                Dim location As New Point(parent.Left + ((parent.Width - Me.Width) / 2), parent.Top + ((parent.Height - Me.Height) / 2))

                If location.X < 0 Then
                    location.X = 0
                End If

                If location.X + Me.Width > currentScreen.Bounds.Right Then
                    location.X = currentScreen.Bounds.Right - Me.Width
                End If

                If location.Y < 0 Then
                    location.Y = 0
                End If

                If location.Y + Me.Height > currentScreen.Bounds.Bottom Then
                    location.Y = currentScreen.Bounds.Bottom - Me.Height
                End If

                Return location
            Case TouchKeyboardPosition.CenterScreen
                Return New Point(((currentScreen.Bounds.Width - Me.Width) / 2), ((currentScreen.Bounds.Height - Me.Height) / 2))
            Case TouchKeyboardPosition.Manual
                Return Me.Location
            Case Else
                Return Me.Location
        End Select
    End Function

    Private Shadows Sub ScaleControl(ByVal control As Control, ByVal ratio As Single)
        Dim roundLeft As Single = IIf(control.Left < 0, -0.5F, 0.5F)
        Dim roundTop As Single = IIf(control.Top < 0, -0.5F, 0.5F)
        Dim roundWidth As Single = IIf(control.Width < 0, -0.5F, 0.5F)
        Dim roundHeight As Single = IIf(control.Height < 0, -0.5F, 0.5F)

        ' Außer bei Formularen müssen erst mal die Left und Top Positionen umgerechnet werden
        If Not (TypeOf control Is Form) Then
            control.Left = CInt(((CSng(control.Left) * ratio) + roundLeft))
            control.Top = CInt(((CSng(control.Top) * ratio) + roundTop))
        End If
        ' außerdem natürlich Breite und Höhe
        control.Width = CInt(((CSng(control.Width) * ratio) + roundWidth))
        control.Height = CInt(((CSng(control.Height) * ratio) + roundHeight))
        ' der Font wird ebenfalls angepasst
        control.Font = ScaleFont(control.Font, ratio)

        ' für alle weiteren Controls wird die Funktion nun rekursiv aufgerufen.
        ' Damit wird erreicht, das z.B alle Steuerelemente innerhalb eines Formulars 
        ' oder auch einer GroupBox richtig skaliert werden.
        For Each ctrl As Control In control.Controls
            ScaleControl(ctrl, ratio)
        Next
    End Sub

    Private Function GetParentForm(ByVal control As Control) As Control
        Dim ctrlParent As Control = control
        While ctrlParent.Parent IsNot Nothing
            ctrlParent = ctrlParent.Parent
        End While
        Return ctrlParent
    End Function

#End Region

#Region "VisiWinNET.AddIn generated code"
    'active=1

#Region "Initialization of VisiWinNET controls"

    Private Sub InitializeVisiWinNETComponents()

        CType(cmd0, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdDecSep, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdSign, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdEnd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdHome, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdRight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdExponent, System.ComponentModel.ISupportInitialize).EndInit()
        CType(lblCaption, System.ComponentModel.ISupportInitialize).EndInit()
        CType(vouMinValue, System.ComponentModel.ISupportInitialize).EndInit()
        CType(vouMaxValue, System.ComponentModel.ISupportInitialize).EndInit()
        CType(lbInputBack, System.ComponentModel.ISupportInitialize).EndInit()
        CType(lblUnit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(pnlPicLimitMin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(pnlPicLimitMax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdEsc, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdLeft, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdClear, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdBack, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdEnter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdDel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(grpBorder, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

#End Region

#End Region

End Class