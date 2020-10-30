using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.AbpExtension;
using Ztgeo.Gis.Winform.Resources;

namespace Ztgeo.Gis.Winform.MainFormDocument.Resources
{
    public interface ISingleFileDocumentResource : IDocumentResource, ISingleFileResource
    { 
        /// <summary>
        /// 拓展名
        /// </summary> 
        /// <summary>
        /// 单文件资源元数据
        /// </summary>
        ISingleFileDocumentResourceMetaData SingleFileDocumentResourceMetaData { get; } 
    }

    public abstract class SingleFileDocumentResourceBase : ISingleFileDocumentResource
    {
        protected readonly IocManager IocManager;
        protected readonly IDocumentManager DocumentManager;
        public SingleFileDocumentResourceBase(IocManager iocManager, IDocumentManager documentManager) {
            IocManager = iocManager;
            DocumentManager = documentManager;
        }
        public abstract ISingleFileDocumentResourceMetaData SingleFileDocumentResourceMetaData { get; } 

        public abstract IType<IDocumentControl> DocumentControlType { get; }

        public virtual string FullName { get; set; }

        public virtual string ExtensionName { get { return Path.GetExtension(FullName); } }

        protected string caption;
        public virtual string Caption
        {
            get
            {
                if (!string.IsNullOrEmpty(caption))
                {
                    return caption;
                }
                if (string.IsNullOrEmpty(this.FullName))
                    return "";
                else
                {
                    caption = Path.GetFileName(FullName);
                    return caption;
                }
            }
            set { caption = value; }
        }
        public virtual IResourceMetaData ResourceMetaData { get { return SingleFileDocumentResourceMetaData; } }

        public virtual void Open()
        {
            if (string.IsNullOrEmpty(FullName))
            {
                throw new FileNotFoundException("未找到文件：" + FullName);
            }
            if (DocumentManager.DocumentList != null && DocumentManager.DocumentList.Count > 0)
            { //假如文档中已经存在，激活
                foreach (IDocumentControl documentControl in DocumentManager.DocumentList)
                {
                    if (documentControl.Document.DocumentResource is ISingleFileDocumentResource &&
                        ((ISingleFileDocumentResource)documentControl.Document.DocumentResource).FullName.Equals(FullName)
                        )
                    {
                        DocumentManager.SetDocumentControlActive(documentControl);
                        return;
                    }
                }
            }
            IDocumentControl docControl = IocManager.Resolve(DocumentControlType.Type) as IDocumentControl;
            if (docControl != null)
            {
                IDocumentControl documentControl = DocumentManager.AddADocument(DocumentControlType,Path.GetFileName(FullName));
                documentControl.Open(this); 
            }
            else
            {
                throw new DocumentOpenException("未找到打开文档的Control," + DocumentControlType.TypeName);
            }
        }

        public void Save()
        {
            //throw new NotImplementedException();
        }
    }
}
