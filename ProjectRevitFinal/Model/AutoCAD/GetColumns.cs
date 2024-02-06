using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using ProjectRevitFinal.Model1;
using ProjectRevitFinal.Revitcontext.Command;
using ProjectRevitFinal.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

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
                MessageBox.Show("The columns have been drawn successfully ", "ITI AECI Track");
            }

        }


        //method to get the Midpoint of the column
        private static XYZ midpoint(double x1, double x2, double y1, double y2, double z1, double z2)
        {

            XYZ midP = new XYZ((x1 + x2) / 2, (y1 + y2) / 2, (z1 + z2) / 2);
            return midP;

        }


        public static void Get_AutoCAD_LayersColumns()
        {
            Document doc = OpenWindowCommand.doc;
            // Get Unique Layers 
            //var uniqueLayers = AutoCAD_AllLayers.Select(x => x.Nameoflayer).Distinct();
            // Clear All item from Combobox
            Columns.GetData.AutoCAD_Layer_Columns.Items.Clear();
            // Insert uniqueLayers to Combobox 
            foreach (var cadlayer in ImportCad.AutoCAD_AllLayers)
            {
                Columns.GetData.AutoCAD_Layer_Columns.Items.Add(cadlayer); // Add Data to Combobox
            }
            //------------------------------------------------------------------------------------
            // Clear All item from Combobox
            Columns.GetData.Revit_Col_Type.Items.Clear();
            var elementsColumns = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Columns).WhereElementIsElementType().ToElements();
            foreach (var ele in elementsColumns)
            {
                Columns.GetData.Revit_Col_Type.Items.Add(ele.Name); // Add Data to Combobox
            }
            //------------------------------------------------------------------------------------

            // Get unique Levels
            Columns.GetData.Revit_Levels.Items.Clear();
            var levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().ToList();
            foreach (var level in levels)
            {
                Columns.GetData.Revit_Levels.Items.Add(level.Name); // Add Data to Combobox
            }
            //------------------------------------------------------------------------------------
        }


        public static void Get_RevitTypes()
        {
            Document doc = OpenWindowCommand.doc;

            //------------------------------------------------------------------------------------
            // Clear All item from Combobox
            Columns.GetData.Revit_Col_Type.Items.Clear();
            var elementsColumns = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Columns).WhereElementIsElementType().ToElements();
            foreach (var ele in elementsColumns)
            {
                Columns.GetData.Revit_Col_Type.Items.Add(ele.Name); // Add Data to Combobox
            }
            //------------------------------------------------------------------------------------

        }

        public static void testcol()
        {
            Document doc = OpenWindowCommand.doc;
            // Create Width and Depth input in feet and output in mm
            double Feet_To_MM = 304.8; // 1 Feet = 304.8 mm 

            double width = 600 / Feet_To_MM;
            double depth = 300 / Feet_To_MM;
            //GetColumns.CreateRectangularColumnType(doc, width, depth);
        }
        public static void CreateRectangularColumnType(Document doc, double width, double depth, String TypeName)
        {
            // elcode da ba3mil create 3la elemnt type w b3ml feha add w b3d kda badwar 3la el paramter w a3mil input feh bs 5ly balk el input bikon fel feet la2ino defult fel revit 

            // Ensure the document is not null
            if (doc == null)
            {
                MessageBox.Show("Document is null.", "Error");
                return;
            }

            // Create Width and Depth input in feet and output in mm
            double Feet_To_MM = 304.8; // 1 Feet = 304.8 mm 

            // x 1000 3lshan bia5od mel cad bel meter w hai7ot gowa el revit bel mm w bi7wlo le feet b3d kda
            width = (width * 1000) / Feet_To_MM;
            depth = (depth * 1000) / Feet_To_MM;


            // Get the family symbol for columns
            FamilySymbol columnSymbol = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .FirstOrDefault(x => x.Family.Name == "M_Rectangular Column" && x.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Columns);

            if (columnSymbol == null)
            {
                TaskDialog.Show("Column family not found.", "Error");
                return;
            }

            // Check if the column type already exists
            FamilySymbol existingColumnType = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .FirstOrDefault(x => x.Name == TypeName);

            if (existingColumnType != null)
            {
                //MessageBox.Show("Column type already exists.", "Information");
                return; // Exit the code if the column type already exists
            }

            // Create a new column type
            using (Transaction transaction = new Transaction(doc, "Create Rectangular Column Type"))
            {
                transaction.Start();

                // Create a new family symbol
                FamilySymbol columnTypeSymbol = columnSymbol.Duplicate(TypeName) as FamilySymbol;

                if (columnTypeSymbol == null)
                {
                    MessageBox.Show("Failed to create column type.", "Error");
                    transaction.RollBack();
                    return;
                }

                // Set the column type parameters (width and depth)
                Parameter widthParameter = columnTypeSymbol.LookupParameter("Width");
                Parameter depthParameter = columnTypeSymbol.LookupParameter("Depth");

                if (widthParameter != null && depthParameter != null)
                {
                    widthParameter.Set(width);
                    depthParameter.Set(depth);
                }
                else
                {
                    MessageBox.Show("Error", "Width or depth parameter not found.");
                    transaction.RollBack();
                    return;
                }

                transaction.Commit();
            }

            //MessageBox.Show("Success", "Column type created successfully.");
        }
        public static void CreateRevitTypes()
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
                        // Create COlumns Type in revit 
                        // Create Width and Depth input in feet and output in mm
                        // double Feet_To_MM = 304.8; // 1 Feet = 304.8 mm // el convert gowa el method
                        double width = minLength;
                        double depth = maxLength;
                        GetColumns.CreateRectangularColumnType(doc, width, depth, MyAutoCAD_Type);

                    }
                }

            }
        }

    }
}













