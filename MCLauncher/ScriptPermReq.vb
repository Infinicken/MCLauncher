Public Class ScriptPermReq
    Private Sub ScriptPermReq_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        I18n.requestHandleTranslationInForm(Me)
    End Sub

    Private allowance As New Dictionary(Of ScriptPermMgr.Permissions, Boolean)

    Public Function ShowPerm(ParamArray perms As ScriptPermMgr.Permissions()) As Dictionary(Of ScriptPermMgr.Permissions, Boolean)
        allowance = New Dictionary(Of ScriptPermMgr.Permissions, Boolean)
        Table.RowCount = perms.Length
        For i As Integer = 0 To perms.Length - 1
            Dim item As ScriptPermMgr.Permissions = perms(i)
            allowance.Add(item, False)
            Dim checkbox As New CheckBox()
            checkbox.Text = ""
            checkbox.Name = "chkbx_" & i
            checkbox.AutoSize = True
            AddHandler checkbox.CheckedChanged, Sub()
                                                    If checkbox.Checked Then
                                                        allowance(item) = True
                                                    Else
                                                        allowance(item) = False
                                                    End If
                                                End Sub
            Table.Controls.Add(checkbox, 0, i)
            Dim permname As New Label()
            permname.Text = item.identifier & vbCrLf & I18n.translate(item.desc)
            permname.Name = "permname_" & i
            permname.Font = Font
            permname.MaximumSize = New Size(400, 0)
            permname.AutoSize = True
            If item.lethal Then permname.ForeColor = Color.Red
            Table.Controls.Add(permname, 1, i)
        Next
        Me.ShowDialog()
        Return allowance
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class