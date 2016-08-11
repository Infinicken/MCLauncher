Public Class BatchFileDownload
    Public Shared progress As New Dictionary(Of String, Integer)
    Public Shared Event ProgressChanged()
    Public Const MaxDownload As Integer = 25
    Private Shared curDLCount As Integer = 0
    Public Shared ReadOnly Property currentDownload As Integer
        Get
            Return curDLCount
        End Get
    End Property
    Public Shared ReadOnly Property totalToDownload As Integer
        Get
            Threads.Mutex.WaitOne()
            Dim ret As Integer = dlQueue.Count
            Threads.Mutex.ReleaseMutex()
            Return ret
        End Get
    End Property
    Private Structure QueueDL
        Public fileUrl As String
        Public filePath As String
        Public name As String
        Public Sub New(url As String, path As String, name As String)
            fileUrl = url
            filePath = path
            Me.name = name
        End Sub
    End Structure
    Private Shared dlQueue As New List(Of QueueDL)

    Shared Sub New()
    End Sub

    Public Shared Function addDownload(name As String, fileUrl As String, filePath As String) As Boolean
        dlQueue.Add(New QueueDL(fileUrl, filePath, name))
        Console.WriteLine("Download queued! " & name)
        RaiseEvent ProgressChanged()
        redelegateDownloadMission()
        Return True
    End Function

    Private Shared Sub downloadFile(url As String, path As String)
        Dim w As New Net.WebClient
        Dim threadName As String = Threading.Thread.CurrentThread.Name.Substring(7)
        Dim done As Boolean = False
        AddHandler w.DownloadProgressChanged, Sub(sender As Object, e As Net.DownloadProgressChangedEventArgs)
                                                  progress(threadName) = e.ProgressPercentage
                                                  RaiseEvent ProgressChanged()
                                              End Sub
        AddHandler w.DownloadFileCompleted, Sub()
                                                done = True
                                                RaiseEvent ProgressChanged()
                                                downloadFinishHandler(threadName)
                                            End Sub
        w.DownloadFileAsync(New Uri(url), path)
        While Not done
            Threading.Thread.Sleep(50)
        End While
    End Sub

    Private Shared Sub downloadFinishHandler(threadname As String)
        progress.Remove(threadname)
        curDLCount -= 1
        redelegateDownloadMission()
        Threads.killThread(threadname)
    End Sub

    Private Shared Sub redelegateDownloadMission()
        Threads.Mutex.WaitOne()
        SyncLock dlQueue
            If curDLCount < MaxDownload Then
                If dlQueue.Count > 0 Then
                    Dim toDL As QueueDL = dlQueue(0)
                    curDLCount += 1
                    Threads.createThread("dl_" & toDL.name)
                    If Threads.hasThread("dl_" & toDL.name) AndAlso Not progress.ContainsKey("dl_" & toDL.name) Then progress.Add("dl_" & toDL.name, 0)
                    Threads.addScheduledTask("dl_" & toDL.name, toDL.name, Sub()
                                                                               downloadFile(toDL.fileUrl, toDL.filePath)
                                                                           End Sub)
                    dlQueue.RemoveAt(0)
                End If
            End If
        End SyncLock
        Threads.Mutex.ReleaseMutex()
        RaiseEvent ProgressChanged()
    End Sub
End Class
