using System;
using System.Collections.Generic;
using UnityEngine;
public sealed class GamePoolMgr: MonoBehaviour
{
    public static GamePoolMgr Instance { get; private set; }
    private Transform m_Transfrom;
    private Dictionary<UIPanelType,string >  m_PanelAssetDic=new Dictionary<UIPanelType,string> ();         //PanelType,Path
    private Dictionary<string ,UnityEngine.Object> m_AssetDic=new Dictionary<string,UnityEngine.Object> ();     //Path,Object

    #region monoBehaviour methods
    void Awake()
    {
        Instance=this;  
        this.m_Transfrom = this.transform;
        this.RegisiterPane();
    }
    #endregion

    #region private methods
    private void RegisiterPane()
    {
        //loginPanel
        this.RegisiterPanelForLogin(UIPanelType.LoginPanel);


    }
    private void RegisiterPanelForLogin(UIPanelType type)
    {
        this.RegisiterPanelAsset(type, PathsConst.Panel_Login + type);
    }
    private void RegisiterPanelForMain(UIPanelType type)
    {
        this.RegisiterPanelAsset(type, PathsConst.Panel_Main + type);
    }

    private void RegisiterPanelForFight(UIPanelType type)
    {
        this.RegisiterPanelAsset(type, PathsConst.Panel_Fight + type);
    }

    private void RegisiterPanelForCommon(UIPanelType type)
    {
        this.RegisiterPanelAsset(type, PathsConst.Component_Common + type);
    }

    private void RegisiterPanelAsset(UIPanelType type,string path)
    {
        if(this.m_PanelAssetDic.ContainsKey(type))
            this.m_PanelAssetDic[type]=path;
        else
            this.m_PanelAssetDic.Add(type,path);
    }

    /// <summary>
    /// 获取资源
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private UnityEngine.Object Get(string path)
    {
        if(string.IsNullOrEmpty(path))
        {
            Debug.Log("Assts path is null !");
            return null;
        }
        try 
	    {	        
		     UnityEngine.Object obj;
            this.m_AssetDic.TryGetValue(path,out obj);
            if(obj==null)
            {
                obj=this.Load(path);
                if(obj!=null)
                  this.Add(path,obj);
            }
            return obj;
	    }
	    catch (Exception ex)
	    {
		    Debug.LogException(ex);
            return null;
	    }
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private UnityEngine.Object Load(string path)
    {
        UnityEngine.Object obj=Resources.Load(path);
        if(obj==null)
            Debug.Log("Can't find asset +"+path);
            return obj;
    }

    private void Add(string path,UnityEngine.Object obj)
    {
        if(this.m_AssetDic.ContainsKey(path))
            this.m_AssetDic.Remove(path);
        this.m_AssetDic.Add(path,obj);
    }

    #endregion

    #region public methods
    /// <summary>
    /// 获取图片
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public Texture2D GetTexture2d(string path)
    {
        return  this.Get(path) as Texture2D;
    }

    /// <summary>
    /// 获取预制物体
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public GameObject GetPrefab(string path)
    {
        return this.Get(path) as GameObject;
    }

    /// <summary>
    /// 获取面板
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public PanelBase GetPanel(UIPanelType type)
    {
        if(type==UIPanelType.None||!this.m_PanelAssetDic.ContainsKey(type))
        {
            Debug.Log("Panel is not Regisiter !");
            return null;
        }
        return TransUtils.InstantiateTransform(this.GetPrefab(this.m_PanelAssetDic[type]).transform, this.m_Transfrom).GetComponent<PanelBase>();
    }

    /// <summary>
    ///  获取组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T GetComponent<T>(string path) where T:ComponentBase
    {
        return this.GetPrefab(path).GetComponent<T>();
    }

    #endregion

}
