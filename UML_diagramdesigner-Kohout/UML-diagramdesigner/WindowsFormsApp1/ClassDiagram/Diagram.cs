using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Arrows;
using WindowsFormsApp1.DiagramConstruction;


namespace WindowsFormsApp1.ClassDiagram
{
    public class Diagram
    {
        public Diagram(RectangleElement _rectangle)
        {
            rectangle = _rectangle;
            List<RectangleHitbox> _rectangleHitboxes = new List<RectangleHitbox>();
            List<SeperatorLine> _seperatorLines = new List<SeperatorLine>();
            _seperatorLines.Add(new SeperatorLine(new Point(rectangle.X, rectangle.Y + 30), new Point(rectangle.X + rectangle.Width, rectangle.Y + 30)));
            _seperatorLines.Add(new SeperatorLine(new Point(rectangle.X, rectangle.Y + 125), new Point(rectangle.X + rectangle.Width, rectangle.Y + 125)));
            _rectangleHitboxes.Add(new RectangleHitbox(rectangle.X, rectangle.Y, 30, rectangle.Width));
            _rectangleHitboxes.Add(new RectangleHitbox(rectangle.X, rectangle.Y + 30, 95, rectangle.Width));
            _rectangleHitboxes.Add(new RectangleHitbox(rectangle.X, rectangle.Y + 125, 75, rectangle.Width));
            rectangleHitboxes = _rectangleHitboxes;
            seperatorLines = _seperatorLines;

        }
        public ClassName className { get; set; }
        public List<Method> methods { get; set; }
        public List<Property> properties { get; set; }
        public RectangleElement rectangle { get; set; }
        public List<SeperatorLine> seperatorLines { get; set; }
        public List<RectangleHitbox> rectangleHitboxes { get; set; }
        public ClassName connectedDiagramClassName { get; set; }

        private Point position;
        public Point Position
        {
            get
            {
                return position;
            }
            set
            {
                var prevPos = position;
                position = value;
                rectangle.X = position.X;
                rectangle.Y = position.Y;             

                var delta = new Point(value.X - prevPos.X, value.Y - prevPos.Y);
                foreach (var seperatorLine in seperatorLines)
                {
                    seperatorLine.startPoint = new Point(delta.X + seperatorLine.startPoint.X, seperatorLine.startPoint.Y + delta.Y);
                    seperatorLine.endPoint = new Point(seperatorLine.endPoint.X + delta.X, seperatorLine.endPoint.Y + delta.Y);
                }
                foreach (var hitbox in rectangleHitboxes)
                {
                    hitbox.X = position.X;
                    hitbox.Y += delta.Y;
                }
            }
        }
    }
}
