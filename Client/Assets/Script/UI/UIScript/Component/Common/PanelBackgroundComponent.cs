using System;
using UnityEngine;

public class PanelBackgroundComponent : ComponentBase
{
    public ButtonComponent CloseBtn;
    public GameObject MaskBackground;
    public event Action<UIPanelType> CloseEvent;

    private UIPanelType m_PanelType;
    private MaskType m_MaskType= MaskType.CloseBtnAndMaskBg_CloseEvent;

    #region override methods

    protected override void AddEvent()
    {
        base.AddEvent();
        UIEventListener.Get(this.MaskBackground).onClick += (go) => this.MaskClosePanel();
        this.CloseBtn.ClickEvent += this.BtnClosePanel;
    }

    public override void Clear()
    {
        base.Clear();
        this.CloseEvent = null;
    }

    #endregion

    #region private 

    private void BtnClosePanel()
    {
        if (this.m_MaskType == MaskType.CloseBtnAndMaskBg_CloseEvent)
            this.ClosePanel();
    }

    private void MaskClosePanel()
    {
        if (this.m_MaskType == MaskType.OnlyMaskBg_CloseEvent)
            this.ClosePanel();
    }

    private void ClosePanel()
    {
        if (this.CloseEvent != null)
        {
            this.CloseEvent(this.m_PanelType);
        }
    }

    private void UpdateState()
    {
        TransUtils.EnableCollider(this.MyTransform, true);
        switch (this.m_MaskType)
        {
            case MaskType.OnlyMaskBg:
            case MaskType.OnlyMaskBg_CloseEvent:
                this.CloseBtn.Hide();
                break;
            case MaskType.CloseBtnAndMaskBg_CloseEvent:
                this.CloseBtn.Show();
                break;
            case MaskType.None:
                this.Hide();
                break;
        }
    }

    #endregion

    #region public methods


    public void Show(Transform parent,UIPanelType type,MaskType maskType)
    {
        this.m_MaskType = maskType;
        this.m_PanelType = type;
        this.UpdateState();
        this.MyTransform.SetParent(parent);
        this.MyTransform.localPosition = Vector3.zero;
        this.MyTransform.localRotation = Quaternion.identity;
        this.MyTransform.localScale = Vector3.one;
        this.MyTransform.SetAsFirstSibling();
        this.Show();
    }

    #endregion
}
