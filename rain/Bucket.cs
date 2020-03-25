using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rain
{
    class Bucket
    {
        private int width, heigth;

        private bool stop = false;
        private Thread t = null;
        public bool IsAlive { get { return t != null && t.IsAlive; } }

        public static int X1;

        public int X { get; private set; }
        public int Y { get; private set; }
        public Point T1 { get; private set; }
        public Point T2 { get; private set; }
        public Point T3 { get; private set; }
        public Point T4 { get; private set; }
        private int b_width;
        private int b_heigth;
        


        public Bucket(Rectangle r)
        {
            Update(r);
            b_heigth = 50;
            b_width = 60;
            X = X1;
            Y = heigth -b_heigth;
            T1 = new Point(X - b_width, Y - b_heigth);
            T2 = new Point(X + b_width, Y - b_heigth);
            T3 = new Point(X + b_width / 2, Y + b_heigth);
            T4 = new Point(X - b_width / 2, Y + b_heigth);

            Start();
           
        }

        private void Move()
        {
            while (!stop)
            {
                X = X1;
                Y = heigth - b_heigth;
                T1 = new Point(X - b_width, Y - b_heigth);
                T2 = new Point(X + b_width, Y - b_heigth);
                T3 = new Point(X + b_width / 2, Y + b_heigth);
                T4 = new Point(X - b_width / 2, Y + b_heigth);
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
            t.Abort();
        }

        public void Update(Rectangle r)
        {
            width = r.Width;
            heigth = r.Height;
        }
        
    }
}

