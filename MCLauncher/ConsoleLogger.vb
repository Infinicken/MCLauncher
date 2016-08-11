Option Strict On

Public Class ConsoleLogger
    Public Shared log As New Text.StringBuilder
    Public Shared currentlyLogging As Process
    Public Shared Event onReceiveLog As DataReceivedEventHandler
    Public Shared Sub startLogging(p As Process)
        If p Is Nothing Then Return
        log = New Text.StringBuilder
        AddHandler p.OutputDataReceived, AddressOf logto
        p.BeginOutputReadLine()
        currentlyLogging = p
    End Sub

    Public Shared Sub stopLogging()
        If currentlyLogging Is Nothing Then Return
        RemoveHandler currentlyLogging.OutputDataReceived, AddressOf logto
        currentlyLogging.CancelOutputRead()
    End Sub

    Private Shared Sub logto(sender As Object, e As DataReceivedEventArgs)
        log.AppendLine(e.Data)
        RaiseEvent onReceiveLog(sender, e)
    End Sub
End Class
