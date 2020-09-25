using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.MainFormDocument;

namespace Ztgeo.Gis.Winform.MainFormStatusBar
{
    /// <summary>
    /// 文档处理状态事件定义。文档处理事件包含：文档的打开、注销，文档内部耗时操作等。
    /// 一般在多线程处理之前，会显示状态信息
    /// </summary>
    public class Status4DocumenEventData: StatusEventData
    {
       public IDocument Document { get; set; }
        public IDocumentControl DocumentControl { get; set; }
        public StatusInfo StatusInfo { get; set; }
    }
}
