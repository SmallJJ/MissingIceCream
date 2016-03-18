using UnityEngine.UI;
public class PropItemComponent : ComponentBase
{
    public Image IconImage;
    public Text NameText;

    #region private methods

    private void UpdateIcon(string IconStr)
    {
        this.IconImage.name = IconStr;
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
