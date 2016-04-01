using System;
using System.Collections.Generic;
using UnityEngine;
public sealed class GamePoolMgr : MonoBehaviourSingleton<GamePoolMgr>
{
    private Transform m_Transfrom;
    private Dictionary<UIPanelType,string >  m_PanelAssetDic=new Dictionary<UIPanelType,string> ();         //PanelType,Path
    private Dictionary<string ,UnityEngine.Object> m_AssetDic=new Dictionary<string,UnityEngine.Object> ();     //Path,Object

    #region monoBehaviour methods
    protected override void Awake()
    {
        this.m_Transfrom = this.transform;
        this.RegisiterPane();
    }
    #endregion

    #region private methods
    private void RegisiterPane()
    {
        //ForCommon
        this.RegisiterPanelForCommon(UIPanelType.DialogPanel);

        //ForLogin
        this.RegisiterPanelForLogin(UIPanelType.StartGamePanel);

        //ForEditer
        this.RegisiterPanelForEditer(UIPanelType.LevelSetingPanel);
        this.RegisiterPanelForEditer(UIPanelType.LevelEditerPanel);
        this.RegisiterPanelForEditer(UIPanelType.LevelListPanel);
        this.RegisiterPanelForEditer(UIPanelType.SaveLevelPanel);
        this.RegisiterPanelForEditer(UIPanelType.EditerHelpPanel);
        //ForMain
    }

    private void RegisiterPanelForCommon(UIPanelType type)
    {
        this.RegisiterPanelAsset(UIPanelType.DialogPanel, PathConst.Panel_Common + type);
    }
    private void RegisiterPanelForLogin(UIPanelType type)
    {
        this.RegisiterPanelAsset(type, PathConst.Panel_Login + type);
    }
    private void RegisiterPanelForMain(UIPanelType type)
    {
        this.RegisiterPanelAsset(type, PathConst.Panel_Main + type);
    }

    private void RegisiterPanelForEditer(UIPanelType type)
    {
        this.RegisiterPanelAsset(type, PathConst.Panel_LevelEditer + type);
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
    private UnityEngine.Object Get(string path,Type type)
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
                obj=this.Load(path, type);
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
    private UnityEngine.Object Load(string path,Type type)
    {
        UnityEngine.Object obj=Resources.Load(path, type);
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
    public Sprite GetSprite(string path)
    {
        return  this.Get(path,typeof(Sprite)) as Sprite;
    }

    /// <summary>
    /// 获取预制物体
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public GameObject GetPrefab(string path)
    {
        return this.Get(path,typeof(GameObject)) as GameObject;
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
    public T LoadComponent<T>(string path) where T:ComponentBase
    {
        Transform trans= TransUtils.InstantiateTransform(this.GetPrefab(path).transform);
        return trans.GetComponent<T>();
    }

    #endregion

}
