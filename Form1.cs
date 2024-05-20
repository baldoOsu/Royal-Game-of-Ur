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
    public partial class Form1 : Form
    {
        private readonly int titleBarHeight = SystemInformation.CaptionHeight;
        private Game game;

        public Form1()
        {
            
            InitializeComponent();
            game = new Game(this);
        }
    }
}
