using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using WindowsFormsApp1.Arrows;
using WindowsFormsApp1.ClassDiagram;
using WindowsFormsApp1.DiagramConstruction;
using WindowsFormsApp1.Form1Edits;

namespace WindowsFormsApp1
{
    public class ActionHandler
    {
        public Bitmap SideArea;
        public string Path { get; set; }
        /* Mouse */
        public Point _previousPoisition;
        public Point arrowStartPoint;
        public Point arrowEndPoint;
        public bool isMouseDown = false;
        public Diagram savedDiagram;
        public Diagram arrStartDiagram;
        public Diagram arrEndDiagram;
        public List<Diagram> diagrams = new List<Diagram>();
        public bool propertyEmpty = true;
        public bool methodEmpty = true;
        /*Arrows*/
        private bool isArrDrawing = false;
        private int arrClickCount = 0;
        public List<ConnectionArrow> connectionarrows = new List<ConnectionArrow>();
        /*Panel/Forms*/
        public bool formOpen = false;


        private readonly PictureBox _pictureBox;
        public Form1 Form;
        public Renderer Renderer { get; set; }
        public ActionHandler(PictureBox pictureBox, Form1 form)
        {
            _pictureBox = pictureBox;
            Form = form;
            Renderer = new Renderer();
            _pictureBox.MouseDown += PictureBoxMouseDown;
            _pictureBox.MouseMove += PictureBoxMouseMove;
            _pictureBox.MouseUp += PictureBoxMouseUp;
            _pictureBox.MouseDoubleClick += PictureBoxMouseDoubleClick;
            _pictureBox.DragDrop += PictureBoxDragDrop;
            _pictureBox.DragEnter += PictureBoxDragEnter;
            _pictureBox.Paint += HandlePaint;
            _pictureBox.Paint += HandleArrowPaint;
            Form.pictureBox2.Paint += PictureBox2_Paint;
            Form.pictureBox2.MouseMove += PictureBox2_MouseMove;
            Form.pictureBox3.Paint += PictureBox3_Paint;
            Form.pictureBox3.Click += PictureBox3_Click;
            Form.Clear.Click += Clear_Click;
            Form.Generate.Click += Generate_Click;
            Form.SaveXML.Click += SaveXML_Click;
            Form.ClearXML.Click += ClearXML_Click;
            Form.SavePNG.Click += SavePNG_Click;
            Form.Exit.Click += Exit_Click;
            Form.deleteToolStripMenuItem.Click += DeleteToolStripMenuItem_Click;
            Form.deleteArrowsToolStripMenuItem.Click += DeleteArrowsToolStripMenuItem_Click;
            Form.button1.Click += Button1_Click;

            SideArea = new Bitmap(Form.pictureBox2.Size.Width, Form.pictureBox2.Size.Height);
            Form.pictureBox2.Image = SideArea;
        }
        private void HandlePaint(object sender, PaintEventArgs e)
        {
            Renderer.Paint(e.Graphics, diagrams, savedDiagram);
        }
        private void HandleArrowPaint(object sender, PaintEventArgs e)
        {
            Renderer.DrawArrows(e.Graphics, connectionarrows);
        }
        private void PictureBox2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            g = Graphics.FromImage(SideArea);
            g.Clear(Color.Black);
            Pen mypen = new Pen(Color.White);
            Font font = new Font("Comic Sans MS", 7);
            Rectangle nameRectangle = new Rectangle(0, 0, 150, 30);
            Rectangle rectangle = new Rectangle(nameRectangle.X, nameRectangle.Y + nameRectangle.Height, nameRectangle.Width, 70);
            Rectangle methodRectangle = new Rectangle(nameRectangle.X, rectangle.Y + rectangle.Height, nameRectangle.Width, rectangle.Height);
            g.DrawRectangle(mypen, nameRectangle);
            g.DrawRectangle(mypen, rectangle);
            g.DrawRectangle(mypen, methodRectangle);
            Form.pictureBox2.Image = SideArea;
        }
        private void PictureBox3_Paint(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(Brushes.White, 4f))
            {

                if (isArrDrawing == false)
                {
                    p.EndCap = LineCap.ArrowAnchor;
                    e.Graphics.DrawLine(p, 0, Form.pictureBox3.Height, Form.pictureBox3.Width, 0);
                }

                if (isArrDrawing == true)
                {
                    Pen pen = new Pen(Brushes.DeepSkyBlue, 4f);
                    pen.EndCap = LineCap.ArrowAnchor;
                    e.Graphics.DrawLine(pen, 0, Form.pictureBox3.Height, Form.pictureBox3.Width, 0);
                }
            }
        }
        private void PictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Form.pictureBox2.DoDragDrop(Form.pictureBox2.Image, DragDropEffects.All);
            }
        }
        private void PictureBox3_Click(object sender, EventArgs e)
        {
            isArrDrawing = true;
            Form.pictureBox3.Refresh();
        }

        private void PictureBoxMouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                var diagram = diagrams.Where(
                item => e.X > item.rectangle.X && e.X < item.rectangle.X + item.rectangle.Width &&
                e.Y > item.rectangle.Y && e.Y < item.rectangle.Y + item.rectangle.Height)
                .First();
                savedDiagram = diagram;
            }
            catch (Exception)
            {

            }
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
            }

            if (isArrDrawing == true && e.Button == MouseButtons.Left)
            {
                foreach (var item in diagrams)
                {
                    if (e.X > item.rectangle.X && e.X < item.rectangle.X + item.rectangle.Width &&
                        e.Y > item.rectangle.Y && e.Y < item.rectangle.Y + item.rectangle.Height)
                    {
                        if (arrClickCount == 0)
                        {
                            var arrstartdiagram = diagrams.Where(
                            startdiagram => e.X > startdiagram.rectangle.X && e.X < startdiagram.rectangle.X + startdiagram.rectangle.Width &&
                            e.Y > startdiagram.rectangle.Y && e.Y < startdiagram.rectangle.Y + startdiagram.rectangle.Height)
                            .First();
                            arrStartDiagram = arrstartdiagram;                          
                            arrClickCount += 1;
                        }
                        else if (arrClickCount == 1)
                        {
                            var arrenddiagram = diagrams.Where(
                            enddiagram => e.X > enddiagram.rectangle.X && e.X < enddiagram.rectangle.X + enddiagram.rectangle.Width &&
                            e.Y > enddiagram.rectangle.Y && e.Y < enddiagram.rectangle.Y + enddiagram.rectangle.Height)
                            .First();
                            arrEndDiagram = arrenddiagram;
                            isArrDrawing = false;
                            arrClickCount = 0;
                            if (arrStartDiagram != arrEndDiagram)
                            {

                                connectionarrows.Add(new ConnectionArrow(arrStartDiagram,arrEndDiagram));
                                arrStartDiagram.connectedDiagramClassName = arrEndDiagram.className;
                            }                         
                        }
                        _pictureBox.Refresh();
                        Form.pictureBox3.Refresh();
                        //manhattan distance
                    }
                }
            }
        }
        
        private void PictureBoxMouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            _pictureBox.Refresh();
        }
        private void PictureBoxMouseMove(object sender, MouseEventArgs e)
        {
            if (savedDiagram != null && isMouseDown == true && isArrDrawing == false)
            {
                if (e.X > savedDiagram.rectangle.X && e.X < savedDiagram.rectangle.X + savedDiagram.rectangle.Width &&
                        e.Y > savedDiagram.rectangle.Y && e.Y < savedDiagram.rectangle.Y + savedDiagram.rectangle.Height)
                {
                    var delta = new Point(e.X - _previousPoisition.X, e.Y - _previousPoisition.Y);

                    savedDiagram.Position = new Point(savedDiagram.rectangle.X + delta.X, savedDiagram.rectangle.Y + delta.Y);
                  
                    _pictureBox.Refresh();
                }             
            }
            _previousPoisition = e.Location;
        }

        private void PictureBoxDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }
        private void PictureBoxDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                diagrams.Add(new Diagram(new RectangleElement(0, 0, 200, 200)) { className = new ClassName() { Name = "New Class"}});
                _pictureBox.Refresh();
            }
        }

        private void PictureBoxMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (savedDiagram != null)
            {
                if (e.X > savedDiagram.rectangleHitboxes[0].X && e.X < savedDiagram.rectangleHitboxes[0].X + savedDiagram.rectangleHitboxes[0].Width)
                {
                    if (e.Y > savedDiagram.rectangleHitboxes[0].Y && e.Y < savedDiagram.rectangleHitboxes[0].Y + savedDiagram.rectangleHitboxes[0].Height)
                    {
                        if (formOpen == true && Form.panel1.Controls.Count > 0)
                        {
                            Form.panel1.Controls.RemoveAt(0);
                            Form.panel1.Refresh();
                        }
                        Form1_Class_Edit _Class_Edit = new Form1_Class_Edit(new ClassName(), savedDiagram);
                        _Class_Edit.TopLevel = false;
                        _Class_Edit.AutoScroll = true;
                        Form.panel1.Controls.Add(_Class_Edit);
                        _Class_Edit.Show();
                        Form.panel1.Refresh();
                        formOpen = true;
                        _Class_Edit.Closed += (sener, eventArgs) =>  _pictureBox.Refresh();
                    }
                }

                if (e.X > savedDiagram.rectangleHitboxes[1].X && e.X < savedDiagram.rectangleHitboxes[1].X + savedDiagram.rectangleHitboxes[1].Width)
                {
                    if (e.Y > savedDiagram.rectangleHitboxes[1].Y && e.Y < savedDiagram.rectangleHitboxes[1].Y + savedDiagram.rectangleHitboxes[1].Height)
                    {
                        if (formOpen == true && Form.panel1.Controls.Count > 0)
                        {
                            Form.panel1.Controls.RemoveAt(0);
                            Form.panel1.Refresh();
                        }
                        Form1_Property_Edit _Property_Edit = new Form1_Property_Edit(new Property(), savedDiagram);
                        _Property_Edit.TopLevel = false;
                        _Property_Edit.AutoScroll = true;
                        Form.panel1.Controls.Add(_Property_Edit);
                        _Property_Edit.Show();
                        Form.panel1.Refresh();
                        formOpen = true;
                        _Property_Edit.Closed += (sener, eventArgs) => _pictureBox.Refresh();
                    }
                }

                if (e.X > savedDiagram.rectangleHitboxes[2].X && e.X < savedDiagram.rectangleHitboxes[2].X + savedDiagram.rectangleHitboxes[2].Width)
                {
                    if (e.Y > savedDiagram.rectangleHitboxes[2].Y && e.Y < savedDiagram.rectangleHitboxes[2].Y + savedDiagram.rectangleHitboxes[2].Height)
                    {
                        if (formOpen == true && Form.panel1.Controls.Count > 0)
                        {
                            Form.panel1.Controls.RemoveAt(0);
                            Form.panel1.Refresh();
                        }
                        Form1_Method_Edit _Method_Edit = new Form1_Method_Edit(new Method(), savedDiagram);
                        _Method_Edit.TopLevel = false;
                        _Method_Edit.AutoScroll = true;
                        Form.panel1.Controls.Add(_Method_Edit);
                        _Method_Edit.Show();
                        Form.panel1.Refresh();
                        formOpen = true;
                        _Method_Edit.Closed += (sener, eventArgs) => _pictureBox.Refresh();
                    }
                }
            }
        }


        private void DiagramSerialize()
        {
            /*
            try
            {
                XmlSerializer diagramserializer = new XmlSerializer(diagrams.GetType());
                using (StreamWriter sw = new StreamWriter("diagrams.xml"))
                {
                    diagramserializer.Serialize(sw, diagrams);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/
        }
        private void ArrowSerialize()
        {
            /*
            try
            {
                XmlSerializer arrowserializer = new XmlSerializer(connectionarrows.GetType());
                using (StreamWriter sw = new StreamWriter("arrows.xml"))
                {
                    arrowserializer.Serialize(sw, connectionarrows);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/
        }

        public void DiagramDeserialize()
        {
            /*
            try
            {
                if (File.Exists("diagrams.xml"))
                {
                    XmlSerializer diagramserializer = new XmlSerializer(diagrams.GetType());
                    using (StreamReader sr = new StreamReader("diagrams.xml"))
                    {
                        diagrams = (List<Diagram>)diagramserializer.Deserialize(sr);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/
        }
        public void GenerateCode(string Path)
        {
            CCodeGenerator cg = new CCodeGenerator();
            
            foreach (var diagram in diagrams)
            {
                cg.CreateNamespace();
                cg.CreateClass(diagram);
                cg.CreateProperty(diagram);
                cg.CreateMethod(diagram);
                cg.SaveAssembly(diagram,Path);
            }          
        }
        public void ArrowDeserialize()
        {
            /*
            try
            {
                if (File.Exists("arrows.xml"))
                {
                    XmlSerializer arrowserializer = new XmlSerializer(connectionarrows.GetType());
                    using (StreamReader sr = new StreamReader("arrows.xml"))
                    {
                        connectionarrows = (List<ConnectionArrow>)arrowserializer.Deserialize(sr);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/
        }
        private void SaveXML_Click(object sender, EventArgs e)
        { 
            /*
            DiagramSerialize();
            ArrowSerialize();
            */
        }
        private void SavePNG_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.FileName = "diagram";
            saveDialog.Filter = "Images|*.png;*.bmp;*.jpg";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                using (var bitMap = new Bitmap(Form.pictureBox1.Width, Form.pictureBox1.Height))
                {
                    Form.pictureBox1.DrawToBitmap(bitMap, Form.pictureBox1.ClientRectangle);
                    bitMap.Save( saveDialog.FileName, ImageFormat.Png);
                }
            }
        }
        private void ClearXML_Click(object sender, EventArgs e)
        {
            /*
            if (File.Exists("diagrams.xml"))
            {
                File.Delete("diagrams.xml");
            }
            if (File.Exists("arrows.xml"))
            {
                File.Delete("arrows.xml");
            }*/
        }
        private void Clear_Click(object sender, EventArgs e)
        {
            diagrams.Clear();
            connectionarrows.Clear();
            savedDiagram = null;
            Form.pictureBox1.Refresh();
        }
        private void Generate_Click(object sender, EventArgs e)
        {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "diagrams";
                dialog.Filter = "C Sharp Files (*.cs)|*.cs";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Path = System.IO.Path.GetDirectoryName(dialog.FileName);
                    GenerateCode(Path);
                }
        }
        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < connectionarrows.Count; i++)
            {
                if (connectionarrows[i].startDiagram == savedDiagram || connectionarrows[i].endDiagram == savedDiagram)
                {
                    connectionarrows.Remove(connectionarrows[i]);
                }
            }
            diagrams.Remove(savedDiagram);
            savedDiagram = null;
            _pictureBox.Refresh();
        }
        private void DeleteArrowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connectionarrows.Clear();
            _pictureBox.Refresh();
        }
        private void Exit_Click(object sender, EventArgs e)
        {
            Form.Close();
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Help_Form help = new Help_Form();
            help.ShowDialog();
        }
    }
}
