Public Class CustomButton
    Inherits ButtonBase

    Private Shared MaterialDesignThread As New Threading.Thread(AddressOf materializeDesignThread)

    Private Shared Sub materializeDesignThread()
        While MDRunning
            For Each item As mdtt In mdttlbuff
                item.c.Invoke(item.d)
            Next
            If mdttl.Count >= 1 Then mdttlbuff = New List(Of mdtt)(mdttl) Else mdttlbuff.Clear()
            mdttl.Clear()
        End While
    End Sub

    Public Shared Sub addToMDThread(c As Control, d As [Delegate])
        mdttl.Add(New mdtt(c, d))
    End Sub

    Private Shared MDRunning As Boolean = True

    Public Shared Sub notifyMDThreadShutdown()
        MDRunning = False
        MaterialDesignThread.Abort()
    End Sub

    Private Shared mdttl As New List(Of mdtt)
    Private Shared mdttlbuff As New List(Of mdtt)

    Private Structure mdtt
        Public Sub New(c As Control, d As [Delegate])
            Me.c = c
            Me.d = d
        End Sub
        Public c As Control
        Public d As [Delegate]
    End Structure
    Private Shared started As Boolean = False
    Public Sub New()
        If Not started Then
            MaterialDesignThread.Start()
            started = True
        End If
        Me.DoubleBuffered = True
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

    Public Property FixedColor As Color = Color.White
    Public Property WavesColor As Color = Color.Blue
    Public Enum WavesStyle
        Color
        Lighter
        Darker
    End Enum
    Public Property WaveStyle As WavesStyle
    Private waves As New List(Of ButtonWave)
    Private imageScaleFactor As Double = 1D
    Private hasCalculatedScale As Boolean = False
    Private resizedImage As Bitmap = New Bitmap(1, 1)
    Private hasChanged As Boolean = True
    Private hasChangedFuture As Boolean = True
    Private lastBMP As Bitmap = Nothing
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If hasChanged Then
            Dim bmp As Bitmap = calculateBMP()
            'e.Graphics.DrawImage(bmp, 0, 0)
            lastBMP = bmp
        Else
            'e.Graphics.DrawImage(lastBMP, 0, 0)
        End If
        hasChanged = hasChangedFuture
        hasChangedFuture = False
        If hasChanged Then Invalidate()
    End Sub

    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        tryScale()
    End Sub

    Private Sub md(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
        CustomButton_MouseUp(sender, e)
        waves.Add(New ButtonWave(e.Location, getWavesColor))
        hasChangedFuture = True
        Invalidate()
    End Sub

    Private Function calculateBMP() As Bitmap
        Dim bmp As New Bitmap(Width, Height)
        Dim g As Graphics = Graphics.FromImage(bmp)
        Dim deadWaves As New List(Of ButtonWave)
        g.FillRectangle(New SolidBrush(FixedColor), New Rectangle(0, 0, bmp.Width, bmp.Height))
        For Each wave As ButtonWave In waves
            g.FillEllipse(New SolidBrush(Color.FromArgb(255 - 255 * wave.Life * 0.1, wave.Color)), New Rectangle(wave.Location.X - wave.Size / 2, wave.Location.Y - wave.Size / 2, wave.Size, wave.Size))
            If wave.Life < 8.5 Then
                If wave.Size < Math.Max(Width * 2.5, Height * 2.5) Then wave.Size += 0.6
                If Not (wave.isStillDown AndAlso wave.Life > 7.5) Then wave.Life += 0.01
            End If
            If wave.Life >= 8.5 Then
                deadWaves.Add(wave)
            End If
        Next
        For Each item As ButtonWave In deadWaves
            waves.Remove(item)
        Next
        If waves.Count >= 1 Then
            addToMDThread(Me, Sub()
                                  hasChangedFuture = True
                                  Me.Invalidate()
                              End Sub)
        End If
        If Image Is Nothing Then
            g.DrawString(Text, Font, New SolidBrush(ForeColor), New Point(Me.Width / 2 - TextRenderer.MeasureText(Text, Font).Width / 2, Me.Height / 2 - TextRenderer.MeasureText(Text, Font).Height / 2))
        Else
            If Not hasCalculatedScale Then tryScale()
            g.DrawImage(resizedImage, New Point((Width - resizedImage.Width) / 2, (Height - resizedImage.Height) / 2))
            g.DrawString(Text, Font, New SolidBrush(ForeColor), New Point((Me.Width - TextRenderer.MeasureText(Text, Font).Width) / 2, (Me.Height - TextRenderer.MeasureText(Text, Font).Height + Image.Height) / 2))
        End If
        Return bmp
    End Function

    Private Sub tryScale()
        resizedImage = Image
        Return
        If Image IsNot Nothing Then
            For i As Double = 1 To 3 Step 0.25
                If Image.Width * i < Me.Width AndAlso Image.Height * i < Me.Height Then
                    'imageScaleFactor = i
                End If
            Next
        End If
        hasCalculatedScale = True
        'resizedImage = New Bitmap(Image, New Size(Image.Width * imageScaleFactor, Image.Height * imageScaleFactor))
    End Sub

    Private Function getWavesColor() As Color
        If WaveStyle = WavesStyle.Color Then
            Return WavesColor
        ElseIf WaveStyle = WavesStyle.Darker Then
            Return Color.FromArgb(FixedColor.R - 25, FixedColor.G - 25, FixedColor.B - 25)
        Else
            Return Color.FromArgb(FixedColor.R + 25, FixedColor.G + 25, FixedColor.B + 25)
        End If
    End Function

    Private Sub CustomButton_MouseUp(sender As Object, e As MouseEventArgs) Handles MyBase.MouseUp
        For Each item As ButtonWave In waves
            item.isStillDown = False
        Next
    End Sub

    Public Class ButtonWave
        Public Sub New(loc As Point, color As Color)
            Me.Location = loc
            isStillDown = True
            Me.Color = color
        End Sub
        Public Location As Point
        Public Size As Double = 0
        Public Color As Color
        Public Life As Double
        Public isStillDown As Boolean
    End Class
End Class
