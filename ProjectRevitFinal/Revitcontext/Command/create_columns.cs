using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using ProjectRevitFinal.Model1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProjectRevitFinal.Revitcontext.Command
{
    public class create_columns
    {
        Document doc = OpenWindowCommand.doc;

        public List<PolyLine> polyLines = new List<PolyLine>();
        //method to get the cad layers and make the user to choose the layer of columns
        public static List<elementsLayers> Getcadlayers()
        {
            Document doc = OpenWindowCommand.doc;
            var cadelements = new FilteredElementCollector(doc).OfClass(typeof(ImportInstance)).WhereElementIsNotElementType().ToElementIds();

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
                        var geoinstance = item as GeometryInstance;
                        var instancegeometry = geoinstance.GetInstanceGeometry();


                        foreach (var instance in instancegeometry)
                        {
                            elementsLayers cadlayers=new elementsLayers();
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


            catch (Exception ex)
            {

                TaskDialog.Show("Error","" + ex.Message);
            }



            return Layernames;
        }

        ////method to get allpolylines




        //public static List<elementsLayers> Getpolylines()
        //{
        //    Document doc = OpenWindowCommand.doc;
        //    var cadelements = new FilteredElementCollector(doc).OfClass(typeof(ImportInstance)).WhereElementIsNotElementType().ToElementIds();

           

        //    try
        //    {
        //        if (cadelements.Count > 0)
        //        {
        //            Options opt = new Options();
        //            var Importainstance = doc.GetElement(cadelements.First()) as ImportInstance;
        //            var geoelements = Importainstance.get_Geometry(opt);

        //            foreach (GeometryObject item in geoelements)

        //            {
        //                var geoinstance = item as GeometryInstance;
        //                var instancegeometry = geoinstance.GetInstanceGeometry();


        //                foreach (var instance in instancegeometry)
        //                {
        //                    elementsLayers cadlayers = new elementsLayers();
        //                    if (instance is PolyLine)
        //                    {

        //                        var polygrahicid = instance.GraphicsStyleId;
        //                        var polyline = doc.GetElement(polygrahicid) as GraphicsStyle;
        //                        cadlayers.Nameoflayer = polyline.GraphicsStyleCategory.Name;

        //                        polyLines.Add(instance as PolyLine);
        //                    }
                            
        //                }
        //            }
        //        }

        //    }


        //    catch (Exception ex)
        //    {

        //        TaskDialog.Show("Error", "" + ex.Message);
        //    }



        //    return polyLines;
        //}



        //method to get the type of the column that comes from cad with their dimensions
        public static List<Columntypes> Getcolumntypes()
        {
            Document doc = OpenWindowCommand.doc;
            var columns = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Columns).WhereElementIsElementType().ToElements();
            List<Columntypes> columntypes = new List<Columntypes>();

            foreach (Element column in columns)
            {
                Columntypes col =new Columntypes();
                col.columntypes = column.Name;

                columntypes.Add(col);

            }
            return columntypes;
        }

     


        //--------------------------------------------------------------------
        //Document doc = OpenWindowCommand.doc;
        //private IList<PolyLine> polyLines = new List<PolyLine>();
        //public static List<string> Getcadlayers()
        //{
        //    Document doc = OpenWindowCommand.doc;
        //    var cadelements = new FilteredElementCollector(doc).OfClass(typeof(ImportInstance)).WhereElementIsNotElementType().ToElementIds();

        //    List<string> Layernames = new List<string>();

        //    try
        //    {
        //        if (cadelements.Count > 0)
        //        {
        //            Options opt = new Options();
        //            var Importainstance = doc.GetElement(cadelements.First()) as ImportInstance;
        //            var geoelements = Importainstance.get_Geometry(opt);

        //            foreach (GeometryObject item in geoelements)

        //            {
        //                var geoinstance = item as GeometryInstance;
        //                var instancegeometry = geoinstance.GetInstanceGeometry();


        //                foreach (var instance in instancegeometry)
        //                {
        //                    if (instance is PolyLine)
        //                    {

        //                        var polygrahicid = instance.GraphicsStyleId;
        //                        var polyline = doc.GetElement(polygrahicid) as GraphicsStyle;
        //                        string layer = polyline.GraphicsStyleCategory.Name;
        //                        Layernames.Add(layer);
        //                        //polyLines.Add(instance as PolyLine);
        //                    }

        //                }
        //            }
        //        }
        //        var columns = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Columns).WhereElementIsElementType().ToElements();
        //        List<string> columntypes = new List<string>();

        //        foreach (Element column in columns)
        //        {

        //            columntypes.Add(column.Name);

        //        }
        //    }


        //    catch (Exception ex)
        //    {

        //        TaskDialog.Show("Error", "Failed to import DWG file. Error: " + ex.Message);
        //    }



        //    return Layernames;
        //}
        //-----------------------------------------------------------





    }
}
