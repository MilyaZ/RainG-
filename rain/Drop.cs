using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Drawing;

namespace rain
{
    class Drop
    {
        private int width, heigth;

        private bool stop = false;
        private Thread t = null;
        public bool IsAlive { get { return t != null && t.IsAlive; } }

        private int dy;
        private int dx;
        private static Random rand = null;
       

        public int X { get; private set; }
        public int Y { get; private set; }
        public static int Count=0;

        public Drop(Rectangle r)
        {
            Update(r);
            if (rand == null) rand = new Random();

            X = rand.Next(-r.Width, r.Width*2);
            while((X > 0 && X < 10)||(X>410 && X<460)|| (X > 860 && X < 910)|| (X > 1310 && X < 1360))//Для облаков
            {
                X = rand.Next(-r.Width, r.Width * 2);
            }
            Y = rand.Next(40,100);
            dy = 10;
            dx = Program.trackBar1_Value;
        }
       
        private void Move()
        {
            while (!stop)
            {
                Thread.Sleep(30);
                Y += dy;
                X += dx;
            }
        }
        public void Start()
        {
            if (t == null || !t.IsAlive)
            {
                stop = false;
                ThreadStart th = new ThreadStart(Move);
                t = new Thread(th);
                t.Start();
            }
        }
        public void Stop()
        {
           stop = true;
        }
        
        public void Update(Rectangle r)
        {
            width = r.Width;
            heigth = r.Height;
        }
        public void Wind()
        {
           
            dx = Program.trackBar1_Value ;
           
        }
    }
}
