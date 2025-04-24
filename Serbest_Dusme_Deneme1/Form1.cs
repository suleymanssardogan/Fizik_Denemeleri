using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serbest_Dusme_Deneme1
{
    public partial class Form1: Form
    {

        const double g = 9.82; // Yer çekimi ivmesi (m/s²)
        double height = 0;     // Topun yüksekliği (piksel)
        double speed = 0;      // Topun hızı (piksel/saniye)
        double time = 0;       // Geçen süre (saniye)
        private Timer timer;
        private double groundLevel = 300; // Zemin seviyesi (piksel)
        private double restitution = 0.8; // Çarpışma esnekliği katsayısı
        private const int BallSize = 50;  // Topun boyutu

        public Form1()
        {
            InitializeComponent();
            this.ClientSize = new Size(800, (int)groundLevel + 100); // Form boyutu

            timer = new Timer();
            timer.Interval = 10; // 10 ms = 0.01 s
            timer.Tick += Timer_Tick;

            Button startBtn = new Button();
            startBtn.Text = "Başlat";
            startBtn.Size = new Size(200, 40);
            startBtn.Location = new Point(10, 10);
            startBtn.Click += startBtn_Click;
            Controls.Add(startBtn);
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            time = 0;
            speed = 0;
            height = 0; // En üst nokta (y=0)
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            time += 0.01; // 10 ms = 0.01 s

            // Fizik hesaplamaları
            speed += g; // Hızı g ile artır (piksel/s²)
            height += speed * 0.01; // Yüksekliği hızla değiştir

            // Top zemine çarptığında (height + BallSize >= groundLevel)
            if (height + BallSize >= groundLevel)
            {
                height = groundLevel - BallSize; // Topu zemin üstünde tut
                speed = -speed * restitution; // Hız ters yönde ve esneklik katsayısı kadar

                // Enerji kaybından dolayı durma kontrolü
                if (Math.Abs(speed) < 0.5)
                {
                    timer.Stop();
                    speed = 0;
                }
            }

            Invalidate(); // Ekranı yenile
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // Topu çiz (375 x koordinatı, yükseklik aşağıya doğru artacak)
            e.Graphics.FillEllipse(Brushes.Red, 375, (float)height, BallSize, BallSize);

            // Zemini çiz
            e.Graphics.DrawLine(Pens.Black, 0, (float)groundLevel, this.Width, (float)groundLevel);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}