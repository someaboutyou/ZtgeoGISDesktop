using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime.LocalPriority;

namespace Ztgeo.Gis.Hybrid.FormIO
{

    [Table("SmartFormSchemes")]
    public class SmartFormScheme : Entity, ILocalPriorityClientEntity 
    {
        public int Version { get; set; } 
        public int GroupId { get; set; }
        [MaxLength(256)] 
        public string Name { get; set; }
        public string Scheme { get; set; } 
        public DateTime CreationTime { get; set; } 
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(256)]
        public string Describe { get; set; }
    }
}
