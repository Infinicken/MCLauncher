''' <summary>
''' Renders toasts on the bound form by using a picture box.
''' </summary>
Public NotInheritable Class ToastRenderer
    Private Sub New()
    End Sub
    Private Shared size As Size
    Private Shared WithEvents timer As New Timer
    Private Shared font As Font
    Private Shared boundBC As Color
    ''' <summary>
    ''' Bind the form to the renderer.
    ''' </summary>
    ''' <param name="f">The form to bind to.</param>
    Public Shared Sub bindForm(ByRef f As Form)
        size = f.Size
        font = f.Font
        boundBC = f.BackColor
        panel = New PictureBox()
        panel.BackColor = Color.Transparent
        f.Controls.Add(panel)
        timer.Interval = 1
        timer.Start()
        AddHandler panel.MouseDown, AddressOf handleFormClick
    End Sub
    Private Shared toasts As New List(Of Toast)
    Private Shared clearCanvas As Boolean = False
    Private Shared WithEvents panel As PictureBox
    Private Shared bitmap As Bitmap
    Private Shared g As Graphics
    Private Shared Sub timer_Tick() Handles timer.Tick
        If Not toasts?.Count > 0 Then Return
        Dim toast As Toast = toasts(0)
        Dim dRect As Rectangle
        Dim animationModifier As Integer = CInt(Math.Floor(toast.maxLife * 0.125))
        If toast.life < animationModifier Then
            dRect = New Rectangle(0 - (animationModifier - toast.life), toast.y, CInt(Toast.toastWidth * (toast.life / animationModifier)), Toast.toastHeight)
        ElseIf toast.life > toast.maxLife - animationModifier AndAlso toast.life < toast.maxLife AndAlso toast.autoDismiss Then
            dRect = New Rectangle((toast.maxLife - toast.life) - animationModifier, toast.y, CInt(Toast.toastWidth * ((toast.maxLife - toast.life) / animationModifier)), Toast.toastHeight)
        Else
            dRect = New Rectangle(0, 0, Toast.toastWidth, Toast.toastHeight)
        End If
        panel.Location = dRect.Location
        panel.Size = dRect.Size
        bitmap = New Bitmap(dRect.Width + 1, dRect.Height)
        g = Graphics.FromImage(bitmap)
        g.FillRectangle(New SolidBrush(toast.backColor), dRect)
        If toast.icon IsNot Nothing Then g.DrawImage(toast.icon, dRect.X, dRect.Y, 32, 32)
        g.DrawString(toast.title, New Font(font, FontStyle.Bold), New SolidBrush(toast.foreColor), dRect.X + 36, dRect.Y + 2)
        g.DrawString(toast.content, font, New SolidBrush(toast.foreColor), dRect.X + 36, dRect.Y + 18)
        g.DrawString(If(toast.autoDismiss, (toast.maxLife - toast.life) \ 100 & "s" & vbCrLf, "") & "(+" & toasts.Count - 1 & ")", font, New SolidBrush(toast.foreColor), dRect.X, dRect.Y + 36)
        If toast.life < animationModifier OrElse toast.autoDismiss Then
            toast.life += 2
        End If
        If toast.forcedDismiss Then
            If toast.life < toast.maxLife * 0.875 Then toast.life = CInt(toast.maxLife * 0.875)
            toast.forcedDismiss = False
            toast.autoDismiss = True
        End If
        If toast.life >= toast.maxLife Then
            toast.autoDismiss = False
            toast.life = 0
            toasts.Remove(toast)
        End If
        panel.Image = bitmap
    End Sub
    ''' <summary>
    ''' Best bound to the bound form.
    ''' </summary>
    ''' <param name="sender">Event handler param</param>
    ''' <param name="e">Event handler param</param>
    Public Shared Sub handleFormClick(sender As Object, e As MouseEventArgs)
        If Not toasts?.Count > 0 Then Return
        Dim toast As Toast = toasts(0)
        Dim dRect As Rectangle
        Dim animationModifier As Integer = CInt(Math.Floor(toast.maxLife * 0.125))
        If toast.life < animationModifier Then
            dRect = New Rectangle(0 - (animationModifier - toast.life), toast.y, CInt(Toast.toastWidth * (toast.life / animationModifier)), Toast.toastHeight)
        ElseIf toast.life > toast.maxLife - animationModifier AndAlso toast.life < toast.maxLife AndAlso toast.autoDismiss Then
            dRect = New Rectangle((toast.maxLife - toast.life) - animationModifier, toast.y, CInt(Toast.toastWidth * ((toast.maxLife - toast.life) / animationModifier)), Toast.toastHeight)
        Else
            dRect = New Rectangle(0, 0, Toast.toastWidth, Toast.toastHeight)
        End If
        If dRect.IntersectsWith(New Rectangle(e.X, e.Y, 1, 1)) Then
            If toast.handler IsNot Nothing Then
                toast.handler.Invoke(toast, e)
            Else
                toast.life = CInt(toast.maxLife * 0.875)
                toast.forcedDismiss = True
            End If
        End If
    End Sub
    ''' <summary>
    ''' Adds a toast to the pool of toasts.
    ''' </summary>
    ''' <param name="toast">The toast to add to.</param>
    Public Shared Sub addToast(toast As Toast)
        If toasts.Count < Short.MaxValue Then
            toasts.Add(toast)
        End If
    End Sub
    ''' <summary>
    ''' Checks if there are any queued toasts.
    ''' </summary>
    ''' <returns>False if there is no toasts, true otherwise.</returns>
    Public Shared Function hasToast() As Boolean
        Return toasts.Count > 0
    End Function
    Protected Shared Function getToastRect() As Rectangle
        If Not toasts?.Count > 0 Then Return Nothing
        Dim toast As Toast = toasts(0)
        Dim dRect As Rectangle
        Dim animationModifier As Integer = CInt(Math.Floor(toast.maxLife * 0.125))
        If toast.life < animationModifier Then
            dRect = New Rectangle(0 - (animationModifier - toast.life), toast.y, CInt(Toast.toastWidth * (toast.life / animationModifier)), Toast.toastHeight)
        ElseIf toast.life > toast.maxLife - animationModifier AndAlso toast.life < toast.maxLife AndAlso toast.autoDismiss Then
            dRect = New Rectangle((toast.maxLife - toast.life) - animationModifier, toast.y, CInt(Toast.toastWidth * ((toast.maxLife - toast.life) / animationModifier)), Toast.toastHeight)
        Else
            dRect = New Rectangle(0, 0, Toast.toastWidth, Toast.toastHeight)
        End If
        Return dRect
    End Function
