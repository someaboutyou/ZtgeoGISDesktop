using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastralManagementDataSync.DataOperation.Dal
{
    public class OracleHelper
    {
        public static string GetConnStr(string address,string port,string serviceName,string userName,string password) {
            return string.Format("User Id={0};Password={1};Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={2})(PORT={3})))(CONNECT_DATA=(SERVICE_NAME={4})))"
                ,userName,password, address, port, serviceName);
        }
        #region 执行SQL语句,返回受影响行数
        public static int ExecuteNonQuery(string connStr,string sql, params OracleParameter[] parameters)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open();
                using (OracleCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion
        #region 执行SQL语句,返回DataTable;只用来执行查询结果比较少的情况
        public static DataTable ExecuteDataTable(string connStr, string sql, params OracleParameter[] parameters)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open();
                using (OracleCommand cmd = conn.CreateCommand())
                { 
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    DataTable datatable = new DataTable();
                    adapter.Fill(datatable);
                    return datatable;
                }
            }
        }
         
        #endregion

    }
}
