using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.CAD.Controls;
using Ztgeo.Gis.CAD.Toolbars;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.Menu;
using Ztgeo.Gis.Winform.ToolBar;

namespace Ztgeo.Gis.CAD.Menus
{
    public static class MenuActions
    {
        public static void OpenCADFile(IDocumentManager documentManager, ICADToolbarControl toolbarControl) {
            OpenFileDialog openFileDialog = new OpenFileDialog(); 
            openFileDialog.Filter = "dxf文件 (*.dxf)|*.dxf|dwg (*.dwg)|*.dwg|All files (*.*)|*.*";   
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                string fileName = openFileDialog.FileName;
                IDocumentControl documentControl = documentManager.AddADocument<CADViewerControl>(Path.GetFileNameWithoutExtension(fileName));
                ((CADViewerControl)documentControl).OpenFile(fileName);
                toolbarControl.CADFileOpen();
            }
        }
         
    }
}
