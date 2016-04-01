using System;
public class RoleLogicDataBase
{
    private RoleDataBase m_Data;
    public RoleLogicDataBase(RoleDataBase data)
    {
        this.m_Data = data;
        this.Asset = data.Asset;
    }

    /// <summary>
    /// 名字
    /// </summary>
    public string Name { get { return this.m_Data.Name; } }

    /// <summary>
    /// 资源
    /// </summary>
    public string Asset { get;private set;}

    

    #region public methods

    public void UpdateData(RoleDataBase data)
    {
        this.m_Data = data;
        this.Asset = data.Asset;
    }

    public void UpdatePos(byte x, byte y)
    {

    }

    public void UpdateAsset(string asset)
    {
        this.Asset = asset;
    }
    #endregion
}
