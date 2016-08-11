Public Class LoginGUI

    Private Sub MaterialRadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles MaterialRadioButton2.CheckedChanged
        If MaterialRadioButton2.Checked Then
            PremiumVerifier.VerifyType = PremiumVerifier.LoginType.Offline
            MaterialSingleLineTextField2.Text = ""
            MaterialSingleLineTextField2.Enabled = False
        End If
    End Sub

    Private Sub MaterialRadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles MaterialRadioButton1.CheckedChanged
        If MaterialRadioButton1.Checked Then
            PremiumVerifier.VerifyType = PremiumVerifier.LoginType.Premium
            MaterialSingleLineTextField2.Enabled = True
        End If
    End Sub

    Private Sub MaterialFlatButton1_Click(sender As Object, e As EventArgs) Handles MaterialFlatButton1.Click
        If Not PremiumVerifier.tryPremiumLogin(MaterialSingleLineTextField1.Text, MaterialSingleLineTextField2.Text, True) Then
            PremiumVerifier.Username = MaterialSingleLineTextField1.Text
            ToastRenderer.addToast(MainGUI.piratedNotification)
        End If
        Me.Close()
    End Sub

    Private Sub LoginGUI_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        MaterialSingleLineTextField1.Text = PremiumVerifier.UserEmail
        MaterialSingleLineTextField2.Text = ""
        If PremiumVerifier.VerifyType = PremiumVerifier.LoginType.Premium Then
            MaterialRadioButton1.Checked = True
            MaterialRadioButton2.Checked = False
        Else
            MaterialRadioButton1.Checked = False
            MaterialRadioButton2.Checked = True
        End If
        I18n.requestHandleTranslationInForm(Me)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

End Class