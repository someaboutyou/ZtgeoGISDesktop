using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.MainFormDocument;

namespace Ztgeo.Gis.Winform.MainFormStatusBar
{
    public class MultiThreadStatusEndEventData: Status4DocumenEventData
    {
        public MultiThreadStatusEndEventData(IDocument document,
            IDocumentControl documentControl) {
            this.Document = document;
            this.DocumentControl = documentControl;
        }
    }
}
