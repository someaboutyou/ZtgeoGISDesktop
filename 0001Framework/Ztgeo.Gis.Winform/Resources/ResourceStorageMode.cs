using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Resources
{
    /// <summary>
    /// 资源存储方式
    /// </summary>
    public enum ResourceStorageMode
    {
        SingleFile, //单一的文件资源，例如.dwg .docx 等
        MultiFile, //多个文件集合的 例如 shpfile
        SingleFolder, //单个文件夹 例如GDB
        MultiFolder, //多个文件夹 例如一些自定义的多个文件夹组成一个资源
        MixedFileFold, //文件和文件夹混合的
        PersonalDB, //个人数据库 例如 access，sqlite
        RemoteDB, //远程数据库
        FromWeb, //从web来源的数据
        StreamResource //流资源
    }
}
