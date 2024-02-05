using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProjectRevitFinal.Api;
using ProjectRevitFinal.Api.EventHandlers;
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
        public static UIApplication UiApp;
        public static UIDocument UiDoc;
        public static Document Doc;

        #endregion


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            doc = uidoc.Document;
            try
            {
                // Get the Revit application and document
                var uiApp = commandData.Application;
                var uiDoc = uiApp.ActiveUIDocument;
                var Doc = uiDoc.Document;

                var levelController = new LevelApiController(Doc);
                System.Collections.Generic.List<Domain.LevelModel> levelDataList = levelController.GetAll();

                // Create the event handlers and ExternalEvent instances within the API context.
                CreateLevelEventHandler createLevelEventHandler = new CreateLevelEventHandler();
                DeleteLevelEventHandler deleteLevelEventHandler = new DeleteLevelEventHandler();
                ExternalEvent createLevelExternalEvent = ExternalEvent.Create(createLevelEventHandler);
                ExternalEvent deleteLevelExternalEvent = ExternalEvent.Create(deleteLevelEventHandler);

                // Open the MainWPF window and pass the ExternalEvent and handler instances.
                MainWPF mainui = new MainWPF(levelDataList, createLevelExternalEvent, deleteLevelExternalEvent,
                                              createLevelEventHandler, deleteLevelEventHandler);
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










