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
                    var currentview = doc.ActiveView;
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
                                    if (instance is PolyLine)
                                    {

                                        var polygrahicid = instance.GraphicsStyleId;
                                        var polyline = doc.GetElement(polygrahicid) as GraphicsStyle;
                                        cadlayers.Nameoflayer = polyline.GraphicsStyleCategory.Name;

                                    }
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

                    // Clear All item from Combobox
                    Columns.GetData.AutoCAD_Col_Type.Items.Clear();
                    var elementsColumns = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Columns).WhereElementIsElementType().ToElements();
                    foreach (var ele in elementsColumns)
                    {
                        Columns.GetData.AutoCAD_Col_Type.Items.Add(ele.Name); // Add Data to Combobox
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
