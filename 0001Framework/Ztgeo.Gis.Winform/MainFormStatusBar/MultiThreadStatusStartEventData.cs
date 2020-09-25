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
    /// 多线程事务处理开始事件
    /// 一般多线程事务开始处理时，会在状态栏有提示信息
    /// </summary>
    public class MultiThreadStatusStartEventData: Status4DocumenEventData
    {
        public MultiThreadStatusStartEventData(string msg,
            IDocument document,
            IDocumentControl documentControl) {
            this.StatusInfo = new StatusInfo
            {
                Message = msg,
                StatusShowType = StatusShowType.Msg
            };
            this.Document = document;
            this.DocumentControl = documentControl;
        }

        public MultiThreadStatusStartEventData(int maxValue,
            IDocument document,
            IDocumentControl documentControl) {
            this.StatusInfo = new StatusInfo
            {
                StatusShowType = StatusShowType.ProcessBar,
                MaxValue = maxValue,
                CurrentValue = 0 //线程起来时，初始值为0
            };
            this.Document = document;
            this.DocumentControl = documentControl;
        }

        public MultiThreadStatusStartEventData(IDocument document,IDocumentControl documentControl) {
            this.StatusInfo = new StatusInfo
            {
                StatusShowType = StatusShowType.ProcessBarWithoutProcess 
            };
            this.Document = document;
            this.DocumentControl = documentControl;
        }
    }
}
