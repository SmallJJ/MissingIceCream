using UnityEngine;
using System.Collections;

/// <summary>
/// 冰淇淋配置数据
/// </summary>
public class IceCreamData : RoleDataBase
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
    /// 冰淇淋类型
    /// </summary>
    public IceCreamType Type { get; set; }

    /// <summary>
    /// 冰淇淋状态
    /// </summary>
    public IceCreamStatusType StatusType { get; set; }
}
