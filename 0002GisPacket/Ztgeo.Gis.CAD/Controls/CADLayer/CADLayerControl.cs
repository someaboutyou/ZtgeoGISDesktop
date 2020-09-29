using CADImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.MainFormLayer;

namespace Ztgeo.Gis.CAD.Controls.CADLayer
{
    public class CADLayerControl : ICADLayerControl
    {
        #region activeDocument
        private IDocument _activeDocument = null;
         
        public IDocument ActiveDocument
        {
            get
            {
                return this._activeDocument;
            }
            set
            {
                if (_activeDocument != value)
                {
                    SetLayerInfo();
                }
                _activeDocument = value;
            }
        }
        protected virtual ICADViewDocument CADViewActiveDocument
        {
            get
            {
                if (_activeDocument != null)
                {
                    return (ICADViewDocument)_activeDocument;
                }
                return null;
            }
        }
        #endregion
         
        public bool IsClosed { get; set; }

        public IDocumentControl ActiveDocumentControl { get; set; }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void SetLayerInfo()
        {
            if (this.CADViewActiveDocument != null&& this.CADViewActiveDocument.Image!=null) {
                if (this.CADViewActiveDocument.Image.Converter.Layers.Count > 0) {
                    CADEntityCollection layerEntities = this.CADViewActiveDocument.Image.Converter.Layers;

                }
            }
        }

        public void Show()
        {
            throw new NotImplementedException();
        }
    }
}
