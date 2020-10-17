using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Resources
{
    public interface IMultiFolderResource : IResource
    {
        /// <summary>
        /// 除了主目录 其他目录
        /// </summary>
        IList<string> OtherFolders { get; set; }
        /// <summary>
        /// 主目录
        /// </summary>
        string MainFolder { get; set; }
    }
}
