using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class LevelEditerPanel : PanelBase
{
    public CommonPackageComponent RoleTypePackage;
    public CommonPackageComponent RoleItemsPackageComp;
    private RoleType m_CurRoleType;
    private RoleTypeComponent m_BackupRoleTypeComp;
    #region override methods
    public override void Open(PanelParamBase prarm)
    {
        this.UpdateRoleTypePackage();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        this.RoleTypePackage.ItemClickEvent += (comp) =>
        {
            RoleTypeComponent roleTypeComp = (RoleTypeComponent)comp;
            this.m_CurRoleType = roleTypeComp.GetRoleType();
            this.UpdateItemsPackageComp();
            roleTypeComp.Selected();
            if (this.m_BackupRoleTypeComp != null) this.m_BackupRoleTypeComp.UnSeleted();
            this.m_BackupRoleTypeComp = roleTypeComp;
        };
    }
    #endregion

    #region private methods

    private void UpdateRoleTypePackage()
    {
        Array roleTypeArray = Enum.GetValues(typeof(RoleType));
        this.RoleTypePackage.UpdateRoleType(roleTypeArray);
        this.RoleTypePackage.SelectionFirstItem();
    }

    private void UpdateItemsPackageComp()
    {
        RoleDataBase[] roleDatas = TestConfigData.GetRoleDataByType(this.m_CurRoleType);
        this.RoleItemsPackageComp.UpdateRoleItems(roleDatas);
    }
    #endregion
}
