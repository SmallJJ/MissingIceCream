using System;
using UnityEngine;
using System.Collections.Generic;
public class LevelDataMgr:MonoBehaviourSingleton<LevelDataMgr>
{
    private LevelData m_CurLevelData;
    private LevelKeyData m_LevelKeyData;

    protected override void Awake()
    {
        base.Awake();
        this.GetLevelKeyData();
    }

    #region public methods

    /// <summary>
    /// 加载关卡数据
    /// </summary>
    /// <param name="levelFileName"></param>
    /// <returns></returns>
    public LevelData LoadLevelData(string levelFileName)
    {
        this.m_CurLevelData = this.LoadLevelDataFormLocal(levelFileName);
        return this.m_CurLevelData;
    }

    /// <summary>
    /// 保存关卡数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="levelName"></param>
    public void SaveLevelData(LevelData data,string levelName)
    {
        string levelFileName = this.GetOnlyFileName();
        Debug.Log("GetOnlyFileName: "+ levelFileName);
        this.m_LevelKeyData.AddLevel(levelFileName, levelName);
        this.SaveLevelKeyDataToLocal(this.m_LevelKeyData);
        this.SaveLevelDataToLocal(data, levelFileName);
    }

    /// <summary>
    /// 关卡名字是否已存在
    /// </summary>
    /// <param name="levelName"></param>
    /// <returns></returns>
    public bool IsHasLevelName(string levelName)
    {
        return this.m_LevelKeyData.IsHasValue(levelName);
    }

    /// <summary>
    /// 删除关卡
    /// </summary>
    /// <param name="LevelFileName"></param>
    public void DeleteLevelData(string LevelFileName)
    {
        this.m_LevelKeyData.RemoveLevel(LevelFileName);
        this.DeleteLocalFile(PathConst.LevelKeyDataPath);
        this.SaveLevelKeyDataToLocal(this.m_LevelKeyData);
        this.DeleteLocalFile(PathConst.LevelDataPath+LevelFileName);
    }


    /// <summary>
    /// 获取关卡索引
    /// </summary>
    /// <returns></returns>
    public LevelKeyData GetLevelKeyData()
    {
        if (this.m_LevelKeyData == null)
        {
            LevelKeyData levelData= this.LoadLevelKeyDataFromLocal();
            if (levelData == null)
                this.m_LevelKeyData = new LevelKeyData();
            else
                this.m_LevelKeyData = levelData;
        }
        return this.m_LevelKeyData;
    }
    #endregion

    #region private methods
    /// <summary>
    /// 保存关卡数据到本地
    /// </summary>
    /// <param name="data"></param>
    private void SaveLevelDataToLocal(LevelData data,string levelFileName)
    {
        LocalDataOperationUtils.SaveData(data, PathConst.LevelDataPath+ levelFileName);
    }

    /// <summary>
    /// 从本地读取关卡数据
    /// </summary>
    /// <param name="levelFileName"></param>
    /// <returns></returns>
    private LevelData LoadLevelDataFormLocal(string levelFileName)
    {
        return (LevelData)LocalDataOperationUtils.LoadData(PathConst.LevelDataPath+levelFileName);
    }

    /// <summary>
    /// 保存关卡索引到本地
    /// </summary>
    /// <param name="levelKeyData"></param>
    private void SaveLevelKeyDataToLocal(LevelKeyData levelKeyData)
    {
        LocalDataOperationUtils.SaveData(levelKeyData, PathConst.LevelKeyDataPath);
    }

    /// <summary>
    /// 从本地加载关卡索引
    /// </summary>
    /// <param name="levelKeyData"></param>
    private LevelKeyData LoadLevelKeyDataFromLocal()
    {
        return (LevelKeyData)LocalDataOperationUtils.LoadData(PathConst.LevelKeyDataPath);
    }

    /// <summary>
    /// 删除本地文件
    /// </summary>
    /// <param name="fileName"></param>
    private void DeleteLocalFile(string fileName)
    {
        LocalDataOperationUtils.DeleteFile(fileName);
    }

    /// <summary>
    /// 获取唯一文件名
    /// </summary>
    /// <returns></returns>
    private string GetOnlyFileName()
    {
        DateTime now = DateTime.Now;
        return string.Format("level_{0}_{1}_{2}_{3}_{4}_{5}.Lz", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
    }
    #endregion
}
