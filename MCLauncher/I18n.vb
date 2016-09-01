Imports System.IO
''' <summary>
''' Translation class.
''' </summary>
Public NotInheritable Class I18n
    Private Sub New()
    End Sub
    Public Class Language
        Private Shared listOfLang As New List(Of Language)
        Public Shared ReadOnly Property RegisteredLanguages As List(Of Language)
            Get
                Return New List(Of Language)(listOfLang)
            End Get
        End Property
        Public Sub New(unlocName As String, locName As String)
            isoName = unlocName
            localName = locName
            id = globalID
            globalID += 1
            listOfLang.Add(Me)
        End Sub
        Private Shared globalID As Integer = 0
        Private ReadOnly id As Integer
        Public Function getID() As Integer
            Return id
        End Function
        Private ReadOnly isoName As String
        Public Function getISOName() As String
            Return isoName
        End Function
        Private ReadOnly localName As String
        Public Function getLocalizedName() As String
            Return localName
        End Function
        Public Shared Operator =(left As Language, right As Language) As Boolean
            If left.id = right.id Then
                Return True
            Else Return False
            End If
        End Operator
        Public Shared Operator <>(left As Language, right As Language) As Boolean
            If Not left = right Then
                Return True
            Else Return False
            End If
        End Operator
        Public Shared Function getFromISOName(isoName As String) As Language
            For Each item As Language In listOfLang
                If item.isoName = isoName Then
                    Return item
                End If
            Next
            Return Languages.English
        End Function
    End Class
    Public Structure Languages
        Public Shared English As New Language("en", "English")
        Public Shared TradChin As New Language("zh_TW", "中文繁體")
        Public Shared SimpChin As New Language("zh_CN", "中文简体")
    End Structure
    Public Shared currentLanguage As Language = Languages.TradChin
    Private Class KeyMapping
        Public lang As Language
        Public localized As String = NO_TRANSLATION
    End Class
    Private Class KeyMappings
        Public map As New List(Of KeyMapping)
    End Class
    Private Shared map As New Dictionary(Of String, KeyMappings)
    Public Const NO_TRANSLATION As String = "Your language has no translation for {0}"
    Public Const NO_TRANSLATION_FOR_LANG As String = "{0} has no translation for {1}"
    Public Const NO_TRANSLATION_RAW As String = "translate.no"
    ''' <summary>
    ''' Returns whether the given parameter key was registered or not.
    ''' </summary>
    ''' <param name="key">The key to check for.</param>
    ''' <returns>If true, the key is registered in the translation mapping, but not necessarily localized.</returns>
    Public Shared Function canTranslate(key As String) As Boolean
        Return map.ContainsKey(key)
    End Function
    Private Shared locked As Boolean = False

    Public Shared Sub addKey(key As String, lang As Language, translation As String)
        If map.ContainsKey(key) Then
            Dim trlForLng As KeyMapping = Nothing
            For Each item As KeyMapping In map(key).map
                If item.lang = lang Then
                    trlForLng = item
                    Exit For
                End If
            Next
            If trlForLng IsNot Nothing Then
                trlForLng.localized = translation
            Else
                map(key).map.Add(New KeyMapping() With {.lang = lang, .localized = translation})
            End If
        Else
            Dim mapList As New KeyMappings()
            mapList.map.Add(New KeyMapping() With {.lang = lang, .localized = translation})
            map.Add(key, mapList)
        End If
    End Sub

    Private Shared Function getKey(key As String) As String
        For i As Integer = 0 To map(key).map.Count - 1
            If map(key).map(i).lang = currentLanguage Then
                Return map(key).map(i).localized
            End If
        Next
        Return String.Format(NO_TRANSLATION, key)
    End Function

    Private Shared Function getKeyForLang(key As String, lang As Language) As String
        For i As Integer = 0 To map(key).map.Count - 1
            If map(key).map(i).lang = lang Then
                Return map(key).map(i).localized
            End If
        Next
        Return String.Format(NO_TRANSLATION_FOR_LANG, lang.getLocalizedName, key)
    End Function

    Public Shared Function translate(key As String) As String
        Return If(canTranslate(key), getKey(key), key)
    End Function

    Public Shared Function translate(key As String, ParamArray format() As String) As String
        Return If(canTranslate(key), String.Format(translate(key), format), key)
    End Function

    Public Shared Function translate(key As String, lang As Language) As String
        Return If(canTranslate(key), getKeyForLang(key, lang), key)
    End Function

    Public Shared Function translate(key As String, lang As Language, ParamArray format() As String) As String
        Return String.Format(translate(key, lang), format)
    End Function

    Private Shared Function getKeyMappings(key As String) As KeyMappings
        Return If(map.ContainsKey(key), map(key), Nothing)
    End Function

    Public Shared Sub loadTranslationsFromFileIntoMappings()
        If Directory.Exists(Application.StartupPath & "\lang") Then
            For Each langfile As String In Directory.GetFiles(Application.StartupPath & "\lang", "*.lang", SearchOption.TopDirectoryOnly)
                Dim langBelongto As Language = Language.getFromISOName(Path.GetFileNameWithoutExtension(langfile))
                Dim r As New StreamReader(langfile)
                While Not r.EndOfStream
                    Dim line As String = r.ReadLine
                    If line Like "#*" Then Continue While
                    If line Like "*=*" Then
                        Dim assignments As String() = line.Split(CType("=", Char()))
                        If assignments.Length <> 2 Then
                            Console.WriteLine("Invalid language key. See lang file " & langfile)
                            Continue While
                        End If
                        Dim raw As String = assignments(0)
                        Dim localized As String = assignments(1).Replace("\n", vbCrLf)
                        If String.IsNullOrEmpty(raw) OrElse String.IsNullOrEmpty(localized) Then
                            Console.WriteLine("Empty rawtext or localized. See lang file " & langfile)
                            Continue While
                        End If
                        Dim map As New KeyMapping() With {.lang = langBelongto, .localized = localized}
                        Dim keymappings As KeyMappings = getKeyMappings(raw)
                        If keymappings IsNot Nothing Then
                            keymappings.map.Add(map)
                            'Console.WriteLine("Added " & raw & " from " & langBelongto.getISOName & " to mappings!")
                        Else
                            keymappings = New KeyMappings()
                            keymappings.map.Add(map)
                            I18n.map.Add(raw, keymappings)
                            'Console.WriteLine("Created mapping " & raw & " for " & langBelongto.getISOName & " since it wasn't there!")
                        End If
                    End If
                End While
            Next
        End If
    End Sub

    Public Shared Sub updateConfig()
        ConfigManager.ConfigValue.selectedLang = currentLanguage.getISOName
    End Sub

    Public Shared Sub readConfig()
        currentLanguage = Language.getFromISOName(ConfigManager.ConfigValue.selectedLang)
    End Sub

    Public Shared Sub requestHandleTranslationInForm(f As Form)
        For Each ctrl As Control In f.Controls
            If ScriptServer.parseParentheness(ctrl.Text).Length = 2 Then
                If ScriptServer.parseParentheness(ctrl.Text)(0) = "translate" Then
                    ctrl.Text = translate(ScriptServer.parseParentheness(ctrl.Text)(1))
                End If
            End If
            For Each child As Control In ctrl.Controls
                If ScriptServer.parseParentheness(child.Text).Length = 2 Then
                    If ScriptServer.parseParentheness(child.Text)(0) = "translate" Then
                        child.Text = translate(ScriptServer.parseParentheness(child.Text)(1))
                    End If
                End If
            Next
        Next
        If ScriptServer.parseParentheness(f.Text).Length = 2 Then
            If ScriptServer.parseParentheness(f.Text)(0) = "translate" Then
                f.Text = translate(ScriptServer.parseParentheness(f.Text)(1))
            End If
        End If
    End Sub

    Public Shared Sub lock()
        If Not locked Then locked = True Else Throw New InvalidOperationException()
    End Sub
End Class
