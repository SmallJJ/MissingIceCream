using UnityEngine;
using System.Collections;

public abstract class PropBase 
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
    /// 道具所在的格子
    /// </summary>
    public PathFindingGrid Gird { get; set; }

    /// <summary>
    /// 道具的功能
    /// </summary>
    public abstract void Function();
}
