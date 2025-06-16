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
            this.lblDefaultNote.Location = new System.Drawing.Point(12, 15);
            this.lblDefaultNote.Name = "lblDefaultNote";
            this.lblDefaultNote.Size = new System.Drawing.Size(101, 13);
            this.lblDefaultNote.TabIndex = 0;
            this.lblDefaultNote.Text = "Varsayılan Açıklama";
            // 
            // txtDefaultNote
            // 
            this.txtDefaultNote.Location = new System.Drawing.Point(150, 12);
            this.txtDefaultNote.Multiline = true;
            this.txtDefaultNote.Name = "txtDefaultNote";
            this.txtDefaultNote.Size = new System.Drawing.Size(300, 60);
            this.txtDefaultNote.TabIndex = 1;
            // 
            // lblTheme
            // 
            this.lblTheme.AutoSize = true;
            this.lblTheme.Location = new System.Drawing.Point(12, 86);
            this.lblTheme.Name = "lblTheme";
            this.lblTheme.Size = new System.Drawing.Size(34, 13);
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
            this.cmbTheme.Location = new System.Drawing.Point(150, 83);
            this.cmbTheme.Name = "cmbTheme";
            this.cmbTheme.Size = new System.Drawing.Size(121, 21);
            this.cmbTheme.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkSmsNewOffer);
            this.groupBox1.Controls.Add(this.chkSmsApproval);
            this.groupBox1.Controls.Add(this.chkEmailNewOffer);
            this.groupBox1.Controls.Add(this.chkEmailApproval);
            this.groupBox1.Location = new System.Drawing.Point(15, 120);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(435, 70);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bildirimler";
            // 
            // chkSmsNewOffer
            // 
            this.chkSmsNewOffer.AutoSize = true;
            this.chkSmsNewOffer.Location = new System.Drawing.Point(215, 42);
            this.chkSmsNewOffer.Name = "chkSmsNewOffer";
            this.chkSmsNewOffer.Size = new System.Drawing.Size(107, 17);
            this.chkSmsNewOffer.TabIndex = 3;
            this.chkSmsNewOffer.Text = "Yeni teklifte SMS";
            this.chkSmsNewOffer.UseVisualStyleBackColor = true;
            // 
            // chkSmsApproval
            // 
            this.chkSmsApproval.AutoSize = true;
            this.chkSmsApproval.Location = new System.Drawing.Point(215, 19);
            this.chkSmsApproval.Name = "chkSmsApproval";
            this.chkSmsApproval.Size = new System.Drawing.Size(124, 17);
            this.chkSmsApproval.TabIndex = 2;
            this.chkSmsApproval.Text = "Teklif onayında SMS";
            this.chkSmsApproval.UseVisualStyleBackColor = true;
            // 
            // chkEmailNewOffer
            // 
            this.chkEmailNewOffer.AutoSize = true;
            this.chkEmailNewOffer.Location = new System.Drawing.Point(6, 42);
            this.chkEmailNewOffer.Name = "chkEmailNewOffer";
            this.chkEmailNewOffer.Size = new System.Drawing.Size(119, 17);
            this.chkEmailNewOffer.TabIndex = 1;
            this.chkEmailNewOffer.Text = "Yeni teklifte e-posta";
            this.chkEmailNewOffer.UseVisualStyleBackColor = true;
            // 
            // chkEmailApproval
            // 
            this.chkEmailApproval.AutoSize = true;
            this.chkEmailApproval.Location = new System.Drawing.Point(6, 19);
            this.chkEmailApproval.Name = "chkEmailApproval";
            this.chkEmailApproval.Size = new System.Drawing.Size(136, 17);
            this.chkEmailApproval.TabIndex = 0;
            this.chkEmailApproval.Text = "Teklif onayında e-posta";
            this.chkEmailApproval.UseVisualStyleBackColor = true;
            // 
            // picLogo
            // 
            this.picLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLogo.Location = new System.Drawing.Point(15, 205);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(150, 80);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 5;
            this.picLogo.TabStop = false;
            // 
            // btnBrowseLogo
            // 
            this.btnBrowseLogo.Location = new System.Drawing.Point(171, 262);
            this.btnBrowseLogo.Name = "btnBrowseLogo";
            this.btnBrowseLogo.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseLogo.TabIndex = 6;
            this.btnBrowseLogo.Text = "Logo Seç";
            this.btnBrowseLogo.UseVisualStyleBackColor = true;
            this.btnBrowseLogo.Click += new System.EventHandler(this.btnBrowseLogo_Click);
            // 
            // lblSignature
            // 
            this.lblSignature.AutoSize = true;
            this.lblSignature.Location = new System.Drawing.Point(12, 298);
            this.lblSignature.Name = "lblSignature";
            this.lblSignature.Size = new System.Drawing.Size(57, 13);
            this.lblSignature.TabIndex = 7;
            this.lblSignature.Text = "Dijital İmza";
            // 
            // txtSignature
            // 
            this.txtSignature.Location = new System.Drawing.Point(150, 295);
            this.txtSignature.Name = "txtSignature";
            this.txtSignature.Size = new System.Drawing.Size(300, 20);
            this.txtSignature.TabIndex = 8;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 324);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(20, 13);
            this.lblName.TabIndex = 9;
            this.lblName.Text = "Ad";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(150, 321);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(300, 20);
            this.txtName.TabIndex = 10;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(12, 350);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(39, 13);
            this.lblTitle.TabIndex = 11;
            this.lblTitle.Text = "Ünvan";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(150, 347);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(300, 20);
            this.txtTitle.TabIndex = 12;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(375, 385);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Kaydet";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 420);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "settings";
            this.Tag = "form";
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
