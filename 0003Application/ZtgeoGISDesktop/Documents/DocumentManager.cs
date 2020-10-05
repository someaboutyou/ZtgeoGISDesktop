using Abp.Dependency;
using DevExpress.XtraRichEdit.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Ztgeo.Gis.Winform.ABPForm;
using Ztgeo.Gis.Winform.MainFormDocument;

namespace ZtgeoGISDesktop.Documents
{
    public class DocumentManager : IDocumentManager
    {
        private readonly IMainForm mainForm;
        private readonly IocManager iocManager;
        public DocumentManager(IMainForm _mainForm,IocManager _iocManager) {
            mainForm = _mainForm;
            iocManager = _iocManager;
            DocumentList = new List<IDocumentControl>(); 
        }
        public IList<IDocumentControl> DocumentList { get; set; }
        //private IDocumentControl activeDocumentControl = null;
        public IDocument GetActiveDocument {
            get {
                if (GetActiveDocumentControl != null)
                    return GetActiveDocumentControl.Document;
                else
                    return null;
            }
        }

        public IDocumentControl GetActiveDocumentControl {
            get {
                return mainForm.ActiveDocumentControl;
            }
        }

        public IDocumentControl AddAChildDocument(IDocument childDocument, IDocument parentDocument)
        {
            throw new NotImplementedException();
        }

        public IDocumentControl AddADocument<T>(string DocumentName) where T : IDocumentControl
        {
            var documentCtr = iocManager.Resolve<T>();
            mainForm.AddADocument(documentCtr, DocumentName); 
            this.DocumentList.Add(documentCtr);
            SetDocumentControlActive(documentCtr);
            return documentCtr;
        }

        public void SetDocumentControlActive(IDocumentControl documentContorl) {
            if (DocumentList != null &&DocumentList.Count>0 && DocumentList.Contains(documentContorl)) {
                mainForm.ManualActiveADocumentControl(documentContorl);
            }
        }
    }
}
