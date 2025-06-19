using System;
using System.Windows.Forms;

namespace RoleAuthenticationApp
{
    public partial class Form2 : Form
    {
        private readonly UsbDetector _usbDetector = new UsbDetector();
        private readonly DatabaseHelper _db = new DatabaseHelper();
        private string _currentRole;
        private byte[] _currentFaceData;

        public Form2()
        {
            InitializeComponent();
            this.btnKeyInserted.Click += new EventHandler(BtnKeyInserted_Click);
            this.btnCheckIdentity.Click += new EventHandler(BtnCheckIdentity_Click);
            this.btnBack.Click += new EventHandler(BtnBack_Click);
            this.FormClosed += new FormClosedEventHandler(Form2_FormClosed);
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Закрываем все приложение при закрытии этой формы
            Application.Exit();
        }

        private void BtnKeyInserted_Click(object sender, EventArgs e)
        {
            string serial = _usbDetector.GetUsbSerialNumber();
            if (string.IsNullOrEmpty(serial))
            {
                lblKeyStatus.Text = "КЛЮЧ не найден!";
                lblKeyStatus.Visible = true;
                btnCheckIdentity.Visible = false;
                return;
            }

            var tup = _db.GetRoleByUsbSerial(serial);
            if (tup.Item1 == null)
            {
                lblKeyStatus.Text = "КЛЮЧ не найден!";
                lblKeyStatus.Visible = true;
                btnCheckIdentity.Visible = false;
                return;
            }

            _currentRole = tup.Item1;
            _currentFaceData = tup.Item2;
            lblKeyStatus.Text = $"Распознан ключ {_currentRole}";
            lblKeyStatus.Visible = true;
            btnCheckIdentity.Visible = true;
        }

        private void BtnCheckIdentity_Click(object sender, EventArgs e)
        {
            new Form3(_currentRole, _currentFaceData).Show();
            this.Hide();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            // Возвращаемся к главной форме
            new Form1().Show();
            this.Hide();
        }
    }
}
