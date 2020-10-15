using Abp.Dependency;
using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.CAD.Toolbars;
using Ztgeo.Gis.Winform.Actions;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.MainFormDocument.Resources;
using Ztgeo.Gis.Winform.Resources;
using Ztgeo.Utils;

namespace Ztgeo.Gis.CAD.Controls
{
    public class CADViewSingleFileDocumentResource : ISingleFileDocumentResource 
    {
        private readonly IocManager IocManager;
        private readonly IDocumentManager DocumentManager;
        private readonly ICADToolbarControl ToolbarControl;
        public CADViewSingleFileDocumentResource(IocManager iocManager,
            IDocumentManager documentManager, 
            ICADToolbarControl toolbarControl
            ) {
            IocManager = iocManager;
            DocumentManager = documentManager;
            ToolbarControl = toolbarControl;
        }
        public string FilePath { get; set; } 

        public string ExtensionName { get { return Path.GetExtension(FilePath); } } 

        public ISingleFileDocumentResourceMetaData SingleFileDocumentResourceMetaData {
            get {
                return this.IocManager.Resolve<CADViewSingleFileDocumentResourceMetaData>();
            }
        }
         
        public IResourceMetaData ResourceMetaData { get { return SingleFileDocumentResourceMetaData; } }

        public Type DocumentControlType { get { return typeof(CADViewerControl); } }
        public IResourceAction ClickAction { get { return null; } }

        public IResourceAction DoubleClickAction { get { return null; } }

        public IOrderedEnumerable<IContextMenuItemAction> ContextActions { get { return null; } }

        public string Caption { get { if (string.IsNullOrEmpty(this.FilePath)) return ""; else return Path.GetFileName(FilePath); } } 

        public void Open()
        {
            if (string.IsNullOrEmpty(FilePath)) {
                throw new FileNotFoundException("未发现文件：" + FilePath);
            }
            IDocumentControl docControl = IocManager.Resolve(DocumentControlType) as IDocumentControl;
            if (docControl != null)
            {
                 IDocumentControl documentControl = DocumentManager.AddADocument<CADViewerControl>(Path.GetFileNameWithoutExtension(FilePath));
                 ((CADViewerControl)documentControl).OpenFile(FilePath);
                 ToolbarControl.CADFileOpen();
            }
            else {
                throw new DocumentOpenException("未找到打开文档的Control,"+ DocumentControlType.Name);
            }
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
