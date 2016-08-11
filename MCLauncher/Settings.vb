Public Class Settings
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
End Class