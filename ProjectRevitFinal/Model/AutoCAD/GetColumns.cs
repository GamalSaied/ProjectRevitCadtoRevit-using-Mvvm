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

                        }
                    }
                }


                foreach (var polyline in polyLines)
                {
                    // Get Layer Name 
                    elementsLayers cadlayers = new elementsLayers();
                    var polygraphicalid = doc.GetElement(polyline.GraphicsStyleId) as GraphicsStyle;
                    cadlayers.Nameoflayer = polygraphicalid.GraphicsStyleCategory.Name;

                    // Get AutoCAD Column Section
                    //====================================================================
                    // Get Col Types  
                    IList<XYZ> vertices = polyline.GetCoordinates();
                    // Calculate the length of each segment
                    double totalLength = 0.0;
                    double maxLength = double.MinValue;
                    double minLength = double.MaxValue;
                    string MyAutoCAD_Type = "";
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
                    MyAutoCAD_Type = ($"{cadlayers.Nameoflayer}({minLength}x{maxLength})");
                    //TaskDialog.Show("Polyline Length", $"Length of Polyline: {totalLength} feet");
                    //====================================================================
                    string SelectedColType = Columns.GetData.AutoCAD_Col_Type.SelectedItem.ToString();
                    if ((cadlayers.Nameoflayer == SelectedLayer) && (MyAutoCAD_Type == SelectedColType))
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
                            if (level.Name == Columns.GetData.Revit_Levels.SelectedItem.ToString())
                            {
                                collevel = level;
                                break;
                            }
                        }
                        FamilySymbol fs = null;
                        foreach (var ele in elementsColumns)
                        {
                            if ((ele.Name == Columns.GetData.Revit_Col_Type.SelectedItem.ToString()))
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
                            //TaskDialog.Show("The columns have been drawn successfully ", "ITI AECI Track");
                        }
                    }
                }
                TaskDialog.Show("ITI AECI Track", "The columns have been drawn successfully ");
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













