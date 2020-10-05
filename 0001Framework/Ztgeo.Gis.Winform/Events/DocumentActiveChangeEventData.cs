using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.MainFormDocument;

namespace Ztgeo.Gis.Winform.Events
{
    /// <summary>
    /// 文档激活切换事件
    /// </summary>
    public class DocumentActiveChangeEventData:EventData
    {
        public IDocumentControl ChangeFromDocumentControl { get; set; }
        public IDocumentControl ChangeToDocumentControl { get; set; }
    }
}
