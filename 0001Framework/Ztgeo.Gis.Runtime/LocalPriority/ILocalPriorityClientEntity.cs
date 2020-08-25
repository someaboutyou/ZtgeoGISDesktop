using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Runtime.LocalPriority
{
    /// <summary>
    /// power by JZW
    /// 本地优先实体类。这种实体类会带有版本号。
    /// 发现本地版本号小于服务上面的版本时，会从服务器上获取最新数据并在本地保存。
    /// 该实体类分为服务器版和客户机版。
    /// 服务器版: 实体类Update and Insert 会被捕捉。捕捉后会自动维护其版本号。服务器程序启动时会缓存所有版本号数据。
    /// 客端端版：（支持远程模式下）客户端会在启动时加载服务器上所有的版本号。并启用本地优先，远程更新本地模式。
    /// 
    /// </summary>  
    public interface ILocalPriorityClientEntity<TPrimaryKey> : ILocalPriority<TPrimaryKey> { 
       
    }
    public interface ILocalPriorityClientEntity : ILocalPriorityClientEntity<int>
    {

    }
}
