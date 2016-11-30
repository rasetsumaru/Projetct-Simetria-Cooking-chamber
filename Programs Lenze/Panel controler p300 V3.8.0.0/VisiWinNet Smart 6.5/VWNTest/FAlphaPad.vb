' Formular zur Anzeige einer alphanumerischen Bildschirmtastatur

#Region "Template information"
' §cmtTemplateInformation1
' Version = 6.5.1.0
' §cmtTemplateInformation2
' 
#End Region

#Region "Aufrufbeispiele!!!"
' Dieses Formular wird nicht direkt aus dem Programmcode heraus aufgerufen,
' sondern durch die Komponente VisiWinNET.Forms.TouchKeyboard.
' Diese Komponente wird z.B. auf dem Startformular "FStart" platziert,
' in die Eigenschaft "AlphaPad" wird dann der Klassenname dieses Formulars (also "FAlphaPad") eingetragen.
' Dadurch wird dieses Formular anstelle der internen VisiWinNET-Bildschirmtastatur aufgerufen.
#End Region

#Region "Abhängigkeiten/zusätzliche Dateien"
' - Indextexte in Gruppe
'   - Benutzertexte.TouchKeyboards.AlphaPad
#End Region

Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports VisiWinNET.Forms
Imports VisiWinNET.DataAccess
Imports VisiWinNET.LanguageSwitching

Partial Public Class FAlphaPad
    Inherits VisiWinNET.Forms.SmartForm
    Implements VisiWinNET.Forms.IInputPad

#Region "Konstruktor, Dispose"

    Public Sub New()
        ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        InitializeComponent()
        InitializeVisiWinNETComponents()
        'Me.Size = grpBorder.Size
		' VWN 6.5.3
		Me.ClientSize = grpBorder.Size


        Localization_LanguageChange(Nothing, Nothing)

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    End Sub

    ''' <summary>
    ''' Verwendete Ressourcen bereinigen.
    ''' </summary>
    ''' <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If components IsNot Nothing Then
                components.Dispose()
            End If
            If _Localization IsNot Nothing Then
                _Localization.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

#End Region

