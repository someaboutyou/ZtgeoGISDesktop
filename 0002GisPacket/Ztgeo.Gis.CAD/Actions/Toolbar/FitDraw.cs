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
    public class FitDraw : IToolbarAction
    {
        private readonly IDocumentManager DocumentManager;
        public FitDraw(IDocumentManager documentManager) {
            DocumentManager = documentManager;
        }
        public WinformToolbar WinformToolbar { private get; set; }

        public void Excute()
        {
            IDocument document = DocumentManager.GetActiveDocument;
            if (document is ICADViewDocument)
            {
                ICADViewDocument cadViewDocument = (ICADViewDocument)document;
                cadViewDocument.ResetScaling();
            } 
        }
    }
}
