using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Win32;
using ProjectRevitFinal.Model1;
using ProjectRevitFinal.View;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectRevitFinal.Revitcontext.Command
{

    public class ImportCad
    {
        public static ElementId MyElemntID;
        public static List<string> AutoCAD_ListType;
        public static void GET_AutoCAD_FilePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DWG Files|*.dwg|All Files|*.*";
            openFileDialog.Title = "Select DWG File";

            //Step3: Insert File Path into Textbox 
            if (openFileDialog.ShowDialog() == true)
            {
                //---------------------------------------------------------------------------------------------
                //1# Get the selected file path and display it in the
                string filePath = openFileDialog.FileName;
                Columns.GetData.Path.Text = filePath;
                //2# import AutoCAD DWG To Revit
                Document doc = OpenWindowCommand.doc;
                try
                {
                    //3# Drawing import Options
                    DWGImportOptions dwgOptions = new DWGImportOptions();
                    dwgOptions.Unit = ImportUnit.Meter;
                    dwgOptions.ColorMode = ImportColorMode.Preserved;
                    dwgOptions.ThisViewOnly = true;
                    Autodesk.Revit.DB.View currentview = doc.ActiveView;
                    ElementId elementid = null;                         // Return ID When its Created 
                    using (Transaction tr = new Transaction(doc, "importfilcadpath"))
                    {
                        tr.Start();
                        // Import the DWG file // + Need Add Units Setting
                        var loadpath = doc.Import(filePath, dwgOptions, currentview, out elementid);
                        MyElemntID = elementid;
                        // Commit the transaction
                        tr.Commit();
                    }
                }
                catch (Exception ex)
                {

                    TaskDialog.Show("Error", "Failed to import DWG file. Error: " + ex.Message);
                }
            }
        }

        public static void Get_AutoCAD_Layers()
        {
            //1. Attach Revit Document
            Document doc = OpenWindowCommand.doc;
            //2. Get AutoCAD Elements
            var cadelements = (IList<ElementId>)new FilteredElementCollector(doc).OfClass(typeof(ImportInstance)).WhereElementIsNotElementType().ToElementIds();
            //3. Create List To input Layers Names
            List<elementsLayers> Layernames = new List<elementsLayers>();
            AutoCAD_ListType = new List<string>();
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
                                        // Get Layer 
                                        var graphicsStyleId = instance.GraphicsStyleId;
                                        var myLine = doc.GetElement(graphicsStyleId) as GraphicsStyle;

                                        if (myLine != null)
                                        {
                                            cadlayers.Nameoflayer = myLine.GraphicsStyleCategory.Name;
                                        }
                                    }

                                    if (instance is PolyLine)
                                    {
                                        // Get Layer 
                                        var polygrahicid = instance.GraphicsStyleId;
                                        var Mypolyline = doc.GetElement(polygrahicid) as GraphicsStyle;
                                        cadlayers.Nameoflayer = Mypolyline.GraphicsStyleCategory.Name;

                                        //====================================================================
                                        // Get Col Types  
                                        PolyLine polyline = instance as PolyLine;
                                        // Get vertices of the polyline
                                        IList<XYZ> vertices = polyline.GetCoordinates();
                                        // Calculate the length of each segment
                                        double totalLength = 0.0;
                                        double maxLength = double.MinValue;
                                        double minLength = double.MaxValue;
                                        string AutoCAD_Type;
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
                                        AutoCAD_Type = ($"{cadlayers.Nameoflayer}({minLength}x{maxLength})");
                                        AutoCAD_ListType.Add(AutoCAD_Type);
                                        //TaskDialog.Show("Polyline Length", $"Length of Polyline: {totalLength} feet");
                                        //====================================================================
                                    }

                                    // Adding values to the list
                                    Layernames.Add(cadlayers);
                                }
                            }
                        }

                    }

                    // Get Unique Layers 
                    var uniqueLayers = Layernames.Select(x => x.Nameoflayer).Distinct();
                    // Clear All item from Combobox
                    Columns.GetData.AutoCAD_Layer_Columns.Items.Clear();
                    // Insert uniqueLayers to Combobox 
                    foreach (var cadlayer in uniqueLayers)
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

            }
            catch (Exception ex)
            {

                TaskDialog.Show("Error", " show the error" + ex.Message);

            }

        }


    }
}
