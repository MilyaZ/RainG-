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



        public int X { get; private set; }
        public int Y { get; private set; }

        public Bucket(Rectangle r)
        {
            Update(r);
            X = Form1.X;
            Y = heigth -20;
           
        }

        private void Move()
        {
            while (!stop)
            {
                
                X = Form1.X;
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
        
    }
}

