using Abp.Dependency;
using Castle.MicroKernel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Hybrid.JsBinder;
using Ztgeo.WebViewControl;

namespace Ztgeo.Gis.Hybrid.HybridUserControl
{
    public interface IHybridControl: ITransientDependency
    {
        void LoadResource(Assembly assembly, string[] path);

        DockStyle Dock { get; set; } 
    }
    public abstract class BaseHybridControl<TApp2JSAdapterApi,TJS2AppAdapterApi>: UserControl, IHybridControl, IDisposable
        where TApp2JSAdapterApi : IApp2JSAdapterApi 
        where TJS2AppAdapterApi : IJS2AppAdapterApi 
    {
        protected WebView webView;

        protected IBindableJSContextProvider bindableJSContextProvider;

        protected TApp2JSAdapterApi app2JSAdapterApi;

        protected TJS2AppAdapterApi js2AppAdapterApi;

        protected CommonAPI jsCommonApi;
        public BaseHybridControl(IocManager iocManager) {
            webView = new WebView();
            webView.SetupWebView<TApp2JSAdapterApi>(out bindableJSContextProvider, 
                (newJsCtx) => {
                    TApp2JSAdapterApi adapterApi =  iocManager.Resolve<TApp2JSAdapterApi>();
                    //adapterApi.JsCtx = bindableJSContextProvider; 
                    return adapterApi;
                },
                out app2JSAdapterApi,
                out jsCommonApi
            );
            js2AppAdapterApi = iocManager.Resolve<TJS2AppAdapterApi>();
            app2JSAdapterApi = iocManager.Resolve<TApp2JSAdapterApi>();
            //js2AppAdapterApi.JsCtx = bindableJSContextProvider;
            webView.Dock = DockStyle.Fill;
            this.Controls.Add(webView);
            app2JSAdapterApi.BindCtx4App2Js(this.bindableJSContextProvider);
            js2AppAdapterApi.BindCtx4JS2App(this.bindableJSContextProvider);
        }


        public void LoadResource(Assembly assembly,string[] path)
        {
            webView.LoadResource(new ResourceUrl(assembly, path));
        }
        public new void Dispose()
        {
            this.webView.Dispose();
        }
    }
}
