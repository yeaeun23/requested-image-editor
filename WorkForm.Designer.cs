namespace ImageWork
{
    partial class WorkForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkForm));
            this.imgPreView2 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.save_BTN = new System.Windows.Forms.Button();
            this.chulgo_BTN = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.color_RB2 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.color_RB1 = new System.Windows.Forms.RadioButton();
            this.retouch_TB = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.height_TB = new System.Windows.Forms.TextBox();
            this.width_TB = new System.Windows.Forms.TextBox();
            this.label_TB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.imgPreView1 = new System.Windows.Forms.PictureBox();
            this.fsw2 = new System.IO.FileSystemWatcher();
            this.fsw1 = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.imgPreView2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgPreView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fsw2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fsw1)).BeginInit();
            this.SuspendLayout();
            // 
            // imgPreView2
            // 
            this.imgPreView2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.imgPreView2.Location = new System.Drawing.Point(6, 20);
            this.imgPreView2.Name = "imgPreView2";
            this.imgPreView2.Size = new System.Drawing.Size(200, 200);
            this.imgPreView2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgPreView2.TabIndex = 0;
            this.imgPreView2.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.save_BTN);
            this.groupBox1.Controls.Add(this.chulgo_BTN);
            this.groupBox1.Controls.Add(this.imgPreView2);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.groupBox1.Location = new System.Drawing.Point(230, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(212, 360);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " 수정 ";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(69)))), ((int)(((byte)(69)))));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.button1.Location = new System.Drawing.Point(6, 331);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(200, 21);
            this.button1.TabIndex = 9;
            this.button1.Text = "닫 기";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.close_BTN_Click);
            // 
            // save_BTN
            // 
            this.save_BTN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.save_BTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(69)))), ((int)(((byte)(69)))));
            this.save_BTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.save_BTN.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.save_BTN.FlatAppearance.BorderSize = 0;
            this.save_BTN.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.save_BTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.save_BTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.save_BTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.save_BTN.Location = new System.Drawing.Point(6, 226);
            this.save_BTN.Name = "save_BTN";
            this.save_BTN.Size = new System.Drawing.Size(200, 21);
            this.save_BTN.TabIndex = 8;
            this.save_BTN.Text = "저 장";
            this.save_BTN.UseVisualStyleBackColor = false;
            this.save_BTN.Click += new System.EventHandler(this.save_BTN_Click);
            // 
            // chulgo_BTN
            // 
            this.chulgo_BTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(69)))), ((int)(((byte)(69)))));
            this.chulgo_BTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chulgo_BTN.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.chulgo_BTN.FlatAppearance.BorderSize = 0;
            this.chulgo_BTN.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.chulgo_BTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.chulgo_BTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chulgo_BTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.chulgo_BTN.Location = new System.Drawing.Point(6, 253);
            this.chulgo_BTN.Name = "chulgo_BTN";
            this.chulgo_BTN.Size = new System.Drawing.Size(200, 21);
            this.chulgo_BTN.TabIndex = 6;
            this.chulgo_BTN.Text = "출 고";
            this.chulgo_BTN.UseVisualStyleBackColor = false;
            this.chulgo_BTN.Click += new System.EventHandler(this.chulgo_BTN_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 389);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(453, 22);
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.toolStripStatusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(438, 17);
            this.toolStripStatusLabel.Spring = true;
            this.toolStripStatusLabel.Text = "(00:00:00) 작업중입니다.";
            this.toolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.color_RB2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.color_RB1);
            this.groupBox3.Controls.Add(this.retouch_TB);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.height_TB);
            this.groupBox3.Controls.Add(this.width_TB);
            this.groupBox3.Controls.Add(this.label_TB);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.imgPreView1);
            this.groupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(212, 360);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = " 전표 ";
            // 
            // color_RB2
            // 
            this.color_RB2.AutoSize = true;
            this.color_RB2.Enabled = false;
            this.color_RB2.Location = new System.Drawing.Point(117, 280);
            this.color_RB2.Name = "color_RB2";
            this.color_RB2.Size = new System.Drawing.Size(47, 16);
            this.color_RB2.TabIndex = 87;
            this.color_RB2.TabStop = true;
            this.color_RB2.Text = "흑백";
            this.color_RB2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 282);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 12);
            this.label1.TabIndex = 86;
            this.label1.Text = "색상:";
            // 
            // color_RB1
            // 
            this.color_RB1.AutoSize = true;
            this.color_RB1.Enabled = false;
            this.color_RB1.Location = new System.Drawing.Point(57, 280);
            this.color_RB1.Name = "color_RB1";
            this.color_RB1.Size = new System.Drawing.Size(47, 16);
            this.color_RB1.TabIndex = 85;
            this.color_RB1.TabStop = true;
            this.color_RB1.Text = "컬러";
            this.color_RB1.UseVisualStyleBackColor = true;
            // 
            // retouch_TB
            // 
            this.retouch_TB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.retouch_TB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.retouch_TB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.retouch_TB.Location = new System.Drawing.Point(6, 302);
            this.retouch_TB.Name = "retouch_TB";
            this.retouch_TB.ReadOnly = true;
            this.retouch_TB.Size = new System.Drawing.Size(200, 50);
            this.retouch_TB.TabIndex = 84;
            this.retouch_TB.Text = "-";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(115, 256);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 12);
            this.label4.TabIndex = 83;
            this.label4.Text = "세로:";
            // 
            // height_TB
            // 
            this.height_TB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.height_TB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.height_TB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.height_TB.Location = new System.Drawing.Point(154, 253);
            this.height_TB.Multiline = true;
            this.height_TB.Name = "height_TB";
            this.height_TB.ReadOnly = true;
            this.height_TB.Size = new System.Drawing.Size(52, 21);
            this.height_TB.TabIndex = 82;
            this.height_TB.Text = "-";
            // 
            // width_TB
            // 
            this.width_TB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.width_TB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.width_TB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.width_TB.Location = new System.Drawing.Point(57, 253);
            this.width_TB.Multiline = true;
            this.width_TB.Name = "width_TB";
            this.width_TB.ReadOnly = true;
            this.width_TB.Size = new System.Drawing.Size(52, 21);
            this.width_TB.TabIndex = 81;
            this.width_TB.Text = "-";
            // 
            // label_TB
            // 
            this.label_TB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.label_TB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.label_TB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.label_TB.Location = new System.Drawing.Point(57, 226);
            this.label_TB.Multiline = true;
            this.label_TB.Name = "label_TB";
            this.label_TB.ReadOnly = true;
            this.label_TB.Size = new System.Drawing.Size(149, 21);
            this.label_TB.TabIndex = 80;
            this.label_TB.Text = "-";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 256);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 12);
            this.label5.TabIndex = 79;
            this.label5.Text = "가로:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 230);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 12);
            this.label6.TabIndex = 78;
            this.label6.Text = "레이블:";
            // 
            // imgPreView1
            // 
            this.imgPreView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.imgPreView1.Location = new System.Drawing.Point(6, 20);
            this.imgPreView1.Name = "imgPreView1";
            this.imgPreView1.Size = new System.Drawing.Size(200, 200);
            this.imgPreView1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgPreView1.TabIndex = 0;
            this.imgPreView1.TabStop = false;
            // 
            // fsw2
            // 
            this.fsw2.EnableRaisingEvents = true;
            this.fsw2.Filter = "*.jpg";
            this.fsw2.NotifyFilter = ((System.IO.NotifyFilters)((((((((System.IO.NotifyFilters.FileName | System.IO.NotifyFilters.DirectoryName) 
            | System.IO.NotifyFilters.Attributes) 
            | System.IO.NotifyFilters.Size) 
            | System.IO.NotifyFilters.LastWrite) 
            | System.IO.NotifyFilters.LastAccess) 
            | System.IO.NotifyFilters.CreationTime) 
            | System.IO.NotifyFilters.Security)));
            this.fsw2.SynchronizingObject = this;
            this.fsw2.Renamed += new System.IO.RenamedEventHandler(this.fsw2_Renamed);
            // 
            // fsw1
            // 
            this.fsw1.EnableRaisingEvents = true;
            this.fsw1.SynchronizingObject = this;
            this.fsw1.Renamed += new System.IO.RenamedEventHandler(this.fsw1_Renamed);
            // 
            // WorkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.ClientSize = new System.Drawing.Size(453, 411);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "WorkForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorkForm_FormClosing);
            this.Load += new System.EventHandler(this.WorkForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imgPreView2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgPreView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fsw2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fsw1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox imgPreView2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button chulgo_BTN;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox height_TB;
        private System.Windows.Forms.TextBox width_TB;
        private System.Windows.Forms.TextBox label_TB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox imgPreView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton color_RB1;
        private System.Windows.Forms.RadioButton color_RB2;
        private System.Windows.Forms.RichTextBox retouch_TB;
        private System.Windows.Forms.Button save_BTN;
        private System.IO.FileSystemWatcher fsw2;
        private System.IO.FileSystemWatcher fsw1;
        private System.Windows.Forms.Button button1;
    }
}