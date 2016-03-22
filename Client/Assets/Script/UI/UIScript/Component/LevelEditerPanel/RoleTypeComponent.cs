using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoleTypeComponent : ComponentBase
{
    public Image SelectedImage;
    public Image NormalImage;
    public LocalLabelComponent SelectedLocalLabel;
    public LocalLabelComponent NormalLocalLabel;
    private RoleType m_RoleType;

    #region private methods

    private void UpdateLabel()
    {
        this.SelectedLocalLabel.UpdateLabel("RoleType." + this.m_RoleType);
        this.NormalLocalLabel.UpdateLabel("RoleType." + this.m_RoleType);
    }
    #endregion

    #region public methods

    public void UpdateInfo(RoleType roleType)
    {
        this.m_RoleType = roleType;
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

    public RoleType GetRoleType()
    {
        return this.m_RoleType;
    }
    #endregion

}
