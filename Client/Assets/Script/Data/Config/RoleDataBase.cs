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
    /// 角色基础类型
    /// </summary>
    public RoleType BaseType { get; set; }

    /// <summary>
    /// 通过这个数据结构定位到准确的数据类型
    /// </summary>
    public GridData GridData { get; set;}
}
