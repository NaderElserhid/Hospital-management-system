using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital_management_system
{
    public partial class PrograssParPage : Form
    {
        public PrograssParPage()
        {
            InitializeComponent();
        }
        private void timer1_Tick(object sender, EventArgs e)
        { 
           
            if (progressBar1.Value < 100)
            {
                progressBar1.Value = progressBar1.Value + 1;

                label1.Text = "Loading " + progressBar1.Value.ToString() + "%";
            }
            else
            {
                timer1.Stop();
                this.Hide();
                Form form = new Hospital_Management_page();
                form.Show();
            }
        }

        private void PrograssParPage_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
