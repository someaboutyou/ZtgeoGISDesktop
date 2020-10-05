using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.Resources;

namespace Ztgeo.Gis.Winform.Configuration
{
    public interface IResourceConfiguration { 
        IList<IResource> Resources { get; }
    }
    public class ResourceConfiguration
    {
        public IList<IResource> Resources { get; set; }

        public ResourceConfiguration() {
            this.Resources = new List<IResource>();
        }
    }
}
