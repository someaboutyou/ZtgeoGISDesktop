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
            AddDocument();
            AddDocument();
            AddDocument();
            ScaleElements();
        }
        void biAddDocumentClick(object sender, ItemClickEventArgs e)
        {
            AddDocument();
        }
        int i = 0;
        void AddDocument()
        {
            XtraUserControl child = new XtraUserControl();
            DocumentSettings settings = new DocumentSettings();
            settings.Caption = "Document" + (i++).ToString();
            //settings.Image = svgImageCollection1.GetImage(i % (svgImageCollection1.Count - 1));
            DocumentSettings.Attach(child, settings);
            child.Padding = new Padding(16);
            LabelControl label = new LabelControl();
            label.Text = "Document" + (i++).ToString();
            label.AutoSizeMode = LabelAutoSizeMode.Vertical;
            label.Parent = child;
            label.Dock = DockStyle.Fill;
            tabbedView.AddDocument(child);
        }
    }
}
