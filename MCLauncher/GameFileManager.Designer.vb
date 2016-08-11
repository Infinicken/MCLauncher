<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GameFileManager
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.checkProvider = New System.Windows.Forms.RadioButton()
        Me.checkMojang = New System.Windows.Forms.RadioButton()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.checkProvider)
        Me.GroupBox1.Controls.Add(Me.checkMojang)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(368, 56)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "translate(info.assets.source)"
        '
        'checkProvider
        '
        Me.checkProvider.AutoSize = True
        Me.checkProvider.Location = New System.Drawing.Point(71, 21)
        Me.checkProvider.Name = "checkProvider"
        Me.checkProvider.Size = New System.Drawing.Size(159, 16)
        Me.checkProvider.TabIndex = 1
        Me.checkProvider.Text = "translate(info.assets.provider)"
        Me.checkProvider.UseVisualStyleBackColor = True
        '
        'checkMojang
        '
        Me.checkMojang.AutoSize = True
        Me.checkMojang.Checked = True
        Me.checkMojang.Location = New System.Drawing.Point(6, 21)
        Me.checkMojang.Name = "checkMojang"
        Me.checkMojang.Size = New System.Drawing.Size(150, 16)
        Me.checkMojang.TabIndex = 0
        Me.checkMojang.TabStop = True
        Me.checkMojang.Text = "translate(info.assets.vanilla)"
        Me.checkMojang.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(187, 96)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(193, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "translate(info.assets.assets)"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(121, 68)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(259, 22)
        Me.TextBox1.TabIndex = 2
        Me.TextBox1.Text = "1.7.10"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 71)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(135, 12)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "translate(info.assets.version)"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(12, 96)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(169, 23)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "translate(info.assets.gameJar)"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(12, 125)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(169, 23)
        Me.Button3.TabIndex = 5
        Me.Button3.Text = "translate(info.assets.dependencies)"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(187, 152)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(193, 23)
        Me.Button4.TabIndex = 6
        Me.Button4.Text = "translate(info.assets.provider.forge)"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(12, 154)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(169, 23)
        Me.Button5.TabIndex = 7
        Me.Button5.Text = "translate(info.assets.provider.mods)"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'GameFileManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(392, 187)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "GameFileManager"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "translate(info.assets.manager)"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents checkProvider As RadioButton
    Friend WithEvents checkMojang As RadioButton
    Friend WithEvents Button1 As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
End Class
