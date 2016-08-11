Option Strict On

Public NotInheritable Class ScriptServer
#Region "Deprecated"
    Public Class ScriptHandler
        Public Delegate Function onCommand(cmd As String, args() As Object) As Object
        Public Delegate Function willHandleCommand(cmd As String) As Boolean
        Public ReadOnly canHandle As willHandleCommand
        Public ReadOnly execCmd As onCommand
        Public Sub New(willHandle As willHandleCommand, cmdExec As onCommand)
            canHandle = willHandle
            execCmd = cmdExec
        End Sub
        Public ReadOnly ide_ As String
    End Class

    Public Class VariableHandler
        Public Delegate Function ValueObtain() As Object
        Public ReadOnly getValue As ValueObtain
        Public ReadOnly name As String
        Public Sub New(name As String, g As ValueObtain)
            Me.name = name
            getValue = g
        End Sub
    End Class

    Public Class ScriptNamespace
        Public Sub New(domain As String, ParamArray funcAndFields() As Object)
            Me.domain = domain
            For Each item In funcAndFields
                If TypeOf (item) Is ScriptHandler Then
                    methods.Add(CType(item, ScriptHandler))
                ElseIf TypeOf (item) Is VariableHandler Then
                    fields.Add(CType(item, VariableHandler))
                Else
                    Throw New ArgumentException(NameOf(funcAndFields))
                End If
            Next
        End Sub
        Protected Friend ide_desc As String
        Public ReadOnly domain As String
        Protected Friend methods As New List(Of ScriptHandler)
        Protected Friend fields As New List(Of VariableHandler)
    End Class

    Private Shared listOfHandlers As New List(Of ScriptHandler)
    Private Shared listOfVars As New List(Of VariableHandler)
    Public Shared namespaces As New List(Of ScriptNamespace)
#End Region
#Region ".NET Wrapper Classes"
    Public Class DotNetCodeWrapper
        Public Sub compileRunnable(code As String)
            ScriptServer.DynamicCompileAndRunDotNetCode(False, code)
        End Sub
        Public Function compileCallable(code As String) As Object
            Return ScriptServer.DynamicCompileAndRunDotNetCode(True, code)
        End Function
        Public Sub alert(prompt As String)
            MsgBox(prompt)
        End Sub
        Public Function prompt(p As String) As String
            Return InputBox(p)
        End Function
    End Class
    Public Class ConsoleWrapper
        Public Sub log(t As Object)
            Logger.log(CStr(t), Logger.LogLevel.INFO)
        End Sub
    End Class
    Public Class MojangAPIWrapper
        Public Function verify(accessToken As String) As Boolean
            Return PremiumVerifier.getAccessTokenValid(accessToken, PremiumVerifier.ClientToken, False)
        End Function
        Public Sub refresh(accessToken As String)
            PremiumVerifier.refreshAccessToken(accessToken, PremiumVerifier.ClientToken, False)
        End Sub
        Public Function getInfo() As String
            Return PremiumVerifier.getUserInfo()
        End Function
        Public ReadOnly Property accessToken As String
            Get
                Return PremiumVerifier.AccessToken
            End Get
        End Property
    End Class
    Public Class PremiumWrapper
        Public ReadOnly Property accessToken As String
            Get
                Return PremiumVerifier.AccessToken
            End Get
        End Property
        Public ReadOnly Property username As String
            Get
                Return PremiumVerifier.Username
            End Get
        End Property
        Public ReadOnly Property useremail As String
            Get
                Return PremiumVerifier.UserEmail
            End Get
        End Property
    End Class
    Public Class LauncherWrapper
        Public Class LanguageWrapper
            Friend Sub New()
            End Sub
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
                ServerSideManager.fetchFromServer()
            End Sub
            Public Function send(url As String, method As String, payload As String) As String
                Return ServerSideManager.Post(url, payload, "application/json", method)
            End Function
        End Class
        Public ReadOnly Property server As New ServerWrapper()
        Public Class ToastWrapper
            Public Sub add(title As String, text As String)
                ToastRenderer.addToast(New Toast(title, text))
            End Sub
        End Class
        Public ReadOnly Property toast As New ToastWrapper()
        Public Class DataWrapper
            Public Sub save(name As String, data As String)
                scriptData.Add(New ScriptSaveData(name) With {.data = data})
            End Sub
            Public Function read(name As String) As String
                Dim ret As String = ""
                Threads.Mutex.WaitOne()
                For Each item As ScriptSaveData In scriptData
                    If item.name = name Then
                        ret = item.data
                    End If
                Next
                Threads.Mutex.ReleaseMutex()
                Return ret
            End Function
            Public Sub delete(name As String)
                Threads.Mutex.WaitOne()
                Dim list As New List(Of ScriptSaveData)(scriptData)
                Threads.Mutex.ReleaseMutex()
                For Each item As ScriptSaveData In list
                    If item.name = name Then
                        scriptData.Remove(item)
                    End If
                Next
            End Sub
        End Class
        Public ReadOnly Property data As New DataWrapper()
        Public Sub gc()
            System.GC.Collect()
        End Sub
        Public Function getConfigString() As String
            If Not IO.File.Exists("config.cfg") Then Return ""
            Dim config As New IO.StreamReader("config.cfg")
            Dim ret As String = BasicEncryption.func_46293525_2_(config.ReadToEnd())
            config.Close()
            config.Dispose()
            Return ret
        End Function
    End Class
