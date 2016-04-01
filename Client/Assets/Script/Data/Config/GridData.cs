using System;
/// <summary>
/// 序列化存储用，纯数据类型
/// </summary>
[Serializable]
public class GridData
{
    public byte Row  { get; private set; }
    public byte  Col { get; private set; }
    public RoadType RoadType { get; private set; } //基础类
    public RoleType? RoleType { get; private set; } //携带类型
    public IceCreamType? IceCreamType { get; private set; } //携带冰淇淋
    public PropType? PropType { get; private set; } //携带道具
    public MaterialType? MaterialType { get; private set; }//携带物品
    public GridData JoinGridData { get; private set; } //关联物品对象

    #region public methods
    //初始化冰淇淋数据
    public GridData(IceCreamType iceCreamType)
    {
        this.RoadType = RoadType.Road;
        this.RoleType = global::RoleType.IceCream;
        this.IceCreamType = iceCreamType;
    }

    //初始化道具数据
    public GridData(PropType propType)
    {
        this.RoadType = RoadType.Road;
        this.RoleType = global::RoleType.Prop;
        this.PropType = propType;
    }

    //初始化物品数据
    public GridData(MaterialType materialType)
    {
        this.RoadType = RoadType.Road;
        this.RoleType = global::RoleType.Marterial;
        this.MaterialType = materialType;
    }

    //初始化基础数据
    public GridData( RoadType roadType)
    {
        this.RoadType = roadType;
    }

    public void SetPosition(byte row, byte col)
    {
        this.Row = row;
        this.Col = col;
    }


    /// <summary>
    /// 设置附加道具
    /// </summary>
    /// <param name="joinGridData"></param>
    public void SetJoinGridData(GridData joinGridData)
    {
        if (joinGridData != null) this.JoinGridData = joinGridData;
    }
    #endregion
}
