using UnityEngine;
using UnityEngine.UI;
public class PropItemComponent : ComponentBase
{
    public Image iconImage;
    public Text NameText;

    private RoleDataBase m_RoleDataBase;

    #region private methods

    private void UpdateIcon(string iconStr)
    {
        
        Sprite sprite= GamePoolMgr.Instance.GetSprite(PathConst.Icon + iconStr);
        if(sprite==null) print(iconStr+"   "+ (PathConst.Icon + iconStr));
        this.iconImage.sprite = sprite;
    }

    private void UpdateName(string nameText)
    {
        this.NameText.text = nameText;
    }
    #endregion

    #region public methods

    public void UpdateData(RoleDataBase data)
    {
        this.m_RoleDataBase = data;
        this.UpdateIcon(data.Asset);
        this.UpdateName(data.Name);
    }

    public RoleDataBase GetRoleData()
    {
        return this.m_RoleDataBase;
    }
    #endregion
}
