using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Resources
{
    public interface IStreamResourceMetaData: IResourceMetaData
    {
        /// <summary>
        /// 鉴别是不是流资源
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        bool Identify(Stream stream);
    }
}
