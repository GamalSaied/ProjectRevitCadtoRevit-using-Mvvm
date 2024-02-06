using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProjectRevitFinal.Model1;
using ProjectRevitFinal.Revitcontext.Command;
using ProjectRevitFinal.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProjectRevitFinal.Model.AutoCAD
{
    public class Getwalls
    {
        public static Line CenterLine;
        public static void GetWallsData()
        {

            XYZ First_CenterPoint;
            XYZ Second_CenterPoint;


            //1-Attach Docment  + Selected Layer from Combobox
            Document doc = OpenWindowCommand.doc;

            string SelectedLayer = Walls.GetData.AutoCAD_Layer_Walls.SelectedItem.ToString(); ///++++++++
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

                    // Get AutoCAD Walls Section
                    //====================================================================
                    // Get Col Types  
                    IList<XYZ> vertices = polyline.GetCoordinates();
                    // Calculate the length of each segment
                    double totalLength = 0.0;
                    double maxLength = double.MinValue;
                    double minLength = double.MaxValue;

                    string MyAutoCAD_Type = "";

                    //Get First Edge 
                    XYZ Point0 = vertices[0];
                    XYZ Point1 = vertices[1];
                    double Length_1 = Point0.DistanceTo(Point1); // Result By Feet

                    //Get Second Edge 
                    XYZ Point2 = vertices[1];
                    XYZ Point3 = vertices[2];
                    double Length_2 = Point2.DistanceTo(Point3); // Result By Feet

                    //Get Third Edge 
                    XYZ Point4 = vertices[2];
                    XYZ Point5 = vertices[3];
                    double Length_3 = Point4.DistanceTo(Point5); // Result By Feet

                    //Get Forth Edge 
                    XYZ Point6 = vertices[3];
                    XYZ Point7 = vertices[4];
                    double Length_4 = Point6.DistanceTo(Point7); // Result By Feet

                    // Get Minimum 2 Sides
                    double[] Vertix = { Length_1, Length_2, Length_3, Length_4 };
                    // Sort the array
                    Array.Sort(Vertix);

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
                    MyAutoCAD_Type = ($"{cadlayers.Nameoflayer}({minLength}x{maxLength})");

                    // Case1 
                    //XYZ First_CenterPoint;
                    //XYZ Second_CenterPoint;
                    //Line CenterLine;
                    double V1;
                    V1 = Vertix[0];
                    if ((V1 == Length_1) || (V1 == Length_3))
                    {
                        // First Point 
                        First_CenterPoint = midpoint(Point0.X, Point1.X, Point0.Y, Point1.Y, Point0.Z, Point1.Z);
                        // Second Point
                        Second_CenterPoint = midpoint(Point4.X, Point5.X, Point4.Y, Point5.Y, Point4.Z, Point5.Z);
                        // BeamLine 
                        CenterLine = Line.CreateBound(First_CenterPoint, Second_CenterPoint);
                    }
                    //Case2
                    V1 = Vertix[0];
                    if ((V1 == Length_2) || (V1 == Length_4))
                    {
                        // First Point 
                        First_CenterPoint = midpoint(Point2.X, Point3.X, Point2.Y, Point3.Y, Point2.Z, Point3.Z);
                        // Second Point
                        Second_CenterPoint = midpoint(Point6.X, Point7.X, Point6.Y, Point7.Y, Point6.Z, Point7.Z);
                        // BeamLine 
                        CenterLine = Line.CreateBound(First_CenterPoint, Second_CenterPoint);
                    }

                    //========================================================



                    string SelectedWallType = Walls.GetData.AutoCAD_Layer_Walls.SelectedItem.ToString(); ///++++++++

                    if ((cadlayers.Nameoflayer == SelectedLayer))//&& (MyAutoCAD_Type == SelectedWallType))
                    {
                        // Get walls Type


                        // Get Levels
                        Level collevel = null;
                        var elementsColumns = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Columns).WhereElementIsElementType().ToElements();
                        var levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().ToList();
                        foreach (var level in levels)
                        {
                            if (level.Name == Walls.GetData.Revit_Levels.SelectedItem.ToString())
                            {
                                collevel = level;
                                break;
                            }
                        }

                        FamilySymbol fs = null;
                        foreach (var ele in elementsColumns)
                        {
                            if ((ele.Name == Walls.GetData.Revit_Wall_Type.SelectedItem.ToString()))
                            {
                                fs = ele as FamilySymbol;
                            }
                        }

                        string SelectedWallTypeRevit = Walls.GetData.Revit_Wall_Type.SelectedItem.ToString(); ///++++++++
                        WallType MyWall = GetWallTypeByName(doc, SelectedWallTypeRevit);

                        using (Transaction trans = new Transaction(doc, "create walls"))
                        {
                            trans.Start();
                            try
                            {


                                // Ensure the level is valid
                                if (collevel != null)
                                {

                                    // Create the wall
                                    Wall wall = Wall.Create(doc, CenterLine, MyWall.Id, collevel.Id, 10, 0, false, false);
                                }
                                else
                                {
                                    TaskDialog.Show("Error", "Level is null.");
                                }
                            }
                            catch (Exception ex)
                            {
                                TaskDialog.Show("Error", ex.Message);
                            }

                            trans.Commit();
                        }

                    }


                }

                MessageBox.Show("The Walls have been drawn successfully ", "ITI AECI Track");

            }

        }




        //method to get the Midpoint of the column
        private static XYZ midpoint(double x1, double x2, double y1, double y2, double z1, double z2)
        {

            XYZ midP = new XYZ((x1 + x2) / 2, (y1 + y2) / 2, (z1 + z2) / 2);
            return midP;

        }

        // Get wall type
        public static WallType GetWallTypeByName(Document doc, string wallTypeName)
        {
            doc = OpenWindowCommand.doc;
            // Get all wall types in the document
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> wallTypes = collector.OfClass(typeof(WallType)).ToElements();

            // Iterate through each wall type and check if the name matches
            foreach (Element elem in wallTypes)
            {
                WallType wallType = elem as WallType;
                if (wallType != null && wallType.Name == wallTypeName)
                {
                    // Return the wall type if found
                    return wallType;
                }
            }

            // Return null if the wall type is not found
            return null;
        }


        public static void Get_AutoCAD_Walls()
        {
            {
                Document doc = OpenWindowCommand.doc;
                // Get Unique Layers 
                //var uniqueLayers = AutoCAD_AllLayers.Select(x => x.Nameoflayer).Distinct();
                // Clear All item from Combobox
                Walls.GetData.AutoCAD_Wall_Type.Items.Clear();
                // Insert uniqueLayers to Combobox 
                foreach (var cadlayer in ImportCad.AutoCAD_AllLayers)
                {
                    Walls.GetData.AutoCAD_Layer_Walls.Items.Add(cadlayer); // Add Data to Combobox
                }
                //------------------------------------------------------------------------------------
                // Clear All item from Combobox
                Walls.GetData.Revit_Wall_Type.Items.Clear();
                var elementsColumns = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsElementType().ToElements();
                foreach (var ele in elementsColumns)
                {
                    Walls.GetData.Revit_Wall_Type.Items.Add(ele.Name); // Add Data to Combobox
                }
                //------------------------------------------------------------------------------------

                // Get unique Levels
                Walls.GetData.Revit_Levels.Items.Clear();
                var levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().ToList();
                foreach (var level in levels)
                {
                    Walls.GetData.Revit_Levels.Items.Add(level.Name); // Add Data to Combobox
                }
                //------------------------------------------------------------------------------------
            }

        }
    }
}

