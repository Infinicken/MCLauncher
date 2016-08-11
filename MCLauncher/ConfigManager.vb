Imports System.IO
Imports Newtonsoft.Json

Public Class ConfigManager
    Public Class ConfigValues
        Public accessToken As String = "0"
        Public clientToken As String = "0"
        Public username As String = ""
        Public loginmail As String = ""
        Public authType As Short = 0
        Public serverUrl As String = "http://www.kongkongmao.club/"
        Public selectedLang As String = "zh_TW"
        Public curMdpVer As String = "none"
        Public guid As String = ""
        Public cachedMods As String = ""
        Public scriptData As String = ""
    End Class
    Public Shared ConfigValue As New ConfigValues
    Public Shared Sub writeToConfig()
        requestUpdateConfig()
        Dim json As String = JsonConvert.SerializeObject(ConfigValue)
        Dim config As New StreamWriter("config.cfg")
        config.Write(BasicEncryption.func_46293525_1_(json))
        config.Flush()
        config.Close()
        config.Dispose()
    End Sub

    Public Shared Sub readFromConfig()
        If Not File.Exists("config.cfg") Then GoTo ReadConfig
        Dim config As New StreamReader("config.cfg")
        Dim json As ConfigValues = JsonConvert.DeserializeObject(Of ConfigValues)(BasicEncryption.func_46293525_2_(config.ReadToEnd))
        config.Close()
        config.Dispose()
        ConfigValue = json
ReadConfig:
        If ConfigValue Is Nothing Then
            ConfigValue = New ConfigValues
        End If
        notifyReadConfig()
    End Sub

    Public Shared Sub requestUpdateConfig()
        I18n.updateConfig()
        PremiumVerifier.updateConfig()
        ServerSideManager.updateConfig()
        ScriptServer.updateConfig()
    End Sub

    Public Shared Sub notifyReadConfig()
        I18n.readConfig()
        PremiumVerifier.readConfig()
        ServerSideManager.readConfig()
        ScriptServer.readConfig()
    End Sub
End Class