#Region "Lokale Variablen"

    Private _bShift As Boolean = False
    Private _bCapsLock As Boolean = False
    Private _bCtrl As Boolean = False
    Private _bAltGr As Boolean = False

    Private _bModal As Boolean = False
    Private _Control As Control = Nothing
    Private WithEvents _Localization As New Localization()

    Private _nCharIndex As Integer = 0

    Private _BackColorDown_LenzeStyle As Color = System.Drawing.Color.White'.FromArgb(CType(CType(0, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(160, Byte), Integer))
	Private _BackColor_LenzeStyle As Color = System.Drawing.Color.SteelBlue '.FromArgb(CType(CType(180, Byte), Integer), CType(CType(190, Byte), Integer), CType(CType(200, Byte), Integer))


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

    Private Sub cmdAlpha_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
		cmdSpace.Click, cmd00.Click, cmd01.Click, cmd02.Click, cmd03.Click, cmd04.Click, cmd05.Click, cmd06.Click, _
		cmd07.Click, cmd08.Click, cmd09.Click, cmd010.Click, cmd011.Click, cmd012.Click, _
		cmd10.Click, cmd11.Click, cmd12.Click, cmd13.Click, cmd14.Click, cmd15.Click, cmd16.Click, _
		cmd17.Click, cmd18.Click, cmd19.Click, cmd110.Click, cmd111.Click, _
		cmd20.Click, cmd21.Click, cmd22.Click, cmd23.Click, cmd24.Click, cmd25.Click, cmd26.Click, _
		cmd27.Click, cmd28.Click, cmd29.Click, _
		cmd30.Click, cmd31.Click, cmd32.Click, cmd33.Click, cmd34.Click, cmd35.Click, cmd36.Click, _
		cmd37.Click, cmd38.Click, cmd39.Click, cmd310.Click, cmd211.Click, cmd210.Click

        Dim s As String = DirectCast(TryCast(sender, Control).Tag, String)

        If s IsNot Nothing AndAlso s.Length > _nCharIndex Then
            numEdit.SendKey(s(_nCharIndex))
        End If
        HandleShift(False)
        HandleCtrl(False)
        HandleAltGr(False)
    End Sub

    Private Sub cmdSpace_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSpace.Click
        numEdit.SendKey(Chr(160))
        HandleShift(False)
        HandleCtrl(False)
        HandleAltGr(False)
    End Sub

    Private Sub cmdShift_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdShiftLeft.Click, cmdShiftRight.Click
        HandleShift(Not _bShift)
        HandleCapsLock(False)
    End Sub

    Private Sub cmdCapsLock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCapsLock.Click
        HandleCapsLock(Not _bCapsLock)
        HandleShift(False)
    End Sub

    Private Sub cmdCtrl_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCtrlLeft.Click, cmdCtrlRight.Click
        HandleCtrl(Not _bCtrl)
    End Sub

    Private Sub cmdAltGr_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAltGr.Click
        HandleAltGr(Not _bAltGr)
    End Sub

    Private Sub cmdBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        numEdit.SendKey(Chr(8))
        ResetControlKeys()
    End Sub

    Private Sub cmdDel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDel.Click
        numEdit.SendKey(Keys.Delete)
        ResetControlKeys()
    End Sub

    Private Sub cmdClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdClear.Click
        numEdit.Text = ""
        ResetControlKeys()
    End Sub

    Private Sub cmdLeft_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLeft.Click
        numEdit.SendKey(Keys.Left)
        ResetControlKeys()
    End Sub

    Private Sub cmdRight_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRight.Click
        numEdit.SendKey(Keys.Right)
        ResetControlKeys()
    End Sub

    Private Sub cmdUp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUp.Click
        numEdit.SendKey(Keys.Up)
        ResetControlKeys()
    End Sub

    Private Sub cmdDown_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDown.Click
        numEdit.SendKey(Keys.Down)
        ResetControlKeys()
    End Sub

    Private Sub cmdTab_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdTab.Click
        numEdit.SendKey(Keys.Tab)
        ResetControlKeys()
    End Sub

    Private Sub cmdEsc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdEsc.Click
        If _bModal Then
            Me.DialogResult = DialogResult.Cancel
        Else
            Me.Hide()
        End If
    End Sub

    Private Sub cmdHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdHome.Click
        numEdit.SendKey(Keys.Home)
    End Sub

    Private Sub cmdEnd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdEnd.Click
        numEdit.SendKey(Keys.End)
    End Sub

    Private Sub cmdEnter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdEnter.Click
        If _bCtrl Then
            numEdit.SendKey(Keys.Enter Or Keys.Control)
            HandleCtrl(False)
        Else
            numEdit.SendKey(Keys.Enter)
        End If
    End Sub

#End Region

