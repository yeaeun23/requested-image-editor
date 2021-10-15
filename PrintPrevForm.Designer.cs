namespace ImageWork
{
    partial class PrintPrevForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintPrevForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.refresh_BTN = new System.Windows.Forms.Button();
            this.browser_BTN = new System.Windows.Forms.Button();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.close_BTN = new System.Windows.Forms.Button();
            this.print_BTN = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.zoom_BTN = new System.Windows.Forms.ToolStripDropDownButton();
            this.zoom_4 = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom_3 = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom_2 = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom_1 = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.refresh_BTN);
            this.groupBox1.Controls.Add(this.browser_BTN);
            this.groupBox1.Controls.Add(this.webBrowser);
            this.groupBox1.Controls.Add(this.close_BTN);
            this.groupBox1.Controls.Add(this.print_BTN);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(570, 715);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // refresh_BTN
            // 
            this.refresh_BTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(69)))), ((int)(((byte)(69)))));
            this.refresh_BTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.refresh_BTN.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.refresh_BTN.FlatAppearance.BorderSize = 0;
            this.refresh_BTN.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.refresh_BTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.refresh_BTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refresh_BTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.refresh_BTN.Location = new System.Drawing.Point(132, 683);
            this.refresh_BTN.Name = "refresh_BTN";
            this.refresh_BTN.Size = new System.Drawing.Size(120, 21);
            this.refresh_BTN.TabIndex = 7;
            this.refresh_BTN.Text = "새로고침(F5)";
            this.refresh_BTN.UseVisualStyleBackColor = false;
            this.refresh_BTN.Click += new System.EventHandler(this.refresh_BTN_Click);
            // 
            // browser_BTN
            // 
            this.browser_BTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(69)))), ((int)(((byte)(69)))));
            this.browser_BTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.browser_BTN.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.browser_BTN.FlatAppearance.BorderSize = 0;
            this.browser_BTN.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.browser_BTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.browser_BTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browser_BTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.browser_BTN.Location = new System.Drawing.Point(6, 683);
            this.browser_BTN.Name = "browser_BTN";
            this.browser_BTN.Size = new System.Drawing.Size(120, 21);
            this.browser_BTN.TabIndex = 6;
            this.browser_BTN.Text = "브라우저 열기";
            this.browser_BTN.UseVisualStyleBackColor = false;
            this.browser_BTN.Click += new System.EventHandler(this.browser_BTN_Click);
            // 
            // webBrowser
            // 
            this.webBrowser.Location = new System.Drawing.Point(6, 20);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(557, 657);
            this.webBrowser.TabIndex = 5;
            this.webBrowser.WebBrowserShortcutsEnabled = false;
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            // 
            // close_BTN
            // 
            this.close_BTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(69)))), ((int)(((byte)(69)))));
            this.close_BTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.close_BTN.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.close_BTN.FlatAppearance.BorderSize = 0;
            this.close_BTN.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.close_BTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.close_BTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.close_BTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.close_BTN.Location = new System.Drawing.Point(463, 683);
            this.close_BTN.Name = "close_BTN";
            this.close_BTN.Size = new System.Drawing.Size(100, 21);
            this.close_BTN.TabIndex = 4;
            this.close_BTN.Text = "닫기";
            this.close_BTN.UseVisualStyleBackColor = false;
            this.close_BTN.Click += new System.EventHandler(this.close_BTN_Click);
            // 
            // print_BTN
            // 
            this.print_BTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(69)))), ((int)(((byte)(69)))));
            this.print_BTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.print_BTN.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.print_BTN.FlatAppearance.BorderSize = 0;
            this.print_BTN.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.print_BTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.print_BTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.print_BTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.print_BTN.Location = new System.Drawing.Point(258, 683);
            this.print_BTN.Name = "print_BTN";
            this.print_BTN.Size = new System.Drawing.Size(199, 21);
            this.print_BTN.TabIndex = 3;
            this.print_BTN.Text = "인쇄하기";
            this.print_BTN.UseVisualStyleBackColor = false;
            this.print_BTN.Click += new System.EventHandler(this.print_BTN_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.zoom_BTN});
            this.statusStrip.Location = new System.Drawing.Point(0, 739);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(594, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.toolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(519, 17);
            this.toolStripStatusLabel.Spring = true;
            this.toolStripStatusLabel.Text = "(00:00:00) 로딩중입니다.";
            this.toolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // zoom_BTN
            // 
            this.zoom_BTN.AutoSize = false;
            this.zoom_BTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.zoom_BTN.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.zoom_BTN.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoom_4,
            this.zoom_3,
            this.zoom_2,
            this.zoom_1});
            this.zoom_BTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.zoom_BTN.Image = ((System.Drawing.Image)(resources.GetObject("zoom_BTN.Image")));
            this.zoom_BTN.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoom_BTN.Name = "zoom_BTN";
            this.zoom_BTN.Size = new System.Drawing.Size(60, 20);
            this.zoom_BTN.Text = "75%";
            this.zoom_BTN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // zoom_4
            // 
            this.zoom_4.Name = "zoom_4";
            this.zoom_4.Size = new System.Drawing.Size(105, 22);
            this.zoom_4.Text = "100%";
            this.zoom_4.Click += new System.EventHandler(this.zoom_4_Click);
            // 
            // zoom_3
            // 
            this.zoom_3.Checked = true;
            this.zoom_3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.zoom_3.Name = "zoom_3";
            this.zoom_3.Size = new System.Drawing.Size(105, 22);
            this.zoom_3.Text = "75%";
            this.zoom_3.Click += new System.EventHandler(this.zoom_3_Click);
            // 
            // zoom_2
            // 
            this.zoom_2.Name = "zoom_2";
            this.zoom_2.Size = new System.Drawing.Size(105, 22);
            this.zoom_2.Text = "50%";
            this.zoom_2.Click += new System.EventHandler(this.zoom_2_Click);
            // 
            // zoom_1
            // 
            this.zoom_1.Name = "zoom_1";
            this.zoom_1.Size = new System.Drawing.Size(105, 22);
            this.zoom_1.Text = "25%";
            this.zoom_1.Click += new System.EventHandler(this.zoom_1_Click);
            // 
            // PrintPrevForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.ClientSize = new System.Drawing.Size(594, 761);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "PrintPrevForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "전표 보기";
            this.Load += new System.EventHandler(this.PrintPrevForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button print_BTN;
        private System.Windows.Forms.Button close_BTN;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Button browser_BTN;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripDropDownButton zoom_BTN;
        private System.Windows.Forms.ToolStripMenuItem zoom_4;
        private System.Windows.Forms.ToolStripMenuItem zoom_3;
        private System.Windows.Forms.ToolStripMenuItem zoom_2;
        private System.Windows.Forms.ToolStripMenuItem zoom_1;
        private System.Windows.Forms.Button refresh_BTN;
    }
}