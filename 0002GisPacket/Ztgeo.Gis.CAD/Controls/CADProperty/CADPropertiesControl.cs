using CADImport.FaceModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.MainFormProperty;

namespace Ztgeo.Gis.CAD.Controls.CADProperty
{
    public class CADPropertiesControl : CADPropertyGrid, ICADPropertiesControl
    {
        public CADPropertiesControl() { 
            
        }
        public IDocument ActiveDocument { get; set; }

        public bool IsClosed { get; set; }

        public void Clear()
        {
            this.SelectedObject = null;
        }

        public void SetPropertyInfos()
        {
            CADImportFace.EntityPropertyGrid = this;
        }

    }
}
