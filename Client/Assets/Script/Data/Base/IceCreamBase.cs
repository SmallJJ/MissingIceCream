using UnityEngine;
using System.Collections;

/// <summary>
/// 冰淇淋父级
/// </summary>
public abstract class IceCreamBase 
{
    /// <summary>
    /// 生命值
    /// </summary>
    public float HP{get;set;}

    /// <summary>
    /// 速度
    /// </summary>
    public float Speed { get; set; }

    /// <summary>
    /// 名字
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 资源
    /// </summary>
    public string Asset { get; set; }

    /// <summary>
    /// 冰淇淋所在的格子
    /// </summary>
    public PathFindingGrid Gird { get; set; }
}
