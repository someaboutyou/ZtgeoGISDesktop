using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.ABPForm;

namespace Ztgeo.Gis.Winform.MainFormStatusBar
{
    public class StatusBarSetting : IEventHandler<StatusEventData>, ITransientDependency
    {
        private  readonly IMainForm mainForm;
        public StatusBarSetting(IMainForm _mainForm) {
            this.mainForm = _mainForm;
        }
        public void HandleEvent(StatusEventData eventData)
        {
            if (eventData is MultiThreadStatusStartEventData)
            {
                var eData = (MultiThreadStatusStartEventData)eventData;
                this.mainForm.SetStatusInfo(eData.DocumentControl, eData.StatusInfo);
            }
            else if (eventData is MultiThreadStatusEndEventData)
            {

                var eData = (MultiThreadStatusEndEventData)eventData;
                this.mainForm.SetStatusInfo(eData.DocumentControl, eData.StatusInfo);
            }
            else if (eventData is StatusClear4DocumentEventData)
            {
                var eData = (StatusClear4DocumentEventData)eventData;
                this.mainForm.ClearStatusInfo(eData.DocumentControl);
            }
        }
    }
}
