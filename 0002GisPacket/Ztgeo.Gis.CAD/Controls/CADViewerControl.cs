using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.MainFormLayer;
using Ztgeo.Gis.Winform.MainFormProperty;

namespace Ztgeo.Gis.CAD.Controls
{
    public partial class CADViewerControl : UserControl, IDocumentControl,Abp.Dependency.ITransientDependency
    {
        private bool isActive;
        public CADViewerControl(ICADViewDocument cadViewDocument)
        { 
            InitializeComponent();
            this.Document = cadViewDocument;
            cadViewDocument.InitParams(this);
        }

        public IDocument Document { get; set; } 

        public ILayerControl LayerControl { get; set; }

        public IPropertiesControl PropertiesControl { get; set; }

        public bool IsActive { get { return this.isActive; }  private set { this.isActive = value; } }

        public Image DocumentImage { get; private set; }

        public void Close()
        {
            this.Document.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        public void SetActive() {

            this.Focus();
            this.isActive = true;
        }

        public void SetUnActive() {

            this.isActive = false;
        }

        public void SetBusyCursor() {
            this.Cursor = Cursors.WaitCursor;
        }

        public void SetCommonCursor() {
            this.Cursor = Cursors.Default;
        }

        public void OpenFile(string filePath) {

            this.Document.LoadFromFile(filePath);
        }
    }
}
