using System;

public abstract class RoleDataBase
{
    /// <summary>
    /// 名字
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 资源
    /// </summary>
    public string Asset { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 角色所在的格子
    /// </summary>
    public PathFindingGrid Gird { get; set; }
}
