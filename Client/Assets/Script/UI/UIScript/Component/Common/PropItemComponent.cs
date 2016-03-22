using UnityEngine;
using UnityEngine.UI;
public class PropItemComponent : ComponentBase
{
    public Image iconImage;
    public Text NameText;

    #region private methods

    private void UpdateIcon(string IconStr)
    {
        
        Sprite sprite= GamePoolMgr.Instance.GetSprite(PathConst.Icon + IconStr);
        if(sprite==null) print(IconStr+"   "+ (PathConst.Icon + IconStr));
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
        this.UpdateIcon(data.Asset);
        this.UpdateName(data.Name);
    }
    #endregion
}
