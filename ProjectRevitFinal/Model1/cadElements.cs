using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRevitFinal.Model1
{
    public class cadElements
    {
        #region Properties
        public string cadElementsName { get; set; }
        public string Layername { get; set; }


        public BuiltInCategory BuiltInCategory { get; set; }


        #endregion

        #region Methods
        public override string ToString()
        {
            return cadElementsName;
        }
        #endregion

    }
}
