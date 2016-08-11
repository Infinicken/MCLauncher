Imports System.ComponentModel

Public Class ConsoleOut
    Private Sub ConsoleOut_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Not MinecraftAppletLauncher.currentMCProcess Is Nothing Then
            RemoveHandler ConsoleLogger.onReceiveLog, AddressOf onDataReceived
            TextBox1.Clear()
        End If
    End Sub

    Private Sub ConsoleOut_Load(sender As Object, e As EventArgs) Handles MyBase.Shown
        If Not MinecraftAppletLauncher.currentMCProcess Is Nothing Then
            TextBox1.AppendText(ConsoleLogger.log.ToString)
            AddHandler ConsoleLogger.onReceiveLog, AddressOf onDataReceived
        End If
    End Sub

    Private Sub onDataReceived(sender As Object, e As DataReceivedEventArgs)
        Try
            Me.Invoke(Sub() TextBox1.AppendText(e.Data & vbCrLf))
        Catch
        End Try
    End Sub
End Class