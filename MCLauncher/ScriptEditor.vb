Public Class ScriptEditor
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ScriptServer.run(RichTextBox1.Text)
    End Sub

    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox1.TextChanged
        IntelliSense()
    End Sub

    Private Sub IntelliSense()
        Dim sel As Integer = RichTextBox1.SelectionStart
        Dim sell As Integer = RichTextBox1.SelectionLength
        RichTextBox1.Hide()
        RichTextBox1.SelectAll()
        RichTextBox1.SelectionColor = Color.Black
        RichTextBox1.SelectionFont = RichTextBox1.Font
        Dim deffont As Font = RichTextBox1.Font
        SH("alert", Color.Blue, deffont)
        SH("prompt", Color.Blue, deffont)
        SH("eval", Color.Blue, New Font(deffont, FontStyle.Bold))
        SH("MojangAPI", Color.LightBlue, deffont)
        SH("Launcher", Color.LightBlue, deffont)
        SH("var", Color.Blue, deffont)
        SH("function", Color.Blue, deffont)
        SH("for", Color.Blue, deffont)
        SH("while", Color.Blue, deffont)
        SH("if", Color.Blue, deffont)
        RichTextBox1.SelectionStart = sel
        RichTextBox1.SelectionLength = sell
        RichTextBox1.Show()
        RichTextBox1.Focus()
    End Sub

    Private Sub SH(text As String, color As Color, font As Font)
        Dim ix As Integer = 0
        While True
            ix = RichTextBox1.Text.IndexOf(text, ix)
            If ix = -1 Then
                Exit While
            End If
            RichTextBox1.SelectionStart = ix
            RichTextBox1.SelectionLength = text.Length
            RichTextBox1.SelectionColor = color
            RichTextBox1.SelectionFont = font
            ix += 1
        End While
    End Sub

    Private Sub ScriptEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
    End Sub
End Class