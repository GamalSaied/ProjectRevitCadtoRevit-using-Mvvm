using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
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
        public void CreateColumn()
        {
          
            var cadelements = new FilteredElementCollector(doc).OfClass(typeof(ImportInstance)).WhereElementIsNotElementType().ToElementIds();

            IList<string> Layernames = new List<string>();

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
                            if (instance is PolyLine)
                            {

                                var polygrahicid = instance.GraphicsStyleId;
                                var polyline = doc.GetElement(polygrahicid) as GraphicsStyle;
                                string layer = polyline.GraphicsStyleCategory.Name;
                                Layernames.Add(layer);
                                polyLines.Add(instance as PolyLine);
                            }

                        }
                    }
                }
                var columns = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Columns).WhereElementIsElementType().ToElements();
                IList<string> columntypes = new List<string>();

                foreach (Element column in columns)
                {

                    columntypes.Add(column.Name);
                   
                } 
            }


               catch (Exception ex)
               {

                TaskDialog.Show("Error", "Failed to import DWG file. Error: " + ex.Message);
               }
        

           
            
        }
        public  void createcolumns()
        {
            Document doc = OpenWindowCommand.doc;
            foreach (var polyline in polyLines)
            {
               var polygraphicalid= doc.GetElement(polyline.GraphicsStyleId) as GraphicsStyle;
               string layer= polygraphicalid.GraphicsStyleCategory.Name;
                if(layer==selectedlayer)
                {
                    Outline putline = polyline.GetOutline();
                   XYZ firstp= putline.MaximumPoint;
                    XYZ secondp= putline.MinimumPoint;
                }
            }
            Reference reference = new Reference(element);
            LocationPoint lcPoint = element.Location as LocationPoint;

            XYZ Point = lcPoint.Point;
        
            using (Transaction trans = new Transaction(doc, "Tag Element"))
            {
                trans.Start();


                doc.Create.NewFamilyInstance(midP,,, structuralType);


                trans.Commit();
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
