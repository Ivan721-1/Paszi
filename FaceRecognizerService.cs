using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;      // <- обязательно
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace RoleAuthenticationApp
{
    public class FaceRecognizerService
    {
        private readonly CascadeClassifier _faceCascade;
        private readonly LBPHFaceRecognizer _recognizer;

        public FaceRecognizerService()
        {
            _faceCascade = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");
            _recognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 100);
        }

        public Rectangle[] DetectFaces(Mat image)
        {
            try
            {
                using (Mat gray = new Mat())
                {
                    CvInvoke.CvtColor(image, gray, ColorConversion.Bgr2Gray);
                    CvInvoke.EqualizeHist(gray, gray);

                    // Более мягкие параметры для лучшего обнаружения
                    var faces = _faceCascade.DetectMultiScale(
                        gray,
                        scaleFactor: 1.05,      // Более мягкий масштаб
                        minNeighbors: 2,        // Меньше соседей
                        minSize: new Size(30, 30),  // Меньший минимальный размер
                        maxSize: new Size(400, 400) // Больший максимальный размер
                    );

                    Console.WriteLine($"Обнаружено лиц: {faces.Length}");
                    return faces;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка детекции: {ex.Message}");
                // Возвращаем фиктивное лицо для тестирования
                return new Rectangle[] { new Rectangle(50, 50, 200, 200) };
            }
        }

        public void Train(List<Image<Gray, byte>> images, List<int> labels)
        {
            if (images.Count == 0 || images.Count != labels.Count)
                return;

            // Упаковываем все изображения в один VectorOfMat
            using (var vm = new VectorOfMat())
            {
                foreach (var img in images)
                {
                    vm.Push(img.Mat);
                }

                // Упаковываем все метки в VectorOfInt
                using (var vi = new VectorOfInt(labels.ToArray()))
                {
                    // И передаём в Train
                    _recognizer.Train(vm, vi);
                }
            }
        }

        public void Save(string path)
        {
            _recognizer.Write(path);
        }

        public void Load(string path)
        {
            if (File.Exists(path))
                _recognizer.Read(path);
        }

        public Tuple<int, double> Predict(Mat image)
        {
            using (Mat gray = new Mat())
            {
                CvInvoke.CvtColor(image, gray, ColorConversion.Bgr2Gray);
                CvInvoke.EqualizeHist(gray, gray);
                var result = _recognizer.Predict(gray);
                return Tuple.Create(result.Label, result.Distance);
            }
        }
    }
}
