using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.Actions;

namespace Ztgeo.Gis.Winform.Resources
{
    public abstract class AbstractFileResource : IResource
    {
        public virtual string FullName { get;protected set; }

        public virtual Image Icon { get; protected set; }

        public virtual string Name { get; protected set; }

        public virtual string ExtensionName { get; protected set; }

        public virtual int IdentifiedOrder { get; protected set; }

        public IResourceAction ClickAction { get; protected set; }

        public IResourceAction DoubleClickAction { get; protected set; }

        public IOrderedEnumerable<IContextMenuItemAction> ContextActions { get; protected set; }

        public IResourceMetaData ResourceMetaData => throw new NotImplementedException();

        public virtual bool IdentifiedResource()
        {
            if (string.IsNullOrEmpty(this.ExtensionName)||string.IsNullOrEmpty(FullName)) {
                return false;
            }
            string fileExtesion = Path.GetExtension(FullName);
            if (this.ExtensionName.Equals(fileExtesion)) {
                return true;
            }
            return false;
        }
    } 
}
