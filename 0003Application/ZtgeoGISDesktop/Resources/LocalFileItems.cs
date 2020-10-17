using Abp.Dependency;
using DevExpress.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.MetadataServices;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.Resources;
using Ztgeo.Utils;

namespace ZtgeoGISDesktop.Resources
{
    public class RootItem : Item
    {
        public RootItem() : base("Root") { }
        public override List<Item> GetDirectories()
        {
            return new List<Item>() { new ThisPCItem() };
        }
        public override Image Image
        {
            get { return null; }
            set { }
        }
        public override List<CustomFileInfo> GetFilesSystemInfo()
        {
            return null;
        }
    }
    public class ThisPCItem : Item
    {
        public ThisPCItem() : base("我的电脑") { }
        public override List<Item> GetDirectories()
        {
            List<Item> items = new List<Item>(10);
            items.Add(new DirectoryItem(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)));
            items.Add(new DirectoryItem(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));
            items.Add(new DirectoryItem(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)));
            items.Add(new DirectoryItem(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)));
            items.Add(new DirectoryItem(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)));
            string[] drives = Environment.GetLogicalDrives();
            foreach (string drive in drives)
                items.Add(new DriveItem(drive));
            return items;
        }
        public override List<CustomFileInfo> GetFilesSystemInfo()
        {
            List<CustomFileInfo> infos = new List<CustomFileInfo>(15);
            infos.Add(CreateSystemFolderInfo(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory))));
            infos.Add(CreateSystemFolderInfo(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))));
            infos.Add(CreateSystemFolderInfo(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic))));
            infos.Add(CreateSystemFolderInfo(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures))));
            infos.Add(CreateSystemFolderInfo(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos))));
            string[] drives = Environment.GetLogicalDrives();
            foreach (string drive in drives)
            {
                DriveInfo driveInfo = new DriveInfo(drive);
                infos.Add(new CustomFileInfo()
                {
                    FullName = driveInfo.Name,
                    Name = drive,
                    Image = GetImage(drive),
                    TypeName = "Drive",
                    Type = FileType.Drive
                });
            }
            return infos;
        }
        CustomFileInfo CreateSystemFolderInfo(FileSystemInfo info)
        {
            return new CustomFileInfo()
            {
                Name = info.Name,
                FullName = info.FullName,
                DateCreated = info.CreationTime,
                DateModified = info.LastWriteTime,
                Image = GetImage(info.FullName),
                TypeName = "System Folder",
                Type = FileType.SystemFolder
            };
        }
        public override Image Image
        {
            get { return null; }
            set { }
        }
    }
    public class DriveItem : DirectoryItem
    {
        public DriveItem(string fullName) : base(fullName) { }
        protected override string GetDirectoryName(string path)
        {
            string _name = path.Replace(Path.DirectorySeparatorChar.ToString(), "");
            return _name;
        }
        protected override string GetDisplayName(string fullName)
        {
            return "本地磁盘(" + Name + ")";
        }
    }
    public class DirectoryItem : Item
    {
        public DirectoryItem(string fullName, IList<IResourceMetaData> metaDataFilter = null) : base(fullName, metaDataFilter) { }
        public override List<Item> GetDirectories()
        {
            List<Item> items = new List<Item>(10);
            try
            {
                if (Directory.Exists(FullName))
                {
                    string[] dirs = Directory.GetDirectories(FullName); 
                    string[] files = Directory.GetFiles(FullName);
                    if (MetaDataFilter == null || MetaDataFilter.Count == 0) //不顾虑
                    {
                        foreach (string dir in dirs)
                        {
                            var attributes = File.GetAttributes(dir);
                            if ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                                items.Add(new DirectoryItem(dir));
                        }
                        foreach (string file in files)
                        {
                            var attributes = File.GetAttributes(file);
                            if ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                                items.Add(new FileItem(file));
                        }
                    }
                    else {
                        bool[] dirStatus=null;
                        if (dirs.Length > 0) {
                              dirStatus = new bool[dirs.Length];
                            foreach (IResourceMetaData metaData in MetaDataFilter)
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
                                            ISingleFolderResource resource = IocManager.Instance.Resolve(singleFolderResourceMetaData.ResourceType.Type) as ISingleFolderResource;
                                            resource.Caption = Path.GetFileName(dirs[i]);
                                            resource.FolderPath = dirs[i];
                                            items.Add(new ResourceItem(dirs[i], resource));
                                        }
                                    }
                                }
                                else if (metaData.ResourceStorageMode == ResourceStorageMode.MultiFolder
                                    && metaData is IMultiFolderResourceMetaData) {
                                    var multiFolderResourceMetaData = metaData as IMultiFolderResourceMetaData;
                                    for (int i = 0; i < dirs.Length; i++) {
                                        if (multiFolderResourceMetaData.Identified(dirs[i])) {
                                            dirStatus[i] = true;
                                            var otherFolders = multiFolderResourceMetaData.FindOtherFolders(dirs[i]);
                                            if (otherFolders.Count > 0) { //设置otherFolder 的状态
                                                foreach (string f in otherFolders) {
                                                    for (int j = 0; j < dirs.Length; j++) {
                                                        if (dirs[j].Equals(f)) {
                                                            dirStatus[j] = true;
                                                        }
                                                    }
                                                }
                                            }
                                            IMultiFolderResource resource = IocManager.Instance.Resolve(multiFolderResourceMetaData.ResourceType.Type) as IMultiFolderResource;
                                            resource.Caption = Path.GetFileName(dirs[i]);
                                            resource.MainFolder = dirs[i];
                                            resource.OtherFolders = otherFolders;
                                            items.Add(new ResourceItem(dirs[i], resource));
                                        }
                                    }
                                }
                            }
                        } 
                        if (files.Length > 0) {
                            bool[] fileStatus = new bool[files.Length];
                            foreach (IResourceMetaData metaData in MetaDataFilter) {
                                if (metaData.ResourceStorageMode == ResourceStorageMode.SingleFile
                                    && metaData is ISingleFileResourceMetaData)
                                {
                                    var singleFileResourceMetaData = metaData as ISingleFileResourceMetaData;
                                    for (int i = 0; i < files.Length; i++)
                                    {
                                        if (singleFileResourceMetaData.Identified(files[i]))
                                        {
                                            fileStatus[i] = true;
                                            ISingleFileResource resource = IocManager.Instance.Resolve(singleFileResourceMetaData.ResourceType.Type) as ISingleFileResource;
                                            resource.Caption = Path.GetFileName(files[i]);
                                            resource.FullName = files[i];
                                            items.Add(new ResourceItem(files[i], resource));
                                        }
                                    }
                                }
                                else if (metaData.ResourceStorageMode == ResourceStorageMode.MultiFile
                                    && metaData is IMultiFileResourceMetaData) {
                                    var muiltFileResourceMetaData = metaData as IMultiFileResourceMetaData;
                                    for (int i = 0; i < files.Length; i++) {
                                        if (muiltFileResourceMetaData.Identified(files[i])) {
                                            fileStatus[i] = true;
                                            var otherFiles = muiltFileResourceMetaData.FindOtherFiles(files[i]);
                                            if (otherFiles.Count > 0) {//设置otherFile 的状态
                                                foreach (string f in otherFiles) {
                                                    for (int j = 0; j < files.Length; j++) {
                                                        if (files[j].Equals(f)) {
                                                            fileStatus[j] = true;
                                                        }
                                                    }
                                                }
                                            }
                                            IMultiFileResource resource = IocManager.Instance.Resolve(muiltFileResourceMetaData.ResourceType.Type) as IMultiFileResource;
                                            resource.Caption = Path.GetFileName(files[i]);
                                            resource.MainFilePath = files[i];
                                            resource.OtherFilePath = otherFiles;
                                            items.Add(new ResourceItem(files[i], resource));
                                        }
                                    }
                                }
                                else if (metaData.ResourceStorageMode == ResourceStorageMode.MixedFileFold
                                    && metaData is IMixedFileFoldResourceMetaData) { //文件和文件夹混合 
                                    var mixedFileFoldResourceMetaData = metaData as IMixedFileFoldResourceMetaData;
                                    for (int i = 0; i < files.Length; i++) {
                                        if (mixedFileFoldResourceMetaData.Identified(files[i])) {
                                            fileStatus[i] = true;
                                            var otherfiles = mixedFileFoldResourceMetaData.FindOtherFiles(files[i]);
                                            var otherFolders = mixedFileFoldResourceMetaData.FindOtherFolders(files[i]);
                                            if (otherfiles.Count > 0) {//设置otherFile 的状态
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
                                            if (otherFolders.Count > 0) { //设置otherFolder 的状态
                                                foreach (string f in otherFolders) {
                                                    for (int j = 0; j < dirs.Length; j++) {
                                                        if (dirs[j].Equals(f)) {
                                                            dirStatus[j] = true;
                                                        }
                                                    }
                                                }
                                            }
                                            IMixedFileFoldResource resource = IocManager.Instance.Resolve(mixedFileFoldResourceMetaData.ResourceType.Type) as IMixedFileFoldResource;
                                            resource.Caption = Path.GetFileName(files[i]);
                                            resource.MainPath = files[i];
                                            resource.OtherFilePaths = otherfiles;
                                            resource.OtherFolderPaths = otherFolders;
                                            items.Add(new ResourceItem(files[i], resource)); 
                                        }
                                    }
                                }
                            }
                        }
                        for (int k = 0; k < dirs.Length; k++) {
                            if (!dirStatus[k]) {
                                items.Add(new DirectoryItem(dirs[k]));
                            }
                        }
                    }
                }
            }
            catch { }
            finally { }
            return items;
        }
         
        protected override string GetDirectoryName(string path)
        {
            return Path.GetFileName(path);
        }
    }

    public class FileItem : Item
    {
        public FileItem(string fullName) : base(fullName)
        {

        }
        public override List<Item> GetDirectories()
        {
            return new List<Item>();
        } 
        protected override string GetDirectoryName(string path)
        {
            return Path.GetFileName(path);
        }
    }

    public class ResourceItem : Item {
        private IResource resource;
        private Image image;
        public ResourceItem(string _fullName, IResource _resource) :base(_fullName) {
            resource = _resource;
            if (resource.ResourceMetaData.Icon != null)
                image = resource.ResourceMetaData.Icon;
            else
                image = AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Winform.Icons.ResourceFile16.png");
        }
        public IResource Resource {
            get { return this.resource; }
        }
        public override List<Item> GetDirectories()
        {
            return new List<Item>();
        }
        public override Image Image { get { return this.image; }  set { this.image = value; } }
    }

    public abstract class Item : IFileImage
    {
        public Item(string fullName,IList<IResourceMetaData> metaDataFilter = null)
        {
            Image = GetImage(fullName);
            Name = GetDirectoryName(fullName);
            FullName = fullName;
            DisplayName = GetDisplayName(fullName);
            MetaDataFilter = metaDataFilter;
        }
        protected virtual string GetDisplayName(string fullName)
        {
            return Name;
        }
        public string DisplayName
        {
            get;
            private set;
        }
        public string Name
        {
            get;
            private set;
        }
        public string FullName
        {
            get;
            private set;
        }
        public virtual Image Image
        {
            get;
            set;
        }

        public IList<IResourceMetaData> MetaDataFilter { get; private set; }
        public abstract List<Item> GetDirectories();
        public static Size ImageSize
        {
            get;
            set;
        }
        protected Image GetImage(string fullName)
        {
            return FileSystemHelper.GetImage(fullName, IconSizeType.Small, ImageSize);
        }
        protected virtual string GetDirectoryName(string fullName)
        {
            return fullName;
        }
        public virtual List<CustomFileInfo> GetFilesSystemInfo()
        {
            List<CustomFileInfo> infos = new List<CustomFileInfo>(15);
            try
            {
                DirectoryInfo dInfo = new DirectoryInfo(FullName);
                if ((int)dInfo.Attributes == -1)
                    return infos;
                var directories = dInfo.GetDirectories();
                foreach (var directory in directories)
                {
                    if ((directory.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        infos.Add(new CustomFileInfo()
                        {
                            Name = directory.Name,
                            FullName = directory.FullName,
                            DateCreated = directory.CreationTime,
                            DateModified = directory.LastWriteTime,
                            Image = GetImage(directory.FullName),
                            TypeName = "File Folder",
                            Type = FileType.FileFolder
                        });
                    }
                }
                var files = dInfo.GetFiles();
                foreach (var file in files)
                {
                    if ((file.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        infos.Add(new CustomFileInfo()
                        {
                            Name = file.Name,
                            FullName = file.FullName,
                            DateCreated = file.CreationTime,
                            DateModified = file.LastWriteTime,
                            Image = GetImage(file.FullName),
                            TypeName = "File",
                            Type = FileType.File,
                            Size = file.Length
                        });
                    }
                }
            }
            catch(Exception ex) {
                throw ex;
            }
            return infos;
        }
    }
    public class CustomFileInfo : IFileImage
    {
        public CustomFileInfo() { }
        public Image Image
        {
            get;
            set;
        }
        public string FullName
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public FileType Type
        {
            get;
            set;
        }
        public string TypeName
        {
            get;
            set;
        }
        public DateTime? DateModified
        {
            get;
            set;
        }
        public DateTime? DateCreated
        {
            get;
            set;
        }
        public long? Size
        {
            get;
            set;
        }
    }
    public enum FileType { Drive, SystemFolder, FileFolder, File }
    interface IFileImage
    {
        Image Image { get; }
    }
}
