using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRevitFinal.Model1
{
    public class GridsLI
    {

        #region Properties

        public string NameofGrids { get; set; }


        public BuiltInCategory BuiltInCategory { get; set; }


        #endregion

        #region Methods
        public override string ToString()
        {
            return NameofGrids;
        }


        #endregion

    }
}
