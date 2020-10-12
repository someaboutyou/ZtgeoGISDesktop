using Abp.Dependency;
using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.MainFormDocument.Resources;
using Ztgeo.Utils;

namespace Ztgeo.Gis.CAD.Controls
{
    public class CADViewSingleFileDocumentResource : ISingleFileDocumentResource 
    {
        public IocManager IocManager { get; set; } 
        public string FilePath { get; set; } 

        public string ExtensionName { get { return Path.GetExtension(FilePath); } }

        public ISingleFileDocumentResourceMetaData SingleFileDocumentResourceMetaData {
            get {
                return this.IocManager.Resolve<CADViewSingleFileDocumentResourceMetaData>();
            }
        }

        public DocumentResourceType DocumentResourceType { get { return SingleFileDocumentResourceMetaData.DocumentResourceType; } }
    }
}
