using UnityEngine;
using UnityEngine.UI;
using System;

public class ButtonComponent : ComponentBase {

    public enum ButtonLabelType : byte { None, TextLabel, ImageLabel };
    public ButtonLabelType LabelType;
    public Text TextLabel;
    public Image ImageLabel;
    public Image NormalImage;
    public Image PressedImage;
    public Image DisabledImage;
    public Image SelectedImage;
    public Image ClickedImage;
    public bool ScaleBtn=true;
    public ButtonStatusType ButtonStatus;

    public Action ClickEvent;
    public Action PressEvent;

    private BtnScaleEffect m_BtnScaleEffect;

    #region override methods

    protected override void Awake()
    {
        base.Awake();
        this.HideAllSprite();
        this.Normal();
    }
    protected override void AddEvent()
    {
        base.AddEvent();
        UIEventListener.Get(this.MyGameObject).onClick += this.InClick;
        UIEventListener.Get(this.MyGameObject).onPress += this.InPress;
    }
    public override void Show()
    {
        base.Show();
        this.SetStatus(this.ButtonStatus);
    }

    #endregion

    #region private methods

    private void InClick(GameObject go)
    {
        if (this.ClickEvent == null)
        { 
            UIController.Instance.ShowTip("CommonButton.Tip");
            return;
        }
        this.ClickEvent();
    }

    private void InPress(GameObject go, bool isPress)
    {
        if (this.ScaleBtn)
        {
            if (this.m_BtnScaleEffect == null)
            {
                this.m_BtnScaleEffect = this.MyGameObject.AddComponent<BtnScaleEffect>();
            }
            this.m_BtnScaleEffect.Play(isPress);
        }
        this.Press(isPress);
        if (this.PressEvent == null)
        {
            return;
        }
        this.PressEvent();
    }

    private void Normal()
    {
        if (this.NormalImage != null)
            this.NormalImage.gameObject.SetActive(true);
        TransUtils.EnableCollider(this.MyTransform, true);
    }

    private void Desabled()
    {
        if (this.DisabledImage != null)
            this.DisabledImage.gameObject.SetActive(true);
        TransUtils.EnableCollider(this.MyTransform,false);
    }

    private void Press(bool isPressed)
    {
        if (this.PressedImage == null) return;
        if (isPressed)
        {
            this.HideAllSprite();
            this.PressedImage.gameObject.SetActive(true);
        }
        else
        {
            this.SetStatus(ButtonStatusType.Normal);
        }
    }

    private void Selected()
    {
        if (this.SelectedImage != null)
            this.SelectedImage.gameObject.SetActive(true);
        TransUtils.EnableCollider(this.MyTransform, false);
    }

    private void Clicked()
    {
        if (this.ClickedImage != null)
            this.ClickedImage.gameObject.SetActive(true);
    }

    private void HideAllSprite()
    {
        if (this.NormalImage != null)
            this.NormalImage.gameObject.SetActive(false);
        if (this.PressedImage != null)
            this.PressedImage.gameObject.SetActive(false);
        if (this.DisabledImage != null)
            this.DisabledImage.gameObject.SetActive(false);
        if (this.SelectedImage != null)
            this.SelectedImage.gameObject.SetActive(false);
        if (this.ClickedImage != null)
            this.ClickedImage.gameObject.SetActive(false);
    }

    private void UpdateLabel(Text label, string str, bool isLocalKey = true)
    {
        if (label != null)
        {
            if (isLocalKey)
            {
                label.GetComponent<LocalLabelComponent>().UpdateLabel(str);
            }
            else
            {
                label.text = str;
            }
        }
    }

    #endregion

    #region public methdods

    public void UpdateAllLabel(string str, bool isLocalKey = true)
    {
        Text[] labels = this.GetComponentsInChildren<Text>(true);
        if (labels != null && labels.Length > 0)
        {
            foreach (Text label in labels)
            {
                this.UpdateLabel(label, str, isLocalKey);
            }
        }
    }

    /// <summary>
    /// 更新按钮文字
    /// </summary>
    /// <param name="str"></param>
    /// <param name="isLocalKey"></param>
    public void UpdateLabel(string str, bool isLocalKey = true)
    {
        switch (this.LabelType)
        {
            case ButtonLabelType.ImageLabel:
                this.ImageLabel.sprite = GamePoolMgr.Instance.GetSprite(str);
                break;
            case ButtonLabelType.TextLabel:
                this.UpdateLabel(this.TextLabel, str, isLocalKey);
                break;
        }
    }

    /// <summary>
    /// 更新按钮状态
    /// </summary>
    /// <param name="state"></param>
    /// <param name="param"></param>
    public void SetStatus(ButtonStatusType state, bool param = true)
    {
        this.ButtonStatus = state;
        this.HideAllSprite();
        switch (state)
        {
            case ButtonStatusType.Normal: this.Normal(); break;
            case ButtonStatusType.Pressed: this.Press(param); break;
            case ButtonStatusType.Disabled: this.Desabled(); break;
            case ButtonStatusType.Selected: this.Selected(); break;
            case ButtonStatusType.Clicked: this.Clicked(); break;
        }
    }

    /// <summary>
    /// 更新按钮文字颜色
    /// </summary>
    /// <param name="color"></param>
    public void SetLabelColor(Color color)
    {
        this.TextLabel.color = color;
    }

    #endregion
}
