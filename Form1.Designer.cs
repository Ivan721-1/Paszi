namespace RoleAuthenticationApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtRoleName;
        private System.Windows.Forms.Button btnReadKey;
        private System.Windows.Forms.Button btnTakePhoto;
        private System.Windows.Forms.Button btnAddToDb;
        private System.Windows.Forms.Button btnAlreadyHaveRole;
        private System.Windows.Forms.Button btnShowAllDevices;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtRoleName = new System.Windows.Forms.TextBox();
            this.btnReadKey = new System.Windows.Forms.Button();
            this.btnTakePhoto = new System.Windows.Forms.Button();
            this.btnAddToDb = new System.Windows.Forms.Button();
            this.btnAlreadyHaveRole = new System.Windows.Forms.Button();
            this.btnShowAllDevices = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtRoleName
            // 
            this.txtRoleName.Location = new System.Drawing.Point(12, 12);
            this.txtRoleName.Name = "txtRoleName";
            this.txtRoleName.Size = new System.Drawing.Size(200, 20);
            this.txtRoleName.TabIndex = 0;
            // 
            // btnReadKey
            // 
            this.btnReadKey.Location = new System.Drawing.Point(12, 40);
            this.btnReadKey.Name = "btnReadKey";
            this.btnReadKey.Size = new System.Drawing.Size(100, 23);
            this.btnReadKey.TabIndex = 1;
            this.btnReadKey.Text = "Считать ключ";
            this.btnReadKey.UseVisualStyleBackColor = true;
            // 
            // btnTakePhoto
            //
            this.btnTakePhoto.Location = new System.Drawing.Point(244, 40);
            this.btnTakePhoto.Name = "btnTakePhoto";
            this.btnTakePhoto.Size = new System.Drawing.Size(100, 23);
            this.btnTakePhoto.TabIndex = 2;
            this.btnTakePhoto.Text = "Сделать снимок";
            this.btnTakePhoto.UseVisualStyleBackColor = true;
            // 
            // btnAddToDb
            //
            this.btnAddToDb.Location = new System.Drawing.Point(350, 40);
            this.btnAddToDb.Name = "btnAddToDb";
            this.btnAddToDb.Size = new System.Drawing.Size(150, 23);
            this.btnAddToDb.TabIndex = 3;
            this.btnAddToDb.Text = "Добавить в базу данных";
            this.btnAddToDb.UseVisualStyleBackColor = true;
            // 
            // btnAlreadyHaveRole
            //
            this.btnAlreadyHaveRole.Location = new System.Drawing.Point(12, 70);
            this.btnAlreadyHaveRole.Name = "btnAlreadyHaveRole";
            this.btnAlreadyHaveRole.Size = new System.Drawing.Size(150, 23);
            this.btnAlreadyHaveRole.TabIndex = 4;
            this.btnAlreadyHaveRole.Text = "У меня уже есть роль";
            this.btnAlreadyHaveRole.UseVisualStyleBackColor = true;
            //
            // btnShowAllDevices
            //
            this.btnShowAllDevices.Location = new System.Drawing.Point(118, 40);
            this.btnShowAllDevices.Name = "btnShowAllDevices";
            this.btnShowAllDevices.Size = new System.Drawing.Size(120, 23);
            this.btnShowAllDevices.TabIndex = 5;
            this.btnShowAllDevices.Text = "Показать устройства";
            this.btnShowAllDevices.UseVisualStyleBackColor = true;
            //
            // Form1
            //
            this.ClientSize = new System.Drawing.Size(520, 105);
            this.Controls.Add(this.btnShowAllDevices);
            this.Controls.Add(this.btnAlreadyHaveRole);
            this.Controls.Add(this.btnAddToDb);
            this.Controls.Add(this.btnTakePhoto);
            this.Controls.Add(this.btnReadKey);
            this.Controls.Add(this.txtRoleName);
            this.Name = "Form1";
            this.Text = "Добавление роли";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
