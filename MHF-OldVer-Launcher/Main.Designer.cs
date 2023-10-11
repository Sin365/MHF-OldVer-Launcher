namespace MHF_OldVer_Launcher
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.btn_start = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_s3 = new System.Windows.Forms.Label();
            this.label_s2 = new System.Windows.Forms.Label();
            this.label_s1 = new System.Windows.Forms.Label();
            this.label_State = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(241, 10);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(142, 23);
            this.btn_start.TabIndex = 0;
            this.btn_start.Text = "START GAME";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(287, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "by axibug.com";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "ServerList Config:";
            // 
            // label_s3
            // 
            this.label_s3.AutoSize = true;
            this.label_s3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_s3.Location = new System.Drawing.Point(13, 70);
            this.label_s3.Name = "label_s3";
            this.label_s3.Size = new System.Drawing.Size(12, 12);
            this.label_s3.TabIndex = 8;
            this.label_s3.Text = "-";
            // 
            // label_s2
            // 
            this.label_s2.AutoSize = true;
            this.label_s2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_s2.Location = new System.Drawing.Point(13, 53);
            this.label_s2.Name = "label_s2";
            this.label_s2.Size = new System.Drawing.Size(12, 12);
            this.label_s2.TabIndex = 7;
            this.label_s2.Text = "-";
            // 
            // label_s1
            // 
            this.label_s1.AutoSize = true;
            this.label_s1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_s1.Location = new System.Drawing.Point(13, 36);
            this.label_s1.Name = "label_s1";
            this.label_s1.Size = new System.Drawing.Size(12, 12);
            this.label_s1.TabIndex = 6;
            this.label_s1.Text = "-";
            // 
            // label_State
            // 
            this.label_State.AutoSize = true;
            this.label_State.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_State.ForeColor = System.Drawing.Color.Green;
            this.label_State.Location = new System.Drawing.Point(13, 85);
            this.label_State.Name = "label_State";
            this.label_State.Size = new System.Drawing.Size(47, 12);
            this.label_State.TabIndex = 9;
            this.label_State.Text = "State:";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 119);
            this.Controls.Add(this.label_State);
            this.Controls.Add(this.label_s3);
            this.Controls.Add(this.label_s2);
            this.Controls.Add(this.label_s1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_start);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MHF-OldVer-Launcher Ver.1.0";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_s3;
        private System.Windows.Forms.Label label_s2;
        private System.Windows.Forms.Label label_s1;
        private System.Windows.Forms.Label label_State;
    }
}

