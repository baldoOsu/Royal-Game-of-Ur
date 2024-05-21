using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Royal_Game_of_Ur
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            new Game(this);
        }
    }
}
