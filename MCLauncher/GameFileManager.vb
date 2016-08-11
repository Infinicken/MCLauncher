Public Class GameFileManager
    Public dlType As DLSRCTYPE = DLSRCTYPE.MOJANG

    Public Enum DLSRCTYPE
        MOJANG = 0
        PROVIDER = 1
    End Enum
    Private Sub GameFileManager_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        I18n.requestHandleTranslationInForm(Me)
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles checkMojang.CheckedChanged
        dlType = DLSRCTYPE.MOJANG
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles checkProvider.CheckedChanged
        If checkProvider.Checked AndAlso MsgBox(I18n.translate("warn.fromProvider"), CType(vbOKCancel + vbCritical, Global.Microsoft.VisualBasic.MsgBoxStyle), I18n.translate("warn")) = vbOK Then
            dlType = DLSRCTYPE.PROVIDER
        Else
            checkProvider.Checked = False
            checkMojang.Checked = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        AssetsManager.downloadForVersion(TextBox1.Text, Application.StartupPath + "\mc")
    End Sub

    Private Sub TextBox1_Leave(sender As Object, e As EventArgs) Handles TextBox1.Leave
        If Not validateMCVersion(TextBox1.Text) Then
            MsgBox(I18n.translate("err.invalidMCVer", TextBox1.Text), CType(vbOKOnly + vbCritical, MsgBoxStyle))
            TextBox1.Text = "1.7.10"
        End If
    End Sub

    Public Shared Function validateMCVersion(toValidate As String) As Boolean
        If String.IsNullOrEmpty(toValidate) Then Return False
        If toValidate Like "#.#" OrElse toValidate Like "#.##" OrElse toValidate Like "#.#.##" OrElse toValidate Like "#.##.##" OrElse toValidate Like "#.#.#" OrElse toValidate Like "#.##.#" OrElse toValidate Like "#.#-pre#" OrElse toValidate Like "#.##-pre#" OrElse toValidate Like "##w##[a-e]" Then
            Return True
        End If
        Return False
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        newThreadedDownloader("https://s3.amazonaws.com/Minecraft.Download/versions/" & TextBox1.Text & "/" & TextBox1.Text & ".jar", Application.StartupPath + "\mc\ver\" & TextBox1.Text & ".jar")
    End Sub

    Public Shared Sub newThreadedDownloader(dl As String, pt As String)
        If Not IO.Directory.Exists(IO.Path.GetDirectoryName(pt)) Then
            IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(pt))
        End If
        Dim fld As New FileDownload()
        fld.StartDownloading(dl, pt)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        DependencyLoader.downloadForVersion(TextBox1.Text, Application.StartupPath + "\mc")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        AssetsManager.downloadForgeFromProvider(TextBox1.Text, Application.StartupPath + "\mc")
    End Sub
End Class