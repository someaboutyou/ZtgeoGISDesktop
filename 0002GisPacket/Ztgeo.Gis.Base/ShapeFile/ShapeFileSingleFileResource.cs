using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.Resources;

namespace Ztgeo.Gis.Base.ShapeFile
{
    public class ShapeFileSingleFileResource :IMultiFileResource
    {

        private readonly IocManager IocManager;
        public ShapeFileSingleFileResource(IocManager _iocManager) {
            this.IocManager = _iocManager;
        }   
        private string caption;
        public string Caption
        {
            get
            {
                if (!string.IsNullOrEmpty(caption))
                {
                    return caption;
                }
                if (string.IsNullOrEmpty(this.MainFilePath))
                    return "";
                else
                {
                    caption = Path.GetFileName(MainFilePath);
                    return caption;
                }
            }
            set { caption = value; }
        }

        public IResourceMetaData ResourceMetaData { get { return this.IocManager.Resolve<ShapeFileSingleFileResourceMetaData>(); } }

        public string MainFilePath { get ; set  ; }
        public IList<string> OtherFilePath { get; set; }
    }
}
