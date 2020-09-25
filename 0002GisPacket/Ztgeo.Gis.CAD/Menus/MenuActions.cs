using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.CAD.Controls;
using Ztgeo.Gis.Winform.MainFormDocument;

namespace Ztgeo.Gis.CAD.Menus
{
    public static class MenuActions
    {
        public static void OpenCADFile(IDocumentManager documentManager) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory ="C:\\";//注意这里写路径时要用c:\而不是c:
            openFileDialog.Filter = "dxf文件 (*.dxf)|*.dxf|dwg (*.dwg)|*.dwg|All files (*.*)|*.*";   
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                string fileName = openFileDialog.FileName;
                IDocumentControl documentControl = documentManager.AddADocument<CADViewerControl>(); 
            }
        }
    }
}
