public class RoadData:RoleDataBase
{
    public bool IsBarrier { get; set; }

    private RoadType m_Type;
    public RoadType Type
    {
        get
        {
            return this.m_Type;
        }

        set
        {
            base.GridData = new GridData(value);
            this.m_Type = value;
        }
    }

    
}
