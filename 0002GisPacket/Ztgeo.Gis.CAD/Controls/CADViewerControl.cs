using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.MainFormLayer;
using Ztgeo.Gis.Winform.MainFormProperty;
using CADImport.FaceModule;
using CADImport;

namespace Ztgeo.Gis.CAD.Controls
{
    public partial class CADViewerControl : CADPictureBox, IDocumentControl,Abp.Dependency.ITransientDependency
    {
        private bool isActive;
        private bool useSelectEntity =true;
        private bool selectedEntitiesChanged = false;
        private CADImport.FaceModule.ClipRect clipRectangle; 
        private int cX;
        private int cY;
        private bool detRMouseDown;
        public CADViewerControl(ICADViewDocument cadViewDocument)
        {
            this.Document = cadViewDocument;
            InitializeComponent();
            InitializePicutreBox();
            InitExtParam();
            this.PerformLayout(); 
            cadViewDocument.InitParams(this); 
        }

        private void InitializePicutreBox() {
            BackColor = System.Drawing.Color.White;
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            Cursor = System.Windows.Forms.Cursors.Default;
            DoubleBuffering = true;
            Name = "cadPictBox";
            Ortho = false;
            Position = new System.Drawing.Point(0, 0);
            ScrollBars = CADImport.FaceModule.ScrollBarsShow.None;
            TabStop = false;
            VirtualSize = new System.Drawing.Size(0, 0); 
            Paint += new System.Windows.Forms.PaintEventHandler(this.cadPictBox_Paint);
            KeyDown += new System.Windows.Forms.KeyEventHandler(this.cadPictBox_KeyDown);
            MouseDown += new System.Windows.Forms.MouseEventHandler(this.cadPictBox_MouseDown);
            MouseMove += new System.Windows.Forms.MouseEventHandler(this.cadPictBox_MouseMove);
            MouseUp += new System.Windows.Forms.MouseEventHandler(this.cadPictBox_MouseUp);
            Resize += new System.EventHandler(this.cadPictBox_Resize);
            SetGDIStyle();
        }
        private void InitExtParam()
        {

            //((ICADViewDocument)Document).Image.GraphicsOutMode = DrawGraphicsMode.GDIPlus;
             SetGDIStyle();        // Style = CADPictureBox.gdiStyle;
            //cadPictBox.ScrollBars = ScrollBarsShow.Always;  
            this.clipRectangle = new ClipRect(this );
            this.clipRectangle.MultySelect = false;  
            MouseWheel += new MouseEventHandler(CADPictBoxMouseWheel);
            ScrollEvent += new CADImport.FaceModule.ScrollEventHandlerExt(CADPictBoxScroll);
        }
        public IDocument Document { get;private set; } 

        public ILayerControl LayerControl { get; set; }

        public IPropertiesControl PropertiesControl { get; set; }

        public bool IsActive { get { return this.isActive; }  private set { this.isActive = value; } }

        public Image DocumentImage { get; private set; }

        public void Close()
        {
            this.Document.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        public void SetActive() {

            this.Focus();
            this.isActive = true;
        }

        public void SetUnActive() {

            this.isActive = false;
        }

        public void SetBusyCursor() {
            this.Cursor = Cursors.WaitCursor;
        }

        public void SetCommonCursor() {
            this.Cursor = Cursors.Default;
        }

        public void OpenFile(string filePath) {

            this.Document.LoadFromFile(filePath);
            
        }

        private void cadPictBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (Document == null) return;
            if (((ICADViewDocument)Document).IsLoading)
                return;
            DrawCADImage(e.Graphics, sender as Control);
        }
        public void DrawCADImage(Graphics gr, Control control)
        {
            try
            {
                SetSizePictureBox(((ICADViewDocument)Document).ImageRectangleF);
                if (((ICADViewDocument)Document).Image.NavigateDrawMatrix)
                    ((ICADViewDocument)Document).Image.Draw(gr, RectangleF.Empty, control);
                else
                    ((ICADViewDocument)Document).Image.Draw(gr, ((ICADViewDocument)Document).ImageRectangleF, control);
            }
            catch
            {
                return;
            }
        }
        public void SetSizePictureBox(RectangleF rect)
        {
            Size sz = new Size((int)(rect.Size.Width + 0.5), (int)(rect.Size.Height + 0.5)) + BorderSize;
            SetVirtualSizeNoInvalidate(sz);
            SetPictureBoxPosition(rect.Location);
        }

        private void cadPictBox_Resize(object sender, System.EventArgs e)
        {
            if (Document!=null && ((ICADViewDocument)Document).Image != null)
            {
                if (((ICADViewDocument)Document).Image.Painter != null)
                    ((ICADViewDocument)Document).Image.Painter.viewportRect = ClientRectangle;
                ((ICADViewDocument)Document).Image.visibleArea =  Size;
                Invalidate();
            }
        }

