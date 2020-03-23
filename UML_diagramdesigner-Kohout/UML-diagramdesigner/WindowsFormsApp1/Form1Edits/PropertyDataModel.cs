using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.ClassDiagram;

namespace WindowsFormsApp1.Form1Edits
{
    public class PropertyDataModel : IListSource
    {
        public BindingList<Property> properties = new BindingList<Property>();
        public bool ContainsListCollection => true;

        public IList GetList()
        {
            return properties;
        }
    }
}
