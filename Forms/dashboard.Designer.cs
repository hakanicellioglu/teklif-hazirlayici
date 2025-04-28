namespace Teklif_Hazırlayıcı.Forms
{
    partial class dashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dashboard));
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnOffer = new System.Windows.Forms.Button();
            this.btnProduct = new System.Windows.Forms.Button();
            this.btnAuth = new System.Windows.Forms.Button();
            this.btnCompany = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlForm = new System.Windows.Forms.Panel();
            this.imgButton = new System.Windows.Forms.ImageList(this.components);
            this.pnlMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMenu
            // 
            this.pnlMenu.Controls.Add(this.btnLogout);
            this.pnlMenu.Controls.Add(this.btnSettings);
            this.pnlMenu.Controls.Add(this.btnOffer);
            this.pnlMenu.Controls.Add(this.btnProduct);
            this.pnlMenu.Controls.Add(this.btnAuth);
            this.pnlMenu.Controls.Add(this.btnCompany);
            this.pnlMenu.Controls.Add(this.btnHome);
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Padding = new System.Windows.Forms.Padding(25);
            this.pnlMenu.Size = new System.Drawing.Size(1264, 100);
            this.pnlMenu.TabIndex = 0;
            // 
            // btnSettings
            // 
            this.btnSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSettings.ImageIndex = 5;
            this.btnSettings.ImageList = this.imgButton;
            this.btnSettings.Location = new System.Drawing.Point(275, 25);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(50, 50);
            this.btnSettings.TabIndex = 5;
            this.toolTip1.SetToolTip(this.btnSettings, "Ayarlar");
            this.btnSettings.UseVisualStyleBackColor = true;
            // 
            // btnLogout
            // 
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnLogout.ImageIndex = 6;
            this.btnLogout.ImageList = this.imgButton;
            this.btnLogout.Location = new System.Drawing.Point(325, 25);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(50, 50);
            this.btnLogout.TabIndex = 6;
            this.toolTip1.SetToolTip(this.btnLogout, "Çıkış Yap");
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnOffer
            // 
            this.btnOffer.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnOffer.ImageIndex = 4;
            this.btnOffer.ImageList = this.imgButton;
            this.btnOffer.Location = new System.Drawing.Point(225, 25);
            this.btnOffer.Name = "btnOffer";
            this.btnOffer.Size = new System.Drawing.Size(50, 50);
            this.btnOffer.TabIndex = 4;
            this.toolTip1.SetToolTip(this.btnOffer, "Teklifler");
            this.btnOffer.UseVisualStyleBackColor = true;
            this.btnOffer.Click += new System.EventHandler(this.btnOffer_Click);
            // 
            // btnProduct
            // 
            this.btnProduct.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnProduct.ImageIndex = 3;
            this.btnProduct.ImageList = this.imgButton;
            this.btnProduct.Location = new System.Drawing.Point(175, 25);
            this.btnProduct.Name = "btnProduct";
            this.btnProduct.Size = new System.Drawing.Size(50, 50);
            this.btnProduct.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnProduct, "Ürünler");
            this.btnProduct.UseVisualStyleBackColor = true;
            this.btnProduct.Click += new System.EventHandler(this.btnProduct_Click);
            // 
            // btnAuth
            // 
            this.btnAuth.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnAuth.ImageIndex = 2;
            this.btnAuth.ImageList = this.imgButton;
            this.btnAuth.Location = new System.Drawing.Point(125, 25);
            this.btnAuth.Name = "btnAuth";
            this.btnAuth.Size = new System.Drawing.Size(50, 50);
            this.btnAuth.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnAuth, "Yetkililer");
            this.btnAuth.UseVisualStyleBackColor = true;
            this.btnAuth.Click += new System.EventHandler(this.btnAuth_Click);
            // 
            // btnCompany
            // 
            this.btnCompany.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCompany.FlatAppearance.BorderSize = 0;
            this.btnCompany.ImageIndex = 1;
            this.btnCompany.ImageList = this.imgButton;
            this.btnCompany.Location = new System.Drawing.Point(75, 25);
            this.btnCompany.Name = "btnCompany";
            this.btnCompany.Size = new System.Drawing.Size(50, 50);
            this.btnCompany.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnCompany, "Firmalar");
            this.btnCompany.UseVisualStyleBackColor = true;
            this.btnCompany.Click += new System.EventHandler(this.btnCompany_Click);
            // 
            // btnHome
            // 
            this.btnHome.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.ImageIndex = 0;
            this.btnHome.ImageList = this.imgButton;
            this.btnHome.Location = new System.Drawing.Point(25, 25);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(50, 50);
            this.btnHome.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btnHome, "Anasayfa");
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipTitle = "Teklif Hazırlayıcı";
            // 
            // pnlForm
            // 
            this.pnlForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlForm.Location = new System.Drawing.Point(0, 100);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(1264, 581);
            this.pnlForm.TabIndex = 1;
            // 
            // imgButton
            // 
            this.imgButton.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgButton.ImageStream")));
            this.imgButton.TransparentColor = System.Drawing.Color.Transparent;
            this.imgButton.Images.SetKeyName(0, "home-48.png");
            this.imgButton.Images.SetKeyName(1, "factory-48.png");
            this.imgButton.Images.SetKeyName(2, "users-48.png");
            this.imgButton.Images.SetKeyName(3, "product-48.png");
            this.imgButton.Images.SetKeyName(4, "document-48.png");
            this.imgButton.Images.SetKeyName(5, "settings-48.png");
            this.imgButton.Images.SetKeyName(6, "logout-48.png");
            // 
            // dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.pnlForm);
            this.Controls.Add(this.pnlMenu);
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "dashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Teklif Hazırlayıcı";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlMenu.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel pnlForm;
        private System.Windows.Forms.ImageList imgButton;
    }
}