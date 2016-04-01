using UnityEngine.UI;
using System.Collections;
using System;

public class SaveLevelPanel : PanelBase
{

    public InputField LevelNameInput;
    public ButtonComponent SaveBtn;

    #region override methods
    public override void OpenComplete()
    {
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        this.SaveBtn.ClickEvent += this.SaveLevel;
    }

    public override void Clear()
    {
        base.Clear();
        this.LevelNameInput.text = string.Empty;
    }
    #endregion

    #region private methods
    private void SaveLevel()
    {
        string levelName = this.LevelNameInput.text;
        string content = string.Empty;
        if (levelName.Equals(string.Empty))
        {
            content = LocalizationUtils.GetText("SaveLevelPanel.Tip.LevelNameIsEmpty");
        }
        else if(LevelDataMgr.Instance.IsHasLevelName(levelName))
        {
            content = LocalizationUtils.GetText("SaveLevelPanel.Tip.LevelNameIsExist");
        }
        if (content.Equals(string.Empty))
        {
            LevelEditerMgr.Instance.SaveLevel(levelName);
            UIController.Instance.ClosePanel(UIPanelType.LevelEditerPanel);
            this.EnterPanel = UIPanelType.LevelListPanel; 
            this.CloseThisPanel();
            
        }
        else
        {
            UIController.Instance.ShowDialog(new DialogParem() { Content = content, Type = DialogType.Ok });
        }
    }
    #endregion
}
