using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProjectRevitFinal.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRevitFinal.Revitcontext.Command
{
    [Transaction(TransactionMode.Manual)]
    public class OpenWindowCommand : IExternalCommand
    {
        #region Properties
        public static Document doc { get; set; }
        #endregion


     public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
              doc = uidoc.Document;
            try
            {
                MainUi mainui= new MainUi();
                mainui.ShowDialog();




                return Result.Succeeded;
            }
            catch (Exception ex)
            {

                message=ex.Message;
                return Result.Failed;
            }
          
        }
    }
}
