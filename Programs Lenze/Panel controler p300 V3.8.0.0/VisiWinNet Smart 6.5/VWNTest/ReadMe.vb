Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms

Public Class ReadMe
    Inherits VisiWinNET.Forms.SmartForm

    #Region "Initialization"
	
    ''' <summary>
    ''' This method is called, when the Form is initialized.
    ''' </summary>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        ' Do not remove this line.
        InitializeComponent()

        ' Add any initialization here:
		
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
    
End Class
