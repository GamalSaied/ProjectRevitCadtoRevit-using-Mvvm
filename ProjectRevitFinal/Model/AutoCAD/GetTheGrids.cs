using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using ProjectRevitFinal.Model1;
using ProjectRevitFinal.Revitcontext.Command;
using ProjectRevitFinal.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Xml.Linq;
using Line = Autodesk.Revit.DB.Line;
using netDxf;
using netDxf.Entities;

namespace ProjectRevitFinal.Model.AutoCAD
{
    public class GetTheGrids

    {


        public static List<elementsLayers> GetGridslayers()
        {

            Document doc = OpenWindowCommand.doc;
            var cadelements = (IList<ElementId>)new FilteredElementCollector(doc).OfClass(typeof(ImportInstance)).WhereElementIsNotElementType().ToElementIds();

            List<elementsLayers> Layeraxis = new List<elementsLayers>();

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
                                    elementsLayers cadlayers = new elementsLayers();
                                    if (instance is Line)
                                    {

                                        var polygrahicid = instance.GraphicsStyleId;
                                        var axisline = doc.GetElement(polygrahicid) as GraphicsStyle;
                                        cadlayers.Nameoflayer = axisline.GraphicsStyleCategory.Name;


                                    }
                                    Layeraxis.Add(cadlayers);
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                TaskDialog.Show("Error", " show the error" + ex.Message);

            }
            return Layeraxis;
        }

        public static void Createthegrids()
        {
            #region Loading Layers From dxf file
            //Get Crids layer form the dxf file.
            DxfDocument DxfFile = DxfDocument.Load(GlobalVariable.file);
            IEnumerable<netDxf.Entities.Line> AxixCad = DxfFile.Lines.Where(ins => ins.Layer.Name == "Axix");
            #endregion

            #region Create Grids from CAD Axix
            //Create the start and end grid point for each grid
            XYZ GridStartPoint, GridEndPoint;

            //Create a list to include all lines which represent the grids.
            List<Curve> GridLines = new List<Curve>();

            //Loop for all lines in the axix layer to draw a line and add to the GridLines list. 
            for (int i = 0; i < AxixCad.Count(); i++)
            {
                //Get the lines from the Axix layer
                netDxf.Entities.Line line = AxixCad.ElementAt(i);
                //The start point of the axix layer line.
                GridStartPoint = new XYZ(line.StartPoint.X, line.StartPoint.Y, line.StartPoint.Z);

                //The end point of the axix layer line.
                GridEndPoint = new XYZ(line.EndPoint.X, line.EndPoint.Y, line.EndPoint.Z);

                //Create a line from the two point to represent the grid.
                Autodesk.Revit.DB.Line GridLine = Autodesk.Revit.DB.Line.CreateBound(GridStartPoint, GridEndPoint);
                GridLines.Add(GridLine); //Add the line to the GridLines list.
            }




            //Creating the Revit Grids.

            using (Transaction t = new Transaction(doc, "Draw Grids from dxf file"))
            {
                t.Start();
                foreach (Autodesk.Revit.DB.Line line in GridLines)
                {
                    Grid.Create(doc, line);
                }
                t.Commit();
            }
            #endregion

            return Result.Succeeded;
        }
            catch (Exception e)
            {
                return Result.Failed;
            }
}
    }

        }

    }
}








       