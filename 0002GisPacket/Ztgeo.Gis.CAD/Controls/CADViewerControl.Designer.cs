namespace Ztgeo.Gis.CAD.Controls
{
    partial class CADViewerControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cadPictureBox1 = new CADImport.FaceModule.CADPictureBox();
            this.SuspendLayout();
            // 
            // cadPictureBox1
            // 
            this.cadPictureBox1.BackColor = System.Drawing.Color.Black;
            this.cadPictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cadPictureBox1.DoubleBuffering = true;
            this.cadPictureBox1.Location = new System.Drawing.Point(0, 0);
            this.cadPictureBox1.Name = "cadPictureBox1";
            this.cadPictureBox1.Ortho = false;
            this.cadPictureBox1.Position = new System.Drawing.Point(0, 0);
            this.cadPictureBox1.ScrollBars = CADImport.FaceModule.ScrollBarsShow.None;
            this.cadPictureBox1.Size = new System.Drawing.Size(509, 383);
            this.cadPictureBox1.TabIndex = 0;
            this.cadPictureBox1.VirtualSize = new System.Drawing.Size(0, 0);
            // 
            // CADViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cadPictureBox1);
            this.Name = "CADViewerControl";
            this.Size = new System.Drawing.Size(509, 383);
            this.ResumeLayout(false);

        }

        #endregion

        private CADImport.FaceModule.CADPictureBox cadPictureBox1;
    }
}
