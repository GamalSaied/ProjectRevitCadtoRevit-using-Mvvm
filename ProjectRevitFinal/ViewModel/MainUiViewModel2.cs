using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using ProjectRevitFinal.Commands;
using ProjectRevitFinal.Model1;
using ProjectRevitFinal.Revitcontext.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ProjectRevitFinal.ViewModel
{
    public class MainUiViewModel : INotifyPropertyChanged
    {

        #region Fields
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion



        #region Constructor

        public MainUiViewModel()
        {
            importcommand = new mycommand(ImportCadfile);
            layernames = CreateColumn.Getcadlayers();
            columnstypes = CreateColumn.Getcolumntypes();
            createcommand = new mycommand(createcolumns);
           
        }
        #endregion



        #region Properties

        private string _loadfilepath;

        public string Loadfilepath
        {
            get { return _loadfilepath; }
            set
            {

                _loadfilepath = value;

                OnPropertyChanged();


            }
        }
        //property to binding on the button importfile
        public mycommand importcommand { get; set; }

        //property for selectedlayer
        private elementsLayers _SelectedLayer;

        public elementsLayers SelectedLayer
        {
            get { return _SelectedLayer; }
            set
            {
                _SelectedLayer = value;
                OnPropertyChanged();
            }
        }

        //selected columntype
        private Columntypes _SelectedColumntype;

        public Columntypes SelectedColumntype
        {
            get { return _SelectedColumntype; }
            set
            {
                _SelectedColumntype = value;
                OnPropertyChanged();
            }
        }

        //property for list of layernames
        public List<elementsLayers> layernames { get; set; } = new List<elementsLayers>();
        //list of column types
        public List<Columntypes> columnstypes { get; set; } = new List<Columntypes>();


        //create command
        //property to binding on the button create
        public mycommand createcommand { get; set; }

        #endregion




        #region Methods
        //method for onpropertychanged
        public void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(PropertyName));

        }

        //method to import file from cad
        public void ImportCadfile()
        {
            //ImportCad.importfilcadpath(Loadfilepath);


        }


        //method to create columns

        public void createcolumns()
        {
            Document doc = OpenWindowCommand.doc;

            var cadelements = new FilteredElementCollector(doc).OfClass(typeof(ImportInstance)).WhereElementIsNotElementType().ToElementIds();

            List<PolyLine> polyLines = new List<PolyLine>();


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


                            foreach (var polyline in polyLines)
                            {
                                elementsLayers cadlayers = new elementsLayers();
                                var polygraphicalid = doc.GetElement(polyline.GraphicsStyleId) as GraphicsStyle;
                                cadlayers.Nameoflayer = polygraphicalid.GraphicsStyleCategory.Name;

                                if (cadlayers.Nameoflayer == SelectedLayer.Nameoflayer)
                                {
                                    Outline putline = polyline.GetOutline();
                                    XYZ firstp = putline.MaximumPoint;
                                    XYZ secondp = putline.MinimumPoint;
                                    XYZ Linemid = midpoint(firstp.X, secondp.X, firstp.Y, secondp.Y, firstp.Z, secondp.Z);

                                    Level collevel = null;
                                    var elementsColumns = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Columns).WhereElementIsElementType().ToElements();
                                    var levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().ToList();
                                    foreach (var level in levels)
                                    {
                                        if (level.Name == "Level 1")
                                        {
                                            collevel = level;
                                        }
                                    }
                                    FamilySymbol fs = null;
                                    foreach (var ele in elementsColumns)
                                    {
                                        if (ele.Name == SelectedColumntype.columntypes)
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
                                    }
                                }
                            }
                        }

                    }




                }
            }
        }
        //method to get the Midpoint of the column
        private static XYZ midpoint(double x1, double x2, double y1, double y2, double z1, double z2)
        {

            XYZ midP = new XYZ((x1 + x2) / 2, (y1 + y2) / 2, (z1 + z2) / 2);
            return midP;

        }
        #endregion
    }
}
