<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BatchDisplay
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
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.tlp = New System.Windows.Forms.TableLayoutPanel()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(294, 183)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(170, 181)
        Me.TextBox1.TabIndex = 0
        '
        'tlp
        '
        Me.tlp.AutoScroll = True
        Me.tlp.ColumnCount = 2
        Me.tlp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlp.Location = New System.Drawing.Point(0, 0)
        Me.tlp.Name = "tlp"
        Me.tlp.RowCount = 1
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlp.Size = New System.Drawing.Size(540, 366)
        Me.tlp.TabIndex = 1
        '
        'BatchDisplay
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(540, 366)
        Me.Controls.Add(Me.tlp)
        Me.Controls.Add(Me.TextBox1)
        Me.Name = "BatchDisplay"
        Me.Text = "BatchDisplay"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents tlp As TableLayoutPanel
End Class
