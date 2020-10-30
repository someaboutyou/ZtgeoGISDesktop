using Abp.Collections;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Resources
{
    public class LocalResourceManager:ILocalResourceManager
    {
        private readonly IocManager iocManager;
        private readonly IResourceMetaDataProvider resourceMetaDataProvider;
        public LocalResourceManager(IocManager _iocManager,
            IResourceMetaDataProvider _resourceMetaDataProvider
            ) {
            iocManager = _iocManager;
            resourceMetaDataProvider = _resourceMetaDataProvider;
        }

        public IList<IResource> GetAllLocalResource(string directoryPath, out IList<string> restDirs, out IList<string> restFiles)
        {
            //throw new NotImplementedException();
            ITypeList<IResourceMetaData> resourceMetaDataTypes= resourceMetaDataProvider.AllResourceMetaDataProviders;
            IList<IResourceMetaData> resourceMetaDatas = new List<IResourceMetaData>();
            foreach (Type resourceMetaDataType in resourceMetaDataTypes) {
                resourceMetaDatas.Add(iocManager.Resolve(resourceMetaDataType) as IResourceMetaData);
            }
            return GetLocalResource(directoryPath, resourceMetaDatas, out restDirs,out restFiles);
        }

        public IList<IResource> GetLocalResource(string directoryPath, IList<IResourceMetaData> metaDataFilter, out IList<string> restDirs, out IList<string> restFiles)
        {
            IList<IResource> resources = new List<IResource>();
            restDirs = new List<string>();
            restFiles = new List<string>();
            string[] dirs = Directory.GetDirectories(directoryPath);
            string[] files = Directory.GetFiles(directoryPath);
            bool[] dirStatus = null;
            bool[] fileStatus = null;
            if (dirs.Length > 0)
            {
                dirStatus = new bool[dirs.Length];
                foreach (IResourceMetaData metaData in metaDataFilter)
                {
                    if (metaData.ResourceStorageMode == ResourceStorageMode.SingleFolder
                        && metaData is ISingleFolderResourceMetaData)
                    {
                        var singleFolderResourceMetaData = metaData as ISingleFolderResourceMetaData;
                        for (int i = 0; i < dirs.Length; i++)
                        {
                            if (singleFolderResourceMetaData.Identified(dirs[i]))
                            {
                                dirStatus[i] = true;
                                ISingleFolderResource resource = iocManager.Resolve(singleFolderResourceMetaData.ResourceType.Type) as ISingleFolderResource;
                                resource.Caption = Path.GetFileName(dirs[i]);
                                resource.FolderPath = dirs[i];
                                resources.Add(resource);
                                // items.Add(new ResourceItem(dirs[i], resource));
                            }
                        }
                    }
                    else if (metaData.ResourceStorageMode == ResourceStorageMode.MultiFolder
                        && metaData is IMultiFolderResourceMetaData)
                    {
                        var multiFolderResourceMetaData = metaData as IMultiFolderResourceMetaData;
                        for (int i = 0; i < dirs.Length; i++)
                        {
                            if (multiFolderResourceMetaData.Identified(dirs[i]))
                            {
                                dirStatus[i] = true;
                                var otherFolders = multiFolderResourceMetaData.FindOtherFolders(dirs[i]);
                                if (otherFolders.Count > 0)
                                { //设置otherFolder 的状态
                                    foreach (string f in otherFolders)
                                    {
                                        for (int j = 0; j < dirs.Length; j++)
                                        {
                                            if (dirs[j].Equals(f))
                                            {
                                                dirStatus[j] = true;
                                            }
                                        }
                                    }
                                }
                                IMultiFolderResource resource = iocManager.Resolve(multiFolderResourceMetaData.ResourceType.Type) as IMultiFolderResource;
                                resource.Caption = Path.GetFileName(dirs[i]);
                                resource.MainFolder = dirs[i];
                                resource.OtherFolders = otherFolders;
                                resources.Add(resource);
                                //items.Add(new ResourceItem(dirs[i], resource));
                            }
                        }
                    }
                }
            }
            if (files.Length > 0)
            {
                fileStatus = new bool[files.Length];
                foreach (IResourceMetaData metaData in metaDataFilter)
                {
                    if (metaData.ResourceStorageMode == ResourceStorageMode.SingleFile
                        && metaData is ISingleFileResourceMetaData)
                    {
                        var singleFileResourceMetaData = metaData as ISingleFileResourceMetaData;
                        for (int i = 0; i < files.Length; i++)
                        {
                            if (singleFileResourceMetaData.Identified(files[i]))
                            {
                                fileStatus[i] = true;
                                ISingleFileResource resource = iocManager.Resolve(singleFileResourceMetaData.ResourceType.Type) as ISingleFileResource;
                                resource.Caption = Path.GetFileName(files[i]);
                                resource.FullName = files[i];
                                resources.Add(resource);
                                //items.Add(new ResourceItem(files[i], resource));
                            }
                        }
                    }
                    else if (metaData.ResourceStorageMode == ResourceStorageMode.MultiFile
                        && metaData is IMultiFileResourceMetaData)
                    {
                        var muiltFileResourceMetaData = metaData as IMultiFileResourceMetaData;
                        for (int i = 0; i < files.Length; i++)
                        {
                            if (muiltFileResourceMetaData.Identified(files[i]))
                            {
                                fileStatus[i] = true;
                                var otherFiles = muiltFileResourceMetaData.FindOtherFiles(files[i]);
                                if (otherFiles.Count > 0)
                                {//设置otherFile 的状态
                                    foreach (string f in otherFiles)
                                    {
                                        for (int j = 0; j < files.Length; j++)
                                        {
                                            if (files[j].Equals(f))
                                            {
                                                fileStatus[j] = true;
                                            }
                                        }
                                    }
                                }
                                IMultiFileResource resource = iocManager.Resolve(muiltFileResourceMetaData.ResourceType.Type) as IMultiFileResource;
                                resource.Caption = Path.GetFileName(files[i]);
                                resource.MainFilePath = files[i];
                                resource.OtherFilePath = otherFiles;
                                resources.Add(resource);
                                //items.Add(new ResourceItem(files[i], resource));
                            }
                        }
                    }
                    else if (metaData.ResourceStorageMode == ResourceStorageMode.MixedFileFold
                        && metaData is IMixedFileFoldResourceMetaData)
                    { //文件和文件夹混合 
                        var mixedFileFoldResourceMetaData = metaData as IMixedFileFoldResourceMetaData;
                        for (int i = 0; i < files.Length; i++)
                        {
                            if (mixedFileFoldResourceMetaData.Identified(files[i]))
                            {
                                fileStatus[i] = true;
                                var otherfiles = mixedFileFoldResourceMetaData.FindOtherFiles(files[i]);
                                var otherFolders = mixedFileFoldResourceMetaData.FindOtherFolders(files[i]);
                                if (otherfiles.Count > 0)
                                {//设置otherFile 的状态
                                    foreach (string f in otherfiles)
                                    {
                                        for (int j = 0; j < files.Length; j++)
                                        {
                                            if (files[j].Equals(f))
                                            {
                                                fileStatus[j] = true;
                                            }
                                        }
                                    }
                                }
                                if (otherFolders.Count > 0)
                                { //设置otherFolder 的状态
                                    foreach (string f in otherFolders)
                                    {
                                        for (int j = 0; j < dirs.Length; j++)
                                        {
                                            if (dirs[j].Equals(f))
                                            {
                                                dirStatus[j] = true;
                                            }
                                        }
                                    }
                                }
                                IMixedFileFoldResource resource = iocManager.Resolve(mixedFileFoldResourceMetaData.ResourceType.Type) as IMixedFileFoldResource;
                                resource.Caption = Path.GetFileName(files[i]);
                                resource.MainPath = files[i];
                                resource.OtherFilePaths = otherfiles;
                                resource.OtherFolderPaths = otherFolders; 
                                resources.Add(resource);
                                //items.Add(new ResourceItem(files[i], resource));
                            }
                        }
                    }
                }
            }
            for (int k = 0; k < dirs.Length; k++)
            {
                if (!dirStatus[k])
                {
                    //items.Add(new DirectoryItem(dirs[k]));
                    restDirs.Add(dirs[k]);
                }
            }
            for(int k = 0; k < files.Length; k++)
            {
                if (!fileStatus[k])
                {
                    restFiles.Add(files[k]);
                }
            }
            return resources;
        }
    }
}
