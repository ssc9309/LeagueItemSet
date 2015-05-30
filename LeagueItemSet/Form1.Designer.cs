namespace LeagueItemSet
{
    partial class Form1
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
            this.rtbx_ProgressBox = new System.Windows.Forms.RichTextBox();
            this.btn_Update = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtbx_ProgressBox
            // 
            this.rtbx_ProgressBox.Location = new System.Drawing.Point(12, 122);
            this.rtbx_ProgressBox.Name = "rtbx_ProgressBox";
            this.rtbx_ProgressBox.Size = new System.Drawing.Size(477, 229);
            this.rtbx_ProgressBox.TabIndex = 0;
            this.rtbx_ProgressBox.Text = "";
            // 
            // btn_Update
            // 
            this.btn_Update.Location = new System.Drawing.Point(12, 12);
            this.btn_Update.Name = "btn_Update";
            this.btn_Update.Size = new System.Drawing.Size(238, 77);
            this.btn_Update.TabIndex = 1;
            this.btn_Update.Text = "Update Sets";
            this.btn_Update.UseVisualStyleBackColor = true;
            this.btn_Update.Click += new System.EventHandler(this.btn_Update_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 363);
            this.Controls.Add(this.btn_Update);
            this.Controls.Add(this.rtbx_ProgressBox);
            this.Name = "Form1";
            this.Text = "LoL Flavor of the Month Item Sets";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbx_ProgressBox;
        private System.Windows.Forms.Button btn_Update;
    }
}

