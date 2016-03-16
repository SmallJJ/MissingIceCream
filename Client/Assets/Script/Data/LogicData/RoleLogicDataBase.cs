using System;
public class RoleLogicDataBase
{
    private RoleDataBase m_Data;
    public RoleLogicDataBase(RoleDataBase data)
    {
        this.m_Data = data;
        this.Asset = data.Asset;
        this.Gird = data.Gird;
    }

    /// <summary>
    /// 名字
    /// </summary>
    public string Name { get { return this.m_Data.Name; } }

    /// <summary>
    /// 资源
    /// </summary>
    public string Asset { get;private set;}

    /// <summary>
    /// 角色所在的格子
    /// </summary>
    public PathFindingGrid Gird { get; private set;} 

    #region public methods

    public void UpdateGird(PathFindingGrid grid)
    {
        this.Gird = Gird;
    }

    public void UpdateAsset(string asset)
    {
        this.Asset = asset;
    }
    #endregion
}
