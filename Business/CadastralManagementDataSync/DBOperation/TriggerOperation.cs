using Abp.Dependency;
using CadastralManagementDataSync.DataOperation;
using CadastralManagementDataSync.DataOperation.Dal;
using CadastralManagementDataSync.DataOperation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastralManagementDataSync.DBOperation
{
//create or replace trigger MaintenanceDirty4YW_FJMC
// before insert or update on YW_FJMC
//  for each row
//declare
//begin
//   if(:new.DIRTY=-1) then
//      :new.DIRTY:=0;
//   else 
//     :new.DIRTY:=1;
//   end if;
//  end MaintenanceDirty4YW_FJMC;

         
//alter table DJQ add dirty number(2); 
//comment on column DJQ.dirty is '0，干净数据 1脏数据';
/// <summary>
/// 触发器构建
/// 构建通过执行SQL
/// </summary>

    public class TriggerOperation:ISingletonDependency
    {
        private readonly ConnectStringCreator connectStringCreator;
        public TriggerOperation(ConnectStringCreator _connectStringCreator) {

            connectStringCreator = _connectStringCreator;
        }
        private string GetconStrByDirection(DataSyncDirection dataSyncDirection)
        {
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
        /// <summary>
        /// 建立脏数据sql
        /// </summary>
        private string CreateDirtyFiledSql(DataSyncConfig dataSyncConfig, DataSyncDirection dataSyncDirection) { 
            IList<DBOutput> wantDBOutputs = null;
            if (dataSyncDirection == DataSyncDirection.InnerDataSync)
            {
                wantDBOutputs = dataSyncConfig.InnerDBOutputs;
            }
            else {
                wantDBOutputs = dataSyncConfig.OutDBOutputs;
            }
            StringBuilder sb = new StringBuilder();
            //alter table DJQ add dirty number(2); comment on column DJQ.dirty is '0，干净数据 1脏数据';
            foreach (var dbOutput in wantDBOutputs) {
                sb.AppendFormat("alter table {0} add {1} number(2) default 1;\n\r comment on column {0}.{1} is '0，干净数据 1脏数据';\n\r", dbOutput.TableName, dataSyncConfig.DirtyField);
            }
            sb.Append("begin \r\n");
            foreach (var dbOutput in wantDBOutputs)
            {
                sb.AppendFormat("update {0} set {1}=-1; \n\r", dbOutput.TableName, dataSyncConfig.DirtyField);
            }
            sb.Append(" commit; \r\n end; \r\n");

            sb.Append("\r\n");
            foreach (var dbOutput in wantDBOutputs)
            {
                sb.AppendFormat(" create index bmIndex_{0}_{1} on {0}({1}) ; \r\n", dbOutput.TableName, dataSyncConfig.DirtyField);
            }
            sb.Append("\r\n");
            return sb.ToString();
        }
        /// <summary>
        /// 建立触发器
        /// </summary>
        /// <param name="dataSyncConfig"></param>
        /// <param name="dataSyncDirection"></param>
        private string CreateTriggerSql(DataSyncConfig dataSyncConfig, DataSyncDirection dataSyncDirection) {
            IList<DBOutput> wantDBOutputs = null;
            if (dataSyncDirection == DataSyncDirection.InnerDataSync)
            {
                wantDBOutputs = dataSyncConfig.InnerDBOutputs;
            }
            else
            {
                wantDBOutputs = dataSyncConfig.OutDBOutputs;
            }
            StringBuilder sb = new StringBuilder();
            foreach (var dbOutput in wantDBOutputs)
            {
                sb.AppendFormat("create or replace trigger WHDirty4{0} before insert or update on {0} for each row declare begin if(:new.{1}=-1) then :new.{1}:=0; else  :new.{1}:=1; end if;  end WHDirty4{0}; \n\r / \n\r", dbOutput.TableName, dataSyncConfig.DirtyField);
            }
            return sb.ToString();
        }

        public string DoDBTriggerOperation(DataSyncConfig dataSyncConfig, DataSyncDirection dataSyncDirection) {
           return  CreateDirtyFiledSql(dataSyncConfig, dataSyncDirection) + " \r\n  \r\n" +
            CreateTriggerSql(dataSyncConfig, dataSyncDirection);
        }
    }
}
