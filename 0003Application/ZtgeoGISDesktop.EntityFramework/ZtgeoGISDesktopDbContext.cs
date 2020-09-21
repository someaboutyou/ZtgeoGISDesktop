using Abp.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.AbpExtension.SettingExtension;
using Ztgeo.Gis.Hybrid.FormIO;
using ZtgeoGISDesktop.Core.Menu;

namespace ZtgeoGISDesktop.EntityFramework
{
    public class ZtgeoGISDesktopDbContext: AbpDbContext
    { 
        public ZtgeoGISDesktopDbContext() : 
            base("Default")
            //base(new SQLiteConnection( 
            // @"Data Source=.\db\desktopSqlite.db;Initial Catalog=sqlite;Integrated Security=True;Max Pool Size=10"
            //), true)
        {

        }
        public virtual DbSet<MenuOrder> MenuOrders { get; set; }

        public virtual DbSet<SmartFormScheme> SmartFormSchemes { get; set; }

        public virtual DbSet<Setting> Settings { get; set; }
    }
}
