using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using CadastralManagementDataSync.DataOperation.Model;
using CadastralManagementDataSync.DataOperation.Dal;
using Castle.Core.Logging;
using DevExpress.Utils.Extensions;

namespace CadastralManagementDataSync.DataOperation
{
    /// <summary>
    /// 数据同步操作
    /// </summary>
    public class DataSyncOperator :ISingletonDependency
    {
        private readonly DataPathManager dataPathManager;
        private readonly ConnectStringCreator connectStringCreator;
        public ILogger Logger { get; set; }
        public DataSyncOperator(DataPathManager _dataPathManager, ConnectStringCreator _connectStringCreator) {
            dataPathManager = _dataPathManager;
            connectStringCreator = _connectStringCreator;
            Logger = NullLogger.Instance;
        }
          
        public void SyncData(DataSyncConfig dataSyncConfig, DataSyncDirection dataSyncDirection) {
            string outerCurrentPath = string.Empty;
            IList<DBOutput> OutdBOutputs = null;
            if (dataSyncDirection == DataSyncDirection.InnerDataSync)
            {
                //内网同步时，获取外网文件夹下面是否有需要同步的文件
                outerCurrentPath = dataPathManager.GetDataCaptureSaveDirectory(DataSyncDirection.OuterDataSync);
                OutdBOutputs = dataSyncConfig.OutDBOutputs;
            }
            else {
                outerCurrentPath = dataPathManager.GetDataCaptureSaveDirectory(DataSyncDirection.InnerDataSync);
                OutdBOutputs = dataSyncConfig.InnerDBOutputs;
            }
            
            string[] filePaths = Directory.GetFiles(outerCurrentPath);
            if (filePaths.Length > 0) {
                foreach (string filePath in filePaths)
                {
                    Logger.Debug("开发同步到" + (DataSyncDirection.InnerDataSync == dataSyncDirection ? "内网" : "外网") + "。同步文件路径：" + filePath);
                    DataSet ds= DatasetSerialize.DataSetDeserialize(filePath);
                    if(ds!=null && ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables.Count; i++) {
                            Logger.Debug("开发" + ds.Tables[i].TableName);
                            string tableName = ds.Tables[i].TableName;
                            DBOutput dBOutput = GetConfigByTableName(OutdBOutputs, tableName);
                            for (int j = 0; j < ds.Tables[i].Rows.Count; j++) {
                                if (IfExistsData(dataSyncConfig, dataSyncDirection, ds.Tables[i].TableName, dBOutput.KeyColumn, ds.Tables[i].Rows[j][dBOutput.KeyColumn]))
                                {
                                    CreateUpdateATableRow(dataSyncDirection, ds.Tables[i], j, dBOutput, dataSyncConfig.DirtyField);
                                }
                                else {
                                    CreateInsertATableRow(dataSyncDirection, ds.Tables[i], j, dBOutput, dataSyncConfig.DirtyField);
                                }
                            }
                        }
                    }
                    //数据同步之后，再将文件移入已同步
                    File.Move(filePath, dataPathManager.GetDataSyncedSaveDirectory(dataSyncDirection) +"//"+ Path.GetFileName(filePath));
                }
            }
        }

