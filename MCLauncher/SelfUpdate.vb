Public Class SelfUpdate
    Private Const checkerURL As String = "http://kongkongmao.club/mc/"
    Public Shared Sub checkUpdate()
        Dim resultRaw As String = (ServerSideManager.Post(checkerURL & "launcher/version.json", "", "application/json", "GET"))
        Dim result As UpdateJson = Newtonsoft.Json.JsonConvert.DeserializeObject(Of UpdateJson)(resultRaw)
        Logger.log(resultRaw, Logger.LogLevel.DEBUG)
        If result IsNot Nothing AndAlso result.latestVer IsNot Nothing Then
            If result.latestVer.forced AndAlso New Version(Application.ProductVersion) < New Version(result.latestVer.verStr) Then
                doUpdate(result.latestVer.download, True)
            ElseIf result.latestForcedVer IsNot Nothing AndAlso New Version(Application.ProductVersion) < New Version(result.latestForcedVer.verStr) Then
                doUpdate(result.latestForcedVer.download, True)
            ElseIf New Version(Application.ProductVersion) < New Version(result.latestVer.verStr) Then
                doUpdate(result.latestVer.download, False)
            End If
        End If
    End Sub
    Public Shared Sub doUpdate(url As String, forced As Boolean)
        MsgBox($"{url}, isforced: {forced}")
    End Sub
    Private Class UpdateJson
        Public Class UpdateVersionJson
            Public download As String
            Public forced As Boolean
            Public verStr As String
        End Class
        Public latestVer As UpdateVersionJson
        Public latestForcedVer As UpdateVersionJson
    End Class
End Class
