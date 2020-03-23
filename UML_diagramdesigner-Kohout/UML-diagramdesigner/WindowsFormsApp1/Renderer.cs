using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Arrows;
using WindowsFormsApp1.ClassDiagram;
using WindowsFormsApp1.DiagramConstruction;

namespace WindowsFormsApp1
{
    public class Renderer
    {

        public void Paint(Graphics graphics,List<Diagram> diagrams,Diagram savedDiagram)
        {
            Graphics g = graphics;
            g.Clear(Color.Gray);
            Pen mypen = new Pen(Color.White);
            Pen arrowpen = new Pen(Color.White, 4f);
            arrowpen.EndCap = LineCap.ArrowAnchor;
            Pen selectedPen = new Pen(Color.DeepSkyBlue);
            SolidBrush brush = new SolidBrush(Color.White);
            Font font = new Font("Comic Sans MS", 8);
            if (diagrams != null)
            {
                foreach (var diagram in diagrams)
                {
                    int propertyOffset = 0;
                    int methodOffset = 0;
                    g.DrawRectangle(mypen, diagram.rectangle.X, diagram.rectangle.Y, diagram.rectangle.Width, diagram.rectangle.Height);
                    foreach (var seperatorline in diagram.seperatorLines)
                    {
                        g.DrawLine(mypen, seperatorline.startPoint, seperatorline.endPoint);
                    }
                    if (diagram.className != null)
                    {
                        if (diagram.className.Name.Length * 9 > diagram.rectangle.Width)
                        {                      
                            diagram.seperatorLines[0].endPoint = new Point(diagram.seperatorLines[0].endPoint.X + diagram.rectangle.Width, diagram.seperatorLines[0].endPoint.Y);
                            diagram.seperatorLines[1].endPoint = new Point(diagram.seperatorLines[1].endPoint.X + diagram.rectangle.Width, diagram.seperatorLines[1].endPoint.Y);
                            diagram.rectangle.Width = diagram.rectangle.Width * 2;
                            foreach (var hitbox in diagram.rectangleHitboxes)
                            {
                                hitbox.Width = hitbox.Width * 2;
                            }
                        }
                        if (diagram.className.IsInterface == true)
                        {
                            g.DrawString("Interface", font, brush, diagram.rectangle.X + 5, diagram.rectangle.Y + 5);
                        }
                        g.DrawString(diagram.className.Name, font, brush, diagram.rectangle.X + (diagram.rectangle.Width / 2) - 20, diagram.rectangle.Y + 5);
                    }
                    if (diagram.properties == null)
                    {
                        g.DrawString("Double click here to add properties", font, brush, diagram.rectangle.X + 5, diagram.seperatorLines[0].startPoint.Y + 5 + propertyOffset);
                    }
                    if (diagram.properties != null)
                    {
                        if (!diagram.properties.Any())
                        {
                            g.DrawString("Double click here to add properties", font, brush, diagram.rectangle.X + 5, diagram.seperatorLines[0].startPoint.Y + 5 + propertyOffset);
                        }
                        foreach (var item in diagram.properties)
                        {                           
                            if ((item.Name.Length + item.DataType.Length + item.AccessModifier.Length) * 5 > diagram.rectangle.Width)
                            {                               
                                diagram.seperatorLines[0].endPoint = new Point(diagram.seperatorLines[0].endPoint.X + diagram.rectangle.Width, diagram.seperatorLines[0].endPoint.Y);
                                diagram.seperatorLines[1].endPoint = new Point(diagram.seperatorLines[1].endPoint.X + diagram.rectangle.Width, diagram.seperatorLines[1].endPoint.Y);
                                diagram.rectangle.Width = diagram.rectangle.Width * 2;
                                foreach (var hitbox in diagram.rectangleHitboxes)
                                {
                                    hitbox.Width = hitbox.Width * 2;
                                }
                            }
                            if (propertyOffset * 6 > diagram.rectangle.Height)
                            {                          
                                diagram.seperatorLines[1].startPoint = new Point(diagram.seperatorLines[1].startPoint.X, diagram.seperatorLines[1].startPoint.Y + diagram.rectangle.Height);
                                diagram.seperatorLines[1].endPoint = new Point(diagram.seperatorLines[1].endPoint.X, diagram.seperatorLines[1].endPoint.Y + diagram.rectangle.Height);
                                diagram.rectangleHitboxes[1].Height = diagram.rectangleHitboxes[1].Height * 2 + diagram.rectangleHitboxes[0].Height;
                                diagram.rectangleHitboxes[2].Y = diagram.rectangleHitboxes[2].Y + diagram.rectangleHitboxes[1].Height;
                                diagram.rectangle.Height = diagram.rectangle.Height * 2;
                            }
                            g.DrawString(item.Name, font, brush, diagram.rectangle.X + 80 + (item.DataType.Length * 2), diagram.seperatorLines[0].startPoint.Y + 5 + propertyOffset);
                            g.DrawString(item.DataType, font, brush, diagram.rectangle.X + 45, diagram.seperatorLines[0].startPoint.Y + 5 + propertyOffset);
                            if (diagram.className.IsInterface == false)
                            {
                                g.DrawString(item.AccessModifier, font, brush, diagram.rectangle.X + 5, diagram.seperatorLines[0].startPoint.Y + 5 + propertyOffset);
                            }
                            propertyOffset += 10;
                        }
                    }
                    if (diagram.methods == null)
                    {
                        g.DrawString("Double click here to add methods", font, brush, diagram.rectangle.X + 5, diagram.seperatorLines[1].startPoint.Y + 5);
                    }
                    if (diagram.methods != null)
                    {
                        if (!diagram.methods.Any())
                        {
                            g.DrawString("Double click here to add methods", font, brush, diagram.rectangle.X + 5, diagram.seperatorLines[1].startPoint.Y + 5);
                        }
                        foreach (var item in diagram.methods)
                        {
                            if ((item.Name.Length + item.DataType.Length + item.AccessModifier.Length) * 5 > diagram.rectangle.Width)
                            {
                                diagram.seperatorLines[0].endPoint = new Point(diagram.seperatorLines[0].endPoint.X + diagram.rectangle.Width, diagram.seperatorLines[0].endPoint.Y);
                                diagram.seperatorLines[1].endPoint = new Point(diagram.seperatorLines[1].endPoint.X + diagram.rectangle.Width, diagram.seperatorLines[1].endPoint.Y);
                                diagram.rectangle.Width = diagram.rectangle.Width * 2;
                                foreach (var hitbox in diagram.rectangleHitboxes)
                                {
                                    hitbox.Width = hitbox.Width * 2;
                                }
                            }
                            if (methodOffset * 5 > diagram.rectangle.Height)
                            {
                                diagram.rectangle.Height = diagram.rectangle.Height + diagram.rectangleHitboxes[2].Height;
                                diagram.rectangleHitboxes[2].Height = diagram.rectangleHitboxes[2].Height * 2;                           
                            }
                            g.DrawString(item.Name, font, brush, diagram.rectangle.X + 45, diagram.seperatorLines[1].startPoint.Y + 5 + methodOffset);
                            g.DrawString(": " + item.DataType, font, brush, diagram.rectangle.X + 45 + item.Name.Length * 8, diagram.seperatorLines[1].startPoint.Y + 5 + methodOffset);
                            if (diagram.className.IsInterface == false)
                            {
                                g.DrawString(item.AccessModifier, font, brush, diagram.rectangle.X + 5, diagram.seperatorLines[1].startPoint.Y + 5 + methodOffset);
                            }
                            g.DrawString("( )", font, brush, diagram.rectangle.X + 45 + item.Name.Length * 6, diagram.seperatorLines[1].startPoint.Y + 5 + methodOffset);
                            if (item.Parameters != null)
                            {
                                foreach (var parameter in item.Parameters)
                                {
                                    g.DrawString(parameter.Name, font, brush, diagram.rectangle.X + 60 + item.Name.Length * 6, diagram.seperatorLines[1].startPoint.Y + 5 + methodOffset);
                                }
                            }                       
                            methodOffset += 10;
                        }
                    }
                }
            }          
            if (savedDiagram != null)
            {
                g.DrawRectangle(selectedPen, savedDiagram.rectangle.X, savedDiagram.rectangle.Y, savedDiagram.rectangle.Width, savedDiagram.rectangle.Height);
                foreach (var seperatorline in savedDiagram.seperatorLines)
                {
                    g.DrawLine(selectedPen, seperatorline.startPoint, seperatorline.endPoint);
                }
            }
        }
        public void DrawArrows(Graphics graphics,List<ConnectionArrow> connectionArrows)
        {
            Pen p = new Pen(Brushes.White, 4f);
            p.EndCap = LineCap.ArrowAnchor;
            foreach (var arrow in connectionArrows)
            {
                graphics.DrawLine(p, new Point(arrow.startDiagram.rectangle.X + (arrow.startDiagram.rectangle.Width/2),arrow.startDiagram.rectangle.Y + (arrow.startDiagram.rectangle.Height / 2)), new Point(arrow.endDiagram.rectangle.X + (arrow.endDiagram.rectangle.Width / 2), arrow.endDiagram.rectangle.Y + (arrow.endDiagram.rectangle.Height / 2)));

            }
        }
    }
}
