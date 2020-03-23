using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.ClassDiagram
{
    public class Method 
    {
        public string Name { get; set; }
        public string AccessModifier { get; set; }
        public string DataType { get; set; }
        public List<Parameter> Parameters { get; set; }
    }
}
