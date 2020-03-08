namespace ScaleTo16x16
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.Hash_label = new System.Windows.Forms.Label();
            this.MakeStuffButton = new System.Windows.Forms.Button();
            this.LoadButton = new System.Windows.Forms.Button();
            this.LightLabel = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.ResultLabel = new System.Windows.Forms.Label();
            this.FolderLabel = new System.Windows.Forms.Label();
            this.DarkLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.DoubleLabel = new System.Windows.Forms.Label();
            this.DeleteLeftButton = new System.Windows.Forms.Button();
            this.DeleteRightButton = new System.Windows.Forms.Button();
            this.DeleteBothButton = new System.Windows.Forms.Button();
            this.FalsePositiveButton = new System.Windows.Forms.Button();
            this.RetrieveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // Hash_label
            // 
            this.Hash_label.AutoSize = true;
            this.Hash_label.Location = new System.Drawing.Point(449, 601);
            this.Hash_label.Name = "Hash_label";
            this.Hash_label.Size = new System.Drawing.Size(62, 13);
            this.Hash_label.TabIndex = 5;
            this.Hash_label.Text = "FingerLabel";
            // 
            // MakeStuffButton
            // 
            this.MakeStuffButton.Location = new System.Drawing.Point(108, 550);
            this.MakeStuffButton.Name = "MakeStuffButton";
            this.MakeStuffButton.Size = new System.Drawing.Size(75, 23);
            this.MakeStuffButton.TabIndex = 12;
            this.MakeStuffButton.Text = "Make Stuff";
            this.MakeStuffButton.UseVisualStyleBackColor = true;
            this.MakeStuffButton.Click += new System.EventHandler(this.MakeStuffButton_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(14, 550);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(75, 23);
            this.LoadButton.TabIndex = 13;
            this.LoadButton.Text = "Load Image";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // LightLabel
            // 
            this.LightLabel.AutoSize = true;
            this.LightLabel.Location = new System.Drawing.Point(517, 601);
            this.LightLabel.Name = "LightLabel";
            this.LightLabel.Size = new System.Drawing.Size(98, 13);
            this.LightLabel.TabIndex = 16;
            this.LightLabel.Text = "LightCompareLabel";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(755, 28);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(419, 498);
            this.listView1.TabIndex = 17;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "FirstImage";
            this.columnHeader1.Width = 199;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "SecondImage";
            this.columnHeader2.Width = 201;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 579);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(277, 23);
            this.progressBar1.TabIndex = 18;
            this.progressBar1.Visible = false;
            // 
            // ResultLabel
            // 
            this.ResultLabel.AutoSize = true;
            this.ResultLabel.Location = new System.Drawing.Point(725, 601);
            this.ResultLabel.Name = "ResultLabel";
            this.ResultLabel.Size = new System.Drawing.Size(63, 13);
            this.ResultLabel.TabIndex = 19;
            this.ResultLabel.Text = "ResultLabel";
            // 
            // FolderLabel
            // 
            this.FolderLabel.AutoSize = true;
            this.FolderLabel.Location = new System.Drawing.Point(381, 601);
            this.FolderLabel.Name = "FolderLabel";
            this.FolderLabel.Size = new System.Drawing.Size(62, 13);
            this.FolderLabel.TabIndex = 20;
            this.FolderLabel.Text = "FolderLabel";
            // 
            // DarkLabel
            // 
            this.DarkLabel.AutoSize = true;
            this.DarkLabel.Location = new System.Drawing.Point(621, 601);
            this.DarkLabel.Name = "DarkLabel";
            this.DarkLabel.Size = new System.Drawing.Size(98, 13);
            this.DarkLabel.TabIndex = 21;
            this.DarkLabel.Text = "DarkCompareLabel";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 530);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Status";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(15, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(332, 478);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 23;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(409, 28);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(298, 478);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 24;
            this.pictureBox2.TabStop = false;
            // 
            // DoubleLabel
            // 
            this.DoubleLabel.AutoSize = true;
            this.DoubleLabel.Location = new System.Drawing.Point(794, 601);
            this.DoubleLabel.Name = "DoubleLabel";
            this.DoubleLabel.Size = new System.Drawing.Size(13, 13);
            this.DoubleLabel.TabIndex = 25;
            this.DoubleLabel.Text = "0";
            // 
            // DeleteLeftButton
            // 
            this.DeleteLeftButton.Enabled = false;
            this.DeleteLeftButton.Location = new System.Drawing.Point(852, 550);
            this.DeleteLeftButton.Name = "DeleteLeftButton";
            this.DeleteLeftButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteLeftButton.TabIndex = 26;
            this.DeleteLeftButton.Text = "DeleteLeft";
            this.DeleteLeftButton.UseVisualStyleBackColor = true;
            this.DeleteLeftButton.Click += new System.EventHandler(this.DeleteLeftButton_Click);
            // 
            // DeleteRightButton
            // 
            this.DeleteRightButton.Enabled = false;
            this.DeleteRightButton.Location = new System.Drawing.Point(933, 550);
            this.DeleteRightButton.Name = "DeleteRightButton";
            this.DeleteRightButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteRightButton.TabIndex = 27;
            this.DeleteRightButton.Text = "DeleteRight";
            this.DeleteRightButton.UseVisualStyleBackColor = true;
            this.DeleteRightButton.Click += new System.EventHandler(this.DeleteRightButton_Click);
            // 
            // DeleteBothButton
            // 
            this.DeleteBothButton.Enabled = false;
            this.DeleteBothButton.Location = new System.Drawing.Point(1014, 550);
            this.DeleteBothButton.Name = "DeleteBothButton";
            this.DeleteBothButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteBothButton.TabIndex = 28;
            this.DeleteBothButton.Text = "DeleteBoth";
            this.DeleteBothButton.UseVisualStyleBackColor = true;
            this.DeleteBothButton.Click += new System.EventHandler(this.DeleteBothButton_Click);
            // 
            // FalsePositiveButton
            // 
            this.FalsePositiveButton.Enabled = false;
            this.FalsePositiveButton.Location = new System.Drawing.Point(1095, 550);
            this.FalsePositiveButton.Name = "FalsePositiveButton";
            this.FalsePositiveButton.Size = new System.Drawing.Size(79, 23);
            this.FalsePositiveButton.TabIndex = 29;
            this.FalsePositiveButton.Text = "FalsePositive";
            this.FalsePositiveButton.UseVisualStyleBackColor = true;
            this.FalsePositiveButton.Click += new System.EventHandler(this.FalsePositiveButton_Click);
            // 
            // RetrieveButton
            // 
            this.RetrieveButton.Enabled = false;
            this.RetrieveButton.Location = new System.Drawing.Point(755, 550);
            this.RetrieveButton.Name = "RetrieveButton";
            this.RetrieveButton.Size = new System.Drawing.Size(91, 23);
            this.RetrieveButton.TabIndex = 30;
            this.RetrieveButton.Text = "RetrieveButton";
            this.RetrieveButton.UseVisualStyleBackColor = true;
            this.RetrieveButton.Click += new System.EventHandler(this.RetrieveButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 623);
            this.Controls.Add(this.RetrieveButton);
            this.Controls.Add(this.FalsePositiveButton);
            this.Controls.Add(this.DeleteBothButton);
            this.Controls.Add(this.DeleteRightButton);
            this.Controls.Add(this.DeleteLeftButton);
            this.Controls.Add(this.DoubleLabel);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.DarkLabel);
            this.Controls.Add(this.FolderLabel);
            this.Controls.Add(this.ResultLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.LightLabel);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.MakeStuffButton);
            this.Controls.Add(this.Hash_label);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label Hash_label;
        private System.Windows.Forms.Button MakeStuffButton;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.Label LightLabel;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label ResultLabel;
        private System.Windows.Forms.Label FolderLabel;
        private System.Windows.Forms.Label DarkLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label DoubleLabel;
        private System.Windows.Forms.Button DeleteLeftButton;
        private System.Windows.Forms.Button DeleteRightButton;
        private System.Windows.Forms.Button DeleteBothButton;
        private System.Windows.Forms.Button FalsePositiveButton;
        private System.Windows.Forms.Button RetrieveButton;
    }
}

