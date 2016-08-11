Public Class BatchDisplay
    Private Sub BatchDisplay_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = listAll()
        Control.CheckForIllegalCrossThreadCalls = False
        AddHandler BatchFileDownload.ProgressChanged, Sub()
                                                          TextBox1.Text = listAll()
                                                      End Sub
    End Sub

    Private Function listAll() As String
        Threads.Mutex.WaitOne()
        SyncLock BatchFileDownload.progress
            Dim Str As New Text.StringBuilder
            Str.AppendLine(I18n.translate("info.download.batch.ticker", CStr(BatchFileDownload.currentDownload), CStr(BatchFileDownload.MaxDownload), CStr(BatchFileDownload.totalToDownload)))
            Try
                For Each item As KeyValuePair(Of String, Integer) In BatchFileDownload.progress
                    Str.AppendLine($"{If(item.Key.Length > 10, item.Key.Remove(10) & "...", item.Key)} {calculateProgressIndicator(item.Value)} ({item.Value})")
                Next
            Catch ex As InvalidOperationException
                Threads.Mutex.ReleaseMutex()
                Return Str.ToString
            End Try
            Threads.Mutex.ReleaseMutex()
            Return Str.ToString
        End SyncLock
    End Function

    Private Function calculateProgressIndicator(prog As Integer) As String
        Dim str As String = "["
        For i As Integer = 0 To prog \ 10
            If i > 0 Then str += ">"
        Next
        For i As Integer = 0 To 10 - prog \ 10
            If i > 0 Then str += "-"
        Next
        str += "]"
        Return str
    End Function
End Class