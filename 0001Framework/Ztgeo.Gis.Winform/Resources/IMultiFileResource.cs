using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Resources
{
    public interface IMultiFileResource : IResource
    {
        string MainFilePath { get; set; }

        IList<string> OtherFilePath { get; set; }

        IList<string> OtherFolderPath { get; set; }
    }
}
