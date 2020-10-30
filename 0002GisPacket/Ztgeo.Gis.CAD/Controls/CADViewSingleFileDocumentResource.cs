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
using Ztgeo.Gis.AbpExtension;
using Ztgeo.Gis.CAD.Toolbars;
using Ztgeo.Gis.Winform.Actions;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.MainFormDocument.Resources;
using Ztgeo.Gis.Winform.Resources;
using Ztgeo.Utils;

namespace Ztgeo.Gis.CAD.Controls
{
    public class CADViewSingleFileDocumentResource : SingleFileDocumentResourceBase 
    { 
        public CADViewSingleFileDocumentResource(IocManager iocManager,
            IDocumentManager documentManager 
            ) :base(iocManager, documentManager)
        { 
        }  
        public override ISingleFileDocumentResourceMetaData SingleFileDocumentResourceMetaData {
            get {
                return this.IocManager.Resolve<CADViewSingleFileDocumentResourceMetaData>();
            }
        }  
        public override IType<IDocumentControl> DocumentControlType { get { return new AbpType<IDocumentControl>(typeof(CADViewerControl)); } }
    }
}
