
using DG.Tweening;
using UnityEngine;
public class OpenPanelEffect : UIEffectBase
{
    public override void Play()
    {
        this.Show();
    }

    public override void Play(object obj)
    {
        this.EffectTime = (float)obj;
        this.Show();
    }

    #region private methods

    private void Show()
    {
        this.MyGameObject.SetActive(true);
        this.MyTransform.localScale = Vector3.one * 3;
        this.MyGameObject.SetActive(true);
        this.MyTransform.DOScale(1, this.EffectTime).OnComplete(() => { this.MyGameObject.GetComponent<PanelBase>().OpenComplete(); });
    }
    #endregion
}
