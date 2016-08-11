<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FileDownload
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.labelStat = New System.Windows.Forms.Label()
        Me.progDL = New System.Windows.Forms.ProgressBar()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'labelStat
        '
        Me.labelStat.AutoSize = True
        Me.labelStat.Location = New System.Drawing.Point(12, 9)
        Me.labelStat.Name = "labelStat"
        Me.labelStat.Size = New System.Drawing.Size(23, 12)
        Me.labelStat.TabIndex = 0
        Me.labelStat.Text = "Idle"
        '
        'progDL
        '
        Me.progDL.Location = New System.Drawing.Point(12, 33)
        Me.progDL.Name = "progDL"
        Me.progDL.Size = New System.Drawing.Size(282, 23)
        Me.progDL.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 59)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(26, 12)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "0B/s"
        '
        'FileDownload
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(306, 78)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.progDL)
        Me.Controls.Add(Me.labelStat)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "FileDownload"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "File Downloader"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents labelStat As Label
    Friend WithEvents progDL As ProgressBar
    Friend WithEvents Label1 As Label
End Class
