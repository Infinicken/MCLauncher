﻿Public Class ScriptEditor
    Private Shared registeredHighlight As New List(Of KeywordHighlight)
    Public Shared prettyFont As New Font("Microsoft YaHei", 10)
    Public NotInheritable Class KeywordHighlight
        Private Sub New(a As String, b As Color, c As Font)
            key = a
            color = b
            font = c
        End Sub
        Public ReadOnly key As String
        Public ReadOnly color As Color
        Public ReadOnly font As Font
        Public Shared Function create(key As String, color As Color, font As Font) As KeywordHighlight
            Dim instance As KeywordHighlight = New KeywordHighlight(key, color, font)
            registeredHighlight.Add(instance)
            Return instance
        End Function
        Public Shared Sub remove(ByRef instance As KeywordHighlight)
            registeredHighlight.Remove(instance)
            instance = Nothing
        End Sub
    End Class
    Private Shared registeredTooltip As New List(Of KeywordTooltip)
    Public NotInheritable Class KeywordTooltip
        Private Sub New(k As String, t As String)
            key = k
            tooltip = t
        End Sub
        Public ReadOnly key As String
        Public ReadOnly tooltip As String
        Public Shared Function create(key As String, tool As String) As KeywordTooltip
            Dim instance As KeywordTooltip = New KeywordTooltip(key, tool)
            registeredTooltip.Add(instance)
            Return instance
        End Function
        Public Shared Sub remove(ByRef instance As KeywordTooltip)
            registeredTooltip.Remove(instance)
            instance = Nothing
        End Sub
    End Class

    Shared Sub New()
        'Create KeywordHighlights
        KeywordHighlight.create("alert", Color.Blue, prettyFont)
        KeywordHighlight.create("prompt", Color.Blue, prettyFont)
        KeywordHighlight.create("eval", Color.Blue, New Font(prettyFont, FontStyle.Bold))
        KeywordHighlight.create("MojangAPI", Color.LightBlue, prettyFont)
        KeywordHighlight.create("Launcher", Color.LightBlue, prettyFont)
        KeywordHighlight.create("var", Color.Blue, prettyFont)
        KeywordHighlight.create("function", Color.Blue, prettyFont)
        KeywordHighlight.create("for", Color.Blue, prettyFont)
        KeywordHighlight.create("while", Color.Blue, prettyFont)
        KeywordHighlight.create("if", Color.Blue, prettyFont)
        KeywordHighlight.create("new", Color.Blue, prettyFont)
        KeywordHighlight.create("this", Color.Blue, prettyFont)
        KeywordHighlight.create("!", Color.Red, New Font(prettyFont, FontStyle.Bold))
        'Create KeywordTooltips
        KeywordTooltip.create("Launcher", "Launcher wrapper")
        KeywordTooltip.create("alert(*)*", "function alert(str)")
        KeywordTooltip.create("prompt(*)*", "string prompt(str)")
        KeywordTooltip.create("eval(*)*", "/* Language construct */ function eval(...)")
    End Sub

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
        For Each item As KeywordHighlight In registeredHighlight
            SH(item.key, item.color, item.font)
        Next
        ClassCheck()
        StringHighlight()
        DirectiveHighlight()
        RichTextBox1.SelectionStart = sel
        RichTextBox1.SelectionLength = sell
        RichTextBox1.Show()
        RichTextBox1.Focus()
    End Sub

    Private Sub StringHighlight()
        Dim ix As Integer = 0
        While True
            ix = RichTextBox1.Text.IndexOf("""", ix)
            If ix = -1 Then
                Exit While
            End If
            Dim mov As Integer = 0
SearchForMatchingQuotationMark:
            Dim jx As Integer = RichTextBox1.Text.IndexOf("""", ix + 1 + mov)
            If jx = -1 Then
                ix += 1
                Continue While
            End If
            If RichTextBox1.Text.Substring(jx - 1, 2) = "\""" Then
                mov = jx - ix
                GoTo SearchForMatchingQuotationMark
            End If
            Dim stringLiteral As String = RichTextBox1.Text.Substring(ix + 1, jx - ix - 1)
            RichTextBox1.SelectionStart = ix
            RichTextBox1.SelectionLength = jx - ix + 1
            RichTextBox1.SelectionColor = Color.DarkRed
            ix = jx + 1
        End While
    End Sub

    Private Sub DirectiveHighlight()
        Dim ix = 0
        While True
            ix = RichTextBox1.Text.IndexOf("#"c, ix)
            If ix = -1 Then
                Exit While
            End If
            Dim selstart = RichTextBox1.SelectionStart
            Dim sellen = RichTextBox1.SelectionLength
            RichTextBox1.SelectionStart = ix
            RichTextBox1.SelectionLength = Math.Min(Math.Max(Math.Max(RichTextBox1.Text.IndexOf(" "c, ix), RichTextBox1.Text.IndexOf(";"c, ix)), RichTextBox1.Text.IndexOf(vbLf, ix)).map(Function(a) If(a = -1, 99999, a)), RichTextBox1.TextLength) - ix
            RichTextBox1.SelectionColor = Color.Gray
            RichTextBox1.SelectionFont = New Font(RichTextBox1.Font, FontStyle.Italic)
            RichTextBox1.SelectionStart = selstart
            RichTextBox1.SelectionLength = sellen
            ix = ix + 1
        End While
    End Sub

    Private Sub ClassCheck()
        Dim ix As Integer = 0
        While True
            ix = RichTextBox1.Text.IndexOf("new ", ix)
            If ix = -1 Then
                Exit While
            End If
            Dim jx = RichTextBox1.Text.IndexOf("(", ix)
            If jx = -1 Then
                ix += 1
                Continue While
            End If
            Dim className As String = RichTextBox1.Text.Substring(ix + 4, jx - ix - 4)
            If RichTextBox1.Text.IndexOf("function " & className) > -1 Then
                SH(" " & className, Color.LightBlue, prettyFont)
            End If
            ix += 1
        End While
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
    Private internalTimer As Timer, hidden As Boolean = True
    Private Sub RichTextBox1_MouseMove(sender As Object, e As MouseEventArgs) Handles RichTextBox1.MouseMove
        Dim chrInd As Integer = RichTextBox1.GetCharIndexFromPosition(e.Location)
        Dim line As Integer = RichTextBox1.GetLineFromCharIndex(chrInd)
        If line = 0 AndAlso RichTextBox1.Lines.Length = 0 Then Return
        Dim firstChrInd As Integer = RichTextBox1.GetFirstCharIndexFromLine(line)
        Dim lastChrInd As Integer = RichTextBox1.Text.IndexOf(vbLf, firstChrInd).map(Function(a) If(a = -1, RichTextBox1.TextLength, a))
        Dim lineStr As String = RichTextBox1.Lines(line)

        Dim pozhenStr As String = lineStr
        LabelTest.Text = $"({firstChrInd},{lastChrInd},{chrInd}:{RichTextBox1.Text(chrInd)}){lineStr}"
        For Each item As KeywordTooltip In registeredTooltip
            If pozhenStr Like item.key Then
                If hidden Then
                    hidden = False
                    internalTimer = New Timer() With {.Interval = 2500}
                    AddHandler internalTimer.Tick, Sub()
                                                       hidden = True
                                                       internalTimer.Stop()
                                                   End Sub
                    ToolTipKeyword.Show(item.tooltip, Me, e.Location, 2500)
                    internalTimer.Start()
                End If
                Exit Sub
            End If
        Next
    End Sub
End Class