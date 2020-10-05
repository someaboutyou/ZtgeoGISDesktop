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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CADPropertiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "CADPropertiesControl";
            this.Size = new System.Drawing.Size(383, 450);
            this.ResumeLayout(false);

        }

        public void SetSelectObjectNull() {
            this.SelectedObject = null;
        }
    }
}