#End Region
    Public Class ScriptSaveData
        Public Sub New(name As String)
            Me.name = name
        End Sub
        Public name As String = ""
        Public data As String = ""
    End Class
    Public Shared jsContext As New Noesis.Javascript.JavascriptContext
    Public Shared scriptData As New List(Of ScriptSaveData)
    Private Sub New()
    End Sub

    Shared Sub New()
        Threads.createThread("JavaScript-V8")
        jsContext.SetParameter("DotNet", New DotNetCodeWrapper())
        jsContext.SetParameter("console", New ConsoleWrapper())
        jsContext.SetParameter("MojangAPI", New MojangAPIWrapper())
        jsContext.SetParameter("Premium", New PremiumWrapper())
        jsContext.SetParameter("Launcher", New LauncherWrapper())

        jsContext.Run("alert = DotNet.alert;")
        jsContext.Run("prompt = DotNet.prompt;")
    End Sub
    Public Shared Sub run(text As String)
        Threads.addScheduledTask("JavaScript-V8", $"mdzzqwq_{(New Random()).Next(1, 10001)}",
                                 Sub()
                                     Try
                                         jsContext.Run(text)
                                     Catch ex As Noesis.Javascript.JavascriptException
                                         MsgBox(ex.Message)
                                     End Try
                                 End Sub)
        Return
        'Old code
        Dim cmdArgs() As String = parseParentheness(text)
        If cmdArgs.Length >= 1 Then
            'evaluateFunctionReturn(text)
        End If
    End Sub

    Public Shared Sub readConfig()
        scriptData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of ScriptSaveData))(ConfigManager.ConfigValue.scriptData)
        If scriptData Is Nothing Then
            scriptData = New List(Of ScriptSaveData)
        End If
    End Sub

    Public Shared Sub updateConfig()
        ConfigManager.ConfigValue.scriptData = Newtonsoft.Json.JsonConvert.SerializeObject(scriptData)
    End Sub

