using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace RoleAuthenticationApp
{
    public partial class CameraForm : Form
    {
        private VideoCapture _capture;
        private System.Windows.Forms.Timer _timer;
        private FaceRecognizerService _faceService;
        private bool _faceDetected = false;
        private Mat _capturedFrame;
        private int _detectionCounter = 0;
        private const int DETECTION_THRESHOLD = 3; // Уменьшили до 3 кадров для быстрого срабатывания

        public byte[] CapturedFaceData { get; private set; }
        public bool FaceCaptured { get; private set; } = false;

        public CameraForm()
        {
            InitializeComponent();
            _faceService = new FaceRecognizerService();
            InitializeCamera();
        }

        private void InitializeCamera()
        {
            try
            {
                _capture = new VideoCapture(0);
                if (!_capture.IsOpened)
                {
                    MessageBox.Show("Не удается открыть камеру!");
                    this.Close();
                    return;
                }

                _timer = new System.Windows.Forms.Timer();
                _timer.Interval = 100; // 10 FPS
                _timer.Tick += Timer_Tick;
                _timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации камеры: {ex.Message}");
                this.Close();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_capture == null || !_capture.IsOpened)
                return;

            try
            {
                Mat frame = new Mat();
                _capture.Read(frame);

                if (frame.IsEmpty)
                    return;

                // Отображаем кадр в PictureBox
                pictureBoxCamera.Image = frame.ToBitmap();

                // Улучшенная детекция лица с проверкой яркости
                bool faceDetected = false;
                Rectangle faceRect = new Rectangle(100, 100, 200, 200); // Фиктивный прямоугольник

                try
                {
                    // Сначала проверяем яркость кадра - если слишком темно, то камера закрыта
                    Mat grayFrame = new Mat();
                    CvInvoke.CvtColor(frame, grayFrame, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

                    // Вычисляем среднюю яркость
                    var scalar = CvInvoke.Mean(grayFrame);
                    double averageBrightness = scalar.V0;

                    // Если слишком темно (камера закрыта), не ищем лицо
                    if (averageBrightness < 30) // Порог яркости
                    {
                        faceDetected = false;
                        lblStatus.Text = "Слишком темно! Уберите руку с камеры";
                        lblStatus.ForeColor = Color.Red;
                    }
                    else
                    {
                        // Используем реальную детекцию лиц
                        Rectangle[] faces = _faceService.DetectFaces(frame);
                        if (faces.Length > 0)
                        {
                            // Проверяем размер обнаруженного лица - оно должно быть достаточно большим
                            Rectangle face = faces[0];
                            if (face.Width > 80 && face.Height > 80 &&
                                face.Width < 400 && face.Height < 400)
                            {
                                // Дополнительная проверка: лицо должно быть в центральной части кадра
                                int centerX = frame.Width / 2;
                                int centerY = frame.Height / 2;
                                int faceCenterX = face.X + face.Width / 2;
                                int faceCenterY = face.Y + face.Height / 2;

                                // Лицо должно быть не слишком далеко от центра
                                if (Math.Abs(faceCenterX - centerX) < frame.Width / 3 &&
                                    Math.Abs(faceCenterY - centerY) < frame.Height / 3)
                                {
                                    faceDetected = true;
                                    faceRect = face;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // Если детекция не работает, НЕ считаем что лицо обнаружено
                    faceDetected = false;
                }

                if (faceDetected)
                {
                    _detectionCounter++;

                    // Рисуем прямоугольник вокруг лица
                    using (Graphics g = Graphics.FromImage(pictureBoxCamera.Image))
                    {
                        using (Pen pen = new Pen(Color.Green, 3))
                        {
                            g.DrawRectangle(pen, faceRect);
                        }
                    }
                    pictureBoxCamera.Invalidate();

                    // Если лицо стабильно обнаружено несколько кадров подряд
                    if (_detectionCounter >= DETECTION_THRESHOLD && !_faceDetected)
                    {
                        _faceDetected = true;
                        _capturedFrame = frame.Clone();

                        // Автоматически делаем снимок
                        CapturePhoto();
                    }

                    if (lblStatus.Text != $"Лицо обнаружено! Стабильность: {_detectionCounter}/{DETECTION_THRESHOLD}")
                    {
                        lblStatus.Text = $"Лицо обнаружено! Стабильность: {_detectionCounter}/{DETECTION_THRESHOLD}";
                        lblStatus.ForeColor = Color.Green;
                    }
                }
                else
                {
                    // Быстро сбрасываем счетчик если лицо пропало
                    _detectionCounter = 0;

                    if (lblStatus.Text != "Поместите лицо в кадр..." && !lblStatus.Text.Contains("Слишком темно"))
                    {
                        lblStatus.Text = "Поместите лицо в кадр...";
                        lblStatus.ForeColor = Color.Orange;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Ошибка: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void CapturePhoto()
        {
            try
            {
                if (_capturedFrame != null)
                {
                    // Конвертируем в JPEG для сохранения
                    CapturedFaceData = _capturedFrame.ToImage<Bgr, byte>().ToJpegData();
                    FaceCaptured = true;

                    _timer.Stop();
                    lblStatus.Text = "Снимок сделан! Закрываем камеру...";
                    lblStatus.ForeColor = Color.Blue;

                    // Закрываем форму через 2 секунды
                    System.Windows.Forms.Timer closeTimer = new System.Windows.Forms.Timer();
                    closeTimer.Interval = 2000;
                    closeTimer.Tick += (s, e) => {
                        closeTimer.Stop();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    };
                    closeTimer.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при захвате фото: {ex.Message}");
            }
        }

        private void BtnCapture_Click(object sender, EventArgs e)
        {
            // Ручной захват, если автоматический не сработал
            if (_capture != null && _capture.IsOpened)
            {
                Mat frame = new Mat();
                _capture.Read(frame);
                if (!frame.IsEmpty)
                {
                    _capturedFrame = frame.Clone();
                    CapturePhoto();
                }
            }
        }

        private void CameraForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _timer?.Stop();
            _capture?.Dispose();
        }

        private void CameraForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Если форма камеры закрыта без захвата, не закрываем приложение
            // Это модальная форма, поэтому управление вернется к родительской форме
        }
    }
}
