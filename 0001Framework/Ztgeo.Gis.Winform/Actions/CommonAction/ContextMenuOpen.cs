using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.Actions;
using Ztgeo.Gis.Winform.MainFormDocument.Resources;
using Ztgeo.Gis.Winform.Resources;
using Ztgeo.Utils;

namespace Ztgeo.Gis.Winform.Actions.CommonAction
{
    public class ContextMenuOpen : IContextMenuItemAction
    {
        public Image ItemIcon { get { return AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.Winform.Icons.Open16.png"); } }

        public object Sender { get; set; }

        public bool IsSplitor { get { return false; } }

        public string Caption { get { return "打开"; } }

        public void Excute()
        { 
            if (Sender is IDocumentResource)
            {
                ((IDocumentResource)Sender).Open();
            }
        }
    }
}
