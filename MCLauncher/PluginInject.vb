Imports System.IO

Public NotInheritable Class PluginInject
    Private Sub New()
    End Sub
    Public Shared Sub load()
        If Directory.Exists("plugins") Then
            For Each file As String In Directory.GetFiles("plugins")
                Dim isAssembly As Boolean = False
                Try
                    Reflection.AssemblyName.GetAssemblyName(file)
                    isAssembly = True
                Catch ex As Exception
                    isAssembly = False
                End Try
                If isAssembly Then

                Else
                    Continue For
                End If
            Next
        End If
    End Sub
End Class
