<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Settings
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.MaterialFlatButton1 = New MaterialSkin.Controls.MaterialFlatButton()
        Me.MaterialSingleLineTextField1 = New MaterialSkin.Controls.MaterialSingleLineTextField()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.MaterialCheckBox1 = New MaterialSkin.Controls.MaterialCheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(139, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "translate(info.pref.lang)"
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(103, 15)
        Me.ComboBox1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(218, 23)
        Me.ComboBox1.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(213, 15)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "translate(info.pref.account.premium)"
        '
        'MaterialFlatButton1
        '
        Me.MaterialFlatButton1.BackColor = System.Drawing.Color.PaleGoldenrod
        Me.MaterialFlatButton1.Depth = 0
        Me.MaterialFlatButton1.HoverColor = System.Drawing.Color.Orange
        Me.MaterialFlatButton1.Location = New System.Drawing.Point(103, 50)
        Me.MaterialFlatButton1.Margin = New System.Windows.Forms.Padding(5, 8, 5, 8)
        Me.MaterialFlatButton1.MouseState = MaterialSkin.MouseState.HOVER
        Me.MaterialFlatButton1.Name = "MaterialFlatButton1"
        Me.MaterialFlatButton1.Primary = False
        Me.MaterialFlatButton1.Size = New System.Drawing.Size(218, 29)
        Me.MaterialFlatButton1.TabIndex = 3
        Me.MaterialFlatButton1.Text = "translate(info.premium.premium)"
        Me.MaterialFlatButton1.UseVisualStyleBackColor = False
        '
        'MaterialSingleLineTextField1
        '
        Me.MaterialSingleLineTextField1.Depth = 0
        Me.MaterialSingleLineTextField1.Hint = ""
        Me.MaterialSingleLineTextField1.Location = New System.Drawing.Point(103, 90)
        Me.MaterialSingleLineTextField1.MaxLength = 32767
        Me.MaterialSingleLineTextField1.MouseState = MaterialSkin.MouseState.HOVER
        Me.MaterialSingleLineTextField1.Name = "MaterialSingleLineTextField1"
        Me.MaterialSingleLineTextField1.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.MaterialSingleLineTextField1.SelectedText = ""
        Me.MaterialSingleLineTextField1.SelectionLength = 0
        Me.MaterialSingleLineTextField1.SelectionStart = 0
        Me.MaterialSingleLineTextField1.Size = New System.Drawing.Size(218, 23)
        Me.MaterialSingleLineTextField1.TabIndex = 4
        Me.MaterialSingleLineTextField1.TabStop = False
        Me.MaterialSingleLineTextField1.UseSystemPasswordChar = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(14, 90)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(162, 15)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "translate(info.pref.provider)"
        '
        'MaterialCheckBox1
        '
        Me.MaterialCheckBox1.AutoSize = True
        Me.MaterialCheckBox1.Depth = 0
        Me.MaterialCheckBox1.Font = New System.Drawing.Font("Microsoft JhengHei UI", 9.0!)
        Me.MaterialCheckBox1.Location = New System.Drawing.Point(103, 116)
        Me.MaterialCheckBox1.Margin = New System.Windows.Forms.Padding(0)
        Me.MaterialCheckBox1.MouseLocation = New System.Drawing.Point(-1, -1)
        Me.MaterialCheckBox1.MouseState = MaterialSkin.MouseState.HOVER
        Me.MaterialCheckBox1.Name = "MaterialCheckBox1"
        Me.MaterialCheckBox1.Ripple = True
        Me.MaterialCheckBox1.Size = New System.Drawing.Size(240, 30)
        Me.MaterialCheckBox1.TabIndex = 6
        Me.MaterialCheckBox1.Text = "translate(info.pref.shittyPCCheck)"
        Me.MaterialCheckBox1.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(14, 123)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(158, 15)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "translate(info.pref.shittyPC)"
        '
        'Settings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(336, 152)
        Me.Controls.Add(Me.MaterialCheckBox1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.MaterialSingleLineTextField1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.MaterialFlatButton1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Microsoft JhengHei UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "Settings"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "translate(info.pref)"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents MaterialFlatButton1 As MaterialSkin.Controls.MaterialFlatButton
    Friend WithEvents MaterialSingleLineTextField1 As MaterialSkin.Controls.MaterialSingleLineTextField
    Friend WithEvents Label3 As Label
    Friend WithEvents MaterialCheckBox1 As MaterialSkin.Controls.MaterialCheckBox
    Friend WithEvents Label4 As Label
End Class
