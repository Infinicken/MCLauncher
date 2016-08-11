Imports System.Runtime.InteropServices

Public Class CustomTextBox
    Inherits TextBox

    Public Const WM_NCPAINT As Integer = &H85

    <Flags()>
    Private Enum RedrawWindowFlags As UInteger
        Invalidate = &H1
        InternalPaint = &H2
        [Erase] = &H4
        Validate = &H8
        NoInternalPaint = &H10
        NoErase = &H20
        NoChildren = &H40
        AllChildren = &H80
        UpdateNow = &H100
        EraseNow = &H200
        Frame = &H400
        NoFrame = &H800
    End Enum

    <DllImport("User32.dll")>
    Public Shared Function GetWindowDC(ByVal hWnd As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Private Shared Function ReleaseDC(ByVal hWnd As IntPtr, ByVal hDC As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll")>
    Private Shared Function RedrawWindow(hWnd As IntPtr, lprcUpdate As IntPtr, hrgnUpdate As IntPtr, flags As RedrawWindowFlags) As Boolean
    End Function

    Public Sub New()
        MyBase.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
    End Sub

    Protected Overrides Sub OnResize(e As System.EventArgs)
        MyBase.OnResize(e)
        RedrawWindow(Me.Handle, IntPtr.Zero, IntPtr.Zero, RedrawWindowFlags.Frame Or RedrawWindowFlags.UpdateNow Or RedrawWindowFlags.Invalidate)
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        MyBase.WndProc(m)

        If m.Msg = WM_NCPAINT Then
            Dim hDC As IntPtr = GetWindowDC(m.HWnd)
            Using g As Graphics = Graphics.FromHdc(hDC)
                OnPaint(New PaintEventArgs(g, New Rectangle(0, 0, Me.Width, Me.Height)))
                g.DrawRectangle(SystemPens.Window, New Rectangle(1, 1, Me.Width - 3, Me.Height - 3))

            End Using
            ReleaseDC(m.HWnd, hDC)
        End If

    End Sub

    Public Property SideColor As Color = Color.Cyan
    Public Property TextPlaceholder As String = "Enter text..."

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If Me.Focused Then
            e.Graphics.DrawRectangle(New Pen(BackColor), New Rectangle(1, 1, Me.Width - 1, Me.Height - 1))
            e.Graphics.DrawRectangle(New Pen(SideColor), New Rectangle(1, 1, Me.Width - 1, Me.Height - 1))
            e.Graphics.DrawRectangle(New Pen(BackColor), New Rectangle(1, 1, Me.Width - 1, 0))
        End If
        'e.Graphics.DrawString(If(String.IsNullOrEmpty(Text), If(Focused, "", TextPlaceholder), Text), Font, New SolidBrush(ForeColor), 4, 4)
    End Sub
End Class
