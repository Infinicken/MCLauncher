Imports System.Security.Cryptography

Public Class BasicEncryption
    Public Shared Function encodeBase64(str As String, Optional encoding As System.Text.Encoding = Nothing) As String
        If encoding Is Nothing Then
            Return Convert.ToBase64String(Text.Encoding.UTF8.GetBytes(str))
        End If
        Return Convert.ToBase64String(encoding.GetBytes(str))
    End Function

    Public Shared Function decodeBase64(str As String, Optional encoding As System.Text.Encoding = Nothing) As String
        Try
            If encoding Is Nothing Then
                Return Text.Encoding.UTF8.GetString(Convert.FromBase64String(str))
            End If
            Return encoding.GetString(Convert.FromBase64String(str))
        Catch
            Return str
        End Try
    End Function

    Public Shared Function getMD5Checksum(filepath As String) As String
        Using s = MD5.Create
            Using stream As IO.Stream = IO.File.OpenRead(filepath)
                Return BitConverter.ToString(s.ComputeHash(stream)).Replace("-", "").ToLower
            End Using
        End Using
    End Function
End Class
