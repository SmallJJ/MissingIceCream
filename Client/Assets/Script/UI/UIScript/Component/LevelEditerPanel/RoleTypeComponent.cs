using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoleTypeComponent : ComponentBase
{
    public Image SelectedImage;
    public Image NormalImage;
    public LocalLabelComponent SelectedLocalLabel;
    public LocalLabelComponent NormalLocalLabel;
    private UIRoleType m_UIRoleType;

    #region private methods

    private void UpdateLabel()
    {
        this.SelectedLocalLabel.UpdateLabel("UIRoleType." + this.m_UIRoleType);
        this.NormalLocalLabel.UpdateLabel("UIRoleType." + this.m_UIRoleType);
    }
    #endregion

    #region public methods

    public void UpdateInfo(UIRoleType uiRoleType)
    {
        this.m_UIRoleType = uiRoleType;
        this.UpdateLabel();
        this.UnSeleted();
    }

    public void Selected()
    {
        this.NormalImage.gameObject.SetActive(false);
        this.SelectedImage.gameObject.SetActive(true);
    }

    public void UnSeleted()
    {
        this.NormalImage.gameObject.SetActive(true);
        this.SelectedImage.gameObject.SetActive(false);
    }

    public UIRoleType GetRoleType()
    {
        return this.m_UIRoleType;
    }
    #endregion

}
