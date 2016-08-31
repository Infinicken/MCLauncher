Imports Ionic.Zip

Public NotInheritable Class Logger
    Private Sub New()
    End Sub
    Shared Sub New()
        If Not IO.Directory.Exists("logs") Then IO.Directory.CreateDirectory("logs")
        filePath = $"logs\dfxlauncher_log_{Today.Year}_{Today.Month}_{Today.Day}_{Now.Hour}_{Now.Minute}_{Now.Second}.txt"
        fileLog = New IO.StreamWriter(filePath, False, Text.Encoding.UTF8) With {.AutoFlush = True}
    End Sub
    Private Shared sb As New Text.StringBuilder
    Private Shared tenline As New Text.StringBuilder
    Private Shared tenlinecount As Integer = 0
    Private Shared fileLog As IO.StreamWriter
    Private Shared filePath As String
    Private Shared isStreamClosed As Boolean
    Public Enum LogLevel
        INFO = 0
        ERR = 1
        WARN = 2
        CRIT = 3
        FATAL = 4
        DEBUG = 5
    End Enum
    Public Shared Sub log(msg As String, level As LogLevel)
        If isStreamClosed Then
            MsgBox("Caught attempt to log after logger has been closed." & vbCrLf &
                           "Offender: " & Environment.StackTrace.Split(CChar(vbCrLf))(3) & vbCrLf &
                           $"Trying to log: [{level.ToString}]{msg}")
            Return
        End If
        Threads.Mutex.WaitOne()
        SyncLock fileLog
            SyncLock tenline
                Dim frmStr As String = $"[{Now.ToLocalTime}][{level.ToString}][{Threading.Thread.CurrentThread.Name}] {msg}"
                sb.AppendLine(frmStr)
                fileLog.WriteLine(frmStr)
                fileLog.Flush()
                tenline.AppendLine(frmStr)
                tenlinecount += 1
                If tenlinecount = 10 Then
                    Debug.WriteLine(tenline.ToString)
                    tenline = New Text.StringBuilder
                    tenlinecount = 0
                End If
            End SyncLock
        End SyncLock
        Threads.Mutex.ReleaseMutex()
    End Sub
    Public Shared Sub [stop]()
        If Not isStreamClosed Then
            isStreamClosed = True
            fileLog.Close()
            Using zip As New ZipFile()
                zip.AddFile(filePath)
                zip.Save(IO.Path.Combine(IO.Path.GetDirectoryName(filePath), IO.Path.GetFileNameWithoutExtension(filePath) & ".zip"))
                IO.File.Delete(filePath)
            End Using
        Else
            MsgBox("Caught attempt to close an already closed logger." & vbCrLf &
                           "Offender: " & Environment.StackTrace.Split(CChar(vbCrLf))(3))
        End If
    End Sub
End Class
