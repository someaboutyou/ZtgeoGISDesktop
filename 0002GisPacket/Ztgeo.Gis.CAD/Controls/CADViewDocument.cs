using Abp.Dependency;
using Abp.Events.Bus;
using CADImport;
using CADImport.FaceModule;
using CADImport.RasterImage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.CAD.Configuration;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.MainFormStatusBar;

namespace Ztgeo.Gis.CAD.Controls
{
    public class CADViewDocument :  ICADViewDocument, IDisposable, ITransientDependency
    {
        private readonly ICadImportConfiguration cadImportConfiguration;
        private CADImport.CADImage cadImage;
        private Thread loadFileThread; //加载文档线程
        private RectangleF imgr; 
        private RenderMode renderMode = RenderMode.Solid;
        private bool navigateDrawMatrix = true;
        private bool showLineWeight = false;
        private bool useDoubleBuffering = false;
        private bool useSelectEntity = false;
        private bool useSHXFonts = false;
        private bool textVisible = true;
        private bool dimVisible = true;
        private bool useWinEllipse = false;
        private string lngFile = "Default";
        private int curLngIndex = 0;
        private bool drawMode = true;
        private Orbit3D orb3D = null;
        public IEventBus EventBus { get; set; }
        public CADViewDocument(ICadImportConfiguration _cadImportConfiguration) {
            EventBus = NullEventBus.Instance;
            cadImportConfiguration = _cadImportConfiguration;
            //InitParams();
        }
        public void InitParams(IDocumentControl hostControl)
        {
            showLineWeight = false;
            useSelectEntity = false;
            useSHXFonts = false;
            ChangeTypeFont();
            textVisible = true;
            dimVisible = true;
            useWinEllipse = false;
            lngFile = "";
            curLngIndex = 0;
            ImageScale = 1;
            drawMode = true;
            Orb3D = new Orbit3D();
            this.Orb3D.RotateEvent += new CADImport.FaceModule.CADImportEventHandler(this.ResizeLayout); 
            this.useDoubleBuffering = true;
            this.HostControl = hostControl;
        }
        public string TypeUniqueCode
        {
            get
            {
                return "Ztgeo.Gis.CAD.Controls.CADViewDocument";
            }
        }
        public Orbit3D Orb3D
        {
            get
            {
                return this.orb3D;
            }
            set
            {
                this.orb3D = value;
            }
        }

        public bool IsSubDocument {
            get {
                return false;
            }
        }

        public IDocument ParentDocument { get; set; }

        public string ExtensionName { get;private set; }

        public string DocumentName { get; private set; }

        public string FilePath { get; private set; }

        public CADImage Image { get; private set; }

        public double ImageScale {
            get
            {
                if (cadImage != null)
                    if (navigateDrawMatrix)
                    {
                        CADMatrix m = (CADMatrix)cadImage.Painter.DrawMatrix.Clone();
                        m.EO = DPoint.Empty;
                        double det = m.Determinant();
                        det = Math.Pow(Math.Abs(det), 1.0 / 3.0);
                        return det;
                    }
                    else
                        return imgr.Width / cadImage.AbsWidth;
                else
                    return 1;
            }
            set
            {
                if (cadImage != null)
                {
                    if (navigateDrawMatrix)
                    {
                        CADMatrix m = (CADMatrix)cadImage.Painter.DrawMatrix.Clone();
                        m.EO = DPoint.Empty;
                        double det = m.Determinant();
                        det = Math.Pow(Math.Abs(det), 1.0 / 3.0);
                        cadImage.Painter.Scale(cadImage.Center, value / det);
                    }
                    else
                    {
                        PointF c = new PointF((float)0.5 * (imgr.Left + imgr.Right), (float)0.5 * (imgr.Top + imgr.Bottom));
                        imgr = new RectangleF(c.X, c.Y, 0, 0);
                        imgr.Inflate((float)(0.5 * value * cadImage.AbsWidth), (float)(0.5 * value * cadImage.AbsHeight));
                        HostControl.Invalidate();
                    }
                }
            }
        }

        public DPoint PictureSize { get; private set; }

        public RectangleF ImageRectangleF { get; private set; }

        public CADLayoutCollection Layouts { get; private set; }

        public IDocumentControl HostControl { get; private set; }


        public void Close()
        {
            //throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (this.cadImage != null) {
                this.cadImage.Dispose();
            }
        }

