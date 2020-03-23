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
    public class ParameterDataModel : IListSource
    {
        public BindingList<Parameter> parameters = new BindingList<Parameter>();
        public bool ContainsListCollection => true;

        public IList GetList()
        {
            return parameters;
        }
    }
}
