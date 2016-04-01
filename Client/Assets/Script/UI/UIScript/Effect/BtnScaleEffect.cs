using DG.Tweening;

public class BtnScaleEffect : UIEffectBase
{
    public float ScaleToSize = 0.9f;

    #region override methods
    public override void Play()
    {
   
    }

    public override void Play(object obj)
    {
        base.EffectTime = 0.2f;
        bool isScaleTo = (bool)obj;
        base.Stop();
        if (isScaleTo)
        {
            this.ScaleTo();
        }
        else
        {
            this.ScaleBack();
        }
    }
    #endregion

    #region private methods

    private void ScaleTo()
    {
        this.transform.DOScale(this.ScaleToSize,base.EffectTime)
            .OnComplete(() => base.IsPlaying = false);
    }

    private void ScaleBack()
    {
        this.MyTransform.DOScale(1, base.EffectTime)
            .OnComplete(() => base.IsPlaying = false);
    }
    #endregion
}
