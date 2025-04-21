namespace Teklif_Hazırlayıcı.Forms.Editor
{
    partial class offerEditor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkFirmalar = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkYetkililer = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.chkTeslimSekli = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkOdemeSekli = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTeklifSuresi = new System.Windows.Forms.TextBox();
            this.chkDovizBirimi = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtLME = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtIskonto = new System.Windows.Forms.TextBox();
            this.txtTevkifat = new System.Windows.Forms.TextBox();
            this.chkTevkifat = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.chkDurum = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtDovizKuru = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtOdemeVadesi = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtKDV = new System.Windows.Forms.TextBox();
            this.chkVade = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label16 = new System.Windows.Forms.Label();
            this.txtİscilik = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Teklif Editörü";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Firma";
            // 
            // chkFirmalar
            // 
            this.chkFirmalar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chkFirmalar.FormattingEnabled = true;
            this.chkFirmalar.Items.AddRange(new object[] {
            "(boş)"});
            this.chkFirmalar.Location = new System.Drawing.Point(53, 96);
            this.chkFirmalar.Name = "chkFirmalar";
            this.chkFirmalar.Size = new System.Drawing.Size(121, 21);
            this.chkFirmalar.TabIndex = 2;
            this.chkFirmalar.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(177, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Yetkili";
            // 
            // chkYetkililer
            // 
            this.chkYetkililer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chkYetkililer.Enabled = false;
            this.chkYetkililer.FormattingEnabled = true;
            this.chkYetkililer.Location = new System.Drawing.Point(180, 96);
            this.chkYetkililer.Name = "chkYetkililer";
            this.chkYetkililer.Size = new System.Drawing.Size(121, 21);
            this.chkYetkililer.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(304, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Tarih";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(307, 96);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(153, 20);
            this.dateTimePicker1.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(50, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Teslim Şekli";
            // 
            // chkTeslimSekli
            // 
            this.chkTeslimSekli.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chkTeslimSekli.FormattingEnabled = true;
            this.chkTeslimSekli.Items.AddRange(new object[] {
            "Fabrika Teslim"});
            this.chkTeslimSekli.Location = new System.Drawing.Point(53, 136);
            this.chkTeslimSekli.Name = "chkTeslimSekli";
            this.chkTeslimSekli.Size = new System.Drawing.Size(121, 21);
            this.chkTeslimSekli.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(50, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Ödeme Şekli";
            // 
            // chkOdemeSekli
            // 
            this.chkOdemeSekli.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chkOdemeSekli.FormattingEnabled = true;
            this.chkOdemeSekli.Items.AddRange(new object[] {
            "Nakit"});
            this.chkOdemeSekli.Location = new System.Drawing.Point(53, 176);
            this.chkOdemeSekli.Name = "chkOdemeSekli";
            this.chkOdemeSekli.Size = new System.Drawing.Size(121, 21);
            this.chkOdemeSekli.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(50, 200);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Teklif Süresi";
            // 
            // txtTeklifSuresi
            // 
            this.txtTeklifSuresi.Location = new System.Drawing.Point(53, 216);
            this.txtTeklifSuresi.Name = "txtTeklifSuresi";
            this.txtTeklifSuresi.Size = new System.Drawing.Size(121, 20);
            this.txtTeklifSuresi.TabIndex = 18;
            // 
            // chkDovizBirimi
            // 
            this.chkDovizBirimi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chkDovizBirimi.FormattingEnabled = true;
            this.chkDovizBirimi.Items.AddRange(new object[] {
            "₺",
            "$"});
            this.chkDovizBirimi.Location = new System.Drawing.Point(286, 175);
            this.chkDovizBirimi.Name = "chkDovizBirimi";
            this.chkDovizBirimi.Size = new System.Drawing.Size(60, 21);
            this.chkDovizBirimi.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(283, 159);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Döviz Birimi";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(350, 159);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Döviz Kuru";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(50, 239);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "Vade";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(50, 278);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "LME(0.##)";
            // 
            // txtLME
            // 
            this.txtLME.Location = new System.Drawing.Point(53, 294);
            this.txtLME.Name = "txtLME";
            this.txtLME.Size = new System.Drawing.Size(121, 20);
            this.txtLME.TabIndex = 24;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(177, 278);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "İskonto(0.##)";
            // 
            // txtIskonto
            // 
            this.txtIskonto.Location = new System.Drawing.Point(180, 294);
            this.txtIskonto.Name = "txtIskonto";
            this.txtIskonto.Size = new System.Drawing.Size(121, 20);
            this.txtIskonto.TabIndex = 26;
            this.txtIskonto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIskonto_KeyPress);
            // 
            // txtTevkifat
            // 
            this.txtTevkifat.Enabled = false;
            this.txtTevkifat.Location = new System.Drawing.Point(413, 294);
            this.txtTevkifat.Name = "txtTevkifat";
            this.txtTevkifat.Size = new System.Drawing.Size(121, 20);
            this.txtTevkifat.TabIndex = 30;
            // 
            // chkTevkifat
            // 
            this.chkTevkifat.AutoSize = true;
            this.chkTevkifat.Location = new System.Drawing.Point(413, 277);
            this.chkTevkifat.Name = "chkTevkifat";
            this.chkTevkifat.Size = new System.Drawing.Size(94, 17);
            this.chkTevkifat.TabIndex = 29;
            this.chkTevkifat.Text = "Tevkifat(0.##)";
            this.chkTevkifat.UseVisualStyleBackColor = true;
            this.chkTevkifat.CheckedChanged += new System.EventHandler(this.chkTevkifat_CheckedChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(50, 317);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(38, 13);
            this.label13.TabIndex = 31;
            this.label13.Text = "Durum";
            // 
            // chkDurum
            // 
            this.chkDurum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chkDurum.FormattingEnabled = true;
            this.chkDurum.Items.AddRange(new object[] {
            "Taslak",
            "Devam",
            "İptal",
            "Bitti"});
            this.chkDurum.Location = new System.Drawing.Point(53, 333);
            this.chkDurum.Name = "chkDurum";
            this.chkDurum.Size = new System.Drawing.Size(121, 21);
            this.chkDurum.TabIndex = 32;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(432, 369);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 33;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(324, 369);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(102, 23);
            this.btnCancel.TabIndex = 34;
            this.btnCancel.Text = "İptal";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtDovizKuru
            // 
            this.txtDovizKuru.Location = new System.Drawing.Point(352, 175);
            this.txtDovizKuru.Name = "txtDovizKuru";
            this.txtDovizKuru.Size = new System.Drawing.Size(121, 20);
            this.txtDovizKuru.TabIndex = 16;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(177, 160);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(76, 13);
            this.label14.TabIndex = 11;
            this.label14.Text = "Ödeme Vadesi";
            // 
            // txtOdemeVadesi
            // 
            this.txtOdemeVadesi.Location = new System.Drawing.Point(180, 176);
            this.txtOdemeVadesi.Name = "txtOdemeVadesi";
            this.txtOdemeVadesi.Size = new System.Drawing.Size(100, 20);
            this.txtOdemeVadesi.TabIndex = 12;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(304, 278);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(58, 13);
            this.label15.TabIndex = 27;
            this.label15.Text = "KDV(0.##)";
            // 
            // txtKDV
            // 
            this.txtKDV.Location = new System.Drawing.Point(307, 294);
            this.txtKDV.Name = "txtKDV";
            this.txtKDV.Size = new System.Drawing.Size(100, 20);
            this.txtKDV.TabIndex = 28;
            // 
            // chkVade
            // 
            this.chkVade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chkVade.FormattingEnabled = true;
            this.chkVade.Items.AddRange(new object[] {
            "Peşin"});
            this.chkVade.Location = new System.Drawing.Point(53, 255);
            this.chkVade.Name = "chkVade";
            this.chkVade.Size = new System.Drawing.Size(121, 21);
            this.chkVade.TabIndex = 22;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeight = 50;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(50, 398);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 50;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(484, 241);
            this.dataGridView1.TabIndex = 35;
            this.dataGridView1.Visible = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(177, 200);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(33, 13);
            this.label16.TabIndex = 19;
            this.label16.Text = "İşçilik";
            // 
            // txtİscilik
            // 
            this.txtİscilik.Location = new System.Drawing.Point(180, 216);
            this.txtİscilik.Name = "txtİscilik";
            this.txtİscilik.Size = new System.Drawing.Size(100, 20);
            this.txtİscilik.TabIndex = 20;
            // 
            // offerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 689);
            this.Controls.Add(this.txtİscilik);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.chkVade);
            this.Controls.Add(this.txtKDV);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtOdemeVadesi);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtDovizKuru);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chkDurum);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.chkTevkifat);
            this.Controls.Add(this.txtTevkifat);
            this.Controls.Add(this.txtIskonto);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtLME);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chkDovizBirimi);
            this.Controls.Add(this.txtTeklifSuresi);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.chkOdemeSekli);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkTeslimSekli);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkYetkililer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkFirmalar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "offerEditor";
            this.Padding = new System.Windows.Forms.Padding(50);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Teklif Hazırlayıcı";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox chkFirmalar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox chkYetkililer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox chkTeslimSekli;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox chkOdemeSekli;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTeklifSuresi;
        private System.Windows.Forms.ComboBox chkDovizBirimi;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtLME;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtIskonto;
        private System.Windows.Forms.TextBox txtTevkifat;
        private System.Windows.Forms.CheckBox chkTevkifat;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox chkDurum;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtDovizKuru;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtOdemeVadesi;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtKDV;
        private System.Windows.Forms.ComboBox chkVade;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtİscilik;
    }
}