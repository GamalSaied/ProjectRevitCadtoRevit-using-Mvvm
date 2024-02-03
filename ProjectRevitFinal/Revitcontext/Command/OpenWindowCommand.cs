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

            try
            {
                // Get the Revit application and document
                var uiApp = commandData.Application;
                var uiDoc = uiApp.ActiveUIDocument;
                var doc = uiDoc.Document;

                var levelController = new LevelApiController(doc);
                System.Collections.Generic.List<Domain.LevelModel> levelDataList = levelController.GetAll();

                // Create the event handlers and ExternalEvent instances within the API context.
                CreateLevelEventHandler createLevelEventHandler = new CreateLevelEventHandler();
                DeleteLevelEventHandler deleteLevelEventHandler = new DeleteLevelEventHandler();
                ExternalEvent createLevelExternalEvent = ExternalEvent.Create(createLevelEventHandler);
                ExternalEvent deleteLevelExternalEvent = ExternalEvent.Create(deleteLevelEventHandler);

                // Open the MainWPF window and pass the ExternalEvent and handler instances.
                MainWPF mainWPF = new MainWPF(levelDataList, createLevelExternalEvent, deleteLevelExternalEvent,
                                              createLevelEventHandler, deleteLevelEventHandler);
                mainWPF.Show();

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










