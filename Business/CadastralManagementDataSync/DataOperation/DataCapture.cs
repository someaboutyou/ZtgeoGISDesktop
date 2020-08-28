using CadastralManagementDataSync.DataOperation.Dal;
using CadastralManagementDataSync.DataOperation.Model;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastralManagementDataSync.DataOperation
{
    /// <summary>
    /// 捕捉数据的变化
    /// 获取Dirty 数据
    /// </summary>
    public class DataCapture:Abp.Dependency.ISingletonDependency
    {
        public ILogger Logger { get; set; }
        private readonly ConnectStringCreator connectStringCreator;
        private readonly DataPathManager dataPathManager;
        public DataCapture(ConnectStringCreator _connectStringCreator,
            DataPathManager _dataPathManager
            ) {
            connectStringCreator = _connectStringCreator;
            dataPathManager = _dataPathManager;
            Logger = NullLogger.Instance;
        }
        /// <summary>
        /// 获得脏数据
        /// </summary>
        /// <returns></returns>
        private DataSet CaptureDirtyFromDB(DataSyncConfig dataSyncConfig, DataSyncDirection dataSyncDirection) {

            DataSet dataSet = new DataSet();
            if (dataSyncConfig != null)
            {
                IList<DBOutput> dBOutputs =null;
                if (dataSyncDirection == DataSyncDirection.InnerDataSync)
                { //内网数据同步
                    if (dataSyncConfig.InnerDBOutputs != null && dataSyncConfig.InnerDBOutputs.Count > 0)
                    {
                        dBOutputs = dataSyncConfig.InnerDBOutputs;
                    }
                    else
                    {
                        Logger.Warn("未发现内网数据同步配置");
                    }
                }
                else { //外网数据同步
                    if (dataSyncConfig.OutDBOutputs != null && dataSyncConfig.OutDBOutputs.Count > 0) {
                        if (dataSyncConfig.OutDBOutputs != null && dataSyncConfig.OutDBOutputs.Count > 0) {
                            dBOutputs = dataSyncConfig.OutDBOutputs;
                        }
                        else
                        {
                            Logger.Warn("未发现外网数据同步配置");
                        }
                    }
                }
                if (dBOutputs != null) {
                    foreach (DBOutput dbOutput in dBOutputs) {
                        string sql = JointCaptureSql(dbOutput.TableName,dbOutput.Columns,dbOutput.KeyColumn, dataSyncConfig.DirtyField);
                        string connstr = connectStringCreator.GetOracleConstr(dataSyncDirection);
                        dataSet.Tables.Add(OracleHelper.ExecuteDataTable(connstr, sql));
                    }
                }
            }
            else {
                Logger.Error("未发现数据同步配置");
            }
            return dataSet;
        }
        /// <summary>
        /// 将数据设置为干净的数据
        /// </summary>
        private void SetUnDirty(DataSyncConfig dataSyncConfig, DataSyncDirection dataSyncDirection) {
            string connstr = connectStringCreator.GetOracleConstr(dataSyncDirection);
            if (dataSyncConfig != null)
            { 
                if (dataSyncDirection == DataSyncDirection.InnerDataSync)
                { //设置内网数据为干净数据 
                    foreach (DBOutput dBOutput in dataSyncConfig.InnerDBOutputs ) {
                        SetUnDirty(dBOutput.TableName, dataSyncConfig.DirtyField, connstr);
                    }        
                }
                else
                { //设置外网数据为干净数据
                    foreach (DBOutput dBOutput in dataSyncConfig.OutDBOutputs)
                    {
                        SetUnDirty(dBOutput.TableName, dataSyncConfig.DirtyField, connstr);
                    }
                }
                 
            }
        }
        private void SetUnDirty(string table,string fildName,string connstr) {
            string sql = string.Format("Update {0} Set {1}=0 Where {1}=1", table, fildName);
            OracleHelper.ExecuteNonQuery(connstr, sql);
        }
        /// <summary>
        /// 获取脏数据并且将数据保存在文件中
        /// </summary>
        /// <returns></returns>
        public bool CaptureDirtyFromDBAndSave(DataSyncConfig dataSyncConfig, DataSyncDirection dataSyncDirection) {
            try
            {
                DataSet ds = CaptureDirtyFromDB(dataSyncConfig, dataSyncDirection);
                string savePath = dataPathManager.GetDataCaptureSavePath(dataSyncDirection);
                DatasetSerialize.DataSetSerialize(savePath, ds);
                SetUnDirty(dataSyncConfig, dataSyncDirection);
                return false;
            }
            catch (Exception ex) {
                Logger.Error("获取脏数据，存储在本地出现错误！", ex);
                throw ex;
            }
            
        }
        private string JointCaptureSql(string tableName,string cloumns,string keyColumn, string dirtyField) {
            return "Select " + keyColumn +","+ cloumns + " from " + tableName + " where " + dirtyField + " = 1";
        }

        
    }
}
