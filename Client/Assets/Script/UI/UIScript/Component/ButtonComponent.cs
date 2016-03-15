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

    public Action OnClickEvent;
    public Action OnPressEvent;

    #region override methods
    protected override void AddEvent()
    {
        base.AddEvent();
        UIEventListener.Get(this.MyGameObject).onClick += this.InClick;
        UIEventListener.Get(this.MyGameObject).onPress += this.InPress;
    }

    #endregion

    #region private methods

    private void InClick(GameObject go)
    {
        
    }

    private void InPress(GameObject go, bool isPress)
    {
 
    }
    #endregion


}
