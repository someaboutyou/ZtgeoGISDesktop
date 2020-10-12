using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Utils
{
    public static class ImageHelp
    {
        public static Bitmap ExColorDepth(Image image)
        {
            int Height = image.Height;
            int Width = image.Width;
            Bitmap bitmap = new Bitmap(Width - 2, Height - 2);
            Bitmap MyBitmap = (Bitmap)image;
            Color pixel;
            for (int x = 1; x < Width - 2; x++)
                for (int y = 1; y < Height - 2; y++)
                {
                    pixel = MyBitmap.GetPixel(x, y);
                    int r, g, b, Result = 0;
                    r = pixel.R;
                    g = pixel.G;
                    b = pixel.B;
                    //实例程序以加权平均值法产生黑白图像   
                    int iType = 1;
                    switch (iType)
                    {
                        case 0://平均值法   
                            Result = ((r + g + b) / 3);
                            break;
                        case 1://最大值法   
                            Result = r > g ? r : g;
                            Result = Result > b ? Result : b;
                            break;
                        case 2://加权平均值法   
                            Result = ((int)(0.7 * r) + (int)(0.2 * g) + (int)(0.1 * b));
                            break;
                    }
                    bitmap.SetPixel(x, y, Color.FromArgb(Result, Result, Result));
                }
            return bitmap;
        }
    }
}
