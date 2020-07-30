using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.Menu;

namespace ZtgeoGISDesktop.Core.Menu
{
    [Table("MenuOrders")]
    public class MenuOrder: Entity<int>
    { 
        public virtual string MenuKey { get; set; } 
        public virtual int Order { get; set; }
    }
}
