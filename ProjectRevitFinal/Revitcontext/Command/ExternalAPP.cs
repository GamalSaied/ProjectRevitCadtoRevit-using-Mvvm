using Autodesk.Revit.UI;
using System;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.IO;
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

            // Load the image from resources
            //var uriImage = new Uri("pack://application:,,,/ProjectRevitFinal.Revitcontext.Command;component/Styles1/Aec.png");
            //var image = new BitmapImage(uriImage);

            //// Set the image for the push button
            //p1.LargeImage = image;

            ////Add the push button to the Ribbon panel
            Panel.AddItem(p1);

            return Result.Succeeded;
        }
    }
}

//if (Panel.AddItem(new PushButtonData("BTN1", "Drawcolumns", assemblypath, "ProjectRevitFinal.Revitcontext.Command.OpenWindowCommand")) is PushButtoz)
//{
//    Button.ToolTip = "Drawcolumns";
//    // Load the image from resources
//    var uriImage = new Uri(Path.Combine(Path.GetDirectoryName(assemblypath), "Resources", "assemblypath"));
//    var image = new BitmapImage(uriImage);

//    // Set the image for the push button
//    p1.LargeImage = image;

//    //Add the push button to the Ribbon panel