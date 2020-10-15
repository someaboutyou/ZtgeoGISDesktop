using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.DXperience.Demos;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Docking2010.Views.Tabbed;
using DevExpress.XtraBars;
using DevExpress.Skins;

namespace ZtgeoGISDesktop.Controls
{
    public partial class DocumentManagerDocking : XtraUserControl
    {
        public StandaloneBarDockControl StandaloneBarDockControl { get { return this.standaloneBarDockControl1; } }

        public Control PropertiesControl { get { return this.PropertiesDockPanel; } }

        public Control ResourceControl { get { return this.ResourceContainer; } }
        public TabbedView TabbedView { get { return this.tabbedView; } }
        public DocumentManagerDocking()
        {
            InitializeComponent(); 
        }
        void ScaleElements(DockPanel panel = null)
        {
            if (panel == null)
            {
                foreach (DockPanel rootPanel in dockManager1.RootPanels)
                    ScaleElements(rootPanel);
                return;
            }
            panel.Width = (int)Math.Round(panel.Width * DpiProvider.Default.DpiScaleFactor);
            panel.Height = (int)Math.Round(panel.Height * DpiProvider.Default.DpiScaleFactor);
            foreach (Control child in panel.Controls)
            {
                if (child is DockPanel)
                    ScaleElements(child as DockPanel);
            }
        }
        void Form1_Load(object sender, EventArgs e)
        { 
            ScaleElements();
        }

        private void PropertiesDockPanel_Click(object sender, EventArgs e)
        {

        }
    }
}
