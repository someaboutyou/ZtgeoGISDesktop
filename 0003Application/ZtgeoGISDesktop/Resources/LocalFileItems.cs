using DevExpress.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public ThisPCItem() : base("This PC") { }
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
            return "Disc (" + Name + ")";
        }
    }
    public class DirectoryItem : Item
    {
        public DirectoryItem(string fullName) : base(fullName) { }
        public override List<Item> GetDirectories()
        {
            List<Item> items = new List<Item>(10);
            try
            {
                if (Directory.Exists(FullName))
                {
                    string[] dirs = Directory.GetDirectories(FullName);
                    foreach (string dir in dirs)
                    {
                        var attributes = File.GetAttributes(dir);
                        if ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                            items.Add(new DirectoryItem(dir));
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
    public abstract class Item : IFileImage
    {
        public Item(string fullName)
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
            catch { }
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
