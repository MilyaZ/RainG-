using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rain
{
    public partial class Form2 : Form
    {
        Rain _f;
        public Form2()
        {
            InitializeComponent();
        }
        public Form2 (Rain f)
        {
            InitializeComponent(); 
            if (Rain.Miss >=5 )
            {
                this.Text = "Miss";
                label1.Text = "Вы устроили потоп =(";
                
            }
            if (Rain.Count >= 15)
            {
                this.Text = "Victory";
                label1.Text = "Поздравляем с победой!";
            }
            Rain.Miss = 0;
            Rain.Count = 0;
            _f = f;
            f.Hide();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            _f.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Rain new_game = new Rain(this);
            new_game.Show();
        }
    }
}
