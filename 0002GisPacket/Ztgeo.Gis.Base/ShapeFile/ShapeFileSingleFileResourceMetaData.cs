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
using Ztgeo.Gis.Winform.Resources;
using Ztgeo.Utils;

namespace Ztgeo.Gis.Base.ShapeFile
{
    public class ShapeFileSingleFileResourceMetaData : IMultiFileResourceMetaData
    {
        private readonly IocManager IocManager;
        public ShapeFileSingleFileResourceMetaData(IocManager iocManager) {
            IocManager = iocManager;
        }  
 
        public IList<string> FindOtherFiles(string identifiedFilePath)
        {
            string[] files = Directory.GetFiles(Path.GetDirectoryName(identifiedFilePath));
            string fileName = Path.GetFileNameWithoutExtension(identifiedFilePath);
            return files.Where(f => Path.GetFileNameWithoutExtension(f).Equals(fileName)&& !f.Equals(fileName)).ToList();
        }

        public bool Identified(string identifiedFilePath)
        {
            return Path.GetExtension(identifiedFilePath).Equals(".shp");
        }
        //public List<string> SelectFilterExtensionName { get { return new List<string> { ".shp"}; } }

        public System.Drawing.Image Icon { get { return AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.Base.Icons.shapefile16.png"); } }

        public string Name { get { return "Shp文件"; } }

        public ResourceStorageMode ResourceStorageMode { get { return ResourceStorageMode.MultiFile; } }

        public Gis.AbpExtension.IType<IResourceAction> ClickResourceActionType
        { 
            get { return null; }
        }

        public Gis.AbpExtension.IType<IResourceAction> DoubleClickResourceActionType
        { 
            get { return null; }
        }

        public ITypeList<IContextMenuItemAction> ContextActionTypes
        { 
            get { return null; }
        }

        public Gis.AbpExtension.IType<IResource> ResourceType
        {
            get { return AbpType.GetType<IResource>(typeof(ShapeFileSingleFileResource)); }
        }
         
    }
}
