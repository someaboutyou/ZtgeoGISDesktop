using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Management;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using CefSharp;
using CefSharp.ModelBinding;
using CefSharp.WinForms;
using Ztgeo.WebViewControl.CefSharpMe;

namespace Ztgeo.WebViewControl
{
	public class WebView : UserControl, IDisposable
	{
		public WebView()
		{ 
			this.CurrentDomainId = WebView.domainId.ToString();
			WebView.domainId++;
			this.DefaultLocalUrl = new ResourceUrl("local", "index.html").WithDomain(this.CurrentDomainId);
			this.Initialize();
		}
		static WebView()
		{
			WebView.InitializeCef(); 
			//WindowsEventsListener.WindowUnloaded += WebView.OnWindowUnloaded;
		}
		public virtual string GetRequestUrl(IRequest request)
		{
			return request.Url;
		}
		/// <summary>
		/// 加载嵌入的资源
		/// </summary>
		/// <param name="resourceHandler"></param>
		/// <param name="url"></param>
		internal virtual void LoadEmbeddedResource(CefSharpMe.ResourceHandler resourceHandler, Uri url)
		{
			Assembly assembly = this.ResolveResourceAssembly(url);
			string[] embeddedResourcePath = ResourceUrl.GetEmbeddedResourcePath(url);
			string extension = Path.GetExtension(embeddedResourcePath.Last<string>()).ToLower();
			Stream stream = this.TryGetResourceWithFullPath(assembly, embeddedResourcePath);
			if (stream != null)
			{
				resourceHandler.RespondWith(stream, extension);
			}
		}
		/// <summary>
		/// 定位资源的程序集
		/// </summary>
		/// <param name="resourceUrl"></param>
		/// <returns></returns>
		protected Assembly ResolveResourceAssembly(Uri resourceUrl)
		{
			if (this.assemblies == null)
			{
				this.assemblies = new Dictionary<string, Assembly>();
				AppDomain.CurrentDomain.AssemblyLoad += this.OnAssemblyLoaded;
			}
			string embeddedResourceAssemblyName = ResourceUrl.GetEmbeddedResourceAssemblyName(resourceUrl);
			Assembly assembly = this.GetAssemblyByName(embeddedResourceAssemblyName);
			if (assembly == null)
			{
				if (this.newAssembliesLoaded)
				{
					this.newAssembliesLoaded = false;
					foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
					{
						this.assemblies[assembly2.GetName().Name] = assembly2;
					}
				}
				assembly = this.GetAssemblyByName(embeddedResourceAssemblyName);
				if (assembly == null)
				{
					assembly = AppDomain.CurrentDomain.Load(new AssemblyName(embeddedResourceAssemblyName));
					if (assembly != null)
					{
						this.assemblies[assembly.GetName().Name] = assembly;
					}
				}
			}
			if (assembly != null)
			{
				return assembly;
			}
			throw new InvalidOperationException("Could not find assembly for: " + resourceUrl);
		}

		private Assembly GetAssemblyByName(string assemblyName)
		{
			Assembly result;
			this.assemblies.TryGetValue(assemblyName, out result);
			return result;
		}

		protected virtual Stream TryGetResourceWithFullPath(Assembly assembly, IEnumerable<string> resourcePath)
		{
			return ResourcesManager.TryGetResourceWithFullPath(assembly, resourcePath);
		}

