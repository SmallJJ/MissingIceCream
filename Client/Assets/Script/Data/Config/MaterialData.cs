using UnityEngine;
using System.Collections;

/// <summary>
/// 物品父级
/// </summary>
public  class MaterialData : RoleDataBase
{
    /// <summary>
    /// 物品类型
    /// </summary>
    private MaterialType m_Type;

    public MaterialType Type
    {
        get
        {
            return this.m_Type;
        }
        set
        {
            base.BaseType = RoleType.Prop;
            base.GridData = new GridData(value);
            this.m_Type = value;
        }
    }
}
