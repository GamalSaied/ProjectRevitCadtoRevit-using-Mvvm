using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProjectRevitFinal.Model1;
using ProjectRevitFinal.Revitcontext.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using Line = Autodesk.Revit.DB.Line;

namespace ProjectRevitFinal.Model.AutoCAD
{
    public class GetTheGrids

    {


        public static void creategrids()
        {

            Autodesk.Revit.DB.Document doc = OpenWindowCommand.doc;
            // 1-Get Data from Lines
            var cadelements = (IList<ElementId>)new FilteredElementCollector(doc).OfClass(typeof(ImportInstance)).WhereElementIsNotElementType().ToElementIds();

            // 2-Create list of lines to store data 
            List<Line> GridsLines = new List<Line>();

            try
            {
                if (cadelements.Count > 0)
                {
                    Options opt = new Options();
                    var Importainstance = doc.GetElement(cadelements.First()) as ImportInstance;
                    var geoelements = Importainstance.get_Geometry(opt);

                    foreach (GeometryObject item in geoelements)

                    {
                        if (item is GeometryInstance)
                        {
                            var geoinstance = item as GeometryInstance;
                            var instancegeometry = geoinstance.GetInstanceGeometry();


                            if (geoelements != null)
                            {
                                foreach (var instance in instancegeometry)
                                {


                                    if (instance is Line)
                                    {

                                        var gridline = instance.GraphicsStyleId;
                                        var axisline = doc.GetElement(gridline) as GraphicsStyle;
                                        elementsLayers cadlayers = new elementsLayers();
                                        cadlayers.Nameoflayer = axisline.GraphicsStyleCategory.Name;

                                        GridsLines.Add(instance as Line);

                                    }
                                }
                            }
                        }
                    }

                    using (Transaction trans = new Transaction(doc, "CreateGrids"))
                    {

                        trans.Start();
                        try
                        {
                            foreach (var eleLine in GridsLines)
                            {
                                //Createlines
                                var Gline = Line.CreateBound(eleLine.GetEndPoint(0), eleLine.GetEndPoint(1));

                                // Create grids using the curves
                                Autodesk.Revit.DB.Grid grid = Autodesk.Revit.DB.Grid.Create(doc, Gline);

                            }


                        }
                        catch (Exception ex)
                        {

                            TaskDialog.Show(ex.Message, ex.ToString());

                        }
                        trans.Commit();
                    }

                }

            }

            catch (Exception ex)
            {

                TaskDialog.Show("Error", " show the error" + ex.Message);

            }






        }
    }

}








