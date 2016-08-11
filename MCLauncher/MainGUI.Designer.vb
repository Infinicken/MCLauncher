<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainGUI
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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.MaterialFlatButton3 = New MaterialSkin.Controls.MaterialFlatButton()
        Me.MaterialFlatButton2 = New MaterialSkin.Controls.MaterialFlatButton()
        Me.MaterialFlatButton1 = New MaterialSkin.Controls.MaterialFlatButton()
        Me.MaterialFlatButton4 = New MaterialSkin.Controls.MaterialFlatButton()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Location = New System.Drawing.Point(575, 0)
        Me.Button1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(29, 31)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "X"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(517, 144)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "translate(info.applet.____DEBUG)"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'MaterialFlatButton3
        '
        Me.MaterialFlatButton3.BackColor = System.Drawing.Color.NavajoWhite
        Me.MaterialFlatButton3.Depth = 0
        Me.MaterialFlatButton3.HoverColor = System.Drawing.Color.DarkOrange
        Me.MaterialFlatButton3.Image = Global.jacky8399.MCLauncher.My.Resources.Resources.ic_search_black_48dp_1x
        Me.MaterialFlatButton3.Location = New System.Drawing.Point(329, 85)
        Me.MaterialFlatButton3.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.MaterialFlatButton3.MouseState = MaterialSkin.MouseState.HOVER
        Me.MaterialFlatButton3.Name = "MaterialFlatButton3"
        Me.MaterialFlatButton3.Primary = False
        Me.MaterialFlatButton3.Size = New System.Drawing.Size(150, 200)
        Me.MaterialFlatButton3.TabIndex = 6
        Me.MaterialFlatButton3.Text = "translate(btn.main.chkFile)"
        Me.MaterialFlatButton3.UseVisualStyleBackColor = False
        '
        'MaterialFlatButton2
        '
        Me.MaterialFlatButton2.BackColor = System.Drawing.Color.LightGreen
        Me.MaterialFlatButton2.Depth = 0
        Me.MaterialFlatButton2.HoverColor = System.Drawing.Color.LimeGreen
        Me.MaterialFlatButton2.Image = Global.jacky8399.MCLauncher.My.Resources.Resources.ic_build_black_48dp_1x
        Me.MaterialFlatButton2.Location = New System.Drawing.Point(171, 85)
        Me.MaterialFlatButton2.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.MaterialFlatButton2.MouseState = MaterialSkin.MouseState.HOVER
        Me.MaterialFlatButton2.Name = "MaterialFlatButton2"
        Me.MaterialFlatButton2.Primary = False
        Me.MaterialFlatButton2.Size = New System.Drawing.Size(150, 200)
        Me.MaterialFlatButton2.TabIndex = 5
        Me.MaterialFlatButton2.Text = "translate(btn.main.settings)"
        Me.MaterialFlatButton2.UseVisualStyleBackColor = False
        '
        'MaterialFlatButton1
        '
        Me.MaterialFlatButton1.BackColor = System.Drawing.Color.LightBlue
        Me.MaterialFlatButton1.Depth = 0
        Me.MaterialFlatButton1.HoverColor = System.Drawing.Color.SteelBlue
        Me.MaterialFlatButton1.Image = Global.jacky8399.MCLauncher.My.Resources.Resources.ic_play_arrow_black_48dp_1x
        Me.MaterialFlatButton1.Location = New System.Drawing.Point(13, 85)
        Me.MaterialFlatButton1.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.MaterialFlatButton1.MouseState = MaterialSkin.MouseState.HOVER
        Me.MaterialFlatButton1.Name = "MaterialFlatButton1"
        Me.MaterialFlatButton1.Primary = True
        Me.MaterialFlatButton1.Size = New System.Drawing.Size(150, 200)
        Me.MaterialFlatButton1.TabIndex = 3
        Me.MaterialFlatButton1.Text = "translate(btn.main.launch)"
        Me.MaterialFlatButton1.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.MaterialFlatButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.MaterialFlatButton1.UseVisualStyleBackColor = False
        '
        'MaterialFlatButton4
        '
        Me.MaterialFlatButton4.BackColor = System.Drawing.Color.MediumSlateBlue
        Me.MaterialFlatButton4.Depth = 0
        Me.MaterialFlatButton4.HoverColor = System.Drawing.Color.BlueViolet
        Me.MaterialFlatButton4.Location = New System.Drawing.Point(13, 297)
        Me.MaterialFlatButton4.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.MaterialFlatButton4.MouseState = MaterialSkin.MouseState.HOVER
        Me.MaterialFlatButton4.Name = "MaterialFlatButton4"
        Me.MaterialFlatButton4.Primary = False
        Me.MaterialFlatButton4.Size = New System.Drawing.Size(150, 200)
        Me.MaterialFlatButton4.TabIndex = 7
        Me.MaterialFlatButton4.Text = "translate(btn.main.console)"
        Me.MaterialFlatButton4.UseVisualStyleBackColor = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Location = New System.Drawing.Point(329, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(70, 70)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 8
        Me.PictureBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft JhengHei", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label1.Location = New System.Drawing.Point(405, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(310, 24)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "translate(info.premium.loginFirst)"
        '
        'MainGUI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(604, 508)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.MaterialFlatButton4)
        Me.Controls.Add(Me.MaterialFlatButton3)
        Me.Controls.Add(Me.MaterialFlatButton2)
        Me.Controls.Add(Me.MaterialFlatButton1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Microsoft JhengHei", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "MainGUI"
        Me.ShowIcon = False
        Me.Text = "translate(info.applet.name)"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents MaterialFlatButton1 As MaterialSkin.Controls.MaterialFlatButton
    Friend WithEvents MaterialFlatButton2 As MaterialSkin.Controls.MaterialFlatButton
    Friend WithEvents MaterialFlatButton3 As MaterialSkin.Controls.MaterialFlatButton
    Friend WithEvents MaterialFlatButton4 As MaterialSkin.Controls.MaterialFlatButton
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label1 As Label
End Class
