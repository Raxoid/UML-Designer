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

    public partial class Form1_Method_Edit : Form
    {
        public Method method { get; set; } = new Method();

        public MethodDataModel modelMethod = new MethodDataModel();

        public ParameterDataModel modelParameter = new ParameterDataModel();
        public Diagram diagram { get; set; }
        private string currentMethodName { get; set; }
        public Form1_Method_Edit(Method _method, Diagram _diagram)
        {
            InitializeComponent();
            method = _method;
            diagram = _diagram;
            dataGridView1.DataSource = modelMethod;
            dataGridView1.ReadOnly = true;
            dataGridView2.DataSource = modelParameter;
            dataGridView2.ReadOnly = true;
            if (diagram.methods != null)
            {
                foreach (var method in diagram.methods)
                {                                    
                    foreach (var parameter in modelParameter.parameters)
                    {
                        if (method.Name == parameter.MethodName)
                        {
                            method.Parameters.Add(parameter);
                        }
                    }
                    modelMethod.methods.Add(method);
                }
            }
            comboBox1.Items.Add("public");
            comboBox1.Items.Add("private");
            comboBox2.Items.Add("string");
            comboBox2.Items.Add("int");
            comboBox2.Items.Add("float");
            comboBox2.Items.Add("bool");
            comboBox2.Items.Add("char");
            comboBox2.Items.Add("void");
            comboBox2.Items.Add("custom");
            comboBox3.Items.Add("string");
            comboBox3.Items.Add("int");
            comboBox3.Items.Add("float");
            comboBox3.Items.Add("bool");
            comboBox3.Items.Add("char");
            comboBox3.Items.Add("void");
            comboBox3.Items.Add("custom");
            if (diagram.className.IsInterface == true)
            {
               comboBox1.Enabled = false;
            }        
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            textBox2.Visible = false;
            textBox3.Visible = false;
        }

        private void Confirm_Click(object sender, EventArgs e)
        {           
            if (diagram.methods != null)
            {
                foreach (var method in diagram.methods)
                {
                    foreach (var parameter in modelParameter.parameters)
                    {
                        if (method.Name == parameter.MethodName)
                        {
                            method.Parameters.Add(parameter);
                        }
                    }
                }
            }
            diagram.methods = modelMethod.methods.ToList();
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            Method method = new Method();
            method.Name = textBox1.Text;
            method.AccessModifier = comboBox1.Text;
            if (comboBox2.Text != "custom")
            {
                method.DataType = comboBox2.Text;
            }
            if (comboBox2.Text == "custom")
            {
                method.DataType = textBox2.Text;
            }
            if (method.Name !="" && method.AccessModifier !="" && method.DataType !="")
            {
                modelMethod.methods.Add(method);
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
                if (dataGridView1.Rows.Count > 0 && row.IsNewRow == false)
                {
                    dataGridView1.Rows.RemoveAt(row.Index);
                }               
            }
        }

        private void Deleteparameter_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                if (dataGridView2.Rows.Count > 0 && row.IsNewRow == false)
                {
                    dataGridView2.Rows.RemoveAt(row.Index);
                }             
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //currentMethodName = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }

        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text == "custom")
            {
                textBox3.Visible = true;
            }
            else if (comboBox3.Text != "custom")
            {
                textBox3.Visible = false;
            }
        }

        private void AddParameter_Click(object sender, EventArgs e)
        {
            Parameter parameter = new Parameter();
            parameter.Name = textBox4.Text;
           // parameter.MethodName = currentMethodName;

            if (comboBox3.Text !="custom")
            {
                parameter.DataType = comboBox3.Text;
            }
            if (comboBox3.Text =="custom")
            {
                parameter.DataType = textBox3.Text;
            }
            if (parameter.Name !="" && parameter.DataType !="" && parameter.MethodName !="")
            {
                modelParameter.parameters.Add(parameter);
            }
            textBox4.Text = null;
            comboBox3.Text = null;
            currentMethodName = "";
        }

        private void DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {          
        }
    }
}
