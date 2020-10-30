using Abp.Dependency;
using CadastralManagementDataSync.Documents;
using CadastralManagementDataSync.Resource;
using DevExpress.XtraTab;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.MainFormDocument.Resources;
using Ztgeo.Gis.Winform.MainFormLayer;
using Ztgeo.Gis.Winform.MainFormProperty;
using Ztgeo.Gis.Winform.ToolBar;
using Ztgeo.Utils;

namespace CadastralManagementDataSync.Controls
{
    public class SyncDataResourceDocumentControl : XtraTabControl, IDocumentControl, Abp.Dependency.ITransientDependency
    {
        private SyscDataDocument syscDataDocument;
        private readonly IocManager iocManager;
        private readonly IWinformToolbarViewManager winformToolbarViewManager;
        public SyncDataResourceDocumentControl(SyscDataDocument _syscDataDocument, IWinformToolbarViewManager _winformToolbarViewManager,
            IocManager _iocManager) {
            syscDataDocument = _syscDataDocument;
            _syscDataDocument.InitDocument(this);
            iocManager = _iocManager;
            winformToolbarViewManager = _winformToolbarViewManager;
        }
        public IDocument Document { get { return syscDataDocument; } } 

        public Image DocumentImage { get { return AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "CadastralManagementDataSync.Icons.DataSync16.png"); } }

        public ILayerControl LayerControl
        {
            get
            {
                return null;
            }
        }

        public IPropertiesControl PropertiesControl {
            get {
                return null; 
            }
        }

        public void Close()
        {
            this.Document.Close();

        } 

        public void SetBusyCursor()
        {
            this.Cursor = Cursors.WaitCursor;
        }

        public void SetCommonCursor()
        {
            this.Cursor = Cursors.Default;
        }

        public void Open (IDocumentResource resource) { 
            this.Document.LoadFromResource(resource); 
        }

        public void Activated()
        {
            winformToolbarViewManager.ClearToolbars();
        }
    }
}
