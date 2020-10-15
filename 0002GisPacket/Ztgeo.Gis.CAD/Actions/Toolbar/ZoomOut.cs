using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.CAD.Controls;
using Ztgeo.Gis.Winform.Actions;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.ToolBar;

namespace Ztgeo.Gis.CAD.Actions.Toolbar
{
    public class ZoomOut : IToolbarAction
    {
        public WinformToolbar WinformToolbar { private get; set; }
        private readonly IDocumentManager DocumentManager;
        public ZoomOut(IDocumentManager documentManager) {
            DocumentManager = documentManager;
        }
        public void Excute()
        {
            IDocument document = DocumentManager.GetActiveDocument;
            if (document != null && document is ICADViewDocument)
            {
                ((ICADViewDocument)document).ZoomOut();
            }
        }
    }
}
