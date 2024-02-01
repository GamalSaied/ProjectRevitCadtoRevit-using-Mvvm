using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProjectRevitFinal.View;
using System;

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
                MainWPF mainui = new MainWPF();
                mainui.ShowDialog();




                return Result.Succeeded;
            }
            catch (Exception ex)
            {

                message = ex.Message;
                return Result.Failed;
            }

        }
    }
}


