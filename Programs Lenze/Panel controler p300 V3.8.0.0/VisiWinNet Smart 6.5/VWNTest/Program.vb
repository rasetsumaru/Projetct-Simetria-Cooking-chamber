''' <summary>
''' Klasse mit dem Haupteinstiegspunkt für die Anwendung.
''' </summary>
Public Class Program

    ''' <summary>
    ''' Der Haupteinstiegspunkt für die Anwendung.
    ''' </summary>
    <System.MTAThread()> _
    Public Shared Sub Main()
        Application.Run(New FStart(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase))
    End Sub

End Class
