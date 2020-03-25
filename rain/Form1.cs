using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rain
{
    public partial class Rain : Form
    {
        Animator a;

        Form2 _f;
        public static int Count { get ; set; }
        public static int Miss { get; set; }

        public Rain()
        {
            InitializeComponent();
            a = new Animator(panel1.CreateGraphics(), panel1.ClientRectangle);
            Count = 0;
        }
        public Rain(Form2 f)
        {
            InitializeComponent();
            a = new Animator(panel1.CreateGraphics(), panel1.ClientRectangle);
            Count = 0;
            timer1.Start();
            _f = f;
            f.Hide();
  
        }
        
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            a.Start();
            if (Count != 0)
            {
                label2.Text = "Собрано: " + Count;
                label3.Text = "Промахи: " + Miss;
            }
            if (Miss >= 5||Count>=15)
            {
                timer1.Stop();
                a.Stop();
                Form2 newForm = new Form2(this);
                newForm.Show();
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            a.Stop();
            if(_f!= null)  _f.Close();
      
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (a == null) return;
            a.Update(panel1.CreateGraphics(), panel1.ClientRectangle);
            
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = String.Format("Текущее значение: {0}", trackBar1.Value);
            Program.trackBar1_Value = trackBar1.Value;
            a.Wind();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Interval = 600;
            Program.bucket = true;
            System.Threading.Thread.Sleep(3000);
            label2.Text = "Собрано: ";
            label3.Text = "Промахи: ";
            
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            Bucket.X1 = e.X;

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            
            Bucket.X1 = e.X;
           
        }

       
    }
}
