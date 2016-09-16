Public Class Settings
    Private settingsUpdated As New Toast(I18n.translate("info.pref.updatedToast.title"), I18n.translate("info.pref.updatedToast.content"), Nothing, True, Toast.ToastLength.Short, Nothing, Color.Green, Nothing)
    Private Sub Settings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()

        For Each item In I18n.Language.RegisteredLanguages
            ComboBox1.Items.Add(item.getLocalizedName & "(" & item.getISOName & ")")
        Next
        ComboBox1.Text = I18n.currentLanguage.getLocalizedName & "(" & I18n.currentLanguage.getISOName & ")"
        I18n.requestHandleTranslationInForm(Me)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim oldLang As I18n.Language = I18n.currentLanguage
        Dim newLang As I18n.Language = I18n.Language.getFromISOName(ScriptServer.parseParentheness(ComboBox1.Text)(1))
        If oldLang = newLang Then
            Return
        End If
        I18n.currentLanguage = newLang
        MsgBox(I18n.translate("info.lang.restart", oldLang.getLocalizedName, newLang.getLocalizedName))
    End Sub

    Private Sub MaterialFlatButton1_Click(sender As Object, e As EventArgs) Handles MaterialFlatButton1.Click
        LoginGUI.ShowDialog()
    End Sub

    Private Sub Settings_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        ToastRenderer.addToast(settingsUpdated.clone())
    End Sub

    Private Sub MaterialSingleLineTextField1_TextChanged(sender As Object, e As EventArgs) Handles MaterialSingleLineTextField1.TextChanged
        ServerSideManager.validateAndUpdateProvider(MaterialSingleLineTextField1.Text)
    End Sub

    Private Sub MaterialCheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles MaterialCheckBox1.CheckedChanged
        ServerSideManager.shittySpec = MaterialCheckBox1.Checked
    End Sub
End Class