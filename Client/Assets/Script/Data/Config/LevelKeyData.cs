using System.Collections.Generic;
using System;

[Serializable]
public class LevelKeyData
{
    private Dictionary<string, string> m_LevelNames = new Dictionary<string, string>(); //Key=LevelFileName,Value=LevelName

    #region public methods
    public Dictionary<string, string> GetData()
    {
        return this.m_LevelNames;
    }

    public void RemoveLevel(string levelFileName)
    {
        this.m_LevelNames.Remove(levelFileName);
    }

    public void AddLevel(string levelFileName, string levelName)
    {
        this.m_LevelNames.Add(levelFileName, levelName);
    }

    public void ChangeLevel(string levelFileName, string levelName)
    {
        this.m_LevelNames[levelFileName] = levelName;
    }

    public bool IsHasKey(string levelFileName)
    {
        return this.m_LevelNames.ContainsKey(levelFileName);
    }

    public bool IsHasValue(string levelName)
    {
        return this.m_LevelNames.ContainsValue(levelName);
    }
    #endregion
}
