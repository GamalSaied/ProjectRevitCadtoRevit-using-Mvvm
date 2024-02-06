using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ProjectRevitFinal.Model.AutoCAD
{
    public class GetWalls
    {
        public static void GetWallsData(Document doc, string selectedLayer)
        {
            // Assuming selectedLayer is the name of the layer containing the walls in CAD
            // and doc is the active Revit document passed from where this method is called

            // 1. Get Import Instances (CAD Imports)
            var cadElements = new FilteredElementCollector(doc)
                .OfClass(typeof(ImportInstance))
                .WhereElementIsNotElementType()
                .ToElementIds();

            if (cadElements.Count == 0)
                return;

            // Assuming there is at least one import instance
            var importInstance = doc.GetElement(cadElements.First()) as ImportInstance;
            var options = new Options();
            var geometryElement = importInstance.get_Geometry(options);

            // 2. Extract lines representing walls
            var wallLines = new List<Line>(); // Using simple lines for this example
            foreach (var obj in geometryElement)
            {
                if (obj is GeometryInstance instance)
                {
                    var instanceGeometry = instance.GetInstanceGeometry();
                    foreach (var geomObj in instanceGeometry)
                    {
                        if (geomObj is Line line && line.GraphicsStyleId.IntegerValue > 0)
                        {
                            var graphicsStyle = doc.GetElement(line.GraphicsStyleId) as GraphicsStyle;
                            if (graphicsStyle != null && graphicsStyle.GraphicsStyleCategory.Name == selectedLayer)
                            {
                                wallLines.Add(line);
                            }
                        }
                    }
                }
            }

            // 3. Create Revit walls from lines
            using (Transaction t = new Transaction(doc, "Create Walls from CAD"))
            {
                t.Start();

                // Assuming a single level for simplicity, adjust as needed
                Level baseLevel = new FilteredElementCollector(doc)
                    .OfClass(typeof(Level))
                    .Cast<Level>()
                    .FirstOrDefault();

                if (baseLevel == null)
                {
                    TaskDialog.Show("Error", "No levels found in the document.");
                    return;
                }

                foreach (var line in wallLines)
                {
                    // Create a wall for each line
                    // This example assumes all walls are straight and have a uniform type and parameters
                    Wall.Create(doc, line, baseLevel.Id, false);
                }

                t.Commit();
            }
        }
    }
}
