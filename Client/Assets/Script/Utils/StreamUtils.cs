using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Text;

public class StreamUtils
{
    public static void StreamToFile(string filePath, object obj)
    {
        IFormatter fm = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        fs.Position = 0;
        fm.Serialize(fs, obj);
        fs.Close();
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
    public static object StreamToObject(byte[] dBytes)
    {
        if (dBytes.Length == 0)
            return null;
        IFormatter fm = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        ms.Write(dBytes, 0, dBytes.Length);
        ms.Position = 0;
        object obj = fm.Deserialize(ms);
        ms.Close();
        return obj;
    }
    public static object StreamFileToObject(string filePath)
    {
        IFormatter fm = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        fs.Position = 0;
        if (fs.Length == 0)
        {
            fs.Close();
            return null;
        }
        object obj = fm.Deserialize(fs);
        fs.Close();
        return obj;
    }
    public static object Clone(object obj)
    {
        IFormatter fm = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
        MemoryStream stream = new MemoryStream();
        fm.Serialize(stream, obj);
        stream.Position = 0;
        object clonedObj = fm.Deserialize(stream);
        stream.Close();
        return clonedObj;
    }
    public static T Clone<T>(T obj)
    {
        IFormatter fm = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
        MemoryStream stream = new MemoryStream();
        fm.Serialize(stream, obj);
        stream.Position = 0;
        object clonedObj = fm.Deserialize(stream);
        stream.Close();
        return (T)clonedObj;
    }
    public static byte[] FileToBytes(string filePath)
    {
        byte[] bytes = null;
        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            bytes = new byte[fs.Length];
            fs.Seek(0, SeekOrigin.Begin);
            fs.Read(bytes, 0, bytes.Length);
        }
        return bytes;
    }
    public static void BytesToFile(string filePath, byte[] bytes)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            fs.Seek(0, SeekOrigin.Begin);
            fs.Write(bytes, 0, bytes.Length);
        }
    }
}