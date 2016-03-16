public class PropLogicData : RoleLogicDataBase
{
    private PropData m_Data;
    public PropLogicData(PropData data):base(data)
    {
        this.m_Data = data;
    }

    /// <summary>
    /// 道具类型
    /// </summary>
    public PropType Type;
}