		private void OnAssemblyLoaded(object sender, AssemblyLoadEventArgs args)
		{
			this.newAssembliesLoaded = true;
		}
		#region Events
		internal event Action WebViewInitialized;
		#region BeforeNavigate
		private event Action<CefSharpMe.Request> BeforeNavigate;
		internal bool DoBeforeNavigate(IRequest request,string url) {
			if (this.BeforeNavigate != null) {
				CefSharpMe.Request wrappedRequest = new Request(request, url);
				this.ExecuteWithAsyncErrorHandling(()=>
				{
					this.BeforeNavigate(wrappedRequest);
				});
				return wrappedRequest.Canceled;
			}
			return false;
		}
		#endregion
		#region BeforeResourceLoad 加载资源之前执行事件
		internal event Action<CefSharpMe.ResourceHandler> BeforeResourceLoad;
		internal void DoBeforeResourceLoad(CefSharpMe.ResourceHandler resourceHandler) {
			if (BeforeResourceLoad != null)
			{
				BeforeResourceLoad(resourceHandler);
			}
		}
		internal void DoBeforeResourceLoadExecuteWithAsyncErrorHandling(CefSharpMe.ResourceHandler resourceHandler) {
			if (BeforeResourceLoad != null)
			{
				this.ExecuteWithAsyncErrorHandling(delegate
				{
					this.BeforeResourceLoad(resourceHandler);
				});
			}
		}
		public void AddBeforeResourceLoadEvent(Action<CefSharpMe.ResourceHandler> handler) {
			this.BeforeResourceLoad += handler;
		}
		#endregion

		internal event Action<string> Navigated;

		internal event Action<string, int> LoadFailed;

		#region ResourceLoadFailed 加载资源失败执行事件
		internal event Action<string> ResourceLoadFailed;
		internal void DoResourceLoadFailed(string url) {
			if (this.ResourceLoadFailed != null)
			{
				this.ResourceLoadFailed(url);
			}
			else {
				this.ExecuteWithAsyncErrorHandling(delegate
				{
					throw new InvalidOperationException("Resource not found: " + url);
				});
			}
		}
		#endregion
		#region DownloadProgressChanged
		private event Action<string, long, long> DownloadProgressChanged;
		internal void DoDownloadProgressChanged(string fullPath,long receivedBytesLength,long totalByresLength) {
			if (DownloadProgressChanged != null) {
				this.AsyncExecuteInUI(() =>
				{
					DownloadProgressChanged(fullPath, receivedBytesLength, totalByresLength);
				});
			}
		}
		#endregion
		#region DownloadCompleted
		private event Action<string> DownloadCompleted;
		internal void DoDownloadCompleted(string fullPath) {
			if (this.DownloadCompleted != null) {
				this.AsyncExecuteInUI(() =>
				{
					DownloadCompleted(fullPath);
				});
			}
		}
		#endregion
		#region DownloadCancelled
		private event Action<string> DownloadCancelled;
		internal void DoDownloadCancelled(string fullPath) {
			if (this.DownloadCancelled != null) {
				this.AsyncExecuteInUI(() =>
				{
					DownloadCancelled(fullPath);
				});
			}
		}
		#endregion
		#region JavascriptContextCreated
		private event Action JavascriptContextCreated;
		internal void DoJavascriptContextCreated() { 
			if (JavascriptContextCreated != null)
			{
				this.ExecuteWithAsyncErrorHandling(()=>
				{
					JavascriptContextCreated();
				});
			}
		}
		internal void AddJavascriptContextCreatedEvent(Action actionWant2Add) {
			JavascriptContextCreated += actionWant2Add;
		}
		internal void RemoveJavascriptContextCreatedEvent(Action actionWant2Remove)
		{
			JavascriptContextCreated -= actionWant2Remove;
		}
		#endregion

		internal event Action TitleChanged;

		internal event Action<CefException.UnhandledExceptionEventArgs> UnhandledAsyncException;

		internal event Action Disposed;

