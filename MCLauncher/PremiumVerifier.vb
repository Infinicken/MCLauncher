Imports System.Net
Imports Newtonsoft.Json

Public Class PremiumVerifier
    Public Shared Username As String = "MCLauncherTest"
    Public Shared UserEmail As String = "email@example.com"
    Public Enum LoginType
        Offline = 0
        Premium = 1
    End Enum
    Public Shared VerifyType As LoginType = PremiumVerifier.LoginType.Premium
    Public Shared AccessToken As String = "0"
    Public Shared ClientToken As String = "0"
    Public Shared Event LoginHook()
    Public Shared Function tryPremiumLogin(username As String, password As String, assert As Boolean) As Boolean
        If VerifyType = LoginType.Offline Then PremiumVerifier.Username = username : AccessToken = "0" : RaiseEvent LoginHook() : Return False
        If AccessToken = "0" OrElse username <> PremiumVerifier.UserEmail Then
            UserEmail = username
            AccessToken = getAccessToken(username, password, ClientToken, assert)
            RaiseEvent LoginHook()
            Return True
        Else
            Dim refresh As Boolean = Not getAccessTokenValid(AccessToken, ClientToken, assert)
            If refresh Then
                AccessToken = refreshAccessToken(AccessToken, ClientToken, assert)
                RaiseEvent LoginHook()
                Return True
            Else
                If assert Then MsgBox(I18n.translate("info.premium.valid", PremiumVerifier.Username, AccessToken))
                RaiseEvent LoginHook()
                Return True
            End If
        End If
    End Function

    Public Shared Function getAccessToken(username As String, password As String, clientToken As String, assert As Boolean) As String
        Dim accessToken As String = "0"
        Dim payload As New AuthPayload
        payload.username = username
        payload.password = password
        payload.agent = New AuthPayload.AgentType With {.name = "Minecraft", .version = 1}
        If clientToken <> "0" Then payload.clientToken = clientToken
        Dim plJSON As String = JsonConvert.SerializeObject(payload)
        Dim req As HttpWebRequest = CType(HttpWebRequest.Create("https://authserver.mojang.com/authenticate"), HttpWebRequest)
        req.ContentType = "application/json"
        req.Method = "POST"
        req.ContentLength = plJSON.Length

        Dim reqStream As IO.Stream = req.GetRequestStream
        reqStream.Write(Text.Encoding.ASCII.GetBytes(plJSON), 0, plJSON.Length)
        reqStream.Close()

        Try
            Dim response As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
            If response.StatusCode = HttpStatusCode.OK Then
                Dim s As New IO.StreamReader(response.GetResponseStream)
                Dim toEncode As String = s.ReadToEnd
                Dim res As AuthResponse = JsonConvert.DeserializeObject(Of AuthResponse)(toEncode)
                s.Close()
                accessToken = res.accessToken
                clientToken = res.clientToken
                VerifyType = LoginType.Premium
                If res.selectedProfile IsNot Nothing Then
                    PremiumVerifier.Username = res.selectedProfile.name
                    If assert Then MsgBox(I18n.translate("info.premium.success", PremiumVerifier.Username, accessToken))
                    response.Close()
                    Return accessToken
                Else
                    VerifyType = LoginType.Offline
                    If assert Then MsgBox(I18n.translate("info.premium.demo"))
                    response.Close()
                    Return accessToken
                End If
            Else
                Dim s As New IO.StreamReader(response.GetResponseStream)
                Dim toEncode As String = s.ReadToEnd
                Dim res As AuthError = JsonConvert.DeserializeObject(Of AuthError)(toEncode)
                If assert Then MsgBox(I18n.translate("info.premium.err", res.error, res.errorMessage, NameOf(getAccessToken) & ":73"))
                VerifyType = LoginType.Offline
                response.Close()
                Return accessToken
            End If
        Catch ex As WebException
            Try
                Dim s As New IO.StreamReader(ex.Response.GetResponseStream)
                Dim res As AuthError = JsonConvert.DeserializeObject(Of AuthError)(s.ReadToEnd)
                If assert Then MsgBox(I18n.translate("info.premium.err", res.error, res.errorMessage, NameOf(getAccessToken) & ":82"))
                s.Close()
                ex.Response.Close()
                VerifyType = LoginType.Offline
                Return accessToken
            Catch exc As Exception
                If assert Then MsgBox(I18n.translate("info.premium.err", exc.Message, "", NameOf(getAccessToken) & ":88"))
                Return accessToken
            End Try
        End Try
    End Function

    Public Shared Function getAccessTokenValid(accessToken As String, clientToken As String, assert As Boolean) As Boolean
        Dim validate As New AuthValidate() With {.accessToken = accessToken, .clientToken = clientToken}
        Dim req As HttpWebRequest = CType(HttpWebRequest.Create("https://authserver.mojang.com/validate"), HttpWebRequest)
        Dim json As String = JsonConvert.SerializeObject(validate)
        req.ContentType = "application/json"
        req.Method = "POST"
        req.ContentLength = json.Length

        Dim s As IO.Stream = req.GetRequestStream
        s.Write(Text.Encoding.ASCII.GetBytes(json), 0, json.Length)
        s.Close()

        Dim refresh As Boolean = False

        Try
            Dim response As HttpWebResponse = CType(req.GetResponse, HttpWebResponse)
            If response.StatusCode = HttpStatusCode.NoContent Then
                refresh = False
            End If
            response.Close()
        Catch ex As WebException
            If ex.Status = WebExceptionStatus.ProtocolError Then
                If CType(ex.Response, HttpWebResponse).StatusCode = HttpStatusCode.Forbidden Then
                    refresh = True
                End If
                ex.Response.Close()
            Else
                If assert Then MsgBox(I18n.translate("info.premium.err", ex.Message, ex.Status.ToString, NameOf(getAccessTokenValid) & ":121"))
                ex.Response.Close()
                Return False
            End If
        End Try
        Return Not refresh
    End Function

    Public Shared Function refreshAccessToken(accessToken As String, clientToken As String, assert As Boolean) As String
        Dim validate As New AuthValidate() With {.accessToken = accessToken, .clientToken = clientToken}
        Dim json As String = JsonConvert.SerializeObject(validate)
        Dim refReq As HttpWebRequest = CType(HttpWebRequest.Create("https://authserver.mojang.com/refresh"), HttpWebRequest)
        Dim validated As String = accessToken
        refReq.ContentType = "application/json"
        refReq.Method = "POST"
        refReq.ContentLength = json.Length

        Dim rs As IO.Stream = refReq.GetRequestStream
        rs.Write(Text.Encoding.ASCII.GetBytes(json), 0, json.Length)
        rs.Close()

        Try
            Dim refRes As HttpWebResponse = CType(refReq.GetResponse, HttpWebResponse)
            Dim rresponse As New IO.StreamReader(refRes.GetResponseStream)
            Dim authRefresh As AuthRefreshResponse = JsonConvert.DeserializeObject(Of AuthRefreshResponse)(rresponse.ReadToEnd)
            validated = authRefresh.accessToken
            PremiumVerifier.Username = authRefresh.selectedProfile.name
            If assert Then MsgBox(I18n.translate("info.premium.refresh", PremiumVerifier.Username, PremiumVerifier.AccessToken))
            rresponse.Close()
            refRes.Close()
            Return validated
        Catch ex As WebException
            Try
                Dim rerrs As New IO.StreamReader(ex.Response.GetResponseStream)
                Dim res As AuthError = JsonConvert.DeserializeObject(Of AuthError)(rerrs.ReadToEnd)
                If assert Then MsgBox(I18n.translate("info.premium.err", res.error, res.errorMessage, NameOf(refreshAccessToken) & ":156"))
                validated = "0"
                rerrs.Close()
                ex.Response.Close()
                VerifyType = LoginType.Offline
                Return validated
            Catch exc As Exception
                If assert Then MsgBox(I18n.translate("info.premium.err", exc.Message, "", NameOf(refreshAccessToken) & ":162"))
                validated = "0"
            End Try
            Return validated
        End Try
    End Function

    Public Shared Function getPlayerHead(username As String) As Bitmap
        If Not IO.Directory.Exists(Application.StartupPath & "\cache") Then
            IO.Directory.CreateDirectory(Application.StartupPath & "\cache")
        End If
        Dim dlg As New FileDownload
        dlg.StartDownloadingAwait("https://minotar.net/avatar/" & username, Application.StartupPath & "\cache\" & username & ".png", "AWAIT_TRANSLATE", True)
        Return CType(Image.FromFile(Application.StartupPath & "\cache\" & username & ".png"), Bitmap)
    End Function

    Public Shared Function getPlayerUUID(username As String) As String
        Dim response As MojangAPIsResponse.UsernameToUUIDAtTime = JsonConvert.DeserializeObject(Of MojangAPIsResponse.UsernameToUUIDAtTime)(ServerSideManager.Post("https://api.mojang.com/users/profiles/minecraft/" & username, "", "application/json", "GET"))
        Return response.id
    End Function

    Public Shared Function getUserInfo() As String
        Dim req As HttpWebRequest = CType(HttpWebRequest.Create("https://api.mojang.com/user"), HttpWebRequest)
        req.Method = "GET"
        req.Headers("Authorization") = "Bearer " & AccessToken
        Try
            Dim res As HttpWebResponse = CType(req.GetResponse, HttpWebResponse)
            Dim ret As String = (New IO.StreamReader(res.GetResponseStream)).ReadToEnd
            res.Close()
            Return ret
        Catch ex As WebException
            Dim res As HttpWebResponse = CType(ex.Response, HttpWebResponse)
            If res Is Nothing Then Return "ERR"
            Dim ret As String = (New IO.StreamReader(res.GetResponseStream)).ReadToEnd
            res.Close()
            Return ret
        End Try
    End Function

    Public Shared Sub updateConfig()
        ConfigManager.ConfigValue.accessToken = AccessToken
        ConfigManager.ConfigValue.authType = CShort(VerifyType)
        ConfigManager.ConfigValue.clientToken = ClientToken
        ConfigManager.ConfigValue.username = Username
        ConfigManager.ConfigValue.loginmail = UserEmail
    End Sub

    Public Shared Sub readConfig()
        AccessToken = ConfigManager.ConfigValue.accessToken
        VerifyType = CType(ConfigManager.ConfigValue.authType, LoginType)
        ClientToken = ConfigManager.ConfigValue.clientToken
        Username = ConfigManager.ConfigValue.username
        UserEmail = ConfigManager.ConfigValue.loginmail
    End Sub
    Public Class AuthValidate
        Public accessToken As String
        Public clientToken As String
    End Class
    Public Class AuthRefreshResponse
        Public accessToken As String
        Public clientToken As String
        Public selectedProfile As AuthResponse.Profile
    End Class
    Public Class AuthPayload
        Public Class AgentType
            Public name As String = "Minecraft"
            Public version As Integer = 1
        End Class
        Public agent As AgentType
        Public username As String
        Public password As String
        Public clientToken As String
    End Class
    Public Class AuthResponse
        Public accessToken As String
        Public clientToken As String
        Public Class Profile
            Public id As String
            Public name As String
            Public legacy As Boolean
        End Class
        Public availableProfiles As List(Of Profile)
        Public selectedProfile As Profile
    End Class
    Public Class AuthError
        Public [error] As String = ""
        Public errorMessage As String = ""
        Public cause As String = ""
    End Class
    Public Class MojangAPIsResponse
        Public Class UsernameToUUIDAtTime
            Public id As String
            Public name As String
        End Class
    End Class
End Class
