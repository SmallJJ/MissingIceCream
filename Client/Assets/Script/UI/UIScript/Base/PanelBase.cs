using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public abstract class PanelBase : UIBase, IPanel
{
    public UIPanelType ThisPanel { get; protected set; }

    #region abstract methods
    public abstract void Open(PanelParamBase prarm);
    #endregion

    #region virtual methods
    public virtual void Close(PanelEffectType type)
    {

    }
    #endregion

    #region override methods
    protected override void Awake()
    {
        base.Awake();
    }
    public override void Clear()
    {
        base.Clear();
        this.StopAllCoroutines();
    }
    protected override void AddEvent()
    {
        base.AddEvent();
    }
    public override void Init()
    {
        base.Init();
    }

    public override void Dispose()
    {
        base.Dispose();
    }
    #endregion

    #region protected methods
    protected void CloseThisPanel()
    {
        
    }
    #endregion

    #region public Methods
    public void Open(PanelParamBase prarm,PanelEffectType type)
    {
        this.Open(prarm);
    }

    #endregion

}
