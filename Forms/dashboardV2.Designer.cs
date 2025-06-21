namespace Teklif_Hazırlayıcı.Forms
{
    partial class dashboardV2
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dashboardV2));
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnTheme = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.pnlLink = new System.Windows.Forms.Panel();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnOffer = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.btnProduct = new System.Windows.Forms.Button();
            this.btnAuth = new System.Windows.Forms.Button();
            this.btnCompany = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.imgButton = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlForm = new System.Windows.Forms.Panel();
            this.pnlMenu.SuspendLayout();
            this.pnlLink.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlMenu
            //
            this.pnlMenu.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pnlMenu.Controls.Add(this.pnlLink);
            this.pnlMenu.Controls.Add(this.btnLogout);
            this.pnlMenu.Controls.Add(this.btnHome);
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Padding = new System.Windows.Forms.Padding(25);
            this.pnlMenu.Size = new System.Drawing.Size(200, 720);
            this.pnlMenu.TabIndex = 0;
            //
            // pnlLink
            //
            this.pnlLink.Controls.Add(this.btnSettings);
            this.pnlLink.Controls.Add(this.btnReports);
            this.pnlLink.Controls.Add(this.btnOffer);
            this.pnlLink.Controls.Add(this.btnProduct);
            this.pnlLink.Controls.Add(this.btnAuth);
            this.pnlLink.Controls.Add(this.btnCompany);
            this.pnlLink.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLink.Location = new System.Drawing.Point(25, 75);
            this.pnlLink.Name = "pnlLink";
            this.pnlLink.Size = new System.Drawing.Size(150, 570);
            this.pnlLink.TabIndex = 0;
            //
            // btnSettings
            //
            this.btnSettings.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSettings.FlatAppearance.BorderSize = 0;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.btnSettings.ImageIndex = 5;
            this.btnSettings.Location = new System.Drawing.Point(0, 520);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(150, 50);
            this.btnSettings.TabIndex = 4;
            this.btnSettings.Text = "Ayarlar";
            this.btnSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.btnSettings, "Ayarlar");
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            //
            // btnOffer
            //
            this.btnOffer.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOffer.FlatAppearance.BorderSize = 0;
            this.btnOffer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOffer.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.btnOffer.ImageIndex = 4;
            this.btnOffer.Location = new System.Drawing.Point(0, 150);
            this.btnOffer.Name = "btnOffer";
            this.btnOffer.Size = new System.Drawing.Size(150, 50);
            this.btnOffer.TabIndex = 3;
            this.btnOffer.Text = "Teklifler";
            this.btnOffer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.btnOffer, "Teklifler");
            this.btnOffer.UseVisualStyleBackColor = true;
            this.btnOffer.Click += new System.EventHandler(this.btnOffer_Click);
            //
            // btnReports
            //
            this.btnReports.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnReports.FlatAppearance.BorderSize = 0;
            this.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReports.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.btnReports.ImageIndex = 0;
            this.btnReports.Location = new System.Drawing.Point(0, 200);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(150, 50);
            this.btnReports.TabIndex = 5;
            this.btnReports.Text = "Raporlar";
            this.btnReports.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.btnReports, "Raporlar");
            this.btnReports.UseVisualStyleBackColor = true;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            //
            // btnProduct
            //
            this.btnProduct.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnProduct.FlatAppearance.BorderSize = 0;
            this.btnProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProduct.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.btnProduct.ImageIndex = 3;
            this.btnProduct.Location = new System.Drawing.Point(0, 100);
            this.btnProduct.Name = "btnProduct";
            this.btnProduct.Size = new System.Drawing.Size(150, 50);
            this.btnProduct.TabIndex = 2;
            this.btnProduct.Text = "Ürünler";
            this.btnProduct.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.btnProduct, "Ürünler");
            this.btnProduct.UseVisualStyleBackColor = true;
            this.btnProduct.Click += new System.EventHandler(this.btnProduct_Click);
            //
            // btnAuth
            //
            this.btnAuth.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAuth.FlatAppearance.BorderSize = 0;
            this.btnAuth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAuth.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.btnAuth.ImageIndex = 2;
            this.btnAuth.Location = new System.Drawing.Point(0, 50);
            this.btnAuth.Name = "btnAuth";
            this.btnAuth.Size = new System.Drawing.Size(150, 50);
            this.btnAuth.TabIndex = 1;
            this.btnAuth.Text = "Yetkililer";
            this.btnAuth.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.btnAuth, "Yetkililer");
            this.btnAuth.UseVisualStyleBackColor = true;
            this.btnAuth.Click += new System.EventHandler(this.btnAuth_Click);
            //
            // btnCompany
            //
            this.btnCompany.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCompany.FlatAppearance.BorderSize = 0;
            this.btnCompany.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompany.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnCompany.ImageIndex = 1;
            this.btnCompany.Location = new System.Drawing.Point(0, 0);
            this.btnCompany.Name = "btnCompany";
            this.btnCompany.Size = new System.Drawing.Size(150, 50);
            this.btnCompany.TabIndex = 0;
            this.btnCompany.Text = "Firmalar";
            this.btnCompany.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.btnCompany, "Firmalar");
            this.btnCompany.UseVisualStyleBackColor = true;
            this.btnCompany.Click += new System.EventHandler(this.btnCompany_Click);
            //
            // btnLogout
            //
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.btnLogout.ImageIndex = 6;
            this.btnLogout.Location = new System.Drawing.Point(25, 645);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(150, 50);
            this.btnLogout.TabIndex = 6;
            this.btnLogout.Text = "Çıkış Yap";
            this.btnLogout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.btnLogout, "Çıkış Yap");
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click_1);
            //
            // btnHome
            //
            this.btnHome.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.btnHome.Location = new System.Drawing.Point(25, 25);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(150, 50);
            this.btnHome.TabIndex = 1;
            this.btnHome.Text = "Anasayfa";
            this.btnHome.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            //
            // imgButton
            //
            this.imgButton.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgButton.ImageStream")));
            this.imgButton.TransparentColor = System.Drawing.Color.Transparent;
            this.imgButton.Images.SetKeyName(0, "left-48.png");
            //
            // toolTip1
            //
            this.toolTip1.ToolTipTitle = "Teklif Hazırlayıcı";
            //
            // pnlForm
            //
            this.pnlForm.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pnlForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlForm.Location = new System.Drawing.Point(200, 50);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(1080, 720);
            this.pnlForm.TabIndex = 1;
            //
            // pnlTop
            //
            this.pnlTop.Controls.Add(this.btnUpdate);
            this.pnlTop.Controls.Add(this.btnTheme);
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(200, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1080, 50);
            this.pnlTop.TabIndex = 2;
            //
            // txtSearch
            //
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(920, 20);
            this.txtSearch.TabIndex = 0;
            //
            // btnTheme
            //
            this.btnTheme.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnTheme.FlatAppearance.BorderSize = 0;
            this.btnTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTheme.Location = new System.Drawing.Point(920, 0);
            this.btnTheme.Name = "btnTheme";
            this.btnTheme.Size = new System.Drawing.Size(80, 50);
            this.btnTheme.TabIndex = 1;
            this.btnTheme.Text = "Tema";
            this.toolTip1.SetToolTip(this.btnTheme, "Tema Değiştir");
            this.btnTheme.UseVisualStyleBackColor = true;
            this.btnTheme.Click += new System.EventHandler(this.btnTheme_Click);
            //
            // btnUpdate
            //
            this.btnUpdate.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Location = new System.Drawing.Point(1000, 0);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(80, 50);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "Güncelle";
            this.toolTip1.SetToolTip(this.btnUpdate, "Güncellemeleri Kontrol Et");
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            //
            // dashboard
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.pnlForm);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "dashboardV2";
            this.Tag = "form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Teklif Hazırlayıcı";
            this.pnlMenu.ResumeLayout(false);
            this.pnlLink.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnCompany;
        private System.Windows.Forms.Button btnOffer;
        private System.Windows.Forms.Button btnProduct;
        private System.Windows.Forms.Button btnAuth;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel pnlForm;
        private System.Windows.Forms.ImageList imgButton;
        private System.Windows.Forms.Panel pnlLink;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnTheme;
        private System.Windows.Forms.Button btnUpdate;
    }
}