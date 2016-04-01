using UnityEngine.UI;

public class DescriptionComponent : ComponentBase
{
    public Text ContentLabel;

    #region private methods
    public override void Clear()
    {
        base.Clear();
        this.ContentLabel.text = string.Empty;
    }
    #endregion

    #region public methods

    public void UpdateInfo(string str)
    {
        this.ContentLabel.text = str;
    }
    #endregion
}
