using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Resources
{
    public interface IMixedFileFoldResource:IResource
    {
        /// <summary>
        /// 鉴别目录
        /// </summary>
        string MainPath { get; set; }

        IList<string> OtherFilePaths { get; set; }

        IList<string> OtherFolderPaths { get; set; }
    }
}