		#region RenderProcessCrashed
		private event Action RenderProcessCrashed;
		internal void AddRenderProcessCrashedEvent(Action actionWant2Add) {
			RenderProcessCrashed += actionWant2Add;
		}
		internal void DoRenderProcessCrashed() {
			if (RenderProcessCrashed != null) {
				RenderProcessCrashed();
			}
		}
		#endregion
		#region JavascriptContextReleased
		private event Action JavascriptContextReleased;
		internal void DoJavascriptContextReleased() {
			if (this.JavascriptContextReleased != null) {
				this.ExecuteWithAsyncErrorHandling(()=>
				{
					JavascriptContextReleased();
				});
			}
		}
        #endregion
        public static Action<WebView> GlobalWebViewInitialized;
        #endregion
		/// <summary>
		/// Form 卸载时的操作
		/// </summary>
		/// <param name="form"></param>
        private static void OnWindowUnloaded(Form form)
		{
			IEnumerable<WebView> disposableWebViews = WebView.DisposableWebViews;
			//Func<WebView, bool> <> 9__0;
			//Func<WebView, bool> predicate;
			//if ((predicate = <> 9__0) == null)
			//{
			//	predicate = (<> 9__0 = ((WebView w) => !w.IsLoaded && Window.GetWindow(w) == window));
			//}
			//WebView[] array = disposableWebViews.Where((w) => !w.IsLoaded && Window.GetWindow(w) == window).ToArray<WebView>();
			WebView[] array = disposableWebViews.Where((w) => !w.Created 
				&& w.FindForm()== form).ToArray<WebView>();
			for (int i = 0; i < array.Length; i++) 
			{
				array[i].Dispose();
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void InitializeCef()
		{
			if (!Cef.IsInitialized)
			{
				CefSettings cefSettings = new CefSettings();
				cefSettings.LogSeverity = LogSeverity.Disable;
				cefSettings.UncaughtExceptionStackSize = 100;
				cefSettings.CachePath = WebView.TempDir;
				cefSettings.WindowlessRenderingEnabled = true;
				CefSharpSettings.LegacyJavascriptBindingEnabled = true;
				CefSharpSettings.SubprocessExitIfParentProcessClosed = true;
				foreach (string schemeName in WebView.CustomSchemes)
				{
					cefSettings.RegisterScheme(new CefCustomScheme
					{
						SchemeName = schemeName,
						SchemeHandlerFactory = new CefSchemeHandlerFactory()
					});
				}
				cefSettings.BrowserSubprocessPath = CefLoader.GetBrowserSubProcessPath();
				Cef.Initialize(cefSettings, false, browserProcessHandler:  null);
				if (System.Windows.Application.Current != null)
				{
					System.Windows.Application.Current.Exit += WebView.OnApplicationExit;
					WebView.subscribedApplicationExit = true;
				}
			}
		}

		[DebuggerNonUserCode]
		public static void Cleanup()
		{
			if (!Cef.IsInitialized)
			{
				return;
			}
			try
			{
				Cef.Shutdown();
			}
			catch (Exception ex) {
				throw ex;
			}
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(WebView.TempDir);
				if (directoryInfo.Exists)
				{
					directoryInfo.Delete(true);
				}
			}
			catch (IOException)
			{
			}
		}


		[MethodImpl(MethodImplOptions.NoInlining)]
		private void Initialize()
		{
			if (!WebView.subscribedApplicationExit)
			{
				//System.Windows.Application.Current.Exit += WebView.OnApplicationExit;  
				WebView.subscribedApplicationExit = true;
			}
			this.settings = new BrowserSettings();
			this.lifeSpanHandler = new CefLifeSpanHandler(this);
			this.chromium = new ChromiumWebBrowser(null);
			this.chromium.BrowserSettings = this.settings;
			this.chromium.IsBrowserInitializedChanged += this.OnWebViewIsBrowserInitializedChanged;
			this.chromium.FrameLoadEnd += this.OnWebViewFrameLoadEnd;
			this.chromium.LoadError += this.OnWebViewLoadError;
			this.chromium.TitleChanged += this.OnWebViewTitleChanged;
			this.chromium.PreviewKeyDown += this.OnPreviewKeyDown;
			this.chromium.RequestHandler = new CefRequestHandler(this);
			this.chromium.ResourceHandlerFactory = new CefResourceHandlerFactory(this);
			this.chromium.LifeSpanHandler = this.lifeSpanHandler;
			this.chromium.RenderProcessMessageHandler = new  CefRenderProcessMessageHandler(this);
			this.chromium.MenuHandler = new CefMenuHandler(this);
			this.chromium.DialogHandler = new CefDialogHandler(this);
			this.chromium.DownloadHandler = new  CefDownloadHandler(this);
			//this.chromium.CleanupElement = new FrameworkElement();
			this.jsExecutor = new JavascriptExecutor(this);
			this.RegisterJavascriptObject("__WebviewListener__", this.eventsListener, null, null, false);
			//base.Content = this.chromium;
			base.Controls.Add(this.chromium);
			Action<WebView> globalWebViewInitialized = WebView.GlobalWebViewInitialized;
			if (globalWebViewInitialized != null)
			{
				globalWebViewInitialized(this);
			}
			WebView.DisposableWebViews.Add(this);
			this.chromium.Focus(); 
		}

		public static void OnApplicationExit(object sender, EventArgs e)
		{
			WebView.Cleanup();
		}

		~WebView()
		{
			this.Dispose();
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (this.isDisposing)
			{
				return;
			}
			this.isDisposing = true;
			if (this.javascriptPendingCalls == 0)
			{
				this.InternalDispose();
			}
			else
			{
				base.BeginInvoke(new Action(this.InternalDispose), Array.Empty<object>());
			}
			GC.SuppressFinalize(this);

		}  

		private void InternalDispose()
		{
			AppDomain.CurrentDomain.AssemblyLoad -= this.OnAssemblyLoaded;
			WebView.DisposableWebViews.Remove(this);
			this.cancellationTokenSource.Cancel();
			this.WebViewInitialized = null;
			this.BeforeNavigate = null;
			this.BeforeResourceLoad = null;
			this.Navigated = null;
			this.LoadFailed = null;
			this.DownloadProgressChanged = null;
			this.DownloadCompleted = null;
			this.DownloadCancelled = null;
			this.JavascriptContextCreated = null;
			this.TitleChanged = null;
			this.UnhandledAsyncException = null;
			this.RenderProcessCrashed = null;
			this.JavascriptContextReleased = null;
			this.jsExecutor.Dispose();
			this.settings.Dispose();
			this.chromium.Dispose();
			this.cancellationTokenSource.Dispose();
			Action disposed = this.Disposed;
			if (disposed == null)
			{
				return;
			}
			disposed();
		}

		/// <summary>
		/// F12 开发者模式
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (this.AllowDeveloperTools && e.KeyData == Keys.F12)
			{
				if (this.isDeveloperToolsOpened)
				{
					this.CloseDeveloperTools();
				}
				else
				{
					this.ShowDeveloperTools();
				}
				//e.Handled = true;
			}
		}
		public void ShowDeveloperTools()
		{
			this.ExecuteWhenInitialized(delegate
			{
				this.chromium.ShowDevTools();
				this.isDeveloperToolsOpened = true;
			});
		}
		public void CloseDeveloperTools()
		{
			if (this.isDeveloperToolsOpened)
			{
				this.chromium.CloseDevTools();
				this.isDeveloperToolsOpened = false;
			}
		}
		public bool AllowDeveloperTools { get; set; }
		public string Address
		{
			get
			{
				return this.chromium.Address;
			}
			set
			{
				this.Load(value);
			}
		}
		private void Load(string address)
		{
			if (address != this.DefaultLocalUrl)
			{
				this.htmlToLoad = null;
			}
			if (address.Contains(Uri.SchemeDelimiter) || address == "about:blank" || address.StartsWith("data:"))
			{
				if (WebView.CustomSchemes.Any((string s) => address.StartsWith(s + Uri.SchemeDelimiter)))
				{
					this.IsSecurityDisabled = true;
				}
				else
				{
					this.IsSecurityDisabled = false;
				}
				this.ExecuteWhenInitialized(delegate
				{
					this.chromium.Load(address);
				});
				return;
			}
			Assembly assembly = WebView.GetUserCallingMethod(false).ReflectedType.Assembly;
			this.Load(new ResourceUrl(assembly, new string[]
			{
				address
			}).WithDomain(this.CurrentDomainId));
		}
		public void LoadResource(ResourceUrl resourceUrl)
		{
			this.Address = resourceUrl.WithDomain(this.CurrentDomainId);
		}
		public void LoadHtml(string html)
		{
			this.htmlToLoad = html;
			this.Load(this.DefaultLocalUrl);
		}
		public bool IsSecurityDisabled
		{
			get
			{
				return this.settings.WebSecurity != CefState.Enabled;
			}
			set
			{
				this.settings.WebSecurity = (value ? CefState.Disabled : CefState.Enabled);
			}
		}
		public bool IgnoreCertificateErrors { get; set; }
		public bool IsHistoryDisabled { get; set; }
		public TimeSpan? DefaultScriptsExecutionTimeout { get; set; }
		public bool DisableBuiltinContextMenus { get; set; }
		public bool DisableFileDialogs { get; set; }

		public bool IsBrowserInitialized
		{
			get
			{
				return this.chromium.IsBrowserInitialized;
			}
		}

		public bool IsJavascriptEngineInitialized
		{
			get
			{
				return this.chromium.CanExecuteJavascriptInMainFrame;
			}
		}

		public ProxyAuthentication ProxyAuthentication { get; set; }

		public bool IgnoreMissingResources { get; set; }

		public bool RegisterJavascriptObject(string name, object objectToBind, Func<Func<object>, object> interceptCall = null, Func<object, Type, object> bind = null, bool executeCallsInUI = false)
		{
			if (this.chromium.JavascriptObjectRepository.IsBound(name))
			{
				return false;
			}
			if (executeCallsInUI)
			{
				Func<Func<object>, object> interceptCall2 = (target) => this.Invoke(target);
				return this.RegisterJavascriptObject(name, objectToBind, interceptCall2, bind, false);
			}
			BindingOptions bindingOptions = new BindingOptions();
			if (bind != null)
			{
				bindingOptions.Binder = new LambdaMethodBinder(bind);
			}
			else
			{
				bindingOptions.Binder = this.binder;
			}
			Func<Func<object>, object> interceptCall3;
			if (interceptCall != null)
			{
				interceptCall3 = delegate (Func<object> target)
				{
					if (this.isDisposing)
					{
						return null;
					}
					object result;
					try
					{
						this.javascriptPendingCalls++;
						result = interceptCall(target);
					}
					finally
					{
						this.javascriptPendingCalls--;
					}
					return result;
				};
			}
			else
			{
				interceptCall3 = delegate (Func<object> target)
				{
					if (this.isDisposing)
					{
						return null;
					}
					object result;
					try
					{
						this.javascriptPendingCalls++;
						result = target();
					}
					finally
					{
						this.javascriptPendingCalls--;
					}
					return result;
				};
			}
			bindingOptions.MethodInterceptor = new LambdaMethodInterceptor(interceptCall3);
			this.chromium.JavascriptObjectRepository.Register(name, objectToBind, true, bindingOptions);
			return true;
		}

		public void RegisterJsObject(string name, object objectToBind) {
			this.chromium.RegisterJsObject(name, objectToBind);
		}
		public T EvaluateScript<T>(string script)
		{
			return this.jsExecutor.EvaluateScript<T>(script, null);
		}

		public T EvaluateScript<T>(string script, TimeSpan? timeout)
		{
			return this.jsExecutor.EvaluateScript<T>(script, timeout);
		}

		public void ExecuteScript(string script)
		{
			this.jsExecutor.ExecuteScript(script);
		}

		public void ExecuteScriptFunction(string functionName, params string[] args)
		{
			this.jsExecutor.ExecuteScriptFunction(functionName, false, args);
		}

		public T EvaluateScriptFunction<T>(string functionName, params string[] args)
		{
			return this.jsExecutor.EvaluateScriptFunction<T>(functionName, false, args);
		}

		internal void ExecuteScriptFunctionWithSerializedParams(string functionName, params object[] args)
		{
			this.jsExecutor.ExecuteScriptFunction(functionName, true, args);
		}

		internal T EvaluateScriptFunctionWithSerializedParams<T>(string functionName, params object[] args)
		{
			return this.jsExecutor.EvaluateScriptFunction<T>(functionName, true, args);
		}

		public bool CanGoBack
		{
			get
			{
				return this.chromium.CanGoBack;
			}
		}

		public bool CanGoForward
		{
			get
			{
				return this.chromium.CanGoForward;
			}
		}

		public void GoBack()
		{
			this.chromium.Back();
		}

		public void GoForward()
		{
			this.chromium.Forward();
		}

		public void Reload(bool ignoreCache = false)
		{
			if (this.chromium.IsBrowserInitialized && !this.chromium.IsLoading)
			{
				this.chromium.Reload(ignoreCache);
			}
		}
		public string Title
		{
			get
			{
				return this.chromium.Text;
			}
		}
		public double ZoomPercentage
		{
			get
			{
				return Math.Pow(1.2000000476837158,  this.chromium.GetZoomLevelAsync().Result);
			}
			set
			{
				this.ExecuteWhenInitialized(delegate
				{
					//this.chromium.ZoomLevel = Math.Log(value, 1.2000000476837158);
					this.chromium.SetZoomLevel(Math.Log(value, 1.2000000476837158));
				});
			}
		}

		public Listener AttachListener(string name, Action handler, bool executeInUI = true)
		{
			Action<string> handler2 = delegate (string eventName)
			{
				if (!this.isDisposing && eventName == name)
				{
					if (executeInUI)
					{
						this.Invoke(handler);
						return;
					}
					this.ExecuteWithAsyncErrorHandling(handler);
				}
			};
			Listener listener = new Listener(name, handler2);
			this.eventsListener.NotificationReceived += listener.Handler;
			return listener;
		}
		 
		public void DetachListener(Listener listener)
		{
			this.eventsListener.NotificationReceived -= listener.Handler;
		}
		 
		private void OnWebViewIsBrowserInitializedChanged(object sender, EventArgs e)
		{
			if (!this.chromium.IsBrowserInitialized)
			{
				this.Dispose();
				return;
			}
			if (this.pendingInitialization != null)
			{
				this.pendingInitialization();
				this.pendingInitialization = null;
			} 
			if (this.WebViewInitialized != null)
			{
				this.WebViewInitialized();
			}
		}
		 
		private void OnWebViewFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
		{
			this.htmlToLoad = null;
			Action<string> navigated = this.Navigated;
			if (e.Frame.IsMain && navigated != null)
			{
				this.AsyncExecuteInUI(delegate
				{
					navigated(e.Url);
				});
			}
		}
		 
		private void OnWebViewLoadError(object sender, LoadErrorEventArgs e)
		{
			this.htmlToLoad = null;
			Action<string, int> loadFailed = this.LoadFailed;
			if (e.ErrorCode != CefErrorCode.Aborted && loadFailed != null)
			{
				this.AsyncExecuteInUI(delegate
				{
					loadFailed(e.FailedUrl, (int)e.ErrorCode);
				});
			}
		}
		 
		private void OnWebViewTitleChanged(object sender, TitleChangedEventArgs e)
		{
			Action titleChanged = this.TitleChanged;
			if (titleChanged == null)
			{
				return;
			}
			titleChanged();
		}
		 
		public static void SetCookie(string url, string domain, string name, string value, DateTime expires)
		{
			Cookie cookie = new Cookie
			{
				Domain = domain,
				Name = name,
				Value = value,
				Expires = new DateTime?(expires)
			};
			Cef.GetGlobalCookieManager().SetCookieAsync(url, cookie);
		} 
		public static string CookiesPath
		{
			set
			{
				Cef.GetGlobalCookieManager().SetStoragePath(value, true, null);
			}
		}
		 
		internal bool FilterRequest(IRequest request)
		{
			return request.Url.StartsWith("chrome-devtools:", StringComparison.InvariantCultureIgnoreCase) || request.Url.Equals(this.DefaultLocalUrl, StringComparison.InvariantCultureIgnoreCase);
		}
		 
		private static bool IsFrameworkAssemblyName(string name)
		{
			return name == "PresentationFramework" || name == "PresentationCore" || name == "mscorlib" || name == "System.Xaml" || name == "WindowsBase";
		} 
		internal static MethodBase GetUserCallingMethod(bool captureFilenames = false)
		{
			Assembly currentAssembly = typeof(WebView).Assembly;
			MethodBase methodBase = (from f in new StackTrace(captureFilenames).GetFrames()
									 select f.GetMethod() into m
									 where m.ReflectedType.Assembly != currentAssembly
									 select m).First((MethodBase m) => !WebView.IsFrameworkAssemblyName(m.ReflectedType.Assembly.GetName().Name));
			if (methodBase == null)
			{
				throw new InvalidOperationException("Unable to find calling method");
			}
			return methodBase;
		}
		 
		private void ExecuteWhenInitialized(Action action)
		{
			if (this.IsBrowserInitialized)
			{
				action();
				return;
			}
			this.pendingInitialization = (Action)Delegate.Combine(this.pendingInitialization, action);
		}
		 
		public event Action<string> PopupOpening
		{
			add
			{
				this.lifeSpanHandler.PopupOpening += value;
			}
			remove
			{
				this.lifeSpanHandler.PopupOpening -= value;
			}
		}
		 
		internal void AsyncExecuteInUI(Action action)
		{
			if (this.isDisposing)
			{
				return;
			}
			base.Invoke(new Action(()=>{
				if (!this.isDisposing)
				{
					this.ExecuteWithAsyncErrorHandling(action);
				}
			} ));
		}
		 
		[DebuggerNonUserCode]
		internal void ExecuteWithAsyncErrorHandling(Action action)
		{
			try
			{
				action();
			}
			catch (Exception e)
			{
				this.ForwardUnhandledAsyncException(e);
			}
		}

		internal void ForwardUnhandledAsyncException(Exception e)
		{
			bool flag = false;
			Action<CefException.UnhandledExceptionEventArgs> unhandledAsyncException = this.UnhandledAsyncException;
			if (unhandledAsyncException != null)
			{
				CefException.UnhandledExceptionEventArgs unhandledExceptionEventArgs = new CefException.UnhandledExceptionEventArgs(e);
				unhandledAsyncException(unhandledExceptionEventArgs);
				flag = unhandledExceptionEventArgs.Handled;
			}
			if (!flag)
			{
				base.BeginInvoke(new Action(delegate
				{
					throw e;
				}), Array.Empty<object>());
			}
		}
		//internal IInputElement FocusableElement
		//{
		//	get
		//	{
		//		return this.chromium;
		//	}
		//}

		protected void InitializeBrowser()
		{
			//this.chromium.CreateBrowser();
		}

		private Dictionary<string, Assembly> assemblies;

 		private bool newAssembliesLoaded = true;

 		private static readonly string[] CustomSchemes = new string[]
		{
			"local",
			"embedded",
			"custom"
		};

		internal static readonly string TempDir = Path.Combine(Path.GetTempPath(), "WebView" + Guid.NewGuid().ToString().Replace("-", null) + DateTime.UtcNow.Ticks);

		internal const string ChromeInternalProtocol = "chrome-devtools:";

		internal const float PercentageToZoomFactor = 1.2f;

		internal static readonly List<WebView> DisposableWebViews = new List<WebView>();

		internal static bool subscribedApplicationExit = false;

		internal ChromiumWebBrowser chromium;

		internal BrowserSettings settings;

		internal bool isDeveloperToolsOpened;

		internal bool isDisposing;

		internal Action pendingInitialization;

		internal string htmlToLoad;

		internal JavascriptExecutor jsExecutor;

		internal CefLifeSpanHandler lifeSpanHandler;

		internal volatile int javascriptPendingCalls;

		internal readonly DefaultBinder binder = new DefaultBinder(new DefaultFieldNameConverter());

		internal readonly BrowserObjectListener eventsListener = new BrowserObjectListener();

		internal readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

		internal static int domainId;

		internal readonly string CurrentDomainId;

 		internal readonly string DefaultLocalUrl;
		 
	 }
}