End Class

Public NotInheritable Class Toast
    Public autoDismiss As Boolean = True
    Friend life As Integer = 0, x As Integer = 0, y As Integer = 0
    Friend ReadOnly maxLife As Integer = 1000
    Public ReadOnly title, content As String
    Public ReadOnly icon As Bitmap
    Public Const toastWidth As Integer = 250
    Public Const toastHeight As Integer = 66
    Public backColor As Color = Color.Gray
    Public foreColor As Color = Color.Black
    Public Delegate Sub onClick(toast As Toast, e As MouseEventArgs)
    Public handler As onClick
    Public forcedDismiss As Boolean = False
    Public Sub New(title As String, content As String, Optional icon As Bitmap = Nothing, Optional autoDismiss As Boolean = True, Optional longToast As ToastLength = ToastLength.Normal, Optional onClick As onClick = Nothing, Optional backColor As Color? = Nothing, Optional foreColor As Color? = Nothing)
        Me.title = title
        Me.content = content
        Me.icon = icon
        Me.autoDismiss = autoDismiss
        Me.backColor = If(backColor, Color.Gray)
        Me.foreColor = If(foreColor, Color.Black)
        maxLife = longToast
        handler = onClick
    End Sub
    Public Enum ToastLength
        NotYourOrdinaryToast = 250
        [Short] = 500
        Normal = 1000
        [Long] = 1500
    End Enum
    Public Function clone() As Toast
        Return New Toast(title, content, icon, autoDismiss, CType(maxLife, ToastLength), handler, backColor, foreColor)
    End Function
End Class
