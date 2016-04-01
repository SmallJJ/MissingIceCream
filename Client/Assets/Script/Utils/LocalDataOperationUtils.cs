using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LocalDataOperationUtils
{
    public static void SaveData(object obj, string path)
    {
        string configDataFullPath = Path.Combine(Application.dataPath, path);
        using (FileStream fStream = new FileStream(configDataFullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))//申请文件流
        {
            try
            {
                MemoryStream stream = new MemoryStream();           //申请内存流
                IFormatter binaryFormatter = new BinaryFormatter();      //格式化
                binaryFormatter.Serialize(stream, obj);                   //序列化对象
                byte[] bytes = new byte[stream.Length];                         //声明字节数组
                Debug.Log("原对象字节长度: " + stream.Length);
                stream.Position = 0;
                stream.Read(bytes, 0, bytes.Length);                               //读取序列化后字节
                stream.Close();                                                               //关闭内存流
                byte[] compressionBytes = CompressionUtils.Compression(bytes);  //压缩字节
                Debug.Log("压缩后字节长度: " + compressionBytes.Length);
                fStream.Position = 0;
                fStream.Write(compressionBytes, 0, compressionBytes.Length);        //压缩后的字节写入文件流
                Debug.Log("文件写入成功");
            }
            catch (Exception e)
            {
                Debug.Log("读取序列化流失败: " + e.Message);
            }
        }
    }

    public static object LoadData(string path)
    {
        string configDataFullPath = Path.Combine(Application.dataPath, path);
        using (FileStream fStream = new FileStream(configDataFullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))//申请文件流
        {
            try
            {
                fStream.Position = 0;
                if (fStream.Length == 0) return null;
                byte[] bytes = new byte[fStream.Length];                                                     //声明字节
                Debug.Log("文件流字节长度: " + fStream.Length);
                fStream.Read(bytes, 0, bytes.Length);                                                           //读取文件流中的字节
                byte[] deCompressionBytes = CompressionUtils.DeCompression(bytes);       //解压字节
                Debug.Log("解压后字节长度: " + deCompressionBytes.Length);
                IFormatter binaryFormatter = new BinaryFormatter();                                 //声明格式化
                object obj =binaryFormatter.Deserialize(new MemoryStream(deCompressionBytes));   //反序列化字节
                Debug.Log("反序列化成功");
                return obj;
            }
            catch (Exception e)
            {
                Debug.Log("读取序列化流失败: " + e.Message);
                return null;
            }
        }
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="fileName"></param>
    public static void DeleteFile(string fileName)
    {
        try
        {
            string configDataFullPath = Path.Combine(Application.dataPath, fileName);
            File.Delete(configDataFullPath);
        }
        catch (Exception e)
        {
            Debug.Log("删除文件失败: " + e.Message);
        }
        
    }
}
