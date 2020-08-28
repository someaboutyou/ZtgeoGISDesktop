using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastralManagementDataSync.DataOperation
{
    /// <summary>
    /// 数据路径 和文件名 管理
    /// </summary>
    public class DataPathManager :ISingletonDependency
    {
        /// <summary>
        /// 获取当前获取脏数据的后本地保存的路径
        /// </summary>
        /// <returns></returns>
        public string GetDataCaptureSavePath(DataSyncDirection dataSyncDirection) { 
            string rootPath = System.Environment.CurrentDirectory;
            string path = string.Empty;
            if (dataSyncDirection == DataSyncDirection.InnerDataSync)
            {
                path = rootPath + "/CurrentDataSync/InnerDataSync";
            }
            else {
                path = rootPath + "/CurrentDataSync/OuterDataSync";
            }
            
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            return path +"/" + Timestamp(); 
        }
        private string Timestamp()
        {
            long ts = ConvertDateTimeToInt(DateTime.Now);
            return ts.ToString();
        }
        private long ConvertDateTimeToInt(System.DateTime time)
        { 
            long t = (time.Ticks - 621356256000000000) / 10000;
            return t;
        }
        /// <summary>
        /// 获取数据同步后需要转存的文件地址
        /// </summary>
        /// <returns></returns>
        public string GetDataSyncedSaveDirectory(DataSyncDirection dataSyncDirection) {
            string rootPath = System.Environment.CurrentDirectory;
            string path = string.Empty;
            if (dataSyncDirection == DataSyncDirection.InnerDataSync)
                path = rootPath + "/SyncedDataSync/InnerDataSync";
            else
            {
                path = rootPath + "/SyncedDataSync/OuterDataSync";
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

    }
}
