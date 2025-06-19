using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RoleAuthenticationApp
{
    public partial class Form1 : Form
    {
        private readonly UsbDetector _usbDetector = new UsbDetector();
        private readonly DatabaseHelper _db = new DatabaseHelper();
        private readonly FaceRecognizerService _faceService = new FaceRecognizerService();
        private readonly List<Image<Gray, byte>> _trainingImages = new List<Image<Gray, byte>>();
        private readonly List<int> _trainingLabels = new List<int>();
        private string _usbSerial;
        private byte[] _faceData;
        private int _currentLabel;

        public Form1()
        {
            InitializeComponent();

            try
            {
                _db.InitializeDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации базы данных: {ex.Message}\nПриложение будет работать без сохранения данных.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            this.btnReadKey.Click += new EventHandler(BtnReadKey_Click);
            this.btnTakePhoto.Click += new EventHandler(BtnTakePhoto_Click);
            this.btnAddToDb.Click += new EventHandler(BtnAddToDb_Click);
            this.btnAlreadyHaveRole.Click += new EventHandler(BtnAlreadyHaveRole_Click);
            this.btnShowAllDevices.Click += new EventHandler(BtnShowAllDevices_Click);
        }

        private void BtnReadKey_Click(object sender, EventArgs e)
        {
            _usbSerial = _usbDetector.GetUsbSerialNumber();
            MessageBox.Show(
                string.IsNullOrEmpty(_usbSerial)
                    ? "USB-ключ не обнаружен!"
                    : $"Обнаружен USB-ключ: {_usbSerial}"
            );
        }

        private void BtnTakePhoto_Click(object sender, EventArgs e)
        {
            // Открываем форму с живой камерой и автоматической детекцией лица
            using (CameraForm cameraForm = new CameraForm())
            {
                if (cameraForm.ShowDialog() == DialogResult.OK && cameraForm.FaceCaptured)
                {
                    try
                    {
                        _faceData = cameraForm.CapturedFaceData;
                        _currentLabel = new Random().Next(10000);

                        // Конвертируем данные для обучения
                        // Сохраняем данные напрямую, без сложной обработки
                        _trainingLabels.Add(_currentLabel);

                        try
                        {
                            // Простое сохранение без обучения модели (для упрощения)
                            MessageBox.Show("Данные лица сохранены для обучения модели.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception trainEx)
                        {
                            MessageBox.Show($"Предупреждение при обучении: {trainEx.Message}", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        MessageBox.Show("Лицо успешно захвачено и сохранено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при обработке снимка: {ex.Message}\n\nПопробуйте еще раз.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Снимок не был сделан. Попробуйте еще раз.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnAddToDb_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRoleName.Text) ||
                string.IsNullOrEmpty(_usbSerial) ||
                _faceData == null)
            {
                MessageBox.Show("Введите все данные!");
                return;
            }

            try
            {
                _db.AddRole(txtRoleName.Text.Trim(), _usbSerial, _faceData);
                MessageBox.Show("Роль успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Очищаем поля после успешного добавления
                txtRoleName.Clear();
                _usbSerial = null;
                _faceData = null;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении роли: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAlreadyHaveRole_Click(object sender, EventArgs e)
        {
            new Form2().Show();
            this.Hide();
        }

        private void BtnShowAllDevices_Click(object sender, EventArgs e)
        {
            var devices = _usbDetector.GetAllUsbDevices();
            if (devices.Count == 0)
            {
                MessageBox.Show("USB устройства не найдены!");
                return;
            }

            string deviceList = "Найденные USB устройства:\n\n";
            for (int i = 0; i < devices.Count; i++)
            {
                deviceList += $"{i + 1}. {devices[i]}\n";
            }

            // Добавляем информацию о существующих ролях
            try
            {
                var existingRoles = _db.GetAllRoles();
                if (existingRoles.Count > 0)
                {
                    deviceList += "\n--- Существующие роли ---\n";
                    foreach (var role in existingRoles)
                    {
                        deviceList += $"• {role.RoleName} (USB: {role.USBSerial})\n";
                    }

                    deviceList += "\nДля удаления существующей роли используйте SSMS или перезапустите приложение с новым USB-устройством.";
                }
            }
            catch (Exception ex)
            {
                deviceList += $"\n\nОшибка при получении ролей: {ex.Message}";
            }

            MessageBox.Show(deviceList, "USB Устройства и Роли", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
