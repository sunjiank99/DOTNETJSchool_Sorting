using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sorting
{
    public partial class jishi : Form
    {
        private int count;
        private string FJ_id;
        public jishi(string FJ_id)
        {
            InitializeComponent();
            this.FJ_id = FJ_id;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            count = 0;
            label2.Text = FJ_id;
            this.timer1.Enabled = true;
            this.timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            count++;
            time.Text = (3 - count) + "秒";

            if (count == 3)
            {
                this.timer1.Stop();
                this.Close();
            }
        }

    }
}
