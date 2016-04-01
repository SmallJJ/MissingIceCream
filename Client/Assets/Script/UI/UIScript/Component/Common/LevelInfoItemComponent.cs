using System;
using UnityEngine.UI;

public class LevelInfoItemComponent : ComponentBase,IUID
{
    public Text LevelNameLabel;
    public ButtonComponent DelBtn;
    public ButtonComponent EditBtn;

    private string m_LevelFileName;
    private string m_LevelName;
    private Action<string, LevelOperationType> m_OperationEvent;

    #region override methods
    protected override void AddEvent()
    {
        base.AddEvent();
        this.EditBtn.ClickEvent += () => this.m_OperationEvent(this.m_LevelFileName, LevelOperationType.Eidt);
        this.DelBtn.ClickEvent += () => this.m_OperationEvent(this.m_LevelFileName, LevelOperationType.Delete);
    }
    #endregion

    #region private methods
    private void UpdateLevelName()
    {
        this.LevelNameLabel.text = this.m_LevelName;
    }
    #endregion
    #region public methods
    public void UpdateInfo(string levelFileName,string levelName,Action<string,LevelOperationType> operationEvent)
    {
        this.m_LevelFileName = levelFileName;
        this.m_LevelName = levelName;
        this.m_OperationEvent = operationEvent;
        this.UpdateLevelName();

    }
    public string GetUID()
    {
        return this.m_LevelFileName;
    }
    #endregion
}
