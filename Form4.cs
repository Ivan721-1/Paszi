using System;
using System.Windows.Forms;

namespace RoleAuthenticationApp
{
    public partial class Form4 : Form
    {
        public Form4(bool isAuthenticated, string roleName)
        {
            InitializeComponent();
            this.Load -= Form4_Load; // если в Designer подвязан
            this.FormClosed += new FormClosedEventHandler(Form4_FormClosed);
            lblWelcome.Text = isAuthenticated
                ? "Добро пожаловать, " + roleName + "!"
                : "Аутентификация не пройдена!";
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Закрываем все приложение при закрытии этой формы
            Application.Exit();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // заглушка
        }
    }
}
