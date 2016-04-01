using UnityEngine;
using System.Collections;

public  class PropData : RoleDataBase
{
    /// <summary>
    /// 道具类型
    /// </summary>
    private PropType m_Type;
    public PropType Type
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

    private GridData m_JoinGridData;
    public GridData JoinGridData
    {
        get
        {
            return m_JoinGridData;
        }

        set
        {
            base.GridData.SetJoinGridData(value);
            this.m_JoinGridData = value;
        }
    }

    
}
