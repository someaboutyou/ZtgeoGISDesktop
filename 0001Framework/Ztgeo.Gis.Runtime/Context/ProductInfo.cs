using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Runtime.Context
{
    /// <summary>
    /// 产品信息
    /// </summary>
    public class ProductInfo
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 技术支持单位
        /// </summary>
        public string SupportBy { get; set; }
        /// <summary>
        /// 著作权
        /// </summary>
        public string CopyRight { get; set; }

        public void Initialize(IConfigurationRoot appConfiguration) {
            this.ProductName = appConfiguration["App:ProductName"] ?? "";
            this.SupportBy = appConfiguration["App:SupportBy"] ?? "";
            this.CopyRight = appConfiguration["App:CopyRight"] ?? "";
        }
    }
}
