Imports System.IO
Imports Newtonsoft.Json

Public Class ConfigManager

    Public Shared HiThere As String = """Oh, it's you. I will say, though, that since you went to all the trouble of waking me up, you must really, really love to test. I love it too. There's just one small thing we need to take care of first."" --GLaDOS, Portal 2 (2011)"

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
        Public shittySpec As Boolean = False
        Public scriptData As String = ""
        Public scriptTooLongTime As Integer = 10000
        Public scriptImmediateDeathTime As Integer = 30000
        Public perms As String = ""
    End Class
    Public Shared ConfigValue As New ConfigValues
    Public Shared Sub writeToConfig()
        requestUpdateConfig()
        Dim json As String = JsonConvert.SerializeObject(ConfigValue)
        Using config As New StreamWriter("config.cfg")
            config.Write(BasicEncryption.encodeBase64(json))
        End Using
    End Sub

    Public Shared Sub readFromConfig()
        Try
            If Not File.Exists("config.cfg") Then GoTo ReadConfig
            Dim json As ConfigValues = Nothing
            Using config As New StreamReader("config.cfg")
                json = JsonConvert.DeserializeObject(Of ConfigValues)(BasicEncryption.decodeBase64(config.ReadToEnd))
            End Using
            ConfigValue = json
ReadConfig:
            If ConfigValue Is Nothing Then
                ConfigValue = New ConfigValues
            End If
            If File.Exists("automate.cfg") Then
                Dim automatedRslt As ConfigValues = Nothing
                Using automation As New StreamReader("automate.cfg")
                    automatedRslt = JsonConvert.DeserializeObject(Of ConfigValues)(BasicEncryption.decodeBase64(automation.ReadToEnd))
                End Using
                ConfigValue = automatedRslt
                File.Delete("automate.cfg")
                MsgBox(I18n.translate("info.config.automated"))
            End If
            notifyReadConfig()
        Catch
            MsgBox(I18n.translate("err.config"))
            ConfigValue = New ConfigValues
        End Try
    End Sub

    Public Shared Sub requestUpdateConfig()
        I18n.updateConfig()
        PremiumVerifier.updateConfig()
        ServerSideManager.updateConfig()
        ScriptServer.updateConfig()
        ScriptPermMgr.updateConfig()
    End Sub

    Public Shared Sub notifyReadConfig()
        I18n.readConfig()
        PremiumVerifier.readConfig()
        ServerSideManager.readConfig()
        ScriptServer.readConfig()
        ScriptPermMgr.readConfig()
    End Sub

    Public Shared Sub releaseSessionLock()
        File.Delete("session.lock")
    End Sub

    Public Shared Sub createSessionLock()
        If File.Exists("session.lock") Then
            'MsgBox("Application was terminated abnormally, or another instance of the application is running.")
        Else
            Using session As New StreamWriter("session.lock")
                session.Write("")
            End Using
        End If
    End Sub
End Class
