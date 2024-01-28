using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using ProjectRevitFinal.Model1;
using ProjectRevitFinal.Revitcontext.Command;
using ProjectRevitFinal.View;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectRevitFinal.Model.AutoCAD
{

    public class GetColumns
    {




        public static void GetColumnsData()
        {
            //1-Attach Docment  + Selected Layer from Combobox
            Document doc = OpenWindowCommand.doc;

            string SelectedLayer = Columns.GetData.AutoCAD_Layer_Columns.SelectedItem.ToString();
            //2-Get Import Data from Drawing  
            var cadelements = new FilteredElementCollector(doc).OfClass(typeof(ImportInstance)).WhereElementIsNotElementType().ToElementIds();
            //3-Create List of Polylines 
            List<PolyLine> polyLines = new List<PolyLine>();
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
                        if (instance is PolyLine)
                        {
                            polyLines.Add(instance as PolyLine);
                            PolyLine polyline = instance as PolyLine;
                            //====================================================================
                            // Get Polyline Vertix 
                            // Get vertices of the polyline

                            // Get vertices of the polyline
                            IList<XYZ> vertices = polyline.GetCoordinates();
                            // Calculate the length of each segment
                            double totalLength = 0.0;
                            double maxLength = double.MinValue;
                            double minLength = double.MaxValue;
                            for (int i = 0; i < vertices.Count - 1; i++)
                            {
                                //Get FirstPoint & Second Point
                                XYZ Point1 = vertices[i];
                                XYZ Point2 = vertices[i + 1];

                                // Calculate the Distance Between Point1 &  Point2
                                double Line_Length = Point1.DistanceTo(Point2); // Result By Feet
                                Line_Length = Line_Length * 0.3048;  // Convert Feet to Meter
                                // Get The Total Length
                                totalLength += Line_Length;

                                // Get the Maximum and Minimum Length // Maximum Length , Minimum Width 
                                maxLength = Math.Max(maxLength, Line_Length);
                                minLength = Math.Min(minLength, Line_Length);

                                maxLength = Math.Round(maxLength, 2);
                                minLength = Math.Round(minLength, 2);
                            }
                            TaskDialog.Show("Polyline Length", $"Length of Polyline: {totalLength} feet");

                            //====================================================================
                        }
                    }
                }

                foreach (var polyline in polyLines)
                {
                    elementsLayers cadlayers = new elementsLayers();
                    var polygraphicalid = doc.GetElement(polyline.GraphicsStyleId) as GraphicsStyle;
                    cadlayers.Nameoflayer = polygraphicalid.GraphicsStyleCategory.Name;
                    if (cadlayers.Nameoflayer == SelectedLayer)
                    {
                        // Get Data from Polylines

                        Outline putline = polyline.GetOutline();
                        XYZ firstp = putline.MaximumPoint;
                        XYZ secondp = putline.MinimumPoint;

                        XYZ Linemid = midpoint(firstp.X, secondp.X, firstp.Y, secondp.Y, firstp.Z, secondp.Z);

                        Level collevel = null;
                        var elementsColumns = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Columns).WhereElementIsElementType().ToElements();
                        var levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().ToList();
                        foreach (var level in levels)
                        {
                            if (level.Name == "Level 1")
                            {
                                collevel = level;
                                break;
                            }
                        }
                        FamilySymbol fs = null;
                        foreach (var ele in elementsColumns)
                        {
                            if (ele.Name == Columns.GetData.AutoCAD_Col_Type.SelectedItem.ToString())
                            {
                                fs = ele as FamilySymbol;
                            }
                        }



                        using (Transaction trans = new Transaction(doc, "create columns"))
                        {

                            trans.Start();
                            try
                            {
                                if (!fs.IsActive)
                                {
                                    fs.Activate();
                                }
                                doc.Create.NewFamilyInstance(Linemid, fs, collevel, StructuralType.NonStructural);

                            }
                            catch (Exception ex)
                            {

                                TaskDialog.Show(ex.Message, ex.ToString());
                            }

                            trans.Commit();
                        }
                    }
                }
            }

        }


        //method to get the Midpoint of the column
        private static XYZ midpoint(double x1, double x2, double y1, double y2, double z1, double z2)
        {

            XYZ midP = new XYZ((x1 + x2) / 2, (y1 + y2) / 2, (z1 + z2) / 2);
            return midP;

        }


    }
}













