Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms

Imports VisiWinNET.UserManagement
Imports VisiWinNET.LanguageSwitching 

Public Class FLogOnChangePassword
    Inherits VisiWinNET.Forms.SmartForm

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

    ''' <summary>
    ''' This method is called by a form swichting.
    ''' Saving the name of the last activated form and setting the name of the current form ('Me.Name')
    ''' </summary>
    Private Sub Me_FormChanged(ByVal sender As System.Object, ByVal e As VisiWinNET.Forms.FormChangedEventArgs) Handles Me.FormChanged
        VisiWinNET.Services.AppService.VWSet("__FORMS.LASTACTIVEFORM", VisiWinNET.Services.AppService.VWGet("__FORMS.ACTIVEFORM"))
	    VisiWinNET.Services.AppService.VWSet("__FORMS.ACTIVEFORM", Me.Name)
    End Sub

    #End Region
	
	Public ConfigUsername As String
	
	Public Overloads Shared Function Show(ByVal username As String) As DialogResult
        Return ShowChange(username)	
    End Function

    Private Shared Function ShowChange(ByVal username As String) As DialogResult
        Dim frm As New FLogOnChangePassword()
        Dim ret As DialogResult
	
        frm.vinUserName.VWItem.EditableValue = username
		frm.SetUsername(username)
		
        ret = frm.ShowDialog()
		frm.Dispose()
		Return ret
    End Function

    Private Sub cmdOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim enmChangePasswordSuccess As VisiWinNET.UserManagement.ChangePasswordSuccess
        Dim user As VisiWinNET.UserManagement.User

        If CheckInput() Then
            user = UserManager.Users(DirectCast(vinUserName.VWItem.EditableValue, String))

            If user IsNot Nothing Then
                enmChangePasswordSuccess = user.ChangePassword(DirectCast(vinNewPassword.VWItem.EditableValue, String), DirectCast(vinPassword.VWItem.EditableValue, String))
            Else
                enmChangePasswordSuccess = ChangePasswordSuccess.UnknownUser
            End If

            Select Case enmChangePasswordSuccess
                Case ChangePasswordSuccess.Succeeded
                    Me.DialogResult = DialogResult.OK
					FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption000"), _
						Localization.GetText("@LabelForms.FMessageBox.Messages.Message009"), MessageBoxButtons.OK)
                    Me.Close()
                    Exit Select

                Case ChangePasswordSuccess.CantAccessDomainInfo, ChangePasswordSuccess.MethodCallFailed, ChangePasswordSuccess.UnableToSavePassword, ChangePasswordSuccess.UnknownUser
                    FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption001"), _
						Localization.GetText("@LabelForms.FMessageBox.Messages.Message005"), MessageBoxButtons.OK)
                    cmdCancel.Focus()
                    Exit Select

                Case ChangePasswordSuccess.ForbiddenPassword, ChangePasswordSuccess.NewPasswordInvalid, ChangePasswordSuccess.NewPasswordInvalidLength, ChangePasswordSuccess.NoCharactersInPassword, ChangePasswordSuccess.NoNumbersInPassword, ChangePasswordSuccess.PasswordMustContainSpecialCharacters
                    FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption001"), _
						Localization.GetText("@LabelForms.FMessageBox.Messages.Message006"), MessageBoxButtons.OK)
                    vinNewPassword.Focus()
                    Exit Select

                Case ChangePasswordSuccess.OldPasswordInvalid
                    FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption001"), _
						Localization.GetText("@LabelForms.FMessageBox.Messages.Message007"), MessageBoxButtons.OK)
                    vinPassword.Focus()
                    Exit Select
            End Select
        End If
    End Sub

	Private Sub SetUsername(ByVal username As String)
        ConfigUsername = username
    End Sub

	#Region "Initialization of VisiWinNET controls"

    Private Sub InitializeVisiWinNETComponents()

        CType(cmdCancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cmdOK, System.ComponentModel.ISupportInitialize).EndInit()
        CType(vinUserName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(vinRepeatNewPassword, System.ComponentModel.ISupportInitialize).EndInit()
        CType(vinNewPassword, System.ComponentModel.ISupportInitialize).EndInit()
        CType(vinPassword, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

	#End Region
	
    Private Function CheckInput() As Boolean
        If DirectCast(vinNewPassword.VWItem.EditableValue, String) <> DirectCast(vinRepeatNewPassword.VWItem.EditableValue, String) Then
            FMessageBox.Show(Localization.GetText("@LabelForms.FMessageBox.Captions.Caption001"), _
				Localization.GetText("@LabelForms.FMessageBox.Messages.Message008"), MessageBoxButtons.OK)
			Return False
        Else
            Return True
        End If
    End Function

	Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub Me_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.vinUserName.VWItem.EditableValue = ConfigUsername
		Me.vinUserName.Text = ConfigUsername 
		Me.vinUserName.Enabled = False
    End Sub

End Class
