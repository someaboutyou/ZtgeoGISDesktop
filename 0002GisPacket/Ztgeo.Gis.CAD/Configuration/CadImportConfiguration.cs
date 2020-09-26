using CADImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Ztgeo.Gis.CAD.Configuration
{
    public interface ICadImportConfiguration {
        bool UseSHXFonts { get; set; }
        DrawGraphicsMode DrawGraphicsMode { get; set; } 
        IList<string> SHXPaths { get; set; }
    }
    public class CadImportConfiguration: ICadImportConfiguration
    {
        public bool UseSHXFonts { get; set; }

        public DrawGraphicsMode DrawGraphicsMode { get; set; } = DrawGraphicsMode.GDIPlus;

        public IList<string> SHXPaths { get; set; }
    }
}
