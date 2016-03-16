using System;
using System.Collections.Generic;

public class IceCreamLogicData : RoleLogicDataBase
{
    private IceCreamData m_Data;
    public IceCreamLogicData(IceCreamData data):base(data)
    {
        this.m_Data = data;
        this.HP = data.HP;
        this.Speed = data.Speed;
        this.StatusType = data.StatusType;
    }

    /// <summary>
    /// 冰淇淋类型
    /// </summary>
    public IceCreamType Type { get { return this.m_Data.Type; } }

    /// <summary>
    /// 生命值
    /// </summary>
    public float HP { get; private set; }

    /// <summary>
    /// 速度
    /// </summary>
    public float Speed { get; private set; }

    /// <summary>
    /// 冰淇淋状态
    /// </summary>
    public IceCreamStatusType StatusType { get; private set; }
}
