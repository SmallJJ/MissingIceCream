using UnityEngine;
using System.Collections;

/// <summary>
/// 物品父级
/// </summary>
public abstract class MaterialBase 
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
    /// 物品所在的格子
    /// </summary>
    public PathFindingGrid Gird { get; set; }
}
