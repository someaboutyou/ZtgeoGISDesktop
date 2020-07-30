using Abp.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZtgeoGISDesktop.Core.Menu;

namespace ZtgeoGISDesktop.EntityFramework
{
    public class ZtgeoGISDesktopDbContext: AbpDbContext
    {
        public ZtgeoGISDesktopDbContext():base(new SQLiteConnection(), true) {

        }
        public virtual DbSet<MenuOrder> MenuOrders { get; set; }
    }
}
