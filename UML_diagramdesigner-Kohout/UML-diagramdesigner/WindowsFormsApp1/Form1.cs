using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private ActionHandler _action;
        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.pictureBox1.AllowDrop = true;
            _action = new ActionHandler(pictureBox1, this);
            _action.ArrowDeserialize();
            _action.DiagramDeserialize();
        }
    }
}
