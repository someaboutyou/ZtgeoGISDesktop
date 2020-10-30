using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Winform.Actions;
using Ztgeo.Gis.Winform.Menu;
using ZtgeoGISDesktop.Winform.Share.Forms;

namespace ZtgeoGISDesktop.Menus.Actions
{
    public class Setting : IMenuAction
    {
        private readonly IocManager iocManager;
        public Setting(IocManager _iocManager) {
            iocManager = _iocManager;
        }
        public WinformMenu SenderMenu { set; private get; }

        public void Excute()
        {
            DialogHybirdForm<MenuSettingControl> dialog = new DialogHybirdForm<MenuSettingControl>(iocManager, typeof(ZtgeoGISDesktopMoudle).Assembly, new string[] {
                "WebViews", "MenuSetting", "index.html"
            });
            dialog.Size = new Size(1260, 560);
            dialog.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            dialog.StartPosition = FormStartPosition.CenterScreen;
            dialog.ShowDialog();
        }
    }
}
