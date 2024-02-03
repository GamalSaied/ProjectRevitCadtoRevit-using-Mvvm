using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProjectRevitFinal.Model1;
using ProjectRevitFinal.Revitcontext.Command;
using ProjectRevitFinal.View;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectRevitFinal.Model.AutoCAD
{
    public class GetGrids
    {

        public static void DrawGrids()
        {

            // Get any Coordinates from AutoCAD From Reviiit  : Okay ya gemy :D ?

            //1-Attach Docment  + Selected Layer from Combobox
            Document doc = OpenWindowCommand.doc;
            string SelectedLayer = Columns.GetData.AutoCAD_Layer_Columns.SelectedItem.ToString();
            //2-Get Import Data from Drawing  
            var cadelements = new FilteredElementCollector(doc).OfClass(typeof(ImportInstance)).WhereElementIsNotElementType().ToElementIds();
            //3-Create List of Polylines 
            List<Line> Lines = new List<Line>();
            //4-Filter Data [ Loop for every polyline ]
            if (cadelements.Count > 0)
            {
                Options opt = new Options();
                var Importainstance = doc.GetElement(cadelements.First()) as ImportInstance;
                var geoelements = Importainstance.get_Geometry(opt);
                foreach (GeometryObject item in geoelements)
                {
                    var geoinstance = item as GeometryInstance;
                    var instancegeometry = geoinstance.GetInstanceGeometry();
                    foreach (var instance in instancegeometry)
                    {
                        if (instance is Line)
                        {
                            Lines.Add(instance as Line);

                        }
                    }
                }
            }
            //--------------------------------------------------------------------------------------------------------------------------------------

            foreach (var line in Lines)
            {
                // Get Layer Name 
                elementsLayers cadlayers = new elementsLayers();
                var polygraphicalid = doc.GetElement(line.GraphicsStyleId) as GraphicsStyle;
                cadlayers.Nameoflayer = polygraphicalid.GraphicsStyleCategory.Name;
                // Get AutoCAD Column Section
                //====================================================================
                // Get Col Types  
                XYZ startPoint = line.GetEndPoint(0); // Start
                XYZ endPoint = line.GetEndPoint(1); // End
                if (cadlayers.Nameoflayer == SelectedLayer)
                {
                    using (Transaction trans = new Transaction(doc, "CreateGrids"))
                    {

                        trans.Start();
                        try
                        {

                            //Createlines
                            var Gridline = Line.CreateBound(startPoint, endPoint);

                            // Create grids using the curves
                            Autodesk.Revit.DB.Grid grid = Autodesk.Revit.DB.Grid.Create(doc, Gridline);


                        }
                        catch (Exception ex)
                        {

                            TaskDialog.Show(ex.Message, ex.ToString());

                        }
                        trans.Commit();
                    }
                }
            }
            TaskDialog.Show("ITI AECI Track", "The columns have been drawn successfully ");
        }
    }
}
