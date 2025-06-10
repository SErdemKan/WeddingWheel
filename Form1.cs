using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;

namespace ÇARK
{
    public partial class Form1 : Form
    {
        private float angle = 0;
        private int spinSpeed;
        private bool isSpinning = false;
        private Image wheelImage;
        private string[] options =
        {
            "Aksesuarlarla poz ver",
            "Gülümse",
            "Şarkı söyle",
            "Gelinin/Damadın taklidini yap",
            "Damadın/Gelinin en sevdiği şeyi tahmin et",
            "En komik anını anlat",
            "Dans et",
            "Anı bırak"
        };

        private SoundPlayer spinSound;

        public Form1()
        {


            InitializeComponent();
            DoubleBuffered = true;

            try
            {
                wheelImage = Image.FromFile("C:\\Users\\yucel\\source\\repos\\ÇARK\\ÇARK\\images\\wheel.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Görsel yüklenemedi: " + ex.Message);
            }

            try
            {
                spinSound = new SoundPlayer("C:\\Users\\yucel\\source\\repos\\ÇARK\\ÇARK\\sounds\\tick.wav");
                spinSound.Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ses dosyası yüklenemedi: " + ex.Message);
            }

            timer1.Interval = 20;
            timer1.Tick += Timer_Tick;
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (timer1.Tag == null) return;

            angle += spinSpeed;
            spinSpeed = Math.Max(spinSpeed - 1, 0);

            if (spinSpeed == 0 || (int)timer1.Tag <= 0)
            {
                timer1.Stop();
                isSpinning = false;
                spinSound?.Stop();

                string result = GetResult();
                MessageBox.Show("Çarkta gelen seçenek: " + result);
                label1.Text = result;

                if (result == "Aksesuarlarla poz ver" || result == "Gülümse")
                {

                    await StartCountdownAndTakePhoto();
                }
                else
                {
                    await RecordVideoAsync(5 ); // 30 saniyelik kayıt
                }
            }
            else
            {
                timer1.Tag = (int)timer1.Tag - 1;
            }

            Invalidate();
        }

        private async Task StartCountdownAndTakePhoto()
        {

            for (int i = 5; i >= 1; i--)
            {
                label1.Text = $"Poz ver! {i}";
                await Task.Delay(1000);
            }

            label1.Text = "Fotoğraf çekiliyor...";
            TakePhoto();
        }


        private void TakePhoto()
        {
            try
            {
                using (var capture = new VideoCapture(0)) // 0 = varsayılan kamera
                {
                    if (!capture.IsOpened())
                    {
                        MessageBox.Show("Kamera açılamadı!");
                        return;
                    }

                    using (var frame = new Mat())
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            capture.Read(frame);
                            Cv2.Flip(frame, frame, FlipMode.Y); // Aynalama (mirror)

                            System.Threading.Thread.Sleep(100);
                        }

                        if (frame.Empty())
                        {

                            MessageBox.Show("Fotoğraf alınamadı!");
                            return;
                        }

                        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), $"fotograf{DateTime.Now.Ticks}.jpg");
                        Cv2.ImWrite(path, frame);
                        MessageBox.Show("Fotoğraf çekildi: " + path);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fotoğraf çekiminde hata: " + ex.Message);
            }
        }

        private async Task RecordVideoAsync(int durationSeconds)
        {
            string outputFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), $"video_{DateTime.Now.Ticks}.avi");

            var process = new Process();
            process.StartInfo.FileName = "ffmpeg";
            process.StartInfo.Arguments = $"-f dshow -t {durationSeconds} -i video=\"USB webcam\":audio=\"Mikrofon (Realtek(R) Audio)\" -vf \"hflip\" \"{outputFile}\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();

            string stderr = await process.StandardError.ReadToEndAsync();
            string stdout = await process.StandardOutput.ReadToEndAsync();

            await process.WaitForExitAsync();

            MessageBox.Show($"Video kaydı tamamlandı!\n\nKaydedilen dosya: {outputFile}\n\nFFmpeg çıktısı:\n{stderr}");
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawWheel(e.Graphics);
        }

        private void DrawWheel(Graphics g)
        {
            int centerX = ClientSize.Width / 2;
            int centerY = ClientSize.Height / 2;
            int radius = Math.Min(ClientSize.Width, ClientSize.Height) / 3;

            int wheelWidth = wheelImage.Width;
            int wheelHeight = wheelImage.Height;
            float scaleFactor = Math.Min(ClientSize.Width / (float)wheelWidth, ClientSize.Height / (float)wheelHeight);
            int scaledWidth = (int)(wheelWidth * scaleFactor);
            int scaledHeight = (int)(wheelHeight * scaleFactor);

            //   g.Clear(Color.White);
            g.TranslateTransform(centerX, centerY);
            g.RotateTransform(angle);
            g.DrawImage(wheelImage, -scaledWidth / 2, -scaledHeight / 2, scaledWidth, scaledHeight);
            g.ResetTransform();

            DrawArrow(g, centerX, centerY - radius - 20);
        }

        private void DrawArrow(Graphics g, float x, float y)
        {
            System.Drawing.Point[] points =
            {
                new System.Drawing.Point((int)x, (int)y - 20),
                new System.Drawing.Point((int)x - 15, (int)y + 10),
                new System.Drawing.Point((int)x + 15, (int)y + 10)
            };

            using (Brush brush = new SolidBrush(Color.Red))
            {
                g.FillPolygon(brush, points);
            }
        }

        private string GetResult()
        {
            float degreesPerOption = 360f / options.Length;
            int selectedIndex = options.Length - 1 - ((int)((angle % 360) / degreesPerOption) % options.Length);
            return options[selectedIndex];
        }

        private void btnSpin_Click_1(object sender, EventArgs e)
        {
            if (isSpinning) return;

            isSpinning = true;
            angle = 0;
            spinSpeed = new Random().Next(40, 60);
            int spinDuration = new Random().Next(300, 400);
            timer1.Tag = spinDuration;
            spinSound?.PlayLooping();
            timer1.Start();
        }
    }
}