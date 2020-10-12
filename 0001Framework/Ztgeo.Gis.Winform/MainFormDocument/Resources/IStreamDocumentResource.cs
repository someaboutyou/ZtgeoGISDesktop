using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.MainFormDocument.Resources
{
    public interface IStreamDocumentResource :IDocumentResource
    {
        Stream Stream { get; }

        IStreamDocumentResourceMetaData StreamDocumentResourceMetaData { get; }
    }
}
