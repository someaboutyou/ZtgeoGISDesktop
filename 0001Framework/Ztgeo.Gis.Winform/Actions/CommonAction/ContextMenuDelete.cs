using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.Actions;
using Ztgeo.Gis.Winform.Resources;
using Ztgeo.Utils;

namespace Ztgeo.Gis.Winform.Actions.CommonAction
{
    public class ContextMenuDelete : IContextMenuItemAction
    {
        public Image ItemIcon { get { return AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.Winform.Icons.Delete16.png"); } }

        public object Sender { get; set; }

        public bool IsSplitor { get { return false; } }

        public string Caption { get { return "删除"; } }

        public void Excute()
        {
            if (Sender is ISingleFileResource)
            {
                ISingleFileResource resource = (ISingleFileResource)Sender;
                File.Delete(resource.FullName);
            }
            if (Sender is ISingleFolderResource) {
                ISingleFolderResource resource = (ISingleFolderResource)Sender;
                Directory.Delete(resource.FolderPath);
            }
            if (Sender is IMultiFileResource) {
                IMultiFileResource resource = (IMultiFileResource)Sender;
                File.Delete(resource.MainFilePath);
                if (resource.OtherFilePath.Count > 0) {
                    foreach (string filePath in resource.OtherFilePath)
                    {
                        File.Delete(filePath);
                    }
                }
            }
            if (Sender is IMultiFolderResource)
            {
                IMultiFolderResource resource = (IMultiFolderResource)Sender;
                Directory.Delete(resource.MainFolder);
                if (resource.OtherFolders.Count > 0)
                {
                    foreach (string folderPath in resource.OtherFolders)
                    {
                        Directory.Delete(folderPath);
                    }
                }
            }
            if (Sender is IMixedFileFoldResource)
            {
                IMixedFileFoldResource resource = (IMixedFileFoldResource)Sender;
                File.Delete(resource.MainPath);
                if (resource.OtherFolderPaths.Count > 0)
                {
                    foreach (string folderPath in resource.OtherFolderPaths)
                    {
                        Directory.Delete(folderPath);
                    }
                }
                if (resource.OtherFilePaths.Count > 0)
                {
                    foreach (string filePath in resource.OtherFilePaths)
                    {
                        File.Delete(filePath);
                    }
                }
            }
        }
    }
}
