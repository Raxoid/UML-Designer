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
using WindowsFormsApp1.Form1Edits;

namespace WindowsFormsApp1
{
    public partial class Form1_Property_Edit : Form
    {
        public Property property { get; set; } = new Property();

        public PropertyDataModel model = new PropertyDataModel();
        public Diagram diagram { get; set; }
        public Form1_Property_Edit(Property _property, Diagram _diagram)
        {
            InitializeComponent();
            property = _property;
            diagram = _diagram;
            dataGridView1.DataSource = model;
            dataGridView1.ReadOnly = true;
            if (diagram.properties != null)
            {
                foreach (var property in diagram.properties)
                {
                    model.properties.Add(property);
                }
            }
            comboBox1.Items.Add("public");
            comboBox1.Items.Add("private");
            comboBox2.Items.Add("string");
            comboBox2.Items.Add("int");
            comboBox2.Items.Add("float");
            comboBox2.Items.Add("bool");
            comboBox2.Items.Add("char");
            comboBox2.Items.Add("custom");
            if (diagram.className.IsInterface == true)
            {
               comboBox1.Enabled = false;
            }               
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            textBox2.Visible = false;
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            diagram.properties = model.properties.ToList();
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            Property property = new Property();
            property.Name = textBox1.Text;
            property.AccessModifier = comboBox1.Text;
            if (comboBox2.Text != "custom")
            {
                property.DataType = comboBox2.Text;
            }          
            if (comboBox2.Text == "custom")
            {
                property.DataType = textBox2.Text;
            }
            if (property.Name != "" && property.DataType != "" && property.AccessModifier != "")
            {
                model.properties.Add(property);
            }
            
            textBox1.Text = null;
            comboBox1.Text = null;
            comboBox2.Text = null;

        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == "custom")
            {
                textBox2.Visible = true;
            }
            else if (comboBox2.Text != "custom")
            {
                textBox2.Visible = false;
            }
        }

           

        private void Delete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (row.IsNewRow != true)
                {
                    dataGridView1.Rows.RemoveAt(row.Index);
                }                               
            }
        }
    }
}
