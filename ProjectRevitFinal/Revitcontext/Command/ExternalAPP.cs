using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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

            //Uri uriImage = new Uri("pack://application:,,,/ProjectRevitFinal.Revitcontext;component/Styles1/Aec.png");
            //BitmapImage image = new BitmapImage(uriImage);
            //  p1.LargeImage = image;
  
           
            Panel.AddItem(p1);

            return Result.Succeeded;
        }
    }
}
