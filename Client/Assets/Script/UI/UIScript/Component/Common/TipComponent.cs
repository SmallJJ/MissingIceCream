using UnityEngine.UI;

public class TipComponent : ComponentBase
{
    public Text ContentLabel;

    #region override methods
    public override void Clear()
    {
        base.Clear();
        this.ContentLabel.text = "";
    }
    #endregion

    #region public methods
    public void UpdateInfo(string str,params object[] param)
    {
        this.ContentLabel.text = LocalizationUtils.GetText(str, param);
    }
    #endregion
}
