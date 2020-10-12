﻿using Abp.Dependency;
using DevExpress.XtraEditors;
using ESRI.ArcGIS.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Winform.Actions;
using Ztgeo.Gis.Winform.MainFormDocument.Resources;

namespace ZtgeoGISDesktop.Menus.Actions
{
    public class Open : IMenuClickAction
    {
        private readonly IocManager iocManager;
        private readonly IDocumentResourceProvider documentResourceProvider;
        public Open(IocManager _iocManager,
            IDocumentResourceProvider _documentResourceProvider
            ) {
            iocManager = _iocManager;
            documentResourceProvider = _documentResourceProvider;
        }
        public void Excute()
        {
            if (documentResourceProvider.MetaDataProviders.Count <=0) 
            {
                XtraMessageBox.Show("程序中未找到打开文件的方法");
                return;
            }
            XtraOpenFileDialog openFileDialog = new XtraOpenFileDialog();
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "dxf文件 (*.dxf)|*.dxf|dwg (*.dwg)|*.dwg|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
            }
        }

        //private string Fileters() {
            

        //}
    }
}