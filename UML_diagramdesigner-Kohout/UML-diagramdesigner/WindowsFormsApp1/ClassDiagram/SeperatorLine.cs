using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.ClassDiagram
{
    public class SeperatorLine
    {
        public SeperatorLine(Point _startPoint,Point _endPoint)
        {
            startPoint = _startPoint;
            endPoint = _endPoint;
        }
        public Point startPoint { get; set; }

        public Point endPoint { get; set; }      

    }
}
