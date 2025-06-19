namespace RoleAuthenticationApp
{
    partial class Form2
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblInsertKey;
        private System.Windows.Forms.Button btnKeyInserted;
        private System.Windows.Forms.Label lblKeyStatus;
        private System.Windows.Forms.Button btnCheckIdentity;
        private System.Windows.Forms.Button btnBack;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblInsertKey = new System.Windows.Forms.Label();
            this.btnKeyInserted = new System.Windows.Forms.Button();
            this.lblKeyStatus = new System.Windows.Forms.Label();
            this.btnCheckIdentity = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblInsertKey
            // 
            this.lblInsertKey.AutoSize = true;
            this.lblInsertKey.Location = new System.Drawing.Point(12, 9);
            this.lblInsertKey.Name = "lblInsertKey";
            this.lblInsertKey.Size = new System.Drawing.Size(120, 13);
            this.lblInsertKey.TabIndex = 0;
            this.lblInsertKey.Text = "Вставьте USB-ключ!";
            // 
            // btnKeyInserted
            //
            this.btnKeyInserted.Location = new System.Drawing.Point(12, 28);
            this.btnKeyInserted.Name = "btnKeyInserted";
            this.btnKeyInserted.Size = new System.Drawing.Size(100, 23);
            this.btnKeyInserted.TabIndex = 1;
            this.btnKeyInserted.Text = "Ключ вставлен";
            this.btnKeyInserted.UseVisualStyleBackColor = true;
            //
            // lblKeyStatus
            //
            this.lblKeyStatus.AutoSize = true;
            this.lblKeyStatus.Location = new System.Drawing.Point(12, 60);
            this.lblKeyStatus.Name = "lblKeyStatus";
            this.lblKeyStatus.Size = new System.Drawing.Size(0, 13);
            this.lblKeyStatus.TabIndex = 2;
            this.lblKeyStatus.Visible = false;
            //
            // btnCheckIdentity
            //
            this.btnCheckIdentity.Location = new System.Drawing.Point(12, 80);
            this.btnCheckIdentity.Name = "btnCheckIdentity";
            this.btnCheckIdentity.Size = new System.Drawing.Size(120, 23);
            this.btnCheckIdentity.TabIndex = 3;
            this.btnCheckIdentity.Text = "Проверить личность";
            this.btnCheckIdentity.UseVisualStyleBackColor = true;
            this.btnCheckIdentity.Visible = false;
            //
            // btnBack
            //
            this.btnBack.Location = new System.Drawing.Point(250, 80);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 4;
            this.btnBack.Text = "Назад";
            this.btnBack.UseVisualStyleBackColor = true;
            //
            // Form2
            //
            this.ClientSize = new System.Drawing.Size(350, 120);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnCheckIdentity);
            this.Controls.Add(this.lblKeyStatus);
            this.Controls.Add(this.btnKeyInserted);
            this.Controls.Add(this.lblInsertKey);
            this.Name = "Form2";
            this.Text = "Вставьте USB-ключ!";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
