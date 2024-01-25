using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProjectRevitFinal.Revitcontext.Command
{
    public class create_columns
    {

        public void CreateColumn()
        {
            Document doc = OpenWindowCommand.doc;



            var cadelements = new FilteredElementCollector(doc).OfClass(typeof(ImportInstance)).WhereElementIsNotElementType().ToElementIds();

            IList<string> layername = new List<string>();

            try
            {
                if (cadelements.Count > 0)
                {
                    Options opt = new Options();
                    var Iminstance = doc.GetElement(cadelements.First()) as ImportInstance;
                    var geoelements = Iminstance.get_Geometry(opt);

                    foreach (GeometryObject item in geoelements)

                    {
                        var geoinstance = item as GeometryInstance;
                        var instancegeometry = geoinstance.GetInstanceGeometry();


                        foreach (var instance in instancegeometry)
                        {
                            if(instance is PolyLine)
                            {
                                var polylineinstance = instance as PolyLine;
                                var polygrahicid = polylineinstance.GraphicsStyleId;
                                var polyline = doc.GetElement(polygrahicid) as GraphicsStyle;
                                var plgrahic = polyline as GraphicsStyle;
                                plgrahic.Name = layername[0];
                            }
                           
                        }


                        foreach (Element element in elements)


                        {

                            Reference reference = new Reference(element);
                            LocationPoint lcPoint = element.Location as LocationPoint;

                            XYZ Point = lcPoint.Point;
                            XYZ firstp = new XYZ();
                            XYZ secondp = new XYZ();
                            using (Transaction trans = new Transaction(Doc, "Tag Element"))
                            {
                                trans.Start();


                                doc.Create.NewFamilyInstance(midP,,, structuralType);


                                trans.Commit();
                            }
                        }
                    }
                }
            }


               catch (Exception ex)
               {

                TaskDialog.Show("Error", "Failed to import DWG file. Error: " + ex.Message);
               }
        

           
            
        }



      
        //method to get the Midpoint of the column
        private static XYZ midpoint(double x1, double x2, double y1 ,double y2,double z1, double z2)
        {

            XYZ midP= new XYZ((x1+x2)/2,(y1+y2)/2,(z1+z2)/2);
            return midP;

        }
    }
}
