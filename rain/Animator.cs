using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing.Drawing2D;


namespace rain
{
    //public GraphicsPath drop { get; set; }
    class Animator
    {
        private Graphics mainG;
        private int width, heigth;
        private List<Drop> drops = new List<Drop>();
        private Thread t;
        private bool stop = false;
        private BufferedGraphics bg;
        private object obj = new object();
        private bool bgChanged = false;
        Bucket buc;

        public Animator(Graphics g, Rectangle r)
        {
            Update(g, r);
        }
        public void Update(Graphics g, Rectangle r)
        {
            mainG = g;
            width = r.Width;
            heigth = r.Height;
            bgChanged = true;
            bg = BufferedGraphicsManager.Current.Allocate(mainG, new Rectangle(0, 0, width, heigth));
            Monitor.Enter(drops);
            foreach (var d in drops)
            {
                d.Update(r);
            }
            if(buc!=null) buc.Update(r);
            Monitor.Exit(drops);
            //Monitor.Enter(drops);//параметр ссылочный тип.
        }
        private void Animate()
        {
            while (!stop)
            {
                Monitor.Enter(obj);
                bgChanged = false;
                Graphics g = bg.Graphics;
               
                Monitor.Exit(obj);
                var x = 60;
                var y = 20;
                g.Clear(Color.FromArgb(192, 255, 255));

                for ( int i =0; i < 4; i++)
                {
                    var cloud = new GraphicsPath();

                    cloud.StartFigure();

                    cloud.AddArc(x, y, 100, 50, 180, 180);
                    cloud.AddArc(x + 100, y, 100, 50, 180, 180);
                    cloud.AddArc(x + 200, y, 100, 50, 180, 180);

                    cloud.AddArc(x + 250, y + 25, 100, 100, -90, 180);

                    cloud.AddArc(x + 200, y + 100, 100, 50, 0, 180);
                    cloud.AddArc(x + 100, y + 100, 100, 50, 0, 180);
                    cloud.AddArc(x, y + 100, 100, 50, 0, 180);

                    cloud.AddArc(x - 50, y + 25, 100, 100, 90, 180);

                    cloud.CloseFigure();
                    Brush br1 = new SolidBrush(Color.White);
                    g.FillPath(br1, cloud);
                    Pen p1 = new Pen(Color.White, 2);
                    g.DrawPath(p1, cloud);
                    x += 450;

                }          
                if (Program.bucket && buc!=null)
                {
                                       
                    var bucket = new GraphicsPath();
                    bucket.StartFigure();
                    bucket.AddLine(buc.T1,buc.T2);
                    bucket.AddLine(buc.T2, buc.T3);
                    bucket.AddLine(buc.T3, buc.T4);
                    bucket.AddLine(buc.T4, buc.T1);

                    bucket.CloseFigure();
                    Brush br = new SolidBrush(Color.Gray);
                    g.FillPath(br, bucket);
                    Pen p = new Pen(Color.Black, 1);
                    g.DrawPath(p, bucket);
                   
                }
                Monitor.Enter(drops);

                for (int i = 0; i < Drop.Count; i++)
                {

                    if (buc != null &&
                         drops[i].X - 5 > buc.T1.X && drops[i].X + 5 < buc.T2.X && drops[i].Y >= buc.T1.Y )
                    {
                        drops[i].Stop();
                        drops.Remove(drops[i]);
                        i--;
                        Drop.Count--;
                        Rain.Count++;
                    }
                    else
                    {

                        if (!drops[i].IsAlive)
                        {
                            if (buc != null&& Rain.Count>0 && drops[i].X > 0 && drops[i].X <width)
                            {
                                Rain.Miss++;
                            }
                            drops[i].Stop();
                            drops.Remove(drops[i]);
                            i--;
                            Drop.Count--;
                        }
                    }

                }
                Monitor.Exit(drops);
                Monitor.Enter(drops);
                foreach (var d in drops)
                {
                    var drop = new GraphicsPath();
                    drop.StartFigure();
                    drop.AddLine(d.X, d.Y, d.X + 5, d.Y + 15);
                    drop.AddArc(d.X - 5, d.Y + 15, 10, 5, 0, 180);
                    drop.AddLine(d.X - 5,d.Y + 15, d.X, d.Y);
                    drop.CloseFigure();

                    Brush br = new SolidBrush(Color.Blue);
                    g.FillPath(br, drop);
                    Pen p = new Pen(Color.Blue, 2);
                    g.DrawPath(p, drop);

                }
                Monitor.Exit(drops);
                try
                {
                    bg.Render();
                }
                catch (Exception e) { }
                Monitor.Enter(obj);
                if (!bgChanged)
                {
                    try
                    {
                        bg.Render();
                    }
                    catch (Exception e)
                    {
                    }
                }
                Monitor.Exit(obj);
                Thread.Sleep(30);
                
            }
        }
        public void Start()
        {
            if (t == null || !t.IsAlive)
            {
                ThreadStart th = new ThreadStart(Animate);
                t = new Thread(th);
                t.Start();
            }
            var rect = new Rectangle(0, 0, width, heigth);
          
            Drop d = new Drop(rect);
            d.Start();
            Monitor.Enter(drops);
            drops.Add(d);
            Drop.Count++;
            Monitor.Exit(drops);
            if (Program.bucket && buc == null)
            {
                buc = new Bucket(rect);
            }


        }
        public void Stop()
        {
            stop = true;
            Monitor.Enter(drops);
            foreach (var d in drops)
            {
                d.Stop();
                Drop.Count--;
            }
            if (buc != null)
            {
                buc.Stop();
                buc = null;
                Program.bucket = false;
            }
            
            drops.Clear();
            Monitor.Exit(drops);
        }
        public void Wind()
        {
            Monitor.Enter(drops);
            foreach (var d in drops)
            {
                d.Wind();
            }
            
            Monitor.Exit(drops);
        }

    }
}