        public void LoadFromFile(string path, params object[] otherParams)
        {
            this.FilePath = path;
            this.DocumentName=Path.GetFileNameWithoutExtension(path);
            this.ExtensionName = Path.GetExtension(path);
            if (loadFileThread != null) {
                if (loadFileThread.IsAlive)
                    return;
            }
            if (string.IsNullOrEmpty(this.FilePath)) {
                if (cadImage != null) //处理现有打开的文档
                {
                    cadImage.DrawMatrixChanged -= new CADImport.EventHandler(cadImage_DrawMatrixChanged);
                    cadImage.AfterRotate -= new CADImport.EventHandler(cadImage_AfterRotate);
                    cadImage.Dispose();
                    cadImage = null;
                    if (this.HostControl != null && this.HostControl.LayerControl != null) {
                        this.HostControl.LayerControl.Clear();
                    }
                } 
                this.HostControl.SetBusyCursor();
                EventBus.Trigger(new MultiThreadStatusStartEventData("正在打开文件" + this.DocumentName + "...", this, this.HostControl));
                cadImage= CADImage.CreateImageByExtension(FilePath);
                cadImage.NavigateDrawMatrix = navigateDrawMatrix;
                cadImage.visibleArea = ((Control)this.HostControl).ClientSize; 
                cadImage.Painter.viewportRect = new DRect(0, 0, cadImage.visibleArea.Width, cadImage.visibleArea.Height);
                ImageRectangleF = new RectangleF(0, 0, (float)cadImage.AbsWidth, (float)cadImage.AbsHeight);
                cadImage.DrawMatrixChanged += new CADImport.EventHandler(cadImage_DrawMatrixChanged);
                cadImage.AfterRotate += new CADImport.EventHandler(cadImage_AfterRotate);
                CADImport.CADConst.DefaultSHXParameters.UseSHXFonts = cadImportConfiguration.UseSHXFonts;
                if (cadImportConfiguration.UseSHXFonts)
                    DoSHXFonts();
                else
                    DoTTFFonts();
                cadImage.GraphicsOutMode = cadImportConfiguration.DrawGraphicsMode;
                cadImage.ChangeDrawMode(cadImportConfiguration.DrawGraphicsMode, (Control)this.HostControl);
                cadImage.ChangeGraphicsMode(cadImportConfiguration.DrawGraphicsMode, renderMode);
                if (cadImage is CADRasterImage)
                    (cadImage as CADRasterImage).Control = (Control)this.HostControl;
            }
            if (this.cadImage != null)
            {
                CADImage.CodePage = System.Text.Encoding.Default.CodePage;//note - charset was set here
                CADImage.LastLoadedFilePath = Path.GetDirectoryName(path);
                CreateNewLoadThread(path);
            } 
        }

        public void LoadFromStream(Stream stream, params object[] otherParams)
        {
            throw new NotImplementedException();
        }

        public void LoadFromWeb(string url, params object[] otherParams)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {

        }

        public void Zoom(float i, PointF pt)
        {
            if (cadImage == null)
                return;
            if (cadImage.NavigateDrawMatrix)
                cadImage.Painter.Scale(pt, i);
            else
            {
                PointF p = new PointF((ImageRectangleF.Left - pt.X) * i + pt.X, (ImageRectangleF.Top - pt.Y) * i + pt.Y);
                SizeF sz = new SizeF(ImageRectangleF.Width * i, ImageRectangleF.Height * i);
                ImageRectangleF = new RectangleF(p, sz);
            }
        }



        public void ZoomIn() {
            if (cadImage == null) return;
            ImageScale = ImageScale * 2.0f;
        }

        public void ZoomOut() {
            if (cadImage == null) return;
            ImageScale = ImageScale / 2.0f;
        }

        private void CreateNewLoadThread(string fileName)
        {
            loadFileThread = new Thread(LoadCADImage);
            loadFileThread.Name = "LoadFileThread";
            loadFileThread.IsBackground = true;
            loadFileThread.Start(fileName);
        }
        private void LoadCADImage(object fileNameObj)
        {
            lock (cadImage)
            {
                //cadImage.Converter.NumberOfPartsInCircle = 8;
                if (fileNameObj is Stream) {
                    cadImage.LoadFromStream(fileNameObj as Stream);
                } else { 
                    string fileName = (string)fileNameObj;
                    CADConst.DefaultSHXParameters.SHXSearchPaths = string.Join(";",this.cadImportConfiguration.SHXPaths.ToArray()); // 设置字体
                    if (CADConst.IsWebPath(fileName))
                        cadImage.LoadFromWeb(fileName);
                    else
                        this.LoadFromFile(fileName);
                }
            }
            ((Control)this.HostControl).Invoke(new EndThread(SetCADImageOptions));
        }

