namespace Teklif_Hazırlayıcı.Forms.Editor
{
    partial class settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblDefaultNote = new System.Windows.Forms.Label();
            this.txtDefaultNote = new System.Windows.Forms.TextBox();
            this.lblTheme = new System.Windows.Forms.Label();
            this.cmbTheme = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkSmsNewOffer = new System.Windows.Forms.CheckBox();
            this.chkSmsApproval = new System.Windows.Forms.CheckBox();
            this.chkEmailNewOffer = new System.Windows.Forms.CheckBox();
            this.chkEmailApproval = new System.Windows.Forms.CheckBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.btnBrowseLogo = new System.Windows.Forms.Button();
            this.lblSignature = new System.Windows.Forms.Label();
            this.txtSignature = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDefaultNote
            // 
            this.lblDefaultNote.AutoSize = true;
            this.lblDefaultNote.Location = new System.Drawing.Point(14, 19);
            this.lblDefaultNote.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefaultNote.Name = "lblDefaultNote";
            this.lblDefaultNote.Size = new System.Drawing.Size(128, 17);
            this.lblDefaultNote.TabIndex = 0;
            this.lblDefaultNote.Text = "Varsayılan Açıklama";
            // 
            // txtDefaultNote
            // 
            this.txtDefaultNote.Location = new System.Drawing.Point(175, 16);
            this.txtDefaultNote.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDefaultNote.Multiline = true;
            this.txtDefaultNote.Name = "txtDefaultNote";
            this.txtDefaultNote.Size = new System.Drawing.Size(349, 77);
            this.txtDefaultNote.TabIndex = 1;
            // 
            // lblTheme
            // 
            this.lblTheme.AutoSize = true;
            this.lblTheme.Location = new System.Drawing.Point(14, 113);
            this.lblTheme.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTheme.Name = "lblTheme";
            this.lblTheme.Size = new System.Drawing.Size(40, 17);
            this.lblTheme.TabIndex = 2;
            this.lblTheme.Text = "Tema";
            // 
            // cmbTheme
            // 
            this.cmbTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTheme.FormattingEnabled = true;
            this.cmbTheme.Items.AddRange(new object[] {
            "Light",
            "Dark"});
            this.cmbTheme.Location = new System.Drawing.Point(175, 108);
            this.cmbTheme.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbTheme.Name = "cmbTheme";
            this.cmbTheme.Size = new System.Drawing.Size(140, 25);
            this.cmbTheme.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkSmsNewOffer);
            this.groupBox1.Controls.Add(this.chkSmsApproval);
            this.groupBox1.Controls.Add(this.chkEmailNewOffer);
            this.groupBox1.Controls.Add(this.chkEmailApproval);
            this.groupBox1.Location = new System.Drawing.Point(18, 157);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(507, 91);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bildirimler";
            // 
            // chkSmsNewOffer
            // 
            this.chkSmsNewOffer.AutoSize = true;
            this.chkSmsNewOffer.Location = new System.Drawing.Point(251, 55);
            this.chkSmsNewOffer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkSmsNewOffer.Name = "chkSmsNewOffer";
            this.chkSmsNewOffer.Size = new System.Drawing.Size(125, 21);
            this.chkSmsNewOffer.TabIndex = 3;
            this.chkSmsNewOffer.Text = "Yeni teklifte SMS";
            this.chkSmsNewOffer.UseVisualStyleBackColor = true;
            // 
            // chkSmsApproval
            // 
            this.chkSmsApproval.AutoSize = true;
            this.chkSmsApproval.Location = new System.Drawing.Point(251, 24);
            this.chkSmsApproval.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkSmsApproval.Name = "chkSmsApproval";
            this.chkSmsApproval.Size = new System.Drawing.Size(142, 21);
            this.chkSmsApproval.TabIndex = 2;
            this.chkSmsApproval.Text = "Teklif onayında SMS";
            this.chkSmsApproval.UseVisualStyleBackColor = true;
            // 
            // chkEmailNewOffer
            // 
            this.chkEmailNewOffer.AutoSize = true;
            this.chkEmailNewOffer.Location = new System.Drawing.Point(7, 55);
            this.chkEmailNewOffer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkEmailNewOffer.Name = "chkEmailNewOffer";
            this.chkEmailNewOffer.Size = new System.Drawing.Size(146, 21);
            this.chkEmailNewOffer.TabIndex = 1;
            this.chkEmailNewOffer.Text = "Yeni teklifte e-posta";
            this.chkEmailNewOffer.UseVisualStyleBackColor = true;
            // 
            // chkEmailApproval
            // 
            this.chkEmailApproval.AutoSize = true;
            this.chkEmailApproval.Location = new System.Drawing.Point(7, 24);
            this.chkEmailApproval.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkEmailApproval.Name = "chkEmailApproval";
            this.chkEmailApproval.Size = new System.Drawing.Size(163, 21);
            this.chkEmailApproval.TabIndex = 0;
            this.chkEmailApproval.Text = "Teklif onayında e-posta";
            this.chkEmailApproval.UseVisualStyleBackColor = true;
            // 
            // picLogo
            // 
            this.picLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLogo.Location = new System.Drawing.Point(18, 268);
            this.picLogo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(175, 104);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 5;
            this.picLogo.TabStop = false;
            // 
            // btnBrowseLogo
            // 
            this.btnBrowseLogo.Location = new System.Drawing.Point(200, 342);
            this.btnBrowseLogo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowseLogo.Name = "btnBrowseLogo";
            this.btnBrowseLogo.Size = new System.Drawing.Size(88, 30);
            this.btnBrowseLogo.TabIndex = 6;
            this.btnBrowseLogo.Text = "Logo Seç";
            this.btnBrowseLogo.UseVisualStyleBackColor = true;
            this.btnBrowseLogo.Click += new System.EventHandler(this.btnBrowseLogo_Click);
            // 
            // lblSignature
            // 
            this.lblSignature.AutoSize = true;
            this.lblSignature.Location = new System.Drawing.Point(14, 390);
            this.lblSignature.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSignature.Name = "lblSignature";
            this.lblSignature.Size = new System.Drawing.Size(72, 17);
            this.lblSignature.TabIndex = 7;
            this.lblSignature.Text = "Dijital İmza";
            // 
            // txtSignature
            // 
            this.txtSignature.Location = new System.Drawing.Point(175, 386);
            this.txtSignature.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSignature.Name = "txtSignature";
            this.txtSignature.Size = new System.Drawing.Size(349, 22);
            this.txtSignature.TabIndex = 8;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(14, 424);
            this.lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(25, 17);
            this.lblName.TabIndex = 9;
            this.lblName.Text = "Ad";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(175, 420);
            this.txtName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(349, 22);
            this.txtName.TabIndex = 10;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(14, 458);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(46, 17);
            this.lblTitle.TabIndex = 11;
            this.lblTitle.Text = "Ünvan";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(175, 454);
            this.txtTitle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(349, 22);
            this.txtTitle.TabIndex = 12;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(438, 504);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 30);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Kaydet";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 549);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtSignature);
            this.Controls.Add(this.lblSignature);
            this.Controls.Add(this.btnBrowseLogo);
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmbTheme);
            this.Controls.Add(this.lblTheme);
            this.Controls.Add(this.txtDefaultNote);
            this.Controls.Add(this.lblDefaultNote);
            this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "settings";
            this.Text = "Ayarlar";
            this.Load += new System.EventHandler(this.settings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDefaultNote;
        private System.Windows.Forms.TextBox txtDefaultNote;
        private System.Windows.Forms.Label lblTheme;
        private System.Windows.Forms.ComboBox cmbTheme;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkSmsNewOffer;
        private System.Windows.Forms.CheckBox chkSmsApproval;
        private System.Windows.Forms.CheckBox chkEmailNewOffer;
        private System.Windows.Forms.CheckBox chkEmailApproval;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Button btnBrowseLogo;
        private System.Windows.Forms.Label lblSignature;
        private System.Windows.Forms.TextBox txtSignature;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Button btnSave;
    }
}
