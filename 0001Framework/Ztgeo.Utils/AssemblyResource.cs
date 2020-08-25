using System.Drawing;
using System.IO; 
using System.Reflection; 
namespace Ztgeo.Utils
{
    public static class AssemblyResource
    {
        /// <summary>
        /// 获取程序集下面资源图片
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="imageName"></param>
        public static Image GetResourceImage(Assembly assembly, string imageName) { 
            Stream stream = assembly.GetManifestResourceStream(imageName);
            if (stream != null)
            {
                return System.Drawing.Image.FromStream(stream);
            }
            else
            {
                return null;
            }
        }
    }
}
