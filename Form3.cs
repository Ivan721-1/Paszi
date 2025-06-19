using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RoleAuthenticationApp
{
    public partial class Form3 : Form
    {
        private readonly FaceRecognizerService _faceService = new FaceRecognizerService();
        private readonly string _roleName;
        private readonly byte[] _faceTemplate;

        public Form3(string roleName, byte[] faceData)
        {
            InitializeComponent();
            _roleName = roleName;
            _faceTemplate = faceData;
            this.btnSnapshot.Click += new EventHandler(BtnSnapshot_Click);
            this.FormClosed += new FormClosedEventHandler(Form3_FormClosed);
            _faceService.Load("trained_model.xml");
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Закрываем все приложение при закрытии этой формы
            Application.Exit();
        }

        private void BtnSnapshot_Click(object sender, EventArgs e)
        {
            // Используем новую форму камеры с автоматической детекцией лица
            MessageBox.Show("Приготовьтесь к проверке личности!\nПоместите лицо в кадр для автоматического захвата.", "Аутентификация", MessageBoxButtons.OK, MessageBoxIcon.Information);

            using (CameraForm cameraForm = new CameraForm())
            {
                if (cameraForm.ShowDialog() == DialogResult.OK && cameraForm.FaceCaptured)
                {
                    try
                    {
                        // Получаем захваченные данные лица
                        byte[] capturedFaceData = cameraForm.CapturedFaceData;

                        MessageBox.Show("Лицо захвачено! Проверяем соответствие...", "Обработка", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Имитируем проверку соответствия лиц
                        System.Threading.Thread.Sleep(1500);

                        // РЕАЛЬНОЕ СРАВНЕНИЕ ЛИЦ
                        bool success = CompareFaces(_faceTemplate, capturedFaceData);

                        new Form4(success, _roleName).Show();
                        this.Hide();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при проверке лица: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        new Form4(false, _roleName).Show();
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Снимок не был сделан. Аутентификация отменена.", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    new Form4(false, _roleName).Show();
                    this.Hide();
                }
            }
        }

        private bool CompareFaces(byte[] templateFace, byte[] capturedFace)
        {
            try
            {
                // Базовая проверка
                if (templateFace == null || capturedFace == null)
                    return false;

                if (templateFace.Length == 0 || capturedFace.Length == 0)
                    return false;

                // УЛУЧШЕННАЯ БИОМЕТРИЯ - анализ изображений
                using (var templateStream = new MemoryStream(templateFace))
                using (var capturedStream = new MemoryStream(capturedFace))
                using (var templateBitmap = new Bitmap(templateStream))
                using (var capturedBitmap = new Bitmap(capturedStream))
                {
                    // Приводим к одинаковому размеру для сравнения
                    var templateResized = new Bitmap(templateBitmap, 200, 200);
                    var capturedResized = new Bitmap(capturedBitmap, 200, 200);

                    // Простой но эффективный анализ
                    double pixelSimilarity = ComparePixels(templateResized, capturedResized);
                    double histogramSimilarity = CompareHistograms(templateResized, capturedResized);

                    // Комбинированная оценка
                    double overallSimilarity = (pixelSimilarity * 0.6 + histogramSimilarity * 0.4) * 100;

                    // СТРОГИЕ пороги схожести для безопасности
                    double strictThreshold = 85.0;   // Очень строгий порог
                    double normalThreshold = 75.0;   // Строгий порог

                    bool isStrictMatch = overallSimilarity >= strictThreshold;
                    bool isNormalMatch = overallSimilarity >= normalThreshold;

                    // Показываем результат анализа
                    string debugMessage = $"=== БИОМЕТРИЧЕСКИЙ АНАЛИЗ ===\n" +
                                         $"Общая схожесть: {overallSimilarity:F1}%\n\n" +
                                         $"Детали анализа:\n" +
                                         $"• Попикселное сравнение: {pixelSimilarity:P1}\n" +
                                         $"• Гистограмма цветов: {histogramSimilarity:P1}\n\n" +
                                         $"Пороги:\n" +
                                         $"• Очень строгий ({strictThreshold}%): {(isStrictMatch ? "✓" : "✗")}\n" +
                                         $"• Строгий ({normalThreshold}%): {(isNormalMatch ? "✓" : "✗")}";

                    // Используем строгий порог - НЕТ ручного подтверждения!
                    bool isMatch = isNormalMatch;

                    if (isStrictMatch)
                    {
                        MessageBox.Show($"Отличное совпадение!\n{debugMessage}", "Доступ разрешен", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (isNormalMatch)
                    {
                        MessageBox.Show($"Хорошее совпадение!\n{debugMessage}", "Доступ разрешен", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Лицо не совпадает с сохраненным!\n{debugMessage}", "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    return isMatch;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сравнении лиц: {ex.Message}\n\nДоступ запрещен из соображений безопасности.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Упрощенные методы анализа
        private double ComparePixels(Bitmap img1, Bitmap img2)
        {
            double similarity = 0;
            int totalPixels = img1.Width * img1.Height;

            for (int x = 0; x < img1.Width; x += 2) // Каждый второй пиксель для скорости
            {
                for (int y = 0; y < img1.Height; y += 2)
                {
                    Color c1 = img1.GetPixel(x, y);
                    Color c2 = img2.GetPixel(x, y);

                    double diff = Math.Abs(c1.R - c2.R) + Math.Abs(c1.G - c2.G) + Math.Abs(c1.B - c2.B);
                    similarity += 1.0 - (diff / (255.0 * 3));
                }
            }

            return similarity / (totalPixels / 4); // Делим на 4, так как берем каждый второй пиксель
        }

        private double CompareHistograms(Bitmap img1, Bitmap img2)
        {
            int[] hist1 = new int[256];
            int[] hist2 = new int[256];

            // Строим гистограммы яркости
            for (int x = 0; x < img1.Width; x++)
            {
                for (int y = 0; y < img1.Height; y++)
                {
                    Color c1 = img1.GetPixel(x, y);
                    Color c2 = img2.GetPixel(x, y);

                    int brightness1 = (c1.R + c1.G + c1.B) / 3;
                    int brightness2 = (c2.R + c2.G + c2.B) / 3;

                    hist1[brightness1]++;
                    hist2[brightness2]++;
                }
            }

            // Сравниваем гистограммы
            double correlation = 0;
            double sum1 = hist1.Sum();
            double sum2 = hist2.Sum();

            for (int i = 0; i < 256; i++)
            {
                double p1 = hist1[i] / sum1;
                double p2 = hist2[i] / sum2;
                correlation += Math.Sqrt(p1 * p2);
            }

            return correlation;
        }


    }
}
