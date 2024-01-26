using Microsoft.Win32;
using ProjectRevitFinal.View;

namespace ProjectRevitFinal.Revitcontext.Command
{

    public class ImportCad
    {

        public void GET_AutoCAD_FilePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DWG Files|*.dwg|All Files|*.*";
            openFileDialog.Title = "Select DWG File";

            //Step3: Insert File Path into Textbox 
            if (openFileDialog.ShowDialog() == true)
            {
                //---------------------------------------------------------------------------------------------
                //1# Get the selected file path and display it in the TextBox
                Columns.GetData.Path.Text = openFileDialog.FileName;
            }
        }
        //public static void importfilcadpath(string filepath)
        //{
        //    Document doc = OpenWindowCommand.doc;

        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "DWG Files|*.dwg|All Files|*.*";
        //    openFileDialog.Title = "Select DWG File";

        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        //1# Get the selected file path and display it in the TextBox
        //        string filePath = openFileDialog.FileName;


        //         // Create DWG import options
        //         DWGImportOptions dwgOptions = new DWGImportOptions();
        //        try
        //        {

        //            var currentview = doc.ActiveView;
        //            ElementId elementid = null;
        //            using (Transaction tr = new Transaction(doc, "importfilcadpath"))
        //            {
        //                tr.Start();

        //                // Import the DWG file

        //                var loadpath = doc.Import(filePath, dwgOptions, currentview, out elementid);

        //                // Commit the transaction
        //                tr.Commit();

        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            TaskDialog.Show("Error", "Failed to import DWG file. Error: " + ex.Message);
        //        }
        //    }
        //}


    }
}
