using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class UIEventListener : EventTrigger
{
    public delegate void VoidDelegate(GameObject go);
    public delegate void BoolDelegate(GameObject go, bool state);
    public delegate void VectorDelegate(GameObject go, Vector2 delta);
    public delegate void ObjectDelegate(GameObject go, GameObject obj);
    public delegate void MoveDelegate(GameObject go, AxisEventData eventData);

    public VoidDelegate onSubmit;
    public VoidDelegate onClick;
    public BoolDelegate onHover;
    public BoolDelegate onPress;
    public BoolDelegate onSelect;
    public VectorDelegate onScroll;
    public VoidDelegate onDragStart;
    public VectorDelegate onDrag;
    public VoidDelegate onDragEnd;
    public ObjectDelegate onDrop;
    public VoidDelegate onCancel;
    public VoidDelegate onUpdateSelect;
    public MoveDelegate onMove;
    public VoidDelegate onInitializePotentialDrag;

    static public UIEventListener Get(GameObject go)
    {
        UIEventListener listener = go.GetComponent<UIEventListener>();
        if (listener == null) listener = go.AddComponent<UIEventListener>();
        return listener;
    }

    //点击
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null) onClick(gameObject);
    }

    //按下
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onPress != null) onPress(gameObject, true);
    }

    //弹起
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onPress != null) onPress(gameObject,false);
    }

    //进入
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onHover != null) onHover(gameObject,true);
    }

    //退出
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onHover != null) onHover(gameObject,false);
    }

    //选中
    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(gameObject,true);
    }

    //选中中
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null) onUpdateSelect(gameObject);
    }

    //取消选中
    public override void OnDeselect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(gameObject, false);
    }

    //开始拖动
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (onDragStart != null) onDragStart(gameObject);
    }

    //拖动中
    public override void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null) onDrag(gameObject,eventData.delta);
    }

    //停止拖动
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (onDragEnd != null) onDragEnd(gameObject);
    }

    //放置
    public override void OnDrop(PointerEventData eventData)
    {
        if (onDrop != null) onDrop(gameObject,eventData.pointerDrag);
    }

    //移动中
    public override void OnMove(AxisEventData eventData)
    {
        if (onMove != null) onMove(gameObject, eventData);
    }

    //取消
    public override void OnCancel(BaseEventData eventData)
    {
        if (onCancel != null) onCancel(gameObject);
    }

    //初始化拖动阻力
    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (onInitializePotentialDrag != null) onInitializePotentialDrag(gameObject);
    }

    //滑动
    public override void OnScroll(PointerEventData eventData)
    {
        if (onScroll != null) onScroll(gameObject,eventData.scrollDelta);
    }

    //提交
    public override void OnSubmit(BaseEventData eventData)
    {
        if (onSubmit != null) onSubmit(gameObject);
    }
}

