<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LoginGUI
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.MaterialRadioButton2 = New MaterialSkin.Controls.MaterialRadioButton()
        Me.MaterialRadioButton1 = New MaterialSkin.Controls.MaterialRadioButton()
        Me.MaterialFlatButton1 = New MaterialSkin.Controls.MaterialFlatButton()
        Me.MaterialSingleLineTextField2 = New MaterialSkin.Controls.MaterialSingleLineTextField()
        Me.MaterialSingleLineTextField1 = New MaterialSkin.Controls.MaterialSingleLineTextField()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(197, 15)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "translate(info.premium.username)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 98)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(196, 15)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "translate(info.premium.password)"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 187)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(186, 15)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "translate(info.premium.mojang)"
        '
        'MaterialRadioButton2
        '
        Me.MaterialRadioButton2.AutoSize = True
        Me.MaterialRadioButton2.BackColor = System.Drawing.Color.White
        Me.MaterialRadioButton2.Depth = 0
        Me.MaterialRadioButton2.Font = New System.Drawing.Font("Microsoft JhengHei UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaterialRadioButton2.Location = New System.Drawing.Point(17, 157)
        Me.MaterialRadioButton2.Margin = New System.Windows.Forms.Padding(0)
        Me.MaterialRadioButton2.MouseLocation = New System.Drawing.Point(-1, -1)
        Me.MaterialRadioButton2.MouseState = MaterialSkin.MouseState.HOVER
        Me.MaterialRadioButton2.Name = "MaterialRadioButton2"
        Me.MaterialRadioButton2.Ripple = True
        Me.MaterialRadioButton2.Size = New System.Drawing.Size(225, 30)
        Me.MaterialRadioButton2.TabIndex = 8
        Me.MaterialRadioButton2.Text = "translate(info.premium.pirated)"
        Me.MaterialRadioButton2.UseVisualStyleBackColor = False
        '
        'MaterialRadioButton1
        '
        Me.MaterialRadioButton1.AutoSize = True
        Me.MaterialRadioButton1.BackColor = System.Drawing.Color.White
        Me.MaterialRadioButton1.Depth = 0
        Me.MaterialRadioButton1.Font = New System.Drawing.Font("Microsoft JhengHei UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaterialRadioButton1.Location = New System.Drawing.Point(17, 127)
        Me.MaterialRadioButton1.Margin = New System.Windows.Forms.Padding(0)
        Me.MaterialRadioButton1.MouseLocation = New System.Drawing.Point(-1, -1)
        Me.MaterialRadioButton1.MouseState = MaterialSkin.MouseState.HOVER
        Me.MaterialRadioButton1.Name = "MaterialRadioButton1"
        Me.MaterialRadioButton1.Ripple = True
        Me.MaterialRadioButton1.Size = New System.Drawing.Size(238, 30)
        Me.MaterialRadioButton1.TabIndex = 7
        Me.MaterialRadioButton1.Text = "translate(info.premium.premium)"
        Me.MaterialRadioButton1.UseVisualStyleBackColor = False
        '
        'MaterialFlatButton1
        '
        Me.MaterialFlatButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MaterialFlatButton1.BackColor = System.Drawing.Color.PaleGreen
        Me.MaterialFlatButton1.Depth = 0
        Me.MaterialFlatButton1.HoverColor = System.Drawing.Color.LimeGreen
        Me.MaterialFlatButton1.Location = New System.Drawing.Point(260, 127)
        Me.MaterialFlatButton1.Margin = New System.Windows.Forms.Padding(5, 8, 5, 8)
        Me.MaterialFlatButton1.MouseState = MaterialSkin.MouseState.HOVER
        Me.MaterialFlatButton1.Name = "MaterialFlatButton1"
        Me.MaterialFlatButton1.Primary = False
        Me.MaterialFlatButton1.Size = New System.Drawing.Size(87, 29)
        Me.MaterialFlatButton1.TabIndex = 6
        Me.MaterialFlatButton1.Text = "translate(info.premium.login)"
        Me.MaterialFlatButton1.UseVisualStyleBackColor = False
        '
        'MaterialSingleLineTextField2
        '
        Me.MaterialSingleLineTextField2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MaterialSingleLineTextField2.BackColor = System.Drawing.Color.Gainsboro
        Me.MaterialSingleLineTextField2.Depth = 0
        Me.MaterialSingleLineTextField2.Hint = ""
        Me.MaterialSingleLineTextField2.Location = New System.Drawing.Point(93, 88)
        Me.MaterialSingleLineTextField2.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaterialSingleLineTextField2.MaxLength = 32767
        Me.MaterialSingleLineTextField2.MouseState = MaterialSkin.MouseState.HOVER
        Me.MaterialSingleLineTextField2.Name = "MaterialSingleLineTextField2"
        Me.MaterialSingleLineTextField2.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.MaterialSingleLineTextField2.SelectedText = ""
        Me.MaterialSingleLineTextField2.SelectionLength = 0
        Me.MaterialSingleLineTextField2.SelectionStart = 0
        Me.MaterialSingleLineTextField2.Size = New System.Drawing.Size(254, 23)
        Me.MaterialSingleLineTextField2.TabIndex = 5
        Me.MaterialSingleLineTextField2.TabStop = False
        Me.MaterialSingleLineTextField2.UseSystemPasswordChar = True
        '
        'MaterialSingleLineTextField1
        '
        Me.MaterialSingleLineTextField1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MaterialSingleLineTextField1.BackColor = System.Drawing.Color.Gainsboro
        Me.MaterialSingleLineTextField1.Depth = 0
        Me.MaterialSingleLineTextField1.Hint = ""
        Me.MaterialSingleLineTextField1.Location = New System.Drawing.Point(93, 51)
        Me.MaterialSingleLineTextField1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaterialSingleLineTextField1.MaxLength = 32767
        Me.MaterialSingleLineTextField1.MouseState = MaterialSkin.MouseState.HOVER
        Me.MaterialSingleLineTextField1.Name = "MaterialSingleLineTextField1"
        Me.MaterialSingleLineTextField1.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.MaterialSingleLineTextField1.SelectedText = ""
        Me.MaterialSingleLineTextField1.SelectionLength = 0
        Me.MaterialSingleLineTextField1.SelectionStart = 0
        Me.MaterialSingleLineTextField1.Size = New System.Drawing.Size(254, 23)
        Me.MaterialSingleLineTextField1.TabIndex = 4
        Me.MaterialSingleLineTextField1.TabStop = False
        Me.MaterialSingleLineTextField1.UseSystemPasswordChar = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft JhengHei UI", 20.0!)
        Me.Label4.Location = New System.Drawing.Point(17, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(383, 35)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "translate(info.premium.login)"
        '
        'Button1
        '
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Location = New System.Drawing.Point(335, 0)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(25, 25)
        Me.Button1.TabIndex = 11
        Me.Button1.Text = "X"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'LoginGUI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightGray
        Me.ClientSize = New System.Drawing.Size(361, 259)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.MaterialRadioButton2)
        Me.Controls.Add(Me.MaterialRadioButton1)
        Me.Controls.Add(Me.MaterialFlatButton1)
        Me.Controls.Add(Me.MaterialSingleLineTextField2)
        Me.Controls.Add(Me.MaterialSingleLineTextField1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Microsoft JhengHei UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "LoginGUI"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "info.premium.login"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents MaterialSingleLineTextField1 As MaterialSkin.Controls.MaterialSingleLineTextField
    Friend WithEvents MaterialSingleLineTextField2 As MaterialSkin.Controls.MaterialSingleLineTextField
    Friend WithEvents MaterialFlatButton1 As MaterialSkin.Controls.MaterialFlatButton
    Friend WithEvents MaterialRadioButton1 As MaterialSkin.Controls.MaterialRadioButton
    Friend WithEvents MaterialRadioButton2 As MaterialSkin.Controls.MaterialRadioButton
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Button1 As Button
End Class
