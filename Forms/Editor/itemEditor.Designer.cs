namespace Teklif_Hazırlayıcı.Forms.Editor
{
    partial class itemEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkUrunler = new System.Windows.Forms.ComboBox();
            this.lblBoy = new System.Windows.Forms.Label();
            this.txtBoy = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAdet = new System.Windows.Forms.TextBox();
            this.lblYuzey = new System.Windows.Forms.Label();
            this.chkYuzey = new System.Windows.Forms.ComboBox();
            this.lblYuzeyKodu = new System.Windows.Forms.Label();
            this.txtYuzeyKodu = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(101, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ürün Editörü";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(80, 69);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Ürün";
            // 
            // chkUrunler
            // 
            this.chkUrunler.FormattingEnabled = true;
            this.chkUrunler.Location = new System.Drawing.Point(83, 91);
            this.chkUrunler.Margin = new System.Windows.Forms.Padding(5);
            this.chkUrunler.Name = "chkUrunler";
            this.chkUrunler.Size = new System.Drawing.Size(121, 21);
            this.chkUrunler.TabIndex = 2;
            this.chkUrunler.SelectedIndexChanged += new System.EventHandler(this.chkUrunler_SelectedIndexChanged);
            // 
            // lblBoy
            // 
            this.lblBoy.AutoSize = true;
            this.lblBoy.Location = new System.Drawing.Point(80, 172);
            this.lblBoy.Margin = new System.Windows.Forms.Padding(5);
            this.lblBoy.Name = "lblBoy";
            this.lblBoy.Size = new System.Drawing.Size(25, 13);
            this.lblBoy.TabIndex = 5;
            this.lblBoy.Text = "Boy";
            this.lblBoy.Visible = false;
            // 
            // txtBoy
            // 
            this.txtBoy.Location = new System.Drawing.Point(83, 194);
            this.txtBoy.Margin = new System.Windows.Forms.Padding(5);
            this.txtBoy.Name = "txtBoy";
            this.txtBoy.Size = new System.Drawing.Size(121, 20);
            this.txtBoy.TabIndex = 6;
            this.txtBoy.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(80, 121);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Adet";
            // 
            // txtAdet
            // 
            this.txtAdet.Location = new System.Drawing.Point(83, 143);
            this.txtAdet.Margin = new System.Windows.Forms.Padding(5);
            this.txtAdet.Name = "txtAdet";
            this.txtAdet.Size = new System.Drawing.Size(121, 20);
            this.txtAdet.TabIndex = 4;
            // 
            // lblYuzey
            // 
            this.lblYuzey.AutoSize = true;
            this.lblYuzey.Location = new System.Drawing.Point(80, 223);
            this.lblYuzey.Margin = new System.Windows.Forms.Padding(5);
            this.lblYuzey.Name = "lblYuzey";
            this.lblYuzey.Size = new System.Drawing.Size(36, 13);
            this.lblYuzey.TabIndex = 7;
            this.lblYuzey.Text = "Yüzey";
            this.lblYuzey.Visible = false;
            // 
            // chkYuzey
            // 
            this.chkYuzey.FormattingEnabled = true;
            this.chkYuzey.Items.AddRange(new object[] {
            "Press",
            "Boyalı"});
            this.chkYuzey.Location = new System.Drawing.Point(83, 245);
            this.chkYuzey.Margin = new System.Windows.Forms.Padding(5);
            this.chkYuzey.Name = "chkYuzey";
            this.chkYuzey.Size = new System.Drawing.Size(121, 21);
            this.chkYuzey.TabIndex = 8;
            this.chkYuzey.Visible = false;
            this.chkYuzey.SelectedIndexChanged += new System.EventHandler(this.chkYuzey_SelectedIndexChanged);
            // 
            // lblYuzeyKodu
            // 
            this.lblYuzeyKodu.AutoSize = true;
            this.lblYuzeyKodu.Location = new System.Drawing.Point(80, 275);
            this.lblYuzeyKodu.Margin = new System.Windows.Forms.Padding(5);
            this.lblYuzeyKodu.Name = "lblYuzeyKodu";
            this.lblYuzeyKodu.Size = new System.Drawing.Size(64, 13);
            this.lblYuzeyKodu.TabIndex = 9;
            this.lblYuzeyKodu.Text = "Yüzey Kodu";
            this.lblYuzeyKodu.Visible = false;
            // 
            // txtYuzeyKodu
            // 
            this.txtYuzeyKodu.Location = new System.Drawing.Point(83, 297);
            this.txtYuzeyKodu.Margin = new System.Windows.Forms.Padding(5);
            this.txtYuzeyKodu.Name = "txtYuzeyKodu";
            this.txtYuzeyKodu.Size = new System.Drawing.Size(121, 20);
            this.txtYuzeyKodu.TabIndex = 10;
            this.txtYuzeyKodu.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(129, 326);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Ekle";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(129, 358);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "Kaydet";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(129, 390);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "İptal";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // itemEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 461);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtYuzeyKodu);
            this.Controls.Add(this.lblYuzeyKodu);
            this.Controls.Add(this.chkYuzey);
            this.Controls.Add(this.lblYuzey);
            this.Controls.Add(this.txtAdet);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtBoy);
            this.Controls.Add(this.lblBoy);
            this.Controls.Add(this.chkUrunler);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "itemEditor";
            this.Padding = new System.Windows.Forms.Padding(50);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Teklif Hazırlayıcı";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox chkUrunler;
        private System.Windows.Forms.Label lblBoy;
        private System.Windows.Forms.TextBox txtBoy;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAdet;
        private System.Windows.Forms.Label lblYuzey;
        private System.Windows.Forms.ComboBox chkYuzey;
        private System.Windows.Forms.Label lblYuzeyKodu;
        private System.Windows.Forms.TextBox txtYuzeyKodu;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnCancel;
    }
}