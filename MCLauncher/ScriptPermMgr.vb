Public Class ScriptPermMgr

    Public NotInheritable Class Permissions
        Private Sub New()
        End Sub
        Public Shared ReadOnly PERMISSION_SENSITIVE_DATA As New Permissions("PERM_SENSITIVE_DATA", "info.jsperm.sensitive", False)
        Public Shared ReadOnly PERMISSION_POTENTIAL_UNSAFE_CODE As New Permissions("PERM_UNSAFE_CODE", "info.jsperm.codeUnsafe", True)
        Public Shared ReadOnly PERMISSION_INTERNET_ACCESS As New Permissions("PERM_INTERNET", "info.jsperm.internet", True)
        Public ReadOnly identifier As String
        Public ReadOnly desc As String
        Public ReadOnly lethal As Boolean
        Private Shared values As List(Of Permissions)
        Private Sub New(strIdf As String, desc As String, lethal As Boolean)
            If values Is Nothing Then
                values = New List(Of Permissions)
            End If
            identifier = strIdf
            Me.desc = desc
            Me.lethal = lethal
            values.Add(Me)
        End Sub
        Public Shared Function parse(identifier As String) As Permissions
            For Each item As Permissions In values
                If item.identifier = identifier Then
                    Return item
                End If
            Next
            Return Nothing
        End Function
    End Class
    Private NotInheritable Class MutablePermissions
        Public Sub New(strIdf As String, desc As String)
        End Sub
        Public identifier As String
        Public desc As String
        Public lethal As Boolean
        Public Function toImmutable() As Permissions
            Return Permissions.parse(identifier)
        End Function
    End Class

    Private Shared persist As Boolean = False
    Private Shared curScript As Guid? = Nothing
    Private Shared curPermSet As List(Of Permissions)
    Private Shared requestCount As Integer = 0
    Public Shared ReadOnly Property currentPermissionSet As ObjectModel.ReadOnlyCollection(Of Permissions)
        Get
            Return curPermSet.AsReadOnly()
        End Get
    End Property
    Private Shared registeredScripts As Dictionary(Of String, List(Of Permissions))
    Public Shared Sub newSession()
        curScript = Guid.NewGuid()
        registeredScripts.Add(curScript.ToString(), New List(Of Permissions))
        curPermSet = registeredScripts(curScript.ToString())
        requestCount = 0
    End Sub

    Public Shared Sub endSession()
        If Not persist Then
            If curScript.HasValue Then
                registeredScripts.Remove(curScript.ToString)
            End If
        End If
        persist = False
        curScript = Nothing
        curPermSet = Nothing
        requestCount = 0
    End Sub

    Public Shared Function getCurrentGuid() As Guid?
        Return curScript
    End Function

    Public Shared Function hasPermission(perm As Permissions) As Boolean
        If curPermSet IsNot Nothing AndAlso curPermSet.Contains(perm) Then
            Return True
        End If
        Return False
    End Function

    Public Shared Sub grantPermissions(ParamArray perm As Permissions())
        If curPermSet IsNot Nothing Then
            curPermSet.AddRange(perm)
        End If
    End Sub

    Public Shared Sub requestPermissions(ParamArray perm As Permissions())
        If requestCount > 2 Then Return
        Dim list As Dictionary(Of Permissions, Boolean) = (New ScriptPermReq()).ShowPerm(perm)
        For Each kvp As KeyValuePair(Of Permissions, Boolean) In list
            If kvp.Value Then
                curPermSet.Add(kvp.Key)
            End If
        Next
        requestCount += 1
    End Sub

    Public Shared Function resumeSession(guid As String) As Boolean
        Dim out As Guid
        If System.Guid.TryParse(guid, out) Then
            If registeredScripts.ContainsKey(out.ToString) Then
                curScript = out
                curPermSet = registeredScripts(out.ToString)
                persist = True
            Else
                Return False
            End If
        Else
            Return False
        End If
        Return True
    End Function

    Public Shared Sub setShouldPersist(bool As Boolean)
        persist = bool
    End Sub

    Public Shared Sub readConfig()
        Dim temp As Dictionary(Of String, List(Of MutablePermissions)) = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Dictionary(Of String, List(Of MutablePermissions)))(ConfigManager.ConfigValue.perms)
        registeredScripts = New Dictionary(Of String, List(Of Permissions))
        For Each item As KeyValuePair(Of String, List(Of MutablePermissions)) In temp
            registeredScripts.Add(item.Key, New List(Of Permissions))
            For Each perm As MutablePermissions In item.Value
                If perm Is Nothing Then Continue For
                registeredScripts(item.Key).Add(perm.toImmutable())
            Next
        Next
    End Sub

    Public Shared Sub updateConfig()
        ConfigManager.ConfigValue.perms = Newtonsoft.Json.JsonConvert.SerializeObject(registeredScripts)
    End Sub
End Class