        private void CreateInsertATableRow(DataSyncDirection dataSyncDirection, DataTable dataTable,int rowNum,DBOutput dBOutput,string dirtyField) {
            string[] cols = dBOutput.Columns.Split(',');
            string sql = string.Empty;
            if (cols.Contains(dBOutput.KeyColumn)) {
                sql = string.Format("Insert into {0}({1},{3}) values({2},-1)", dBOutput.TableName, dBOutput.Columns, string.Join(",", cols.Select(col => ":" + col)), dirtyField);
                Oracle.ManagedDataAccess.Client.OracleParameter[] oracleParameters = new Oracle.ManagedDataAccess.Client.OracleParameter[cols.Length];
                for (int i = 0; i < cols.Length; i++)
                {
                    oracleParameters[i] = new Oracle.ManagedDataAccess.Client.OracleParameter(cols[i], dataTable.Rows[rowNum][cols[i]]);
                }
                string connStr = GetconStrByDirection(dataSyncDirection);
                OracleHelper.ExecuteNonQuery(connStr, sql, oracleParameters);
            }
            else
            {
                sql = string.Format("Insert into {0}({1},{3},{4}) values({2},-1,:{4})", dBOutput.TableName, dBOutput.Columns, string.Join(",", cols.Select(col => ":" + col)), dirtyField, dBOutput.KeyColumn);
                Oracle.ManagedDataAccess.Client.OracleParameter[] oracleParameters = new Oracle.ManagedDataAccess.Client.OracleParameter[cols.Length+1];
                for (int i = 0; i < cols.Length; i++)
                {
                    oracleParameters[i] = new Oracle.ManagedDataAccess.Client.OracleParameter(cols[i], dataTable.Rows[rowNum][cols[i]]);
                }
                oracleParameters[cols.Length] = new Oracle.ManagedDataAccess.Client.OracleParameter(dBOutput.KeyColumn, dataTable.Rows[rowNum][dBOutput.KeyColumn]);
                string connStr = GetconStrByDirection(dataSyncDirection);
                OracleHelper.ExecuteNonQuery(connStr, sql, oracleParameters);
            } 
            
        }

        private void CreateUpdateATableRow(DataSyncDirection dataSyncDirection, DataTable dataTable, int rowNum, DBOutput dBOutput, string dirtyField) {
            string[] cols = dBOutput.Columns.Split(',');
            if (cols.Any(c => c.Equals(dBOutput.KeyColumn))) {
                cols.Remove(c => c.Equals(dBOutput.KeyColumn));
            }
            string sql = string.Format("Update {0} Set {1},{3} Where {2} =:{2}", dBOutput.TableName,string.Join(",", cols.Select(col => " " + col + "=:" + col + " ")), 
                dBOutput.KeyColumn, dirtyField+"=:"+ dirtyField);
            Oracle.ManagedDataAccess.Client.OracleParameter[] oracleParameters = new Oracle.ManagedDataAccess.Client.OracleParameter[cols.Length+2];
            for (int i = 0; i < cols.Length; i++)
            {
                oracleParameters[i] = new Oracle.ManagedDataAccess.Client.OracleParameter(cols[i], dataTable.Rows[rowNum][cols[i]]);
            }
            oracleParameters[cols.Length] = new Oracle.ManagedDataAccess.Client.OracleParameter(dirtyField, (object)-1);
            oracleParameters[cols.Length+1] = new Oracle.ManagedDataAccess.Client.OracleParameter(dBOutput.KeyColumn, dataTable.Rows[rowNum][dBOutput.KeyColumn]);
            string connStr = GetconStrByDirection(dataSyncDirection);
            OracleHelper.ExecuteNonQuery(connStr, sql, oracleParameters);
        }
        /// <summary>
        /// 获得
        /// </summary>
        /// <param name="outputs"></param>
        /// <returns></returns>
        private DBOutput GetConfigByTableName(IList<DBOutput> outputs, string tableName) { 
            return outputs.FirstOrDefault(o => o.TableName.Equals(tableName));
        }

        private string GetconStrByDirection(DataSyncDirection dataSyncDirection) {
            string conStr = string.Empty;
            if (dataSyncDirection == DataSyncDirection.InnerDataSync)
            {
                conStr = connectStringCreator.GetOracleConstr(DataSyncDirection.InnerDataSync);
            }
            else
            {
                conStr = connectStringCreator.GetOracleConstr(DataSyncDirection.OuterDataSync);
            }
            return conStr;
        }

        private bool IfExistsData(DataSyncConfig dataSyncConfig,DataSyncDirection dataSyncDirection,string tableName,string keyColumn, object keyValue) {
            string conStr = GetconStrByDirection(dataSyncDirection);
            string sql = string.Format("Select Count(1) from {0} Where {1} = :{1}", tableName, keyColumn);
            DataTable dataTable= OracleHelper.ExecuteDataTable(conStr, sql, new Oracle.ManagedDataAccess.Client.OracleParameter[] { 
                new Oracle.ManagedDataAccess.Client.OracleParameter(keyColumn, keyValue)
            });
            if (Convert.ToInt32(dataTable.Rows[0][0].ToString()) > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

    }
}
