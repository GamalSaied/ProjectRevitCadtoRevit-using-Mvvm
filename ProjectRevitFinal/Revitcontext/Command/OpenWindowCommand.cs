using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProjectRevitFinal.Api.EventHandlers;
using ProjectRevitFinal.Api;
using ProjectRevitFinal.View;
using ProjectRevitFinalApp.Windows;
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
            // Get the Revit application and document
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var doc = uiDoc.Document;
            try
            {
                var levelController = new LevelApiController(doc);
                var levelDataList = levelController.GetAll();

                var createLevelEventHandler = new CreateLevelEventHandler();
                var deleteLevelEventHandler = new DeleteLevelEventHandler();
                var createLevelEvent = ExternalEvent.Create(createLevelEventHandler);
                var deleteLevelEvent = ExternalEvent.Create(deleteLevelEventHandler);


                // Create an instance of MainWindow from the class library
                Levels _levels = new Levels(levelDataList, createLevelEventHandler, deleteLevelEventHandler, createLevelEvent, deleteLevelEvent);
        
                MainWPF mainui = new MainWPF();
                mainui.Show();




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