#Region "Hilfsmethoden"

    Private Sub Initialize(ByVal editControl As Control, ByVal position As TouchKeyboardPosition)
        _Control = editControl
        Dim varIn As VarIn = TryCast(_Control, VarIn)

        ResetControlKeys()

        If varIn IsNot Nothing Then
            numEdit.VWItem.Value = varIn.VWItem.EditableState
			numEdit.Text = varIn.VWItem.EditableState
            numEdit.LimitCheck = varIn.LimitCheck
            numEdit.LimitMax.Value = varIn.LimitMax.Value
            numEdit.LimitMin.Value = varIn.LimitMin.Value

            Dim caption As String = Localization.GetText("@TouchKeyboards.AlphaPad.Caption")
            If Not String.IsNullOrEmpty(varIn.Label.DisplayText) Then
                caption = varIn.Label.DisplayText
            ElseIf Not String.IsNullOrEmpty(varIn.VWItem.Text) Then
                caption = varIn.VWItem.Text
            End If
            lblCaption.Text = caption

            numEdit.Multiline = False
            numEdit.PasswordChar = varIn.PasswordChar
        Else
            numEdit.VWItem.Value = _Control.Text
            lblCaption.Text = Localization.GetText("@TouchKeyboards.AlphaPad.Caption")
            If TypeOf _Control Is TextBox Then
                numEdit.Multiline = TryCast(_Control, TextBox).Multiline
                numEdit.PasswordChar = TryCast(_Control, TextBox).PasswordChar
            End If
        End If

        'If numEdit.Multiline Then
        '    numEdit.BorderStyle = System.Windows.Forms.BorderStyle.None
        '    lblInputBack.Visible = False
        '    numEdit.Size = lblInputBack.Size
        '    numEdit.Location = lblInputBack.Location
        'Else
        '    numEdit.BorderStyle = System.Windows.Forms.BorderStyle.None
        '    lblInputBack.Visible = False
        '    numEdit.Width = lblInputBack.Width - 4
        '    numEdit.Left = lblInputBack.Left + 2
        '    numEdit.Top = lblInputBack.Top + (lblInputBack.Height - numEdit.Height) / 2
        'End If

        Me.Location = CalcLocation(editControl, position)
        numEdit.Focus()
    End Sub

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

    Private Function ScaleFont(ByVal font As Font, ByVal ratio As Single) As Font
        Dim fontSize As Single = font.Size * (ratio * 0.5F + 0.5F)
        Dim newFont As New Font(font.Name, fontSize, font.Style)
        If newFont IsNot Nothing Then
            Return newFont
        Else
            Return font
        End If
    End Function

    Private Sub ResetControlKeys()
        HandleShift(False)
        HandleCtrl(False)
        HandleCapsLock(False)
    End Sub

    Private Sub HandleShift(ByVal bShift As Boolean)
        If _bShift = bShift Then
            Return
        End If

        _bShift = bShift

        _nCharIndex = IIf(_bShift OrElse _bCapsLock, 1, 0) + IIf(_bAltGr, 2, 0)
        If _bShift Then
            cmdShiftLeft.BackColor = _BackColorDown_LenzeStyle
            cmdShiftRight.BackColor = _BackColorDown_LenzeStyle
            
			cmdShiftLeft.ForeColor = _BackColorDown_LenzeStyle
            cmdShiftRight.ForeColor = _BackColorDown_LenzeStyle
        Else
            cmdShiftLeft.BackColor = _BackColor_LenzeStyle
            cmdShiftRight.BackColor = _BackColor_LenzeStyle
            
			cmdShiftLeft.ForeColor = _BackColorDown_LenzeStyle 
            cmdShiftRight.ForeColor = _BackColorDown_LenzeStyle
        End If
    End Sub

    Private Sub HandleCapsLock(ByVal bLock As Boolean)
        If _bCapsLock = bLock Then
            Return
        End If

        If bLock Then
            HandleShift(False)
            HandleCtrl(False)
        End If

        _bCapsLock = bLock

        _nCharIndex = IIf(_bShift OrElse _bCapsLock, 1, 0) + IIf(_bAltGr, 2, 0)
        If _bCapsLock Then
            cmdCapsLock.BackColor = _BackColorDown_LenzeStyle
            cmdCapsLock.ForeColor = _BackColorDown_LenzeStyle
        Else
            cmdCapsLock.BackColor = _BackColor_LenzeStyle
            cmdCapsLock.ForeColor = _BackColor_LenzeStyle
        End If
    End Sub

    Private Sub HandleCtrl(ByVal bCtrl As Boolean)
        If _bCtrl = bCtrl Then
            Return
        End If

        _bCtrl = bCtrl

        If _bCtrl Then
            _nCharIndex = 0
            cmdCtrlLeft.BackColor = _BackColorDown_LenzeStyle
            cmdCtrlRight.BackColor = _BackColorDown_LenzeStyle
            
			cmdCtrlLeft.ForeColor = _BackColor_LenzeStyle
            cmdCtrlRight.ForeColor = _BackColor_LenzeStyle
		Else
            _nCharIndex = 0
            cmdCtrlLeft.BackColor = _BackColor_LenzeStyle
            cmdCtrlRight.BackColor = _BackColor_LenzeStyle
            
			cmdCtrlLeft.ForeColor = _BackColorDown_LenzeStyle
            cmdCtrlRight.ForeColor = _BackColorDown_LenzeStyle
        End If
    End Sub
	
    Private Sub HandleAltGr(ByVal bAltGr As Boolean)
        If _bAltGr = bAltGr Then
            Return
        End If

        _bAltGr = bAltGr

        _nCharIndex = IIf(_bShift OrElse _bCapsLock, 1, 0) + IIf(_bAltGr, 2, 0)
        If _bAltGr Then
            cmdAltGr.BackColor = _BackColorDown_LenzeStyle
            cmdAltGr.ForeColor = _BackColor_LenzeStyle
        Else
            cmdAltGr.BackColor = _BackColor_LenzeStyle
            cmdAltGr.ForeColor = _BackColorDown_LenzeStyle
        End If
    End Sub

    Private Shadows Sub ScaleControl(ByVal control As Control, ByVal ratio As Single)
        Dim roundLeft As Single = IIf(control.Left < 0, -0.5F, 0.5F)
        Dim roundTop As Single = IIf(control.Top < 0, -0.5F, 0.5F)
        Dim roundWidth As Single = IIf(control.Width < 0, -0.5F, 0.5F)
        Dim roundHeight As Single = IIf(control.Height < 0, -0.5F, 0.5F)

        If Not (TypeOf control Is Form) Then
            control.Left = CInt(((CSng(control.Left) * ratio) + roundLeft))
            control.Top = CInt(((CSng(control.Top) * ratio) + roundTop))
        End If

        control.Width = CInt(((CSng(control.Width) * ratio) + roundWidth))
        control.Height = CInt(((CSng(control.Height) * ratio) + roundHeight))

        control.Font = ScaleFont(control.Font, ratio)
		
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

