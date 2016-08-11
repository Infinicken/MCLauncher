Imports System.ComponentModel
Imports System.Net

Public Class FileDownload

    Private Shadows Function ShowDialog() As DialogResult
        MyBase.Show()
        Return DialogResult
    End Function

    Private Shadows Sub Show()
        MyBase.Show()
    End Sub
    Private WithEvents wc As New WebClient
    Public Sub StartDownloading(fileToDownload As String, saveTo As String, Optional ticker As String = "AWAIT_TRANSLATE")
        MyBase.Show()
        If ticker = "AWAIT_TRANSLATE" Then ticker = I18n.translate("info.download.async", fileToDownload)
        labelStat.Text = String.Format(ticker, fileToDownload)
        wc.DownloadFileAsync(New Uri(fileToDownload), IO.Path.GetFullPath(saveTo))
        done = False
    End Sub

    Public Sub StartDownloadingAwait(fileToDownload As String, saveTo As String, Optional ticker As String = "AWAIT_TRANSLATE")
        MyBase.Show()
        If ticker = "AWAIT_TRANSLATE" Then ticker = I18n.translate("info.download.sync", fileToDownload)
        labelStat.Text = String.Format(ticker, fileToDownload)
        wc.DownloadFileAsync(New Uri(fileToDownload), IO.Path.GetFullPath(saveTo))
        done = False
        While Not done
            Application.DoEvents()
        End While
    End Sub
    Private lastByte As Integer
    Private lastSec As Integer = 0
    Private Sub client_ProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs) Handles wc.DownloadProgressChanged
        Application.DoEvents()
        progDL.Value = e.ProgressPercentage
        Label1.Text = Math.Abs((e.BytesReceived - lastByte) \ 1024).ToString & "KB/s - " & e.ProgressPercentage & "%"
        If Now.Second <> lastSec Then
            lastByte = CInt(e.BytesReceived)
            lastSec = Now.Second
        End If
    End Sub
    Private done As Boolean = False
    Private Sub wc_DownloadFileCompleted(sender As Object, e As AsyncCompletedEventArgs) Handles wc.DownloadFileCompleted
        done = True
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Const CP_NOCLOSE_BUTTON As Integer = &H200
    Protected Overloads Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim myCp As CreateParams = MyBase.CreateParams
            myCp.ClassStyle = myCp.ClassStyle Or CP_NOCLOSE_BUTTON
            Return myCp
        End Get
    End Property
End Class