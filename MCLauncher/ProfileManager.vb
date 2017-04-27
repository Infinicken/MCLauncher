Public NotInheritable Class ProfileManager
    Private Sub New()
    End Sub

    Public Class Profile
        Protected Friend Sub New(identifier As String)
            Me.identifier = identifier
        End Sub
        Public ReadOnly identifier As String
    End Class

    Public Shared Function getProfiles() As List(Of Profile)
        Return Nothing
    End Function

    Public Shared Function getProfile(idf As String) As Profile
        Return Nothing
    End Function

    Public Shared Sub setProfile(prof As Profile)

    End Sub

    Public Shared Function createProfile(idf As String) As Profile
        Return New Profile(idf)
    End Function
End Class
