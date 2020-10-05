using CADImport;
using CADImport.FaceModule;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.MainFormDocument;

namespace Ztgeo.Gis.CAD.Controls
{
    public interface ICADViewDocument: IDocument, Abp.Dependency.ITransientDependency
    {
        CADImage Image { get; }
        /// <summary>
        /// 比例尺
        /// </summary>
        double ImageScale { get; set; }
        /// <summary>
        /// 绘图区域尺寸
        /// Gets or sets base dimensions of the image drawing area
        /// </summary>
        DPoint PictureSize { get; set; }

        RectangleF ImageRectangleF { get; set; }

        Orbit3D Orb3D { get; }
        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="i"></param>
        /// <param name="pt"></param>
        void Zoom(float i, PointF pt);
        /// <summary>
        /// Zoom In
        /// </summary>
        void ZoomIn();
        /// <summary>
        /// Zoom Out
        /// </summary>
        void ZoomOut();

        void ResetScaling();
        /// <summary>
        /// 图层列表
        /// </summary>
        CADEntityCollection Layers { get; }
        /// <summary>
        /// 初始化参数
        /// </summary>
        void InitParams(IDocumentControl hostControl);
        /// <summary>
        /// 是否正在加载文档
        /// </summary>
        bool IsLoading { get; }
    }
}
