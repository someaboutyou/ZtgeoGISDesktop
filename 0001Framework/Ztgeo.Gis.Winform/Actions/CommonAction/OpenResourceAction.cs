using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.MainFormDocument.Resources;
using Ztgeo.Gis.Winform.Resources;
using Ztgeo.Utils;

namespace Ztgeo.Gis.Winform.Actions.CommonAction
{
    public class OpenResourceAction :IResourceAction
    { 
        public IResource Resource {private get; set; }

        public void Excute()
        {
            if (Resource is IDocumentResource)
            {
                ((IDocumentResource)Resource).Open();
            }
        }
    }
}
