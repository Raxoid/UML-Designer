using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.ClassDiagram;

namespace WindowsFormsApp1.Arrows
{
    public class ConnectionArrow
    {
        public ConnectionArrow(Diagram startdiagram,Diagram enddiagram)
        {
            startDiagram = startdiagram;
            endDiagram = enddiagram;
        }
        public Diagram startDiagram { get; set; }
        public Diagram endDiagram { get; set; }
    }
}
