using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : PanelBase
{
    public Text TitleLabel;
    public Text ContentLabel;
    public ButtonComponent EnsureBtn;
    public ButtonComponent OkBtn;
    public ButtonComponent CancelBtn;

    private DialogParem m_DialogParem;

    #region override methods
    public override void OpenComplete()
    {
        this.m_DialogParem = (DialogParem)base.ThisPanelParam;
        this.UpdateInfo();
    }

    public override void Clear()
    {
        base.Clear();
        this.TitleLabel.text = string.Empty;
        this.ContentLabel.text = string.Empty;
    }
    #endregion

    #region private methods
    private void UpdateInfo()
    {
        this.UpdateTitle();
        this.UpdateContent();
        this.UpdateButsStatus();
    }

    private void UpdateTitle()
    {
        string title = this.m_DialogParem.Title==null ? LocalizationUtils.GetText("DialogPanel.Text.Tip"):this.m_DialogParem.Title ;
        this.TitleLabel.text = title;
    }

    private void UpdateContent()
    {
        this.ContentLabel.text = this.m_DialogParem.Content;
    }

    private void UpdateButsStatus()
    {
        switch (this.m_DialogParem.Type)
        {
            case DialogType.Ensure_Cansel:
                this.OkBtn.Hide();
                this.EnsureBtn.ClickEvent += () => this.m_DialogParem.ExecuteResultEvent(true);
                this.CancelBtn.ClickEvent += () =>
                {
                    this.m_DialogParem.ExecuteResultEvent(false);
                    this.CloseThisPanel();
                };
                break;
            case DialogType.Ok:
                this.EnsureBtn.Hide();
                this.CancelBtn.Hide();
                this.OkBtn.ClickEvent += () => this.CloseThisPanel();
                break;
        }
    }
    #endregion

}
