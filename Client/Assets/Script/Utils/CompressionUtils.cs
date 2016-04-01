using ICSharpCode.SharpZipLib.GZip;
using System;
using System.IO;

public class CompressionUtils
{
    /// <summary>
    /// —πÀı
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
	public static byte[] Compression(byte[] bytes)
	{
		byte[] result = null;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (GZipOutputStream gZipOutputStream = new GZipOutputStream(memoryStream))
			{
				gZipOutputStream.Write(bytes, 0, bytes.Length);
			}
			result = memoryStream.ToArray();
		}
		return result;
	}

    /// <summary>
    /// Ω‚—π
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
	public static byte[] DeCompression(byte[] bytes)
	{
		byte[] result = null;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (GZipInputStream gZipInputStream = new GZipInputStream(new MemoryStream(bytes)))
			{
				byte[] array = new byte[2000];
				int count;
				while ((count = gZipInputStream.Read(array, 0, array.Length)) > 0)
				{
					memoryStream.Write(array, 0, count);
				}
			}
			result = memoryStream.ToArray();
		}
		return result;
	}
}
