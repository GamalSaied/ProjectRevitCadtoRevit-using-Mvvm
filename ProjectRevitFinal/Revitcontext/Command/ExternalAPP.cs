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

            // Create a new Ribbon Panel in Revit's UI
            RibbonPanel Panel = application.CreateRibbonPanel("AEC Project", "Draw elements");

            // Get the current assembly for the command and resources
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Configure and create a button linked to a command class
            PushButtonData p1 = new PushButtonData("BTN1", "CAD to Revit", assembly.Location, "ProjectRevitFinal.Revitcontext.Command.OpenWindowCommand");
            PushButton pushp1 = Panel.AddItem(p1) as PushButton; // Add button to the Ribbon Panel

            // Set up and assign an icon to the button
            Uri UriPath = new Uri("pack://application:,,,/ProjectRevitFinal;component/Resources/iti-icon.png");
            BitmapImage Image = new BitmapImage(UriPath);
            pushp1.LargeImage = Image; // Assign icon to the button

            return Result.Succeeded;
        }
    }
}
