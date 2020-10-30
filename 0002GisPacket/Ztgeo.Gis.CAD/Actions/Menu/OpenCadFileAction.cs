using Abp.Dependency;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.CAD.Controls;
using Ztgeo.Gis.Winform.Actions;
using Ztgeo.Gis.Winform.Menu;
using Ztgeo.Utils;

namespace Ztgeo.Gis.CAD.Actions.Menu
{
    public class OpenCadFileAction : IMenuAction
    {
        private readonly CADViewSingleFileDocumentResourceMetaData cadFileDocMetaData;
        private readonly IocManager IocManager;
        public OpenCadFileAction(IocManager iocManager) {
            cadFileDocMetaData = iocManager.Resolve<CADViewSingleFileDocumentResourceMetaData>();
            IocManager = iocManager;
        }

        public WinformMenu SenderMenu { set; private get; } 

        public void Excute()
        {
            XtraOpenFileDialog openFileDialog = new XtraOpenFileDialog();
            openFileDialog.Filter = FileHelper.GetOpenFileFilter(cadFileDocMetaData.SelectFilterExtensionName,true);
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                var cadViewSingleFileDocumentResource = IocManager.Resolve( cadFileDocMetaData.TypeOfDocumentResource.Type) as CADViewSingleFileDocumentResource;
                cadViewSingleFileDocumentResource.FullName = fileName;
                cadViewSingleFileDocumentResource.Open(); 
                //IDocumentControl documentControl = documentManager.AddADocument<CADViewerControl>(Path.GetFileNameWithoutExtension(fileName));
                //((CADViewerControl)documentControl).OpenFile(fileName);
                //toolbarControl.CADFileOpen();
            }
        }
    }
}
