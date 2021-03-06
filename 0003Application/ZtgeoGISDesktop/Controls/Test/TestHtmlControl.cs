﻿using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Ztgeo.Gis.Hybrid.HybridUserControl;

namespace ZtgeoGISDesktop.Test
{
    public class TestHtmlControl: BaseHybridControl<TestApp2JSAdapterApi,TestJs2AppAdapterApi>
    {
        private readonly TestApp2JSAdapterApi testApp2JSAdapterApi;
        private readonly TestJs2AppAdapterApi testJs2AppAdapterApi;
        public TestHtmlControl(IocManager iocManager,
                TestApp2JSAdapterApi _testApp2JSAdapterApi,
                TestJs2AppAdapterApi _testJs2AppAdapterApi
            ) : base(iocManager) {
            testApp2JSAdapterApi = _testApp2JSAdapterApi;
            testJs2AppAdapterApi = _testJs2AppAdapterApi;
            //testApp2JSAdapterApi.JsCtx = this.bindableJSContextProvider;
            testApp2JSAdapterApi.BindCtx4App2Js(this.bindableJSContextProvider);
            testJs2AppAdapterApi.BindCtx4JS2App(this.bindableJSContextProvider);
        }

        public void JsAlert(string message) {
            testApp2JSAdapterApi.AlertMessage(message);
        }
    }
}
