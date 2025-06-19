namespace RoleAuthenticationApp
{
    partial class Form3
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblCameraPrompt;
        private System.Windows.Forms.Button btnSnapshot;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblCameraPrompt = new System.Windows.Forms.Label();
            this.btnSnapshot = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblCameraPrompt
            // 
            this.lblCameraPrompt.AutoSize = true;
            this.lblCameraPrompt.Location = new System.Drawing.Point(12, 9);
            this.lblCameraPrompt.Name = "lblCameraPrompt";
            this.lblCameraPrompt.Size = new System.Drawing.Size(200, 13);
            this.lblCameraPrompt.TabIndex = 0;
            this.lblCameraPrompt.Text = "Держите свое лицо пред камерой";
            // 
            // btnSnapshot
            // 
            this.btnSnapshot.Location = new System.Drawing.Point(12, 28);
            this.btnSnapshot.Name = "btnSnapshot";
            this.btnSnapshot.Size = new System.Drawing.Size(100, 23);
            this.btnSnapshot.TabIndex = 1;
            this.btnSnapshot.Text = "Проверить лицо";
            this.btnSnapshot.UseVisualStyleBackColor = true;
            // 
            // Form3
            // 
            this.ClientSize = new System.Drawing.Size(284, 61);
            this.Controls.Add(this.btnSnapshot);
            this.Controls.Add(this.lblCameraPrompt);
            this.Name = "Form3";
            this.Text = "Держите свое лицо пред камерой";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
