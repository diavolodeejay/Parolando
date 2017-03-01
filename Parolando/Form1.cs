using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parolando
{
    public partial class Form1 : Form
    {
        Gioco a = new Gioco();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            a.Gioca(2);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            a.Carica();
        }
    }
}