        private void cadPictBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            { 
                case Keys.Add:
                    ((ICADViewDocument)Document).ZoomIn();
                    break;
                case Keys.Subtract:
                    ((ICADViewDocument)Document).ZoomOut();
                    break;
            }
        }
        private void SelectedEntitiesChanged(object sender, PropertyChangedEventArgs args)
        {
            selectedEntitiesChanged = true;
        }
        private void cadPictBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ( Document==null||((ICADViewDocument)Document).Image == null)
                return;
            selectedEntitiesChanged = false;
            ((ICADViewDocument)Document).Image.SelectedEntities.PropertyChanged += new PropertyChangedEventHandler(this.SelectedEntitiesChanged);
            if (e.Button == MouseButtons.Left)
            {
                if ((((ICADViewDocument)Document).Image.SelectionMode == SelectionEntityMode.Enabled) && useSelectEntity)
                {
                    if (((ICADViewDocument)Document).Image.Select(e.X, e.Y))
                        return;
                }
            }
            else
            {
                if ((e.Button == MouseButtons.Right) || (e.Button == MouseButtons.Middle))
                {
                    if ((e.Button == MouseButtons.Middle) && (e.Clicks > 1))
                        DoResize(true, true);
                    else
                    {
                        SetAppCursor(Cursors.Hand);
                        cX = e.X;
                        cY = e.Y;
                        detRMouseDown = true;
                    }
                    return;
                }
            }
            if ((!this.clipRectangle.Enabled) && ((((ICADViewDocument)Document).Image != null) && (!((ICADViewDocument)Document).Orb3D.Visible) && (((ICADViewDocument)Document).Image.GraphicsOutMode == DrawGraphicsMode.GDIPlus)))
                this.clipRectangle.EnableRect(RectangleType.Zooming, new Rectangle(e.X, e.Y, 0, 0));
        }
        public void DoResize(bool center, bool resize)
        {
            if (Document==null ||((ICADViewDocument)Document).Image == null)
                return;
            DRect rect = ((ICADViewDocument)Document).Image.Extents;
            double cw = ClientSize.Width;
            double ch = ClientSize.Height;
            double s = cw / rect.Width;
            if (rect.Height * s > ch)
                s *= (ch / (rect.Height * s));
            if (((ICADViewDocument)Document).Image.NavigateDrawMatrix)
            {
                CADMatrix m = (CADMatrix)((ICADViewDocument)Document).Image.Painter.DrawMatrix.Clone();
                if (resize)
                {
                    DPoint scale;
                    scale = new DPoint(s, s, s);
                    if (((ICADViewDocument)Document).Image.GraphicsOutMode == DrawGraphicsMode.OpenGL)
                        scale.Z = -scale.Z;
                    else
                        if (((ICADViewDocument)Document).Image.Converter.IsCrossoverMatrix)
                        scale.Y = -scale.Y;
                    m = ((ICADViewDocument)Document).Image.GetRealImageMatrix().Scale(scale);
                }
                if (center)
                {
                    CADMatrix.MatOffset(m, new DPoint(0.5 * cw, 0.5 * ch, 0) - m.AffinePtXMat(((ICADViewDocument)Document).Image.Center));
                }
                if (resize || center)
                    ((ICADViewDocument)Document).Image.Painter.DrawMatrix = m;
            }
            else
            {
                ((ICADViewDocument)Document).PictureSize = rect.Size;
                if (resize)
                {
                    ((ICADViewDocument)Document).ImageScale = s;
                }
               ((ICADViewDocument)Document).Image.visibleArea = this.Size;
                if (center)
                {
                    RectangleF r = new RectangleF(new PointF((float)(0.5 * cw), (float)(0.5 * ch)), SizeF.Empty);
                    r.Inflate(0.5f * ((ICADViewDocument)Document).ImageRectangleF.Width, 0.5f * ((ICADViewDocument)Document).ImageRectangleF.Height);
                    ((ICADViewDocument)Document).ImageRectangleF = r;
                }
            }
        }
        private void cadPictBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (Document == null || ((ICADViewDocument)Document).Image == null)
                return;
            //CADGraphicsOpenGL ogl = cadImage.Painter as CADGraphicsOpenGL;
            if (detRMouseDown)
            {
                if (((ICADViewDocument)Document).Image.NavigateDrawMatrix)
                {
                    if (((ICADViewDocument)Document).Image.GraphicsOutMode == DrawGraphicsMode.GDIPlus)
                        ((ICADViewDocument)Document).Image.Painter.Move(new PointF((e.X - cX), (e.Y - cY)));
                    else
                        ((ICADViewDocument)Document).Image.Painter.Move(new PointF((e.X - cX), (cY - e.Y)));
                    cX = e.X;
                    cY = e.Y;
                }
                else
                {
                    PointF pos = PointF.Empty;
                    pos.X -= (cX - e.X);
                    if (((ICADViewDocument)Document).Image.GraphicsOutMode == DrawGraphicsMode.GDIPlus)
                        pos.Y = -(cY - e.Y);
                    else
                        pos.Y = (cY - e.Y);
                    cX = e.X;
                    cY = e.Y;
                    RectangleF r = ((ICADViewDocument)Document).ImageRectangleF;
                    r.Offset(pos);
                    ((ICADViewDocument)Document).ImageRectangleF = r;
                }
            }

           // DPoint vPt = GetRealPoint(e.X, e.Y);
            //stBar.Panels[1].Text = vPt.X.ToString() + ApplicationConstants.sepstr2 + vPt.Y.ToString() +
            //                       ApplicationConstants.sepstr2 + vPt.Z.ToString();
        } 
        private void cadPictBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            detRMouseDown = false;
            if (Cursor == Cursors.Hand)
                SetAppCursor(Cursors.Default);
            if (this.clipRectangle.Enabled)
                if ((this.clipRectangle.Type == RectangleType.Zooming))
                {
                    SizeF visibleAreaSize = new SizeF( Width, Height);

                    if ((this.clipRectangle.ClientRectangle.Width > 5) && (this.clipRectangle.ClientRectangle.Height > 5))
                    {
                        if (!MultipleSelect() && ((ICADViewDocument)Document).ImageScale <= 2000.0f)
                        {
                            Rectangle rect = clipRectangle.ClientRectangle;
                            float vScl = 1.0f;
                            if (visibleAreaSize.Width / rect.Width * rect.Height > visibleAreaSize.Height)
                            {
                                vScl = visibleAreaSize.Height / rect.Height;
                            }
                            else
                            {
                                vScl = visibleAreaSize.Width / rect.Width;
                            }
                            ((ICADViewDocument)Document).Zoom(vScl, new PointF((float)(rect.X + 0.5 * rect.Width), (float)(rect.Y + 0.5 * rect.Height)));
                        }
                    }
                }
            if (selectedEntitiesChanged)
                Invalidate();
            if (((ICADViewDocument)Document).Image != null)
                ((ICADViewDocument)Document).Image.SelectedEntities.PropertyChanged -= new PropertyChangedEventHandler(this.SelectedEntitiesChanged);
            selectedEntitiesChanged = false;
        }
        private bool MultipleSelect()
        {
            if (((ICADViewDocument)Document).Image.SelectionMode == SelectionEntityMode.Enabled)
            {
                int l = this.clipRectangle.ClientRectangle.Left;
                int t = this.clipRectangle.ClientRectangle.Top;
                DPoint pt1 = this.GetRealPoint(l, t);
                DPoint pt2 = this.GetRealPoint(this.clipRectangle.ClientRectangle.Right, this.clipRectangle.ClientRectangle.Bottom);
                DRect tmpRect = new DRect(pt1.X, pt1.Y, pt2.X, pt2.Y);
                ((ICADViewDocument)Document).Image.MultipleSelect(tmpRect, false, true);
                return true;
            }
            return false;
        }
        public DPoint GetRealPoint(int x, int y)
        {
            RectangleF tmpRect = ((ICADViewDocument)Document).ImageRectangleF;
            return CADConst.GetRealPoint(x, y, ((ICADViewDocument)Document).Image, tmpRect);
        } 
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
        private void SetAppCursor(Cursor cursor)
        {
            this.Cursor = cursor;
        }
        private void CADPictBoxMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            float z = 1;
            if ((Control.ModifierKeys & Keys.Control) != 0)
                z = 1.2f;
            if (e.Delta < 0)
                ((ICADViewDocument)Document).Zoom(0.8f * z, e.Location);
            else
                ((ICADViewDocument)Document).Zoom(1.25f / z, e.Location);
        }
        private void CADPictBoxScroll(object sender, CADImport.FaceModule.ScrollEventArgsExt e)
        {
            //if ((e.NewValue == 0) && (e.OldValue == 0) && (e.Type != ScrollEventType.ThumbPosition))
            //    e.NewValue = -5;
            bool update_pos = false;
            PointF pos = PointF.Empty;
            if (e.ScrollOrientation == CADImport.FaceModule.ScrollOrientation.VerticalScroll)
            {
                pos.Y = -(e.NewValue - e.OldValue);
                update_pos = true;
            }
            if (e.ScrollOrientation == CADImport.FaceModule.ScrollOrientation.HorizontalScroll)
            {
                pos.X = -(e.NewValue - e.OldValue);
                update_pos = true;
            }
            if (update_pos)
            {
                RectangleF r = ((ICADViewDocument)Document).ImageRectangleF;
                r.Offset(pos);
                ((ICADViewDocument)Document).ImageRectangleF = r;
            }
        }
        private void SetPictureBoxPosition(PointF value)
        {
            int w1, h1;
            if (value.X > 0)
                w1 = 0;
            else
                w1 = (int)Math.Abs(value.X);
            if (w1 > VirtualSize.Width)
                w1 = VirtualSize.Width;
            if (value.Y > 0)
                h1 = 0;
            else
                h1 = (int)Math.Abs(value.Y);
            if (h1 > VirtualSize.Height)
                h1 = VirtualSize.Height;
           SetPositionNoInvalidate(new Point(w1, h1));
        }
    }
}
