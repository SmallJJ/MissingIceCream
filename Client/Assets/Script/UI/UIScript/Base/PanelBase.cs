using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public abstract class PanelBase : UIBase, IPanel
{
    
    public MaskType MyMaskType= MaskType.CloseBtnAndMaskBg_CloseEvent;
    public UIPanelType EnterPanel { get; protected set; }
    public PanelParamBase EnterPanelParam { get; set; }
    public UIPanelType ThisPanel { get; private set; }
    public PanelParamBase ThisPanelParam { get;private set; }
    public PanelParamBase NewPanelParam { get;protected set; }

    private UIEffectBase m_OpenPanelEffect;
    private UIEffectBase m_ClosePanelEffect;
    #region abstract methods
    public abstract void OpenComplete();
    #endregion

    #region virtual methods
    public virtual void Close(PanelEffectType type)
    {
        switch (type)
        {
            case PanelEffectType.None:
                break;
            case PanelEffectType.Close:
                if (this.m_ClosePanelEffect == null)
                {
                    this.m_ClosePanelEffect = this.MyGameObject.GetComponent<ClosePanelEffect>();
                    if (this.m_ClosePanelEffect == null)
                    {
                        this.m_ClosePanelEffect = this.MyGameObject.AddComponent<ClosePanelEffect>();
                    }
                }
                this.m_ClosePanelEffect.Play();
                break;
        }
    }

    public virtual void CloseComplete()
    {
        this.Clear();
        TransUtils.EnableCollider(this.MyTransform, false);
        this.MyGameObject.SetActive(false);
        this.MyTransform.localPosition = Vector3.one;
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
        if (this.ThisPanel == UIPanelType.None) { Debug.Log("this panel is none can't close!"); return; }
        UIController.Instance.ClosePanel(this.ThisPanel);
    }
    #endregion

    #region public Methods
    public void Open(UIPanelType type,PanelParamBase prarm,PanelEffectType effectType)
    {
        this.ThisPanel = type;
        this.ThisPanelParam = prarm;
        this.EnterPanel = UIPanelType.None;
        this.MyGameObject.SetActive(false);
        switch (effectType)
        {
            case PanelEffectType.None:
                this.MyGameObject.SetActive(true);
                this.OpenComplete();
                return;
            case PanelEffectType.Open:
                if (this.m_OpenPanelEffect==null)
                {
                    this.m_OpenPanelEffect = this.MyGameObject.GetComponent<OpenPanelEffect>();
                    if (this.m_OpenPanelEffect == null)
                    {
                        this.m_OpenPanelEffect = this.MyGameObject.AddComponent<OpenPanelEffect>();
                    }
                }
                this.m_OpenPanelEffect.Play();
                break;
        }
    }

    #endregion

}
