
public class MaterialLogicData:RoleLogicDataBase
{
    private MaterialData m_Data;
    public MaterialLogicData(MaterialData data):base(data)
    {
        this.m_Data = data;
    }

    /// <summary>
    /// 物品类型
    /// </summary>
    public MaterialType Type { get { return this.m_Data.Type;} }
}
