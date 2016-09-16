Option Strict On

Public NotInheritable Class ScriptServer
#Region ".NET Wrapper Classes"
    Public Class DotNetCodeWrapper
        Public Sub compileRunnable(code As String)
            If ScriptPermMgr.hasPermission(ScriptPermMgr.Permissions.PERMISSION_POTENTIAL_UNSAFE_CODE) Then
                ScriptServer.DynamicCompileAndRunDotNetCode(False, code)
            End If
        End Sub
        Public Function compileCallable(code As String) As Object
            If ScriptPermMgr.hasPermission(ScriptPermMgr.Permissions.PERMISSION_POTENTIAL_UNSAFE_CODE) Then
                Return ScriptServer.DynamicCompileAndRunDotNetCode(True, code)
            Else
                Return Nothing
            End If
        End Function
        Public Sub alert(prompt As String)
            If scriptAnnoyances < Byte.MaxValue Then
                MsgBox(prompt, CType(vbOKOnly, MsgBoxStyle), CStr(scriptAnnoyances))
                scriptAnnoyances += CShort(1)
            End If
        End Sub
        Public Function prompt(p As String) As String
            If scriptAnnoyances < Byte.MaxValue Then
                Return InputBox(p, CStr(scriptAnnoyances))
                scriptAnnoyances += CShort(1)
            End If
            Return ""
        End Function
    End Class
    Public Class ConsoleWrapper
        Public Sub log(t As Object)
            If scriptAnnoyances < Byte.MaxValue Then
                Logger.log(CStr(t), Logger.LogLevel.INFO)
                scriptAnnoyances += CShort(1)
            End If
        End Sub
        Public Sub logDebug(t As Object)
            If scriptAnnoyances < Byte.MaxValue Then
                Debug.WriteLine(CStr(t))
                scriptAnnoyances += CShort(1)
            End If
        End Sub
    End Class
    Public Class MojangAPIWrapper
        Public Function verify(accessToken As String) As Boolean
            Return PremiumVerifier.getAccessTokenValid(accessToken, PremiumVerifier.ClientToken, False)
        End Function
        Public Sub refresh(accessToken As String)
            PremiumVerifier.refreshAccessToken(accessToken, PremiumVerifier.ClientToken, False)
        End Sub
        Public Function getUserInfo() As String
            If ScriptPermMgr.hasPermission(ScriptPermMgr.Permissions.PERMISSION_SENSITIVE_DATA) Then
                Return PremiumVerifier.getUserInfo()
            End If
            Return ""
        End Function
    End Class
    Public Class PremiumWrapper
        Public ReadOnly Property accessToken As String
            Get
                If ScriptPermMgr.hasPermission(ScriptPermMgr.Permissions.PERMISSION_SENSITIVE_DATA) Then
                    Return PremiumVerifier.AccessToken
                End If
                Return ""
            End Get
        End Property
        Public ReadOnly Property username As String
            Get
                Return PremiumVerifier.Username
            End Get
        End Property
        Public ReadOnly Property useremail As String
            Get
                If ScriptPermMgr.hasPermission(ScriptPermMgr.Permissions.PERMISSION_SENSITIVE_DATA) Then
                    Return PremiumVerifier.UserEmail
                End If
                Return ""
            End Get
        End Property
    End Class
    Public Class LauncherWrapper
        Public Class LanguageWrapper
            Public Sub addLocal(key As String, lang As String, local As String)
                I18n.addKey(key, I18n.Language.getFromISOName(lang), local)
            End Sub
            Public Function translate(key As String, lang As String) As String
                Return I18n.translate(key, I18n.Language.getFromISOName(lang))
            End Function
            Public Function translate(key As String) As String
                Return I18n.translate(key)
            End Function
            Public ReadOnly Property currentLang As String
                Get
                    Return I18n.currentLanguage.getISOName
                End Get
            End Property
        End Class
        Public ReadOnly Property lang As New LanguageWrapper
        Public Class ServerWrapper
            Public Function checksum(path As String) As String
                Return BasicEncryption.getMD5Checksum(path)
            End Function
            Public Sub fetch()
                If scriptAnnoyances < Short.MaxValue Then
                    ServerSideManager.fetchFromServer()
                    scriptAnnoyances += CShort(1)
                End If
            End Sub
            Public Function send(url As String, method As String, payload As String) As String
                If ScriptPermMgr.hasPermission(ScriptPermMgr.Permissions.PERMISSION_INTERNET_ACCESS) Then
                    Return ServerSideManager.Post(url, payload, "application/json", method)
                End If
                Return ""
            End Function
        End Class
        Public ReadOnly Property server As New ServerWrapper()
        Public Class ToastWrapper
            Public Sub add(title As String, text As String)
                If scriptAnnoyances < Short.MaxValue Then
                    ToastRenderer.addToast(New Toast(title, text, Nothing, True, MCLauncher.Toast.ToastLength.NotYourOrdinaryToast))
                    scriptAnnoyances += CShort(1)
                End If
            End Sub
            Public Sub add(title As String, text As String, bmp As Bitmap)
                If scriptAnnoyances < Short.MaxValue Then
                    ToastRenderer.addToast(New Toast(title, text, bmp, True, MCLauncher.Toast.ToastLength.NotYourOrdinaryToast))
                    scriptAnnoyances += CShort(1)
                End If
            End Sub
        End Class
        Public ReadOnly Property toast As New ToastWrapper()
        Public Class DataWrapper
            Public Sub save(name As String, data As String)
                If Not scriptData.ContainsKey(name) Then
                    scriptData.Add(name, data)
                Else
                    scriptData(name) = data
                End If
            End Sub
            Public Function read(name As String) As String
                Dim ret As String = ""
                ThreadWrapper.Mutex.WaitOne()
                If scriptData.ContainsKey(name) Then
                    ret = scriptData(name)
                End If
                ThreadWrapper.Mutex.ReleaseMutex()
                Return ret
            End Function
            Public Sub delete(name As String)
                ThreadWrapper.Mutex.WaitOne()
                Dim list As New Dictionary(Of String, String)(scriptData)
                ThreadWrapper.Mutex.ReleaseMutex()
                For Each item As KeyValuePair(Of String, String) In list
                    If item.Key = name Then
                        scriptData.Remove(item.Key)
                    End If
                Next
            End Sub
            Public Function exist(name As String) As Boolean
                Dim ret As Boolean = False
                ThreadWrapper.Mutex.WaitOne()
                ret = scriptData.ContainsKey(name)
                ThreadWrapper.Mutex.ReleaseMutex()
                Return ret
            End Function
        End Class
        Public ReadOnly Property data As New DataWrapper()
        Public Class PermissionWrapper
            Public ReadOnly Property sensitive_data As ScriptPermMgr.Permissions = ScriptPermMgr.Permissions.PERMISSION_SENSITIVE_DATA
            Public ReadOnly Property unsafe_code As ScriptPermMgr.Permissions = ScriptPermMgr.Permissions.PERMISSION_POTENTIAL_UNSAFE_CODE
            Public ReadOnly Property internet As ScriptPermMgr.Permissions = ScriptPermMgr.Permissions.PERMISSION_INTERNET_ACCESS
            Public Sub create()
                ScriptPermMgr.newSession()
            End Sub
            Public Function loadPerm(identifier As String) As Boolean
                Return ScriptPermMgr.resumeSession(identifier)
            End Function
            Public Sub request(ParamArray p As ScriptPermMgr.Permissions())
                tempPause = True
                scriptStopwatch.Stop()
                ScriptPermMgr.requestPermissions(p)
                scriptStopwatch.Start()
                tempPause = False
            End Sub
            Public Function setPermPersist(bool As Boolean) As String
                ScriptPermMgr.setShouldPersist(bool)
                Return ScriptPermMgr.getCurrentGuid.ToString()
            End Function
            Public Function canI(perm As ScriptPermMgr.Permissions) As Boolean
                Return ScriptPermMgr.hasPermission(perm)
            End Function
            Public Function listAllPerm() As String
                Return Newtonsoft.Json.JsonConvert.SerializeObject(ScriptPermMgr.currentPermissionSet)
            End Function
        End Class
        Public ReadOnly Property perm As New PermissionWrapper()
        Public Sub test()
            MsgBox(String.Format("{0}", "Too many args", "123"))
            MsgBox(String.Format("{0},{1}", "Insufficient args"))
        End Sub
        Public Function getScriptRunningTime() As Long
            Return scriptStopwatch.ElapsedMilliseconds
        End Function
        Public Sub gc()
            System.GC.Collect()
        End Sub
        Public Function getConfigString() As String
            If Not ScriptPermMgr.hasPermission(ScriptPermMgr.Permissions.PERMISSION_SENSITIVE_DATA) Then Return ""
            If Not IO.File.Exists("config.cfg") Then Return ""
            Using config As New IO.StreamReader("config.cfg")
                Return BasicEncryption.decodeBase64(config.ReadToEnd())
            End Using
        End Function
        Public Sub flushConfig()
            ConfigManager.writeToConfig()
        End Sub
    End Class
