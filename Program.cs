using System;
using System.Windows.Forms;

namespace RoleAuthenticationApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Запуск
            Application.Run(new Form1());
        }
    }
}
