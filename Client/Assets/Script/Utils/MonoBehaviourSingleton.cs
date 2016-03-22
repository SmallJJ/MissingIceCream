using UnityEngine;
public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
{
    static T m_instance;

    public static T Instance
    {
        get { return m_instance ? m_instance : m_instance = GetInstance(); }
    }

    static T GetInstance()
    {
        T[] instances = FindObjectsOfType<T>();
        if (instances.Length == 0)
        {
            Debug.Log("不存在单例:" + typeof(T) + ",立即创建一个单例");
            GameObject go = new GameObject(typeof(T).ToString());
            DontDestroyOnLoad(go);
            return go.AddComponent<T>();
        }

        if (instances.Length > 1)
        {
            Debug.Log("有多个单例: " + typeof(T) + ",返回第一个发现的");
        }
        return instances[0];
    }

    protected virtual void Awake()
    {
        if (m_instance) Debug.Log("已存在一个单例类的实例: " + typeof(T) + ",你不应该再创建它");
        else m_instance = this as T;
    }
}