#End Region
    Public Shared jsContext As Noesis.Javascript.JavascriptContext
    Public Shared scriptData As New Dictionary(Of String, String)
    Private Shared scriptStopwatch As Stopwatch
    Private Shared scriptAnnoyances As Short = 0
    Private Shared tempPause As Boolean = False
    Private Shared tooLongPromptTime As Integer
    Private Shared immediateDeathTime As Integer
    Private Sub New()
    End Sub

    Shared Sub New()
        ThreadWrapper.createThread("JavaScript-V8")
        ThreadWrapper.createThread("JavaScript-V8-Helper")
        jsContext = New Noesis.Javascript.JavascriptContext()
        jsContext.SetParameter("DotNet", New DotNetCodeWrapper())
        jsContext.SetParameter("console", New ConsoleWrapper())
        jsContext.SetParameter("MojangAPI", New MojangAPIWrapper())
        jsContext.SetParameter("Premium", New PremiumWrapper())
        jsContext.SetParameter("Launcher", New LauncherWrapper())

        jsContext.Run("alert = DotNet.alert;prompt = DotNet.prompt;")
    End Sub
    Public Shared Sub run(text As String)
        ThreadWrapper.addScheduledTask("JavaScript-V8", $"mdzzqwq_{(New Random()).Next(1, 10001)}",
                                 Sub()
                                     Try
                                         scriptAnnoyances = 0
                                         scriptStopwatch = New Stopwatch()
                                         scriptStopwatch.Start()
                                         jsContext.Run(text)
                                         scriptStopwatch.Stop()
                                         ScriptPermMgr.endSession()
                                     Catch ex As Noesis.Javascript.JavascriptException
                                         MsgBox(ex.Message)
                                     Catch ex As AccessViolationException
                                         MsgBox(ex.Message, vbCritical Or vbOKOnly, "Critical error")
                                         End
                                     End Try
                                 End Sub)
        ThreadWrapper.addScheduledTask("JavaScript-V8-Helper", $"mdzzqwq_{(New Random()).Next(1, 10001)}",
                                 Sub()
                                     Dim hasRefused As Boolean = False
                                     While scriptStopwatch IsNot Nothing AndAlso (scriptStopwatch.IsRunning Or tempPause)
                                         If (Not hasRefused) AndAlso scriptStopwatch.ElapsedMilliseconds > tooLongPromptTime Then
                                             If MsgBox(I18n.translate("warn.script.tooLong", CStr(tooLongPromptTime \ 1000)), vbYesNo, "JavaScript") = vbYes Then
                                                 jsContext.TerminateExecution()
                                                 scriptStopwatch.Stop()
                                                 Exit Sub
                                             Else
                                                 hasRefused = True
                                             End If
                                         End If
                                         If scriptStopwatch.ElapsedMilliseconds > immediateDeathTime Then
                                             jsContext.TerminateExecution()
                                             scriptStopwatch.Stop()
                                             MsgBox(I18n.translate("err.script.forceDeath", CStr(immediateDeathTime \ 1000)), CType(vbCritical + vbOKOnly, MsgBoxStyle), "JavaScript")
                                             Exit Sub
                                         End If
                                     End While
                                 End Sub)
        Return
        'Old code
        Dim cmdArgs() As String = parseParentheness(text)
        If cmdArgs.Length >= 1 Then
            'evaluateFunctionReturn(text)
        End If
    End Sub

    Public Shared Sub readConfig()
        scriptData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(ConfigManager.ConfigValue.scriptData)
        If scriptData Is Nothing Then
            scriptData = New Dictionary(Of String, String)
        End If
        tooLongPromptTime = ConfigManager.ConfigValue.scriptTooLongTime
        immediateDeathTime = ConfigManager.ConfigValue.scriptImmediateDeathTime
    End Sub

    Public Shared Sub updateConfig()
        ConfigManager.ConfigValue.scriptData = Newtonsoft.Json.JsonConvert.SerializeObject(scriptData)
    End Sub

    Public Shared Function parseQuotationMarks(textToParse As String) As String
        Dim text As String = textToParse.Trim
        If text.IndexOf("""") < 0 OrElse text.LastIndexOf("""") = text.IndexOf("""") Then
            Return text
        End If
        Dim quoStart As Integer = text.IndexOf("""")
        Dim quoEnd As Integer = text.LastIndexOf("""")
        Return text.Substring(quoStart + 1, quoEnd - (quoStart + 1))
    End Function

    Public Shared Function parseParentheness(textToParse As String) As String()
        Dim text As String = textToParse.Trim()
        If text.IndexOf("(") <= 0 OrElse text.LastIndexOf(")") <= 0 Then
            Return New String() {text}
        End If
        Dim parStart As Integer = text.IndexOf("(")
        Dim parEnd As Integer = text.LastIndexOf(")")
        Dim cmdArgs As New List(Of String)
        cmdArgs.Add(text.Substring(0, parStart))
        Dim quoStr As String = ""
        Dim q As Boolean
        Dim split() As String = text.Substring(parStart + 1, parEnd - parStart - 1).Split(CType(",", Char))
        For i As Integer = 0 To split.Length - 1
            Dim arg As String = split(i)
            Dim m As Boolean = False
            If arg.IndexOf("""") > -1 Then
                If arg.LastIndexOf("""") = arg.IndexOf("""") Then
                    If q Then m = True
                    q = True
                    GoTo ifcond
                Else
                    m = True
                    q = True
                    GoTo ifcond
                End If
            End If
            If q Then
                m = False
                q = True
                GoTo ifcond
            End If
            If False Then
ifcond:
                If (Not m) AndAlso q Then
                    GoTo ifcondappend
                ElseIf m AndAlso q Then
                    GoTo ifcondparse
                ElseIf m AndAlso Not q Then
                    q = True
                    GoTo ifcondappend
                End If
ifcondappend:
                quoStr += arg & If(i < split.Length - 1, ",", "")
                Continue For
ifcondparse:
                quoStr += arg
                cmdArgs.Add(parseQuotationMarks(quoStr))
                quoStr = ""
                q = False
                m = False
                Continue For
            End If
            cmdArgs.Add(arg.Trim)
        Next
        Return cmdArgs.ToArray
    End Function

    Public Shared Function DynamicCompileAndRunDotNetCode(canReturn As Boolean, code As String) As Object
        Dim codeCompiler As VBCodeProvider = New VBCodeProvider
        Dim param As New CodeDom.Compiler.CompilerParameters
        param.GenerateInMemory = True
        param.GenerateExecutable = False
        param.ReferencedAssemblies.Add("mscorlib.dll")
        param.ReferencedAssemblies.Add("System.dll")
        param.ReferencedAssemblies.Add("Microsoft.VisualBasic.dll")
        Dim toCompile As String = "
Option Strict On
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Namespace DFXL
Public Class CR
Public " & If(canReturn, "Function", "Sub") & " Run()
" & If(canReturn, "Return ", "") & code & "
End " & If(canReturn, "Function", "Sub") & "
End Class
End Namespace"
        Dim results As CodeDom.Compiler.CompilerResults = codeCompiler.CompileAssemblyFromSource(param, toCompile)
        If results.Errors.HasErrors Then
            MsgBox("Failed to compile code:" & (Function() As String
                                                    Dim retStr As String = ""
                                                    For Each e As CodeDom.Compiler.CompilerError In results.Errors
                                                        If Not e.IsWarning Then
                                                            retStr += e.ErrorText & vbCrLf
                                                        End If
                                                    Next
                                                    Return retStr
                                                End Function).Invoke())
            Return Nothing
        End If
        Dim assembly As Reflection.Assembly = results.CompiledAssembly
        Dim objAssem As Object = assembly.CreateInstance("DFXL.CR")
        If canReturn Then
            Return objAssem.GetType.InvokeMember("Run", Reflection.BindingFlags.InvokeMethod, Nothing, objAssem, Nothing)
        Else
            objAssem.GetType.InvokeMember("Run", Reflection.BindingFlags.InvokeMethod, Nothing, objAssem, Nothing)
            Return Nothing
        End If
    End Function
End Class
