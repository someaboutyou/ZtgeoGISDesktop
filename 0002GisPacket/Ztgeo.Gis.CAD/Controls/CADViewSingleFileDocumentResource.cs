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
        public CADViewSingleFileDocumentResource(IocManager iocManager,
            IDocumentManager documentManager
            ) {
            IocManager = iocManager;
            DocumentManager = documentManager;

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

        public IDocumentControl IDocumentControl => throw new NotImplementedException();

        public void Open()
        { 
            IDocumentControl docControl = IocManager.Resolve(DocumentControlType) as IDocumentControl;
            if (docControl != null)
            {
                 IDocumentControl documentControl = DocumentManager.AddADocument<CADViewerControl>(Path.GetFileNameWithoutExtension(fileName));
                //((CADViewerControl)documentControl).OpenFile(fileName);
                //toolbarControl.CADFileOpen();
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
