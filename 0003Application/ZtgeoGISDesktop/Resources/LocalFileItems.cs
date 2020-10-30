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
        public override List<Item> GetDirectories(IList<IResourceMetaData> metaDataFilter = null)
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
        public override List<Item> GetDirectories(IList<IResourceMetaData> metaDataFilter = null)
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
        public DirectoryItem(string fullName, IList<IResourceMetaData> metaDataFilter = null) : base(fullName) { }
        public override List<Item> GetDirectories(IList<IResourceMetaData> metaDataFilter = null)
        {
            List<Item> items = new List<Item>(10);
            try
            {
                if (Directory.Exists(FullName))
                { 
                    if (metaDataFilter == null || metaDataFilter.Count == 0) //没有过滤是，显示所有
                    {
                        string[] dirs = Directory.GetDirectories(FullName);
                        string[] files = Directory.GetFiles(FullName);
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
                        ILocalResourceManager localResourceManager = IocManager.Instance.Resolve<ILocalResourceManager>();
                        IList<string> restDirs, restFiles;
                        IList<IResource> resources= localResourceManager.GetLocalResource(FullName, metaDataFilter, out restDirs, out restFiles);
                        if (resources.Count > 0) {
                            foreach(IResource resource in resources)
                            {
                                if(resource is ISingleFileResource)
                                    items.Add(new ResourceItem((resource as ISingleFileResource).FullName ,resource));
                                if(resource is ISingleFolderResource)
                                    items.Add(new ResourceItem((resource as ISingleFolderResource).FolderPath, resource));
                                if (resource is IMultiFileResource)
                                    items.Add(new ResourceItem((resource as IMultiFileResource).MainFilePath, resource));
                                if (resource is IMultiFolderResource)
                                    items.Add(new ResourceItem((resource as IMultiFolderResource).MainFolder, resource));
                            }
                        }
                        if (restDirs.Count > 0) {
                            foreach (string restDir in restDirs) {
                                items.Add(new DirectoryItem(restDir, null));
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
        public override List<Item> GetDirectories(IList<IResourceMetaData> metaDataFilter = null)
        {
            return new List<Item>();
        } 
        protected override string GetDirectoryName(string path)
        {
            return Path.GetFileName(path);
        }
        protected override string GetDisplayName(string path)
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
        public override List<Item> GetDirectories(IList<IResourceMetaData> metaDataFilter = null)
        {
            return new List<Item>();
        }
        public override Image Image { get { return this.image; }  set { this.image = value; } }
        protected override string GetDirectoryName(string path)
        {
            return Path.GetFileName(path);
        }
        protected override string GetDisplayName(string path)
        {
            return Path.GetFileName(path);
        }
    }

    public abstract class Item : IFileImage
    {
        public Item(string fullName )
        {
            Image = GetImage(fullName);
            Name = GetDirectoryName(fullName);
            FullName = fullName;
            DisplayName = GetDisplayName(fullName); 
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
         
        public abstract List<Item> GetDirectories(IList<IResourceMetaData> metaDataFilter = null);
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
