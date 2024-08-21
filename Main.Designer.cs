namespace AlbionFlipperServer
{
    partial class Main
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.LoggerTXT = new System.Windows.Forms.RichTextBox();
            this.CheckProfitsBtn = new System.Windows.Forms.Button();
            this.profitData = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alertprofit = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.minprofit = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.caerleon_Numeric = new System.Windows.Forms.NumericUpDown();
            this.bm_Numeric = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ClearBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.profitData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.alertprofit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minprofit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.caerleon_Numeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bm_Numeric)).BeginInit();
            this.SuspendLayout();
            // 
            // LoggerTXT
            // 
            this.LoggerTXT.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.LoggerTXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LoggerTXT.Location = new System.Drawing.Point(708, 12);
            this.LoggerTXT.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LoggerTXT.Name = "LoggerTXT";
            this.LoggerTXT.ReadOnly = true;
            this.LoggerTXT.Size = new System.Drawing.Size(532, 53);
            this.LoggerTXT.TabIndex = 2;
            this.LoggerTXT.Text = "";
            // 
            // CheckProfitsBtn
            // 
            this.CheckProfitsBtn.BackColor = System.Drawing.Color.CornflowerBlue;
            this.CheckProfitsBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.CheckProfitsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheckProfitsBtn.Location = new System.Drawing.Point(472, 12);
            this.CheckProfitsBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CheckProfitsBtn.Name = "CheckProfitsBtn";
            this.CheckProfitsBtn.Size = new System.Drawing.Size(104, 51);
            this.CheckProfitsBtn.TabIndex = 14;
            this.CheckProfitsBtn.Text = "Check Profits";
            this.CheckProfitsBtn.UseVisualStyleBackColor = false;
            this.CheckProfitsBtn.Click += new System.EventHandler(this.CheckProfitsBtn_Click);
            // 
            // profitData
            // 
            this.profitData.BackgroundColor = System.Drawing.Color.Teal;
            this.profitData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.profitData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.profitData.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.profitData.GridColor = System.Drawing.Color.DarkGray;
            this.profitData.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.profitData.Location = new System.Drawing.Point(0, 73);
            this.profitData.Name = "profitData";
            this.profitData.Size = new System.Drawing.Size(1253, 549);
            this.profitData.TabIndex = 15;
            // 
            // Column1
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column1.HeaderText = "Item";
            this.Column1.Name = "Column1";
            this.Column1.Width = 350;
            // 
            // Column2
            // 
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.Format = "N0";
            dataGridViewCellStyle2.NullValue = null;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column2.HeaderText = "Profit";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column3.HeaderText = "Caerleon";
            this.Column3.Name = "Column3";
            this.Column3.Width = 300;
            // 
            // Column4
            // 
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            this.Column4.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column4.HeaderText = "Black Market";
            this.Column4.Name = "Column4";
            this.Column4.Width = 300;
            // 
            // alertprofit
            // 
            this.alertprofit.BackColor = System.Drawing.Color.CornflowerBlue;
            this.alertprofit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.alertprofit.ForeColor = System.Drawing.Color.AliceBlue;
            this.alertprofit.Location = new System.Drawing.Point(361, 42);
            this.alertprofit.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.alertprofit.Name = "alertprofit";
            this.alertprofit.Size = new System.Drawing.Size(104, 23);
            this.alertprofit.TabIndex = 23;
            this.alertprofit.Value = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(274, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 20);
            this.label4.TabIndex = 22;
            this.label4.Text = "Alert Profit";
            // 
            // minprofit
            // 
            this.minprofit.BackColor = System.Drawing.Color.CornflowerBlue;
            this.minprofit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.minprofit.ForeColor = System.Drawing.Color.AliceBlue;
            this.minprofit.Location = new System.Drawing.Point(361, 12);
            this.minprofit.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.minprofit.Name = "minprofit";
            this.minprofit.Size = new System.Drawing.Size(104, 23);
            this.minprofit.TabIndex = 21;
            this.minprofit.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(243, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 20);
            this.label3.TabIndex = 20;
            this.label3.Text = "Minimum Profit";
            // 
            // caerleon_Numeric
            // 
            this.caerleon_Numeric.BackColor = System.Drawing.Color.CornflowerBlue;
            this.caerleon_Numeric.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.caerleon_Numeric.ForeColor = System.Drawing.Color.AliceBlue;
            this.caerleon_Numeric.Location = new System.Drawing.Point(123, 12);
            this.caerleon_Numeric.Name = "caerleon_Numeric";
            this.caerleon_Numeric.Size = new System.Drawing.Size(104, 23);
            this.caerleon_Numeric.TabIndex = 19;
            this.caerleon_Numeric.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // bm_Numeric
            // 
            this.bm_Numeric.BackColor = System.Drawing.Color.CornflowerBlue;
            this.bm_Numeric.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.bm_Numeric.ForeColor = System.Drawing.Color.AliceBlue;
            this.bm_Numeric.Location = new System.Drawing.Point(123, 42);
            this.bm_Numeric.Name = "bm_Numeric";
            this.bm_Numeric.Size = new System.Drawing.Size(104, 23);
            this.bm_Numeric.TabIndex = 18;
            this.bm_Numeric.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 20);
            this.label2.TabIndex = 17;
            this.label2.Text = "Caerleon Time";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(49, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 20);
            this.label1.TabIndex = 16;
            this.label1.Text = "BM Time";
            // 
            // ClearBtn
            // 
            this.ClearBtn.BackColor = System.Drawing.Color.CornflowerBlue;
            this.ClearBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClearBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClearBtn.Location = new System.Drawing.Point(584, 13);
            this.ClearBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(104, 51);
            this.ClearBtn.TabIndex = 24;
            this.ClearBtn.Text = "Clear All";
            this.ClearBtn.UseVisualStyleBackColor = false;
            this.ClearBtn.Click += new System.EventHandler(this.ClearBtn_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(1253, 622);
            this.Controls.Add(this.ClearBtn);
            this.Controls.Add(this.alertprofit);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.minprofit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.caerleon_Numeric);
            this.Controls.Add(this.bm_Numeric);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.profitData);
            this.Controls.Add(this.CheckProfitsBtn);
            this.Controls.Add(this.LoggerTXT);
            this.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Main";
            this.Text = "AlbionFlipperServer";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.profitData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.alertprofit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minprofit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.caerleon_Numeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bm_Numeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox LoggerTXT;
        private System.Windows.Forms.Button CheckProfitsBtn;
        private System.Windows.Forms.DataGridView profitData;
        private System.Windows.Forms.NumericUpDown alertprofit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown minprofit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown caerleon_Numeric;
        private System.Windows.Forms.NumericUpDown bm_Numeric;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.Button ClearBtn;
    }
}

