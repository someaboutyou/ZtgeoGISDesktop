using Abp.Dependency;
using CadastralManagementDataSync.Controls;
using CadastralManagementDataSync.DataOperation;
using CadastralManagementDataSync.Resource;
using DevExpress.XtraTab;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.MainFormDocument.Resources;

namespace CadastralManagementDataSync.Documents
{
    public class SyscDataDocument :IDocument, ITransientDependency
    {
        public DataSet DataSet;
        public IDocumentResource DocumentResource { get; private set; }

        public string TypeUniqueCode { get { return "SyscDataDocument"; } }

        public bool IsSubDocument { get { return false; } }

        public IDocumentControl HostControl { get { return syncDataResourceDocumentControl; } }

        private SyncDataResourceDocumentControl syncDataResourceDocumentControl;
        public string DocumentName { get; private set; }

        public void Close()
        {
            //throw new NotImplementedException();
        }
        public void InitDocument(SyncDataResourceDocumentControl control) {
            syncDataResourceDocumentControl = control; 
        }
        public bool IsLoadedResource()
        {
            return this.DocumentResource != null;
        }

        public void LoadFromResource(IDocumentResource documentResource, params object[] otherParams)
        {
            DocumentResource = documentResource;
            var syncDataResource = documentResource as SyncDataResource;
            if (syncDataResource != null) {
                DocumentName = Path.GetFileNameWithoutExtension(syncDataResource.FullName);
                DataSet = DatasetSerialize.DataSetDeserialize(syncDataResource.FullName);
                ShowDataSet(DataSet);
            }
        }

        public void Save()
        {
            //throw new NotImplementedException();
        }

        private void ShowDataSet(DataSet ds) {
            if (ds != null && ds.Tables.Count > 0) {
                for (int i = 0; i < ds.Tables.Count; i++) {
                    XtraTabPage xtraTabPage = new XtraTabPage();
                    xtraTabPage.Text = ds.Tables[i].TableName;
                    xtraTabPage.Controls.Add(CreateTableShow(ds.Tables[i]));
                    syncDataResourceDocumentControl.TabPages.Add(xtraTabPage);
                }
            }
        }
        private Control CreateTableShow(DataTable dt) {
            DataGridView dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.DataSource = dt;
            return dataGridView;
        }
    }
}
