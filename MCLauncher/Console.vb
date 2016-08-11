Public Class Console
    Public Shared Sub WriteLine(text As String)
        Logger.log(text, Logger.LogLevel.INFO)
    End Sub
End Class
