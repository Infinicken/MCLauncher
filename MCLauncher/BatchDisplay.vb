Public Class BatchDisplay
    Private Sub BatchDisplay_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        listAll()
        Control.CheckForIllegalCrossThreadCalls = False
        AddHandler BatchFileDownload.ProgressChanged, Sub()
                                                          listAll()
                                                      End Sub
    End Sub
    Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hWnd As Integer,
                                                                ByVal wMsg As Integer,
                                                                ByVal wParam As Integer,
                                                                ByVal lParam As Integer) As Integer
    Private displayWait As Integer = 0

    Private Sub listAll()
        Try
            If displayWait <> 0 Then
                displayWait -= 1
                Return
            End If
            Invoke(Sub()
                       Try
                           'Disable render
                           SendMessage(CInt(tlp.Handle), 11, 0, 0)
                           tlp.Controls.Clear()
                           tlp.RowCount = 0
                           tlp.ColumnStyles.Clear()
                           tlp.RowStyles.Clear()
                           tlp.ColumnCount = 2
                           tlp.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
                           tlp.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
                           ThreadWrapper.Mutex.WaitOne()
                           Dim sbThread = New Dictionary(Of String, Integer)(BatchFileDownload.progress)
                           ThreadWrapper.Mutex.ReleaseMutex()
                           For Each item In sbThread
                               tlp.Controls.Add(New Label With {.Text = item.Key}, 0, tlp.RowCount)
                               tlp.Controls.Add(New ProgressBar With {.Value = item.Value}, 1, tlp.RowCount)
                               tlp.RowCount += 1
                           Next
                           SendMessage(CInt(tlp.Handle), 11, 1, 0)
                           tlp.Refresh()
                       Catch
                       End Try
                   End Sub
                )
            displayWait = 3
        Catch
        End Try
    End Sub
End Class