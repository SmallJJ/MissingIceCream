using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class LevelEditerPanel : PanelBase
{
    public LocalLabelComponent IceCreamCountLabel;
    public LocalLabelComponent PropCountLabel;
    public LocalLabelComponent MaterialCountLabel;
    public LocalLabelComponent FreezerCountLabel;
    public ButtonComponent SettingBtn;
    public ButtonComponent SaveBtn;
    public CommonPackageComponent RoleTypePackage;
    public CommonPackageComponent RoleItemsPackageComp;
    public float ShowDescritionWaitTime = 0.3f;

    private UIRoleType m_CurUIRoleType;
    private RoleTypeComponent m_BackupRoleTypeComp;
    private LevelEditerParam m_LevelEditerParam;
    private PropItemComponent m_BackupPropItemComp;
    private int m_IceCreamCount;
    private int m_PropCount;
    private int m_MaterialCount;
    private int m_FreezerCount;

    #region override methods

    public override void OpenComplete()
    {
        this.m_LevelEditerParam = (LevelEditerParam)this.ThisPanelParam;
        this.UpdateRoleTypePackage();
        switch (this.m_LevelEditerParam.OperationType)
        {
            case LevelOperationType.Eidt:
                LevelEditerMgr.Instance.LoadLevel(this.m_LevelEditerParam.LevelFileName);
                break;
            case LevelOperationType.Create:
                LevelEditerMgr.Instance.CreateLevel(this.m_LevelEditerParam.Row, this.m_LevelEditerParam.Col);
                break;
        }
        
    }

    public override void CloseComplete()
    {
        base.CloseComplete();
        LevelEditerMgr.Instance.ClearMap();
    }

    public override void Clear()
    {
        base.Clear();
        this.RoleTypePackage.Clear();
        this.RoleItemsPackageComp.Clear();
        this.m_IceCreamCount = 0;
        this.m_PropCount = 0;
        this.m_MaterialCount = 0;
        this.m_FreezerCount = 0;
        this.IceCreamCountLabel.text = string.Empty;
        this.PropCountLabel.text = string.Empty;
        this.MaterialCountLabel.text = string.Empty;
        this.FreezerCountLabel.text = string.Empty;
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        this.RoleTypePackage.ItemClickEvent += (comp) =>
        {
            RoleTypeComponent roleTypeComp = (RoleTypeComponent)comp;
            this.m_CurUIRoleType = roleTypeComp.GetRoleType();
            this.UpdateItemsPackageComp();
            roleTypeComp.Selected();
            if (this.m_BackupRoleTypeComp != null) this.m_BackupRoleTypeComp.UnSeleted();
            this.m_BackupRoleTypeComp = roleTypeComp;
            LevelEditerMgr.Instance.PanelPackageSelect(null);
            this.m_BackupPropItemComp = null;
        };

        this.RoleItemsPackageComp.ItemClickEvent += (comp) =>
          {
                PropItemComponent propItemComp = comp as PropItemComponent;
              if (this.m_BackupPropItemComp == propItemComp)
              {
                  this.ClearPanelSelected();
              }
              else
              {
                  this.m_BackupPropItemComp = propItemComp;
                  this.RoleItemsPackageComp.ShowSelectionEffect(propItemComp);
                  LevelEditerMgr.Instance.PanelPackageSelect(propItemComp.GetRoleData());
              }
                  
          };
        this.RoleItemsPackageComp.ItemPressEvent += (comp,isPress) =>
          {
              if (isPress)
              {
                  this.StartCoroutine(this.WaitPressIterator(comp));
              }
              else
              {
                  this.StopAllCoroutines();
                  UIController.Instance.HideDescription();
              }
          };

        this.SettingBtn.ClickEvent += () =>
        {
            UIController.Instance.OpenPanel(UIPanelType.EditerHelpPanel, null, PanelEffectType.Open);
        };

        this.SaveBtn.ClickEvent += () =>
        {
            if (this.m_FreezerCount > UIConst.FreezerCount || this.m_FreezerCount <= 0)
            {
                UIController.Instance.ShowDialog(new DialogParem() { Content = LocalizationUtils.GetText("LevelEditerPanel.Tip.FreezerCountRange", UIConst.FreezerCount), Type = DialogType.Ok });
                return;
            }
            else if(this.m_IceCreamCount > UIConst.IceCreamCount || this.m_IceCreamCount <= 0)
            {
                UIController.Instance.ShowDialog(new DialogParem() { Content = LocalizationUtils.GetText("LevelEditerPanel.Tip.IceCaremCountRange",UIConst.IceCreamCount), Type = DialogType.Ok });
                return;
            }
            UIController.Instance.OpenPanel(UIPanelType.SaveLevelPanel,null, PanelEffectType.Open);
        };

        LevelEditerMgr.Instance.IceCreamCountChangeEvent +=this.UpdateIceCreamCount;
        LevelEditerMgr.Instance.PropCountChangeEvent += this.UpdatePropCount;
        LevelEditerMgr.Instance.MaterialCountChangeEvent += this.UpdateMaterialCount;
        LevelEditerMgr.Instance.FreezerCountChangeEvent += this.UpdateFreezerCount;
        LevelEditerMgr.Instance.ClearPanelSelected += this.ClearPanelSelected;
    }
    #endregion

    #region private methods

    private void ClearPanelSelected()
    {
        this.RoleItemsPackageComp.ClearSelection();
        this.m_BackupPropItemComp = null;
        LevelEditerMgr.Instance.PanelPackageSelect(null);
    }

    private void UpdateRoleTypePackage()
    {
        Array roleTypeArray = Enum.GetValues(typeof(UIRoleType));
        this.RoleTypePackage.UpdateRoleType(roleTypeArray);
        this.RoleTypePackage.SelectionFirstItem();
    }

    private void UpdateItemsPackageComp()
    {
        RoleDataBase[] roleDatas = TestConfigData.GetRoleDataByType(this.m_CurUIRoleType);
        this.RoleItemsPackageComp.UpdateRoleItems(roleDatas);
    }

    private void UpdateIceCreamCount(int count)
    {
        this.m_IceCreamCount = count;
        this.IceCreamCountLabel.UpdateLabel("LevelEditerPanel.Label.IceCreamCount", count);
    }

    private void UpdatePropCount(int count)
    {
        this.m_PropCount = count;
        this.PropCountLabel.UpdateLabel("LevelEditerPanel.Label.PropCount", count);
    }

    private void UpdateMaterialCount(int count)
    {
        this.m_MaterialCount = count;
        this.MaterialCountLabel.UpdateLabel("LevelEditerPanel.Label.MaterialCount", count);
    }


    private void UpdateFreezerCount(int count)
    {
        this.m_FreezerCount = count;
        this.FreezerCountLabel.UpdateLabel("LevelEditerPanel.Label.FreezerCountLabel", count);
    }


    private IEnumerator WaitPressIterator(ComponentBase comp)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < this.ShowDescritionWaitTime)
        {
            yield return null;
        }
        PropItemComponent propItemComp = comp as PropItemComponent;
        UIController.Instance.ShowDescription(propItemComp.GetRoleData().Description);
    }
    #endregion
}
