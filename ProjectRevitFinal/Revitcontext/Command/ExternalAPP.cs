using Autodesk.Revit.UI;
using System.Reflection;

namespace ProjectRevitFinal.Revitcontext.Command
{
    public class ExternalAPP : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab("AEC Project");

            RibbonPanel Panel = application.CreateRibbonPanel("AEC Project", "Draw elements");


            string path = Assembly.GetExecutingAssembly().Location;

            PushButtonData p1 = new PushButtonData("BTN1", "Drawcolumns", path, "ProjectRevitFinal.Revitcontext.Command.OpenWindowCommand");

            // sh3'l el sora bta3t el ribbon
            //Uri uriImage = new Uri("pack://application:,,,/ProjectRevitFinal.Revitcontext;component/Styles1/Aec.png");
            //BitmapImage image = new BitmapImage(uriImage);
            //  p1.Image = image;


            Panel.AddItem(p1);

            return Result.Succeeded;
        }
    }
}
