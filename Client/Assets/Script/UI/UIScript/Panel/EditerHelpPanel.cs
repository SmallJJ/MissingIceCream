
using UnityEngine.UI;
public class EditerHelpPanel : PanelBase
{
    public Text HelpLabel;
    public ButtonComponent MainMenuBtn;
    public ButtonComponent LevelListBtn;

    #region override methods
    public override void OpenComplete()
    {
        this.UpdateHelpLabel();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        this.MainMenuBtn.ClickEvent += () =>
        {
            UIController.Instance.ClosePanel(UIPanelType.LevelEditerPanel, PanelEffectType.Close);
            this.EnterPanel = UIPanelType.StartGamePanel;
            base.CloseThisPanel();
        };

        this.LevelListBtn.ClickEvent += () =>
        {
            this.EnterPanel = UIPanelType.LevelListPanel;
            UIController.Instance.ClosePanel(UIPanelType.LevelEditerPanel, PanelEffectType.Close);
            base.CloseThisPanel();
        };
    }

    public override void Clear()
    {
        base.Clear();
        this.HelpLabel.text = string.Empty;
    }
    #endregion

    #region private methods
    private void UpdateHelpLabel()
    {
        this.HelpLabel.text = StringUtils.GetFileStr(PathConst.HelpTxtPath);
    }
    #endregion
}

