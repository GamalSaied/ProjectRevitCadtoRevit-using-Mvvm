using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProjectRevitFinal.Domain;

namespace ProjectRevitFinal.Api.EventHandlers
{
    public class CreateLevelEventHandler : IExternalEventHandler
    {
        public LevelModel Input { get; set; }
        private LevelApiController LevelApiController { get; set; }

        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;

            LevelApiController = new LevelApiController(doc);

            LevelApiController.Create(Input);
        }

        public string GetName()
        {
            return "CreateLevelEventHandler";
        }

    }

}
