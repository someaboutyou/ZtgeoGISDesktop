using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.MainFormDocument.Resources;
using Ztgeo.Utils;

namespace Ztgeo.Gis.CAD.Controls
{
    public class CADViewSingleFileDocumentResourceMetaData : ISingleFileDocumentResourceMetaData
    {
        public IocManager IocManager { get; set; } //注入
        public List<string> SelectFilterExtensionName { get { return new List<string>() {".dwg",".dxf" }; } }

        public DocumentResourceType DocumentResourceType { get { return DocumentResourceType.SingleFile; } }

        public Image Icon { get { return AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.CADFileIcon.png"); } }

        public Type TypeOfDocumentResource { get { return typeof(CADViewSingleFileDocumentResource); } }

        public IList<ISingleFileDocumentResource> FindSingleFileDocumentResourceInDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                string[] files = Directory.GetFiles(directoryPath);
                return files.Where(f => this.IsSingleFileDocumentResource(f)).Select(f =>
                {
                    var r = IocManager.Resolve<CADViewSingleFileDocumentResource>();
                    r.FilePath = f;
                    return (ISingleFileDocumentResource)r;
                }).ToList();
            }
            else
            {
                throw new DirectoryNotFoundException(directoryPath + ",目录不存在");
            }
        }

        public bool IsSingleFileDocumentResource(string filePath)
        {
            return SelectFilterExtensionName.Any(en => en.Trim(new char[2] { ' ', '.' }).Equals(Path.GetExtension(filePath)));
        }
    }
}
