using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Actions
{
    public class ContextMenuItemSplitor : IContextMenuItemAction
    {
        public Image ItemIcon { get { return null; } }
         
        public bool IsSplitor { get { return true; } }

        public string Caption { get { return ""; } }
        public  object Sender { get  ; set ; }

        public void Excute()
        {
            //throw new NotImplementedException();
        }
    }
}
