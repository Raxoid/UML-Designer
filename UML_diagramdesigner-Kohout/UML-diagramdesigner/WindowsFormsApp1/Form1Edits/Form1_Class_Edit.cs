using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.ClassDiagram;

namespace WindowsFormsApp1
{
    public partial class Form1_Class_Edit : Form
    {
        public ClassName className { get; set; } = new ClassName();
        private Diagram diagram { get; set; }
        public Form1_Class_Edit(ClassName _className, Diagram _diagram)
        {
            InitializeComponent();
            className = _className;
            diagram = _diagram;
            if (diagram.className != null)
            {
                textBox1.Text = diagram.className.Name;
                if (diagram.className.IsInterface == true)
                {
                    checkBox1.Checked = true;
                }
            }           
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                className.IsInterface = true;
            }
            if (textBox1.Text != "")
            {
                className.Name = textBox1.Text;
                diagram.className = this.className;
            }                      
            this.Close();
        }
    }
}
