using Autodesk.Revit.DB;
using ProjectRevitFinal.Model1;
using ProjectRevitFinal.Revitcontext.Command;
using ProjectRevitFinal.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ProjectRevitFinal.Model.AutoCAD
{
    public class GetGrids
    {

        public static void DrawGrids()
        {

            // Get any Coordinates from AutoCAD From Reviiit  : Okay ya gemy :D ?

            //1-Attach Docment  + Selected Layer from Combobox
            Document doc = OpenWindowCommand.doc;
            string SelectedLayer = Grids.GetData.AutoCAD_Layer_Grids.SelectedItem.ToString();
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

                            MessageBox.Show(ex.Message, ex.ToString());

                        }
                        trans.Commit();
                    }
                }
            }
            MessageBox.Show("The Grids have been drawn successfully ", "ITI AECI Track");
        }
        public static void Get_AutoCAD_LayersGrids()
        {
            Document doc = OpenWindowCommand.doc;
            // Get Unique Layers 
            //var uniqueLayers = AutoCAD_AllLayers.Select(x => x.Nameoflayer).Distinct();
            // Clear All item from Combobox
            Grids.GetData.AutoCAD_Layer_Grids.Items.Clear();
            // Insert uniqueLayers to Combobox 
            foreach (var cadlayer in ImportCad.AutoCAD_AllLayers)
            {
                Grids.GetData.AutoCAD_Layer_Grids.Items.Add(cadlayer); // Add Data to Combobox
            }
            //------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------
        }
    }

}
