Imports System.Security.Cryptography

Public Class BasicEncryption
    Public Shared Function func_46293525_1_(_p_64909215_1_ As String, Optional _p_64909215_2_ As System.Text.Encoding = Nothing) As String
        If _p_64909215_2_ Is Nothing Then
            Return System.Convert.ToBase64String(Text.Encoding.UTF8.GetBytes(_p_64909215_1_))
        End If
        Return System.Convert.ToBase64String(_p_64909215_2_.GetBytes(_p_64909215_1_))
    End Function

    Public Shared Function func_46293525_2_(_p_56094950_1_ As String) As String
        Return Text.Encoding.UTF8.GetString(Convert.FromBase64String(_p_56094950_1_))
    End Function

    Public Shared Function getMD5Checksum(filepath As String) As String
        Using s = MD5.Create
            Using stream As IO.Stream = IO.File.OpenRead(filepath)
                Return BitConverter.ToString(s.ComputeHash(stream)).Replace("-", "").ToLower
            End Using
        End Using
    End Function
End Class
