using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Utils
{
	public static class StreamUtils
	{
 		public static void PumpStream(Stream inputStream, Stream outputStream, int bufferSize)
		{
			byte[] array = new byte[bufferSize];
			for (; ; )
			{
				int num = inputStream.Read(array, 0, array.Length);
				if (num <= 0)
				{
					break;
				}
				outputStream.Write(array, 0, num);
			}
		}

 		public static void PumpStream(Stream inputStream, Stream outputStream)
		{
			StreamUtils.PumpStream(inputStream, outputStream, 65536);
		}

 		public static void WriteString(Stream outputStream, string value)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			int num = bytes.Length;
			StreamUtils.WriteInt32(outputStream, num);
			outputStream.Write(bytes, 0, num);
		}

 		public static string ReadString(Stream inputStream)
		{
			byte[] array = new byte[StreamUtils.ReadInt32(inputStream)];
			inputStream.Read(array, 0, array.Length);
			return Encoding.UTF8.GetString(array);
		}

 		public static void WriteInt32(Stream outputStream, int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			outputStream.Write(bytes, 0, bytes.Length);
		}

 		public static int ReadInt32(Stream inputStream)
		{
			byte[] array = new byte[4];
			inputStream.Read(array, 0, array.Length);
			return BitConverter.ToInt32(array, 0);
		}

 		public static void WriteBool(Stream outputStream, bool value)
		{
			outputStream.WriteByte(value ? (byte)1 : (byte)0);
		}

 		public static bool ReadBool(Stream inputStream)
		{
			byte[] array = new byte[1];
			inputStream.Read(array, 0, 1);
			return array[0] > 0;
		}

 		public static void WriteGuid(Stream outputStream, Guid guid)
		{
			byte[] array = guid.ToByteArray();
			outputStream.Write(array, 0, array.Length);
		}

 		public static Guid ReadGuid(Stream inputStream)
		{
			byte[] array = new byte[16];
			inputStream.Read(array, 0, 16);
			return new Guid(array);
		}

 		public static Stream ToStream(string str)
		{
			return new MemoryStream(Encoding.UTF8.GetBytes(str));
		}

 		public static byte[] ToArray(Stream stream, int bufferSize)
		{
			MemoryStream memoryStream = stream as MemoryStream;
			if (memoryStream == null)
			{
				memoryStream = new MemoryStream();
				StreamUtils.PumpStream(stream, memoryStream, bufferSize);
			}
			return memoryStream.ToArray();
		}

 		public static byte[] ToArray(Stream stream)
		{
			return StreamUtils.ToArray(stream, 65536);
		}

 		[DebuggerNonUserCode]
		public static T SafeDeserialize<T>(BinaryFormatter formatter, Stream stream) where T : class
		{
			T result;
			try
			{
				result = (formatter.Deserialize(stream) as T);
			}
			catch (FileLoadException)
			{
				result = default(T);
			}
			return result;
		}

 		private const int BufferSize = 65536;

 		private const int GuidSize = 16;
	}
}
