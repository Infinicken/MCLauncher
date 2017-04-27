Imports Ionic.Zip

Public NotInheritable Class Logger
    Private Sub New()
    End Sub
    Shared Sub New()
        Try
            If Not IO.Directory.Exists("logs") Then IO.Directory.CreateDirectory("logs")
            Using zip As New ZipFile()
                filePath = $"logs\mclauncher_log_{Today.Year}_{Today.Month}_{Today.Day}_{Now.Hour}_{Now.Minute}_{Now.Second}.llog"
                Dim lastFileName As String = filePath
                Dim count = 0
                For Each file As String In IO.Directory.GetFiles("logs", "*.llog")
                    zip.AddFile(file)
                    lastFileName = file
                    count += 1
                Next
                If count > 0 Then zip.Save(IO.Path.Combine(IO.Path.GetDirectoryName(lastFileName), IO.Path.GetFileNameWithoutExtension(lastFileName) & ".zip"))
                IO.Directory.GetFiles("logs", "*.llog").forEach(Sub(file As String)
                                                                    IO.File.Delete(file)
                                                                End Sub)
            End Using
            fileLog = New IO.StreamWriter(filePath, False, Text.Encoding.UTF8) With {.AutoFlush = True}
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Shared sb As New Text.StringBuilder
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
        ThreadWrapper.Mutex.WaitOne()
        SyncLock fileLog
            Dim frmStr As String = $"[{Now.ToLocalTime}][{level.ToString}][{Threading.Thread.CurrentThread.Name}] {msg}"
            sb.AppendLine(frmStr)
            fileLog.WriteLine(frmStr)
            fileLog.Flush()
            Debug.WriteLine(frmStr.Replace(vbCrLf, "").Replace(vbLf, "").Replace(vbCr, ""))
        End SyncLock
        ThreadWrapper.Mutex.ReleaseMutex()
    End Sub
    Public Shared Sub [stop]()
        If Not isStreamClosed Then
            isStreamClosed = True
            fileLog.Close()
        Else
            MsgBox("Caught attempt to close an already closed logger." & vbCrLf &
                           "Offender: " & Environment.StackTrace.Split(CChar(vbCrLf))(3))
        End If
    End Sub
End Class
