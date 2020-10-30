using Abp.Dependency;
using CadastralManagementDataSync.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.AbpExtension;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.MainFormDocument.Resources;
using Ztgeo.Gis.Winform.Resources;

namespace CadastralManagementDataSync.Resource
{
    /// <summary>
    /// 同步文件的资源
    /// </summary>
    public class SyncDataResource : SingleFileDocumentResourceBase
    { 
        public SyncDataResource(IocManager iocManager, IDocumentManager documentManager):base(iocManager, documentManager) {
             
        }
       
        public override ISingleFileDocumentResourceMetaData SingleFileDocumentResourceMetaData
        {
            get
            {
                return this.IocManager.Resolve<SyncDataResourceMetaData>();
            }
        }
         
        public override IType<IDocumentControl> DocumentControlType { get { return AbpType.GetType<IDocumentControl>(typeof(SyncDataResourceDocumentControl)); } }
         
    }
}
