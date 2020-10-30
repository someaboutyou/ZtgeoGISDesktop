using Abp.Collections;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.AbpExtension;
using Ztgeo.Gis.Winform.Actions;
using Ztgeo.Gis.Winform.Actions.CommonAction;
using Ztgeo.Gis.Winform.MainFormDocument.Resources;
using Ztgeo.Gis.Winform.Resources;
using Ztgeo.Utils;

namespace CadastralManagementDataSync.Resource
{
    public class SyncDataResourceMetaData : ISingleFileDocumentResourceMetaData
    {
        private readonly IocManager IocManager;
        public SyncDataResourceMetaData(IocManager iocManager) {
            IocManager = iocManager;
        }
        public static string ExtenstionName{ get { return ".sync"; } }
        public List<string> SelectFilterExtensionName { get { return new List<string> { ExtenstionName }; } }
        public IType<IDocumentResource> TypeOfDocumentResource { get { return AbpType.GetType<IDocumentResource>(typeof(SyncDataResource)); } }
        public Image Icon { get { return AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "CadastralManagementDataSync.Icons.sync16.png"); } }

        public string Name { get { return "同步文件"; } }

        public ResourceStorageMode ResourceStorageMode { get { return ResourceStorageMode.SingleFile; } }

        public IType<IResourceAction> ClickResourceActionType { get { return null; } }

        public IType<IResourceAction> DoubleClickResourceActionType { get { return AbpType.GetType<IResourceAction>(typeof(OpenResourceAction)); } }

        public ITypeList<IContextMenuItemAction> ContextActionTypes { get { return null; } }

        public IType<IResource> ResourceType { get { return AbpType.GetType<IResource>(typeof(SyncDataResource)); } }

        public IList<ISingleFileResource> FindSingleFileResourceInDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                string[] files = Directory.GetFiles(directoryPath);
                return files.Where(f => this.Identified(f)).Select(f =>
                {
                    var r = IocManager.Resolve<SyncDataResource>();
                    r.FullName = f;
                    return (ISingleFileResource)r;
                }).ToList();
            }
            else
            {
                throw new DirectoryNotFoundException(directoryPath + ",目录不存在");
            }
        }

        public bool Identified(string filePath)
        {
            return Path.GetExtension(filePath).Equals(ExtenstionName);
        }
    }
}
