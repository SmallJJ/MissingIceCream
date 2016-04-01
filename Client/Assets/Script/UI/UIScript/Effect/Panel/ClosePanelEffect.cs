using DG.Tweening;
using UnityEngine;
public class ClosePanelEffect : UIEffectBase
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
        this.MyTransform.localScale = Vector3.one;
        this.MyTransform.DOScale(0, this.EffectTime).OnComplete(() => { this.MyGameObject.GetComponent<PanelBase>().CloseComplete(); });
    }
    #endregion
}