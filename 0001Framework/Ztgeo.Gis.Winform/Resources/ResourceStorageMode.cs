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
        PersonalMDBDB, //个人数据库   access  
        PersonalSqliteDB, //个人数据库   qlite
        RemoteOracleDB, //远程数据库
        RemoteSqlServerDB, //远程数据库
        RemoteMySqlDB, //远程数据库
        FromWebGet, //从web来源的数据(get方式)
        FromWebPost, //从web来源的数据(post方式)
        StreamResource, //流资源
        StreamResourceBase64 //流资源(以base64 字符传播)
    }

    public static class ResourceStorageModeClassify { 
        /// <summary>
        /// 本地资源
        /// </summary>
        public static ResourceStorageMode[] LocalResource {
            get {
                return new ResourceStorageMode[] {
                    ResourceStorageMode.SingleFile,
                    ResourceStorageMode.MultiFile,
                    ResourceStorageMode.SingleFolder,
                    ResourceStorageMode.MultiFolder,
                    ResourceStorageMode.MixedFileFold,
                    ResourceStorageMode.PersonalMDBDB,
                    ResourceStorageMode.PersonalSqliteDB
                };
            }
        } 
        public static ResourceStorageMode[] LocalFolderResource {
            get {
                return new ResourceStorageMode[]
                {
                    ResourceStorageMode.SingleFolder,
                    ResourceStorageMode.MultiFolder,
                    ResourceStorageMode.MixedFileFold
                };
            }
        }
        public static ResourceStorageMode[] LocalFileResource
        {
            get
            {
                return new ResourceStorageMode[]
                {
                    ResourceStorageMode.SingleFile,
                    ResourceStorageMode.MultiFile 
                };
            }
        }
        /// <summary>
        /// 数据库资源
        /// </summary>
        public static ResourceStorageMode[] DBResource {
            get {
                return new ResourceStorageMode[] {
                    ResourceStorageMode.PersonalMDBDB,
                    ResourceStorageMode.PersonalSqliteDB,
                    ResourceStorageMode.RemoteOracleDB,
                    ResourceStorageMode.RemoteSqlServerDB,
                    ResourceStorageMode.RemoteMySqlDB
                };
            }
        }
        /// <summary>
        /// web 资源
        /// </summary>
        public static ResourceStorageMode[] WebResource {
            get { 
                return new ResourceStorageMode[] {
                    ResourceStorageMode.FromWebGet,
                    ResourceStorageMode.FromWebPost
                };
            }
        }
        /// <summary>
        /// 流资源
        /// </summary>
        public static ResourceStorageMode[] StreamResource
        {
            get
            {
                return new ResourceStorageMode[] {
                    ResourceStorageMode.StreamResource,
                    ResourceStorageMode.StreamResourceBase64
                };
            }
        }
    }
}
