using UnityEngine;
using System.Collections.Generic;
using System;

public class LevelListPanel : PanelBase
{
    public CommonPackageComponent LevelListPackage;
    public ButtonComponent CreateLevelBtn;
    private Dictionary<string, string> m_LevelNames = new Dictionary<string, string>();

    #region override methods
    public override void OpenComplete()
    {
        this.m_LevelNames = LevelDataMgr.Instance.GetLevelKeyData().GetData();
        if (this.m_LevelNames.Count > 0)
        {
            this.UpdateLevelListPackage();
        }
    }

    public override void Clear()
    {
        base.Clear();
        this.LevelListPackage.Clear();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        this.CreateLevelBtn.ClickEvent += () =>
          {
              UIController.Instance.OpenPanel(UIPanelType.LevelSetingPanel);
          };
    }
    #endregion

    #region private methods
    private void UpdateLevelListPackage()
    {
        this.LevelListPackage.UpdateLevelListPackge(this.m_LevelNames, this.LevelOperator);
    }

    private void LevelOperator(string levelFileName,LevelOperationType type)
    {
        switch (type)
        {
            case LevelOperationType.Eidt:
                this.EnterPanel = UIPanelType.LevelEditerPanel;
                this.EnterPanelParam = new LevelEditerParam() { OperationType=type, LevelFileName=levelFileName };
                this.CloseThisPanel();
                break;
            case LevelOperationType.Delete:
                this.m_LevelNames.Remove(levelFileName);
                LevelDataMgr.Instance.DeleteLevelData(levelFileName);
                ComponentBase comp= this.LevelListPackage.GetItemByUID(levelFileName);
                this.LevelListPackage.DeleteItem(comp);
                break;
        }
    }
    #endregion
}
