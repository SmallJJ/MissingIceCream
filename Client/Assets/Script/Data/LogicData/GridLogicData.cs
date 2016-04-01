using UnityEngine;

public class GridLogicData
{
    public RoadData CurRoadData { get; private set; }    //基础数据（墙和路）
    public RoleDataBase AttachRoleData { get; private set; }  //附加角色

    private GridData m_GridData;


    #region public methods
    public GridLogicData(GridData data)
    {
        this.m_GridData = data;
        this.InitData();
    }
    #endregion

    #region private methods

    private void InitData()
    {
        this.CurRoadData = TestConfigData.GetRoadDataByType(this.m_GridData.RoadType);
        if (this.CurRoadData == null)
        {
            Debug.LogError("数据异常!");
            return;
        }
        switch (this.m_GridData.RoadType)
        {
            case RoadType.Wall:
                this.AttachRoleData = null;
                break;
            case RoadType.Road:
                if (this.m_GridData.RoleType.HasValue)
                {
                    RoleType roleType = this.m_GridData.RoleType.Value;
                    switch (roleType)
                    {
                        case RoleType.IceCream:
                            this.AttachRoleData = TestConfigData.GetIceCreamDataByType(this.m_GridData.IceCreamType.Value);
                            break;
                        case RoleType.Prop:
                            this.AttachRoleData = TestConfigData.GetPropDataByType(this.m_GridData.PropType.Value);
                            break;
                        case RoleType.Marterial:
                            this.AttachRoleData = TestConfigData.GetMaterialDataByType(this.m_GridData.MaterialType.Value);
                            break;
                    }
                }
                else
                {
                    this.AttachRoleData = null;
                }
                break;
        }
    }
    #endregion
    #region public methods
    public GridData GetGridData()
    {
        return this.m_GridData;
    }

    public void SetAttachRoleData(GridData gridData)
    {
        //把位置给要附加的角色
        gridData.SetPosition(this.m_GridData.Row, this.m_GridData.Col);
        if (this.m_GridData.JoinGridData==null)
        {
            this.m_GridData.SetJoinGridData(null);  //清空关联对象
        }
        //覆盖数据结构
        this.m_GridData = gridData;
        this.InitData();
    }
    #endregion
}
