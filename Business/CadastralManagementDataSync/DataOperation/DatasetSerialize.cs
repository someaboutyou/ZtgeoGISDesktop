using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CadastralManagementDataSync.DataOperation
{
    public static class DatasetSerialize
    {
        /// <summary>
        /// 反序列化压缩的DataSet
        /// </summary>
        /// <param name="_filePath"></param>
        /// <returns></returns>
        static DataSet DataSetDeserializeDecompress(string _filePath)
        {
            FileStream fs = File.OpenRead(_filePath);//打开文件 
            fs.Position = 0;//设置文件流的位置 
            GZipStream gzipStream = new GZipStream(fs, CompressionMode.Decompress);//创建解压对象 
            byte[] buffer = new byte[4096];//定义数据缓冲 
            int offset = 0;//定义读取位置 
            MemoryStream ms = new MemoryStream();//定义内存流 
            while ((offset = gzipStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                ms.Write(buffer, 0, offset);//解压后的数据写入内存流
            }
            BinaryFormatter sfFormatter = new BinaryFormatter();//定义BinaryFormatter以反序列化DataSet对象 
            ms.Position = 0;//设置内存流的位置 
            DataSet ds;
            try
            {
                ds = (DataSet)sfFormatter.Deserialize(ms);//反序列化
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();//关闭内存流
                ms.Dispose();//释放资源
            }
            fs.Close();//关闭文件流
            fs.Dispose();//释放资源
            gzipStream.Close();//关闭解压缩流
            gzipStream.Dispose();//释放资源
            return ds;
        }
        /// <summary>
        /// 反序列化未压缩的DataSet
        /// </summary>
        /// <param name="_filePath"></param>
        /// <returns></returns>
        public static DataSet DataSetDeserialize(string _filePath)
        {
            FileStream fs = File.OpenRead(_filePath);//打开文件 
            fs.Position = 0;//设置文件流的位置 
            BinaryFormatter sfFormatter = new BinaryFormatter();//定义BinaryFormatter以反序列化DataSet对象 
            DataSet ds; 
            try
            {
                ds = (DataSet)sfFormatter.Deserialize(fs);//反序列化
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                fs.Close();//关闭内存流
                fs.Dispose();//释放资源
            }
            fs.Close();//关闭文件流
            fs.Dispose();//释放资源
            return ds;
        }

        public static void DataSetSerialize(string _filePath,DataSet ds) {
            using (FileStream fs = new FileStream(_filePath, FileMode.Create)) {
                BinaryFormatter bf = new BinaryFormatter();
                ds.RemotingFormat = SerializationFormat.Binary;
                bf.Serialize(fs, ds);
                fs.Flush();
                fs.Close();
            }
        }

    }
}