#Region "Deprecated"
    <Obsolete> Public Shared Sub addHandlers()
        'Namespace
        namespaces.Add(New ScriptNamespace("MojangAPI",
                                           New ScriptHandler(Function(cmd As String) cmd = "verify",
                                             Function(cmd As String, args() As Object) PremiumVerifier.getAccessTokenValid(args(0).ToString, PremiumVerifier.ClientToken, False)),
                                           New ScriptHandler(Function(cmd As String) cmd = "getInfo",
                                             Function(cmd As String, args() As Object) PremiumVerifier.getUserInfo())))
        namespaces.Add(New ScriptNamespace("Premium",
                                            New VariableHandler("accessToken", Function() PremiumVerifier.AccessToken),
                                            New VariableHandler("username", Function() PremiumVerifier.Username),
                                            New VariableHandler("useremail", Function() PremiumVerifier.UserEmail)))
        namespaces.Add(New ScriptNamespace("Server",
                                           New ScriptHandler(Function(cmd As String) cmd = "fetch",
                                             Function(cmd As String, args() As Object)
                                                 ServerSideManager.fetchFromServer()
                                                 Return Nothing
                                             End Function),
                                           New ScriptHandler(Function(cmd As String) cmd = "checksum",
                                            Function(cmd As String, args() As Object)
                                                Return BasicEncryption.getMD5Checksum(CStr(args(0)))
                                            End Function)))
        'Handlers
        listOfHandlers.Add(New ScriptHandler(Function(cmd As String) cmd = "alert",
                                             Function(cmd As String, args() As Object)
                                                 For Each arg As Object In args
                                                     MsgBox(If(TypeOf (arg) Is String, CType(arg, String), arg.ToString))
                                                 Next
                                                 Return Nothing
                                             End Function))
        listOfHandlers.Add(New ScriptHandler(Function(cmd As String) cmd = "prompt",
                                             Function(cmd As String, args() As Object)
                                                 Return InputBox(args(0).ToString)
                                             End Function))
        listOfHandlers.Add(New ScriptHandler(Function(cmd As String) cmd = "eval",
                                             Function(cmd As String, args() As Object)
                                                 Return evaluateFunctionReturn(Join(args, ","))
                                             End Function))
        listOfHandlers.Add(New ScriptHandler(Function(cmd As String) cmd = "help",
                           Function(cmd As String, args() As Object)
                               Dim str As String = "Useful functions: alert(promptObj), eval(evalStr), prompt(promptStr)," & vbCrLf &
                               "Use ! or """" to represent a string constant which shouldn't get evaluated. Examples are alert(!Hi), prompt(""text"")"
                               MsgBox(str)
                               Return Nothing
                           End Function))
        listOfHandlers.Add(New ScriptHandler(Function(cmd As String) cmd = "dotnet",
                           Function(cmd As String, args() As Object)
                               Dim conjureArgs As [Delegate] = Function() As String
                                                                   Dim r As String = ""
                                                                   For i = 1 To args.Length - 1
                                                                       r += CStr(args(i)) & vbCrLf
                                                                   Next
                                                                   Return r
                                                               End Function
                               If CStr(args(0)) = "true" Then
                                   Return DynamicCompileAndRunDotNetCode(True, CType(conjureArgs.DynamicInvoke(), String))
                               Else
                                   DynamicCompileAndRunDotNetCode(False, CType(conjureArgs.DynamicInvoke(), String))
                                   Return Nothing
                               End If
                           End Function))
        listOfHandlers.Add(New ScriptHandler(Function(cmd As String) cmd = "err",
                           Function(cmd As String, args() As Object)
                               Throw New Exception("Script exception")
                               Return Nothing
                           End Function))
        'Vars
    End Sub


    <Obsolete> Public Shared Function evaluateFunctionReturn(cmd As String) As Object
        Console.WriteLine("Evaluating: " & cmd)
        Dim cmdArgs() As String = parseParentheness(cmd)
        Dim c As String = cmdArgs(0)
        Dim newlist As New List(Of Object)
        For i = 1 To cmdArgs.Length - 1
            newlist.Add(getValueOf(cmdArgs(i)))
        Next
        Dim args As Object() = newlist.ToArray
        If c.LastIndexOf(".") > -1 Then
            Dim nspace As String = c.Remove(c.LastIndexOf("."))
            Dim cmdNsp As String = c.Substring(c.LastIndexOf(".") + 1)
            Console.WriteLine("Evaluating namespace " & nspace & " and function " & cmdNsp)
            For Each nsp As ScriptNamespace In namespaces
                If nsp.domain = nspace Then
                    For Each handler As ScriptHandler In nsp.methods
                        If handler.canHandle(cmdNsp) Then
                            Return handler.execCmd(cmdNsp, args)
                        End If
                    Next
                End If
            Next
        End If
        'Global funcs like alert()
        For Each handler As ScriptHandler In listOfHandlers
            If handler.canHandle(c) Then
                Return handler.execCmd(c, args)
            End If
        Next
        Return ""
    End Function

    <Obsolete> Public Shared Function getValueOf(var As String) As Object
        If var.Length = 0 Then Return var
        If var.IndexOf("!") = 0 Then GoTo returnconst
        If var.IndexOf("""") = 0 Then GoTo returnquotes
        Dim funcRet As String = evaluateFunctionReturn(var).ToString
        If funcRet <> "" Then Return funcRet
        Dim varRet As String = ""
        Dim c As String = var
        If c.LastIndexOf(".") > -1 Then
            Dim nspace As String = c.Remove(var.LastIndexOf("."))
            Dim cmdNsp As String = c.Substring(c.LastIndexOf(".") + 1)
            Console.WriteLine("Getting value " & cmdNsp & " from namespace " & nspace)
            For Each nsp As ScriptNamespace In namespaces
                If nsp.domain = nspace Then
                    For Each handler As VariableHandler In nsp.fields
                        If handler.name = cmdNsp Then
                            varRet = handler.getValue().ToString
                        End If
                    Next
                End If
            Next
        End If
        If varRet <> "" Then Return varRet
        'Global vars
        For Each i As VariableHandler In listOfVars
            If i.name = var.Substring(1) Then
                varRet = i.getValue().ToString
            End If
        Next
        If varRet <> "" Then Return varRet
        Return var
returnconst:
        Return var.Substring(1)
returnquotes:
        Return parseParentheness(var)
    End Function

    Public Shared Function parseQuotationMarks(textToParse As String) As String
        Dim text As String = textToParse.Trim
        If text.IndexOf("""") < 0 OrElse text.LastIndexOf("""") = text.IndexOf("""") Then
            Return text
        End If
        Dim quoStart As Integer = text.IndexOf("""")
        Dim quoEnd As Integer = text.LastIndexOf("""")
        Return text.Substring(quoStart + 1, quoEnd - (quoStart + 1))
    End Function
#End Region
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
