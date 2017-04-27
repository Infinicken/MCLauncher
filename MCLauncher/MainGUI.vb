Imports System.Runtime.InteropServices

Public Class MainGUI
    Public Shared piratedNotification As New Toast(I18n.translate("warn"),
                                                   I18n.translate("warn.toast.pirated"), Nothing, False, Toast.ToastLength.Long, AddressOf piratedNotificationClick) With {.backColor = Color.Red, .foreColor = Color.Yellow}

    Public Declare Auto Function ReleaseCapture Lib "user32.dll" () As Boolean
    Public Declare Auto Function SendMessage Lib "user32.dll" (hWnd As IntPtr, msg As Integer, wParam As Integer, lParam As Integer) As Integer

    Public Shared Sub piratedNotificationClick(toast As Toast, e As MouseEventArgs)
        If MsgBox(I18n.translate("warn.toast.piratedDlg"), vbOKOnly, I18n.translate("warn")) = vbOK Then
            piratedNotification.forcedDismiss = True
        End If
    End Sub

    Private Sub MainGUI_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ToastRenderer.bindForm(Me, 30)
        AddHandler Me.MouseDown, AddressOf ToastRenderer.handleFormClick
        ToastRenderer.addToast(ServerSideManager.fetchingInfoNotif)
        ServerSideManager.fetchFromServer()
        If PremiumVerifier.VerifyType = PremiumVerifier.LoginType.Offline Then ToastRenderer.addToast(piratedNotification)
        I18n.requestHandleTranslationInForm(Me)
        AddHandler PremiumVerifier.LoginHook, AddressOf UserLoginHook
        UserLoginHook()
    End Sub

    Private Sub MainGUI_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        If Me.WindowState = FormWindowState.Maximized Then
            Me.WindowState = FormWindowState.Normal
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ScriptEditor.Show()
        'ScriptServer.run(InputBox("Script line"))
        'ToastRenderer.addToast(New Toast("Mi", "Yan Su!", My.Resources.ic_play_arrow_black_48dp_1x, True, Toast.ToastLength.Short))
        'Throw New System.IndexOutOfRangeException("test")
    End Sub

    Private Sub CustomButton2_Click(sender As Object, e As EventArgs) 
        Call LoginGUI.ShowDialog()
    End Sub

    Private Sub MaterialFlatButton1_Click(sender As Object, e As EventArgs) Handles MaterialFlatButton1.Click
        ConsoleLogger.startLogging(MinecraftAppletLauncher.launchMC(PremiumVerifier.Username, GameFileManager.TextBox1.Text, ""))
    End Sub

    Private Sub MaterialFlatButton2_Click(sender As Object, e As EventArgs) Handles MaterialFlatButton2.Click
        Settings.ShowDialog()
    End Sub

    Private Sub MaterialFlatButton3_Click(sender As Object, e As EventArgs) Handles MaterialFlatButton3.Click
        GameFileManager.ShowDialog()
    End Sub

    Private Sub MaterialFlatButton4_Click(sender As Object, e As EventArgs) Handles MaterialFlatButton4.Click
        Call ConsoleOut.Show()
    End Sub

    Private Sub UserLoginHook()
        PictureBox1.Image = PremiumVerifier.getPlayerHead(PremiumVerifier.Username)
        Label1.Text = PremiumVerifier.Username
    End Sub

    Private Sub Panel1_MouseDown(sender As Object, e As MouseEventArgs) Handles Panel1.MouseDown, LabelTitle.MouseDown
        ReleaseCapture()
        SendMessage(Me.Handle, &HA1, &H2, 0)
    End Sub
End Class