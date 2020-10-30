using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastralManagementDataSync.DataOperation.Model
{
    public class DataSyncConfig
    {
        public string DirtyField { get; set; }

        public IList<DBOutput> InnerDBOutputs { get; set; }

        public IList<DBOutput> OutDBOutputs { get; set; }

        public static DataSyncConfig GetDataSyncConfig() {
            string configPath = System.Windows.Forms.Application.StartupPath + "/DataOperation/DataCaptureConfig.json";
            DataSyncConfig config = null;
            using (System.IO.StreamReader file = System.IO.File.OpenText(configPath))
            {
                string json = file.ReadToEnd();
                config = JsonConvert.DeserializeObject<DataSyncConfig>(json);
            }
            return config;
        }
    }

    public class DBOutput {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 需要同步的列
        /// </summary>
        public string Columns { get; set; }
        /// <summary>
        /// 主键字段
        /// </summary>
        public string KeyColumn { get; set; }
    }
}
