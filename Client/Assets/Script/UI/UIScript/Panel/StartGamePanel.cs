using UnityEngine;
using System.Collections;
using System;

public class StartGamePanel : PanelBase
{
    public ButtonComponent StartGameBtn;
    public ButtonComponent LevelEditBtn;
    public ButtonComponent AboutBtn;

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

    #region override methods
    public override void OpenComplete()
    {
        
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        //this.StartGameBtn.ClickEvent

        this.LevelEditBtn.ClickEvent += () =>
        {
            this.EnterPanel = UIPanelType.LevelListPanel;
            base.CloseThisPanel();
        };

        //this.LevelEditBtn.ClickEvent
    }
    #endregion

    #region private methods
    #endregion

    #region public methods
    #endregion

}
