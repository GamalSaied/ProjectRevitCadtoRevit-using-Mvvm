using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectRevitFinal.Revitcontext.Command
{
    public class CreateWalls
    {
        public static void CreateWallsFromCADLayers()
        {
            Document doc = OpenWindowCommand.doc;

            // Assuming you have a way to get the selected CAD layer for walls
            string selectedLayer = "YourSelectedLayerForWalls"; // This should come from user input

            // Assuming you have a method to get the thickness for walls from CAD
            double wallThicknessFromCAD = 0.1; // Example thickness in meters

            var cadElements = new FilteredElementCollector(doc)
                .OfClass(typeof(ImportInstance))
                .WhereElementIsNotElementType()
                .ToElementIds();

            if (cadElements.Count == 0) return;

            Options opt = new Options();
            var importInstance = doc.GetElement(cadElements.First()) as ImportInstance;
            var geometryElements = importInstance.get_Geometry(opt);

            List<Line> wallLines = new List<Line>(); // Revit lines to create walls

            foreach (GeometryObject geomObj in geometryElements)
            {
                if (geomObj is GeometryInstance geomInstance)
                {
                    foreach (GeometryObject instanceGeom in geomInstance.GetInstanceGeometry())
                    {
                        if (instanceGeom is Line line)
                        {
                            // Add logic to filter lines based on selectedLayer and other criteria
                            wallLines.Add(line);
                        }
                    }
                }
            }

            Level baseLevel = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Cast<Level>()
                .FirstOrDefault(); // Adjust as needed

            if (baseLevel == null)
            {
                TaskDialog.Show("Error", "Base level not found.");
                return;
            }

            WallType wallType = new FilteredElementCollector(doc)
                .OfClass(typeof(WallType))
                .Cast<WallType>()
                .FirstOrDefault(wt => wt.Kind == WallKind.Basic); // Adjust as needed

            if (wallType == null)
            {
                TaskDialog.Show("Error", "Wall type not found.");
                return;
            }

            using (Transaction t = new Transaction(doc, "Create Walls"))
            {
                t.Start();

                foreach (Line line in wallLines)
                {
                    // Adjust the wall creation logic as needed
                    Wall.Create(doc, line, wallType.Id, baseLevel.Id, wallThicknessFromCAD, 0, false, false);
                }

                t.Commit();
            }
        }
    }
}
