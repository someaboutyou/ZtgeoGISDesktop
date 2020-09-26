using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private IDocumentControl activeDocumentControl = null;
        public IDocument GetActiveDocument {
            get {
                if (activeDocumentControl != null)
                    return activeDocumentControl.Document;
                else
                    return null;
            }
        }

        public IDocumentControl GetActiveDocumentControl {
            get {
                return activeDocumentControl;
            }
        }

        public IDocumentControl AddAChildDocument(IDocument childDocument, IDocument parentDocument)
        {
            throw new NotImplementedException();
        }

        public IDocumentControl AddADocument<T>(string DocumentName) where T : IDocumentControl
        {
            var cocumentCtr = iocManager.Resolve<T>();
            mainForm.AddADocument(cocumentCtr, DocumentName);
            this.DocumentList.Add(cocumentCtr);
            cocumentCtr.SetActive();
            return cocumentCtr;
        }
    }
}
