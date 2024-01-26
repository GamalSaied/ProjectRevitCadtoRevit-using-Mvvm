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
        private IList<PolyLine> polyLines = new List<PolyLine>();

        //method to get the cad layers and make the user to choose the layer of columns
        public static List<cadElements> Getcadlayers()
        {
            Document doc = OpenWindowCommand.doc;
            var cadelements = new FilteredElementCollector(doc).OfClass(typeof(ImportInstance)).WhereElementIsNotElementType().ToElementIds();

            List<cadElements> Layernames = new List<cadElements>();

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
                            cadElements cadlayers=new cadElements();
                            if (instance is PolyLine)
                            {

                                var polygrahicid = instance.GraphicsStyleId;
                                var polyline = doc.GetElement(polygrahicid) as GraphicsStyle;
                                cadlayers.Nameoflayer = polyline.GraphicsStyleCategory.Name;
                               
                                //polyLines.Add(instance as PolyLine);
                            }
                            Layernames.Add(cadlayers);
                        }
                    }
                }
               
            }


               catch (Exception ex)
               {

                TaskDialog.Show("Error", "Failed to import DWG file. Error: " + ex.Message);
               }



            return Layernames;
        }

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

        public void createcolumns()
        {
            Document doc = OpenWindowCommand.doc;
            foreach (var polyline in polyLines)
            {
                var polygraphicalid = doc.GetElement(polyline.GraphicsStyleId) as GraphicsStyle;
                string layer = polygraphicalid.GraphicsStyleCategory.Name;
                if (layer == selectedlayer)
                {
                    Outline putline = polyline.GetOutline();
                    XYZ firstp = putline.MaximumPoint;
                    XYZ secondp = putline.MinimumPoint;
                    XYZ linemid =midpoint(firstp.X,secondp.X,firstp.Y,secondp.Y,firstp.Z,secondp.Z);
                }
                Level collevel = null;
                var elementsColumns = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Columns).WhereElementIsElementType().ToElements();
                var levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().ToList();
                foreach (var level in levels)
                {
                    if(level.Name=="Level 0")
                    {
                        collevel=level;
                    }
                }
            }
            Reference reference = new Reference(element);
            LocationPoint lcPoint = element.Location as LocationPoint;

            XYZ Point = lcPoint.Point;

            using (Transaction trans = new Transaction(doc, "Tag Element"))
            {
                trans.Start();


                doc.Create.NewFamilyInstance(linemid,,, structuralType);


                trans.Commit();
            }


        }
        //method to get the Midpoint of the column
        private static XYZ midpoint(double x1, double x2, double y1, double y2, double z1, double z2)
        {

            XYZ midP = new XYZ((x1 + x2) / 2, (y1 + y2) / 2, (z1 + z2) / 2);
            return midP;

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
