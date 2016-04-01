using UnityEngine;
using DG.Tweening;
public class TipCompEffect : UIEffectBase
{
    public float Yoffset = 300f;
    public float WaitTime = 0.5f;
    public override void Play()
    {
        this.Show();
    }

    public override void Play(object obj)
    {
        base.EffectTime = (float)obj;
        this.Show();
    }

    #region private methods

    private void Show()
    {
        this.MyGameObject.SetActive(true);
        this.MyTransform.DOLocalMoveY(this.Yoffset, this.EffectTime)
            .SetDelay(this.WaitTime)
            .OnComplete(() => {
                this.MyGameObject.SetActive(false);
                this.MyTransform.localPosition = Vector3.zero;
                UIController.Instance.TipCompPlayComplate(this.MyGameObject.GetComponent<TipComponent>());
        });
    }
    #endregion
}
