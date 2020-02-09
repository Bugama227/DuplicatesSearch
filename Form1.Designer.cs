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
            this.Orig_pb = new System.Windows.Forms.PictureBox();
            this.DownScaled_pb = new System.Windows.Forms.PictureBox();
            this.ReducedColours_pb = new System.Windows.Forms.PictureBox();
            this.Hash_label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.Orig_pb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DownScaled_pb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReducedColours_pb)).BeginInit();
            this.SuspendLayout();
            // 
            // Orig_pb
            // 
            this.Orig_pb.Location = new System.Drawing.Point(12, 28);
            this.Orig_pb.Name = "Orig_pb";
            this.Orig_pb.Size = new System.Drawing.Size(321, 499);
            this.Orig_pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Orig_pb.TabIndex = 0;
            this.Orig_pb.TabStop = false;
            // 
            // DownScaled_pb
            // 
            this.DownScaled_pb.Location = new System.Drawing.Point(339, 28);
            this.DownScaled_pb.Name = "DownScaled_pb";
            this.DownScaled_pb.Size = new System.Drawing.Size(251, 234);
            this.DownScaled_pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.DownScaled_pb.TabIndex = 1;
            this.DownScaled_pb.TabStop = false;
            // 
            // ReducedColours_pb
            // 
            this.ReducedColours_pb.Location = new System.Drawing.Point(339, 281);
            this.ReducedColours_pb.Name = "ReducedColours_pb";
            this.ReducedColours_pb.Size = new System.Drawing.Size(251, 246);
            this.ReducedColours_pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ReducedColours_pb.TabIndex = 3;
            this.ReducedColours_pb.TabStop = false;
            // 
            // Hash_label
            // 
            this.Hash_label.AutoSize = true;
            this.Hash_label.Location = new System.Drawing.Point(497, 585);
            this.Hash_label.Name = "Hash_label";
            this.Hash_label.Size = new System.Drawing.Size(35, 13);
            this.Hash_label.TabIndex = 5;
            this.Hash_label.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Original";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(339, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "DownScaled";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(339, 265);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Final";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(607, 550);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Hash";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(113, 550);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Make Stuff";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(14, 550);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Load Image";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(194, 550);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(645, 20);
            this.textBox1.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(298, 585);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "label1";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(599, 28);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(239, 498);
            this.listView1.TabIndex = 17;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "FirstImage";
            this.columnHeader1.Width = 125;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "SecondImage";
            this.columnHeader2.Width = 112;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 607);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Hash_label);
            this.Controls.Add(this.ReducedColours_pb);
            this.Controls.Add(this.DownScaled_pb);
            this.Controls.Add(this.Orig_pb);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.Orig_pb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DownScaled_pb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReducedColours_pb)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Orig_pb;
        private System.Windows.Forms.PictureBox DownScaled_pb;
        private System.Windows.Forms.PictureBox ReducedColours_pb;
        private System.Windows.Forms.Label Hash_label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}

