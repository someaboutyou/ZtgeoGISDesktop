using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using CADImport;
using CADImport.FaceModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.CAD.Controls;
using Ztgeo.Gis.Winform.Events;

namespace Ztgeo.Gis.CAD.Events
{
    class PropertyControlChangeEventHandler: IEventHandler<DocumentActiveChangeEventData>, ITransientDependency
    {
        public void HandleEvent(DocumentActiveChangeEventData documentActiveChangeEventData) {
            if (documentActiveChangeEventData.ChangeToDocumentControl is CADViewerControl) {
                var cadViewerControl = documentActiveChangeEventData.ChangeToDocumentControl as CADViewerControl;
                if (cadViewerControl != null && cadViewerControl.PropertiesControl != null) {
                    ObjEntity.propGrid = (CADPropertyGrid)cadViewerControl.PropertiesControl;
                    CADImportFace.EntityPropertyGrid = (CADPropertyGrid)cadViewerControl.PropertiesControl;
                }
            }
        }
    }
}