#Region "Sprachumschaltung"

    Private Sub Localization_LanguageChange(ByVal sender As Object, ByVal e As VisiWinNET.LanguageSwitching.LanguageChangeEventArgs) Handles _Localization.LanguageChange
        LoadTexts()
    End Sub

    Private Sub LoadTexts()
        For Each control As Control In Me.grpBorder.Controls
            Dim cmd As CommandButton = TryCast(control, CommandButton)
            If cmd IsNot Nothing AndAlso Char.IsDigit(cmd.Name, 3) Then
                Dim s As String = Localization.GetText("@TouchKeyboards.AlphaPad." & cmd.Name)
                cmd.Tag = s

                If s IsNot Nothing AndAlso s.Length >= 2 Then
                    If Char.IsUpper(s, 1) Then
                        'Letter
                        If s.Length = 2 Then
                            cmd.Text = String.Format("{0}", s(1))
                        Else
                            cmd.Text = String.Format("{0}" & Chr(10) & "   {1}", s(1), s(2))
                        End If
                    Else
                        'Non Letter
                        If s.Length = 2 Then
                            cmd.Text = String.Format("{0}" & Chr(10) & "{1}", s(1), s(0))
                        Else
                            cmd.Text = String.Format("{0}" & Chr(10) & "{1}   {2}", s(1), s(0), s(2))
                        End If
                    End If
                Else
                    cmd.Text = ""
                End If
            End If
        Next
    End Sub

#End Region

#Region "VisiWinNET.AddIn generated code"

#Region "Initialization of VisiWinNET controls"

    Private Sub InitializeVisiWinNETComponents()

        CType(cmd00, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd01, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd02, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd03, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd04, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd05, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd06, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd07, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd08, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd09, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd010, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd011, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd012, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd111, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd110, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd19, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd18, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd17, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd16, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd15, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd14, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd13, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdBack, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdTab, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdEnter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdCapsLock, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd211, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd210, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd29, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd28, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd27, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd26, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd25, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd24, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd23, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd22, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd20, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd310, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd39, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd38, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd37, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd36, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd35, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd34, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd33, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd32, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd31, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmd30, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdShiftLeft, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdShiftRight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdCtrlRight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdCtrlLeft, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdSpace, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdLeft, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdRight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdUp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdDel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdEsc, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdHome, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdEnd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(lblInputBack, System.ComponentModel.ISupportInitialize).EndInit()
        CType(lblCaption, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdClear, System.ComponentModel.ISupportInitialize).EndInit()
        CType(grpBorder, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdAltGr, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

#End Region

#End Region


End Class