        public void SetCADImageOptions()
        {
            //cadImage.UseWinEllipse = false;
            cadImage.IsShowLineWeight = this.showLineWeight;
            cadImage.IsWithoutMargins = true;
            //cadImage.IsShowBackground = false;
            //cadImage.BorderSize = 0;
            //cadImage.IsShowLineWeight = true;
            //this.showLineWeight = false;
            //if( (cadImage.Painter == null) && (cadImage.GraphicsOutMode == DrawGraphicsMode.GDIPlus))
            //{
            //    cadImage.ChangeDrawMode(DrawGraphicsMode.GDIPlus, cadPictBox);
            //}
            cadImage.UseDoubleBuffering = this.useDoubleBuffering;
            cadImage.TTFSmoothing = TTFSmoothing.None;
            this.useSelectEntity = false;
            Orb3D.CADImage = cadImage;
            Orb3D.Visible = false;
            Orb3D.Disable3dOrbit();  
            //this.cadImage.IsDraw3DAxes = this.tlbAxes.Pushed;
            HostControl.SetCommonCursor();  
            ObjEntity.cadImage = cadImage;  
            HostControl.Focus();
            if (cadImage.Painter.OpenVPort(cadImage.Converter.ActiveVPort) == 0)
                DoResize(true, true);
            //ChangeControlState();
            HostControl.Invalidate();
        }
        public void DoResize(bool center, bool resize)
        {
            if (cadImage == null)
                return;
            var hCrl= ((Control)HostControl);
            DRect rect = cadImage.Extents;
            double cw = hCrl.ClientSize.Width;
            double ch = hCrl.ClientSize.Height;
            double s = cw / rect.Width;
            if (rect.Height * s > ch)
                s *= (ch / (rect.Height * s));
            if (cadImage.NavigateDrawMatrix)
            {
                CADMatrix m = (CADMatrix)cadImage.Painter.DrawMatrix.Clone();
                if (resize)
                {
                    DPoint scale;
                    scale = new DPoint(s, s, s);
                    if (cadImage.GraphicsOutMode == DrawGraphicsMode.OpenGL)
                        scale.Z = -scale.Z;
                    else
                        if (cadImage.Converter.IsCrossoverMatrix)
                        scale.Y = -scale.Y;
                    m = cadImage.GetRealImageMatrix().Scale(scale);
                }
                if (center)
                {
                    CADMatrix.MatOffset(m, new DPoint(0.5 * cw, 0.5 * ch, 0) - m.AffinePtXMat(cadImage.Center));
                }
                if (resize || center)
                    cadImage.Painter.DrawMatrix = m;
            }
            else
            {
                PictureSize = rect.Size;
                if (resize)
                {
                    ImageScale = s;
                }
                this.cadImage.visibleArea = hCrl.Size;
                if (center)
                {
                    RectangleF r = new RectangleF(new PointF((float)(0.5 * cw), (float)(0.5 * ch)), SizeF.Empty);
                    r.Inflate(0.5f * ImageRectangleF.Width, 0.5f * ImageRectangleF.Height);
                    ImageRectangleF = r;
                }
            }
        }
        private void cadImage_DrawMatrixChanged(Object Sender)
        {
            if (cadImage.NavigateDrawMatrix)
            {
                // update ImageRectangleF
                DRect r = cadImage.CurrentLayout.Box;
                r.TransRectCorners(cadImage.Painter.DrawMatrix);
                imgr = new RectangleF((float)r.left, (float)r.bottom, (float)r.right, (float)r.top);
            } 
            this.HostControl.Invalidate();
        }

        private void cadImage_AfterRotate(Object Sender)
        {
            if (cadImage == null) return;
            if (!cadImage.NavigateDrawMatrix)
            {
                RectangleF r = RectangleF.Empty;
                r.X = (float)cadImage.Painter.updateRect.left;
                r.Y = (float)cadImage.Painter.updateRect.bottom;
                r.Width = (float)cadImage.Painter.updateRect.Width;
                r.Height = (float)cadImage.Painter.updateRect.Height;
                imgr = r;
                this.HostControl.Invalidate();
            }
        }
        private void DoSHXFonts()
        {
            if (cadImage != null)
            {
                CADConst.DefaultSHXParameters.UseTTFFonts = false;
                CADConst.DefaultSHXParameters.UseMultyTTFFonts = false;
            }
        }

        private void DoTTFFonts()
        {
            if (cadImage != null)
            {
                CADConst.DefaultSHXParameters.UseTTFFonts = true;
                CADConst.DefaultSHXParameters.UseMultyTTFFonts = true;
            }
        }
        private void ChangeTypeFont()
        {
            this.useSHXFonts = !this.useSHXFonts;
            //if(cadImage!=null)
            //cadImage.Converter.SHXSettings.UseSHXFonts = this.useSHXFonts;
            CADConst.DefaultSHXParameters.UseSHXFonts = this.useSHXFonts;
            if (this.useSHXFonts)
                DoSHXFonts();
            else
                DoTTFFonts();
            if (this.cadImage != null)
                ReOpen();
        }
        private void ResizeLayout()
        {
            //cadImage.GetExtents();
            this.DoResize(false, false);
        }
        public void ReOpen()
        {
            if (this.FilePath != null)
                if (File.Exists(this.FilePath))
                    this.LoadFromFile(this.FilePath);
        } 
    }
}
