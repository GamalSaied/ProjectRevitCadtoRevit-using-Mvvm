using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProjectRevitFinal.Model1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRevitFinal.Revitcontext.Command
{
    public class CreateColumn
    {
        Document doc = OpenWindowCommand.doc;


        //method to get the cad layers and make the user to choose the layer of columns
                public static List<elementsLayers> Getcadlayers()
        {
            Document doc = OpenWindowCommand.doc;
            var cadelements = (IList<ElementId>)new FilteredElementCollector(doc).OfClass(typeof(ImportInstance)).WhereElementIsNotElementType().ToElementIds();

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
                }

            }
            catch (Exception ex)
            {

                TaskDialog.Show("Error", " show the error" + ex.Message);

            }
            return Layernames;
        }



        //        method to get the type of the column that comes from cad with their dimensions
        public static List<Columntypes> Getcolumntypes()
        {
            Document doc = OpenWindowCommand.doc;
            var columns = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Columns).WhereElementIsElementType().ToElements();
            List<Columntypes> columntypes = new List<Columntypes>();

            foreach (Element column in columns)
            {
                Columntypes col = new Columntypes();
                col.columntypes = column.Name;

                columntypes.Add(col);

            }
            return columntypes;
        }


    }
}
