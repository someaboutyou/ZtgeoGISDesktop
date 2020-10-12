using Abp.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.MainFormDocument.Resources;

namespace Ztgeo.Gis.Winform.Resources
{
    public interface IResourceMetaDataProvider:Abp.Dependency.ISingletonDependency {
        ITypeList<IDocumentResourceMetaData> DocumentResourceMetaDataProviders { get; }
        ITypeList<IResourceMetaData> ResourceMetaDataProviders { get; }
        ITypeList<IResourceMetaData> AllResourceMetaDataProviders { get; }
    }
    public class ResourceMetaProvider: IResourceMetaDataProvider
    {
        private ITypeList<IDocumentResourceMetaData> _DocumentResourceMetaDataProviders = new TypeList<IDocumentResourceMetaData>();
       public ITypeList<IDocumentResourceMetaData> DocumentResourceMetaDataProviders { get { return _DocumentResourceMetaDataProviders; } }
        private ITypeList<IResourceMetaData> _ResourceMetaDataProviders = new TypeList<IResourceMetaData>();
        public ITypeList<IResourceMetaData> ResourceMetaDataProviders { get {
                return _ResourceMetaDataProviders;
         } }

        public ITypeList<IResourceMetaData> AllResourceMetaDataProviders {
            get {
                ITypeList<IResourceMetaData> allTypes = new TypeList<IResourceMetaData>();
                foreach(Type t in _DocumentResourceMetaDataProviders)
                {
                    if (!allTypes.Contains(t)) {
                        allTypes.Add(t);
                    }
                }
                foreach (Type t in _ResourceMetaDataProviders)
                {
                    if (!allTypes.Contains(t))
                    {
                        allTypes.Add(t);
                    }
                }
                return allTypes;
            }
        }
    }
}
