using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// 包裹结构的基类
/// </summary>
public abstract class PackageComponentBase : ComponentBase
{
    public LayoutGroup LayoutGroup; //布局组件（水平，垂直，格子布局）
    public ScrollRect ScrollRect;   //滑动组件
    public RectMask2D RectMask; //裁切组件
    public PackageItemClickType ClickType; //点击类型
    public int ImmediatelyShowCount;       //打开面板时，立即必须就看到的格子数量
    public bool IsAutoResetScrollView = true; //自动重置ScrollRect 在每次更新包裹以后
    public GameObject SelectionEffect;
    public List<ComponentBase> CurrentItemList { get; private set; }
    public abstract event Action<ComponentBase> ItemClickEvent; //点击Item事件
    public abstract event Action<ComponentBase,bool> ItemPressEvent; //按Item事件
    public bool IsUpdating { get; protected set; }

    protected ComponentBase backupSelectionItem;
    protected Vector2 hidePosition;

    #region override methods

    public override void Init()
    {
        base.Init();
        this.CurrentItemList = new List<ComponentBase>();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
    }

    public override void Clear()
    {
        StopAllCoroutines();
        foreach (ComponentBase item in this.CurrentItemList)
        {
            DestroyImmediate(item.MyGameObject);
        }
        this.backupSelectionItem = null;
        this.HideSelectionEffect();
        base.Clear();
    }

    public override void Hide()
    {
        this.backupSelectionItem = null;
        StopAllCoroutines();
        base.Hide();
    }

    #endregion

    #region abstract methods

    protected abstract void ItemClick(ComponentBase comp);
    protected abstract void ItemPress(ComponentBase comp, bool isPress);

    public virtual void DeleteItem(ComponentBase comp)
    {
        this.CurrentItemList.Remove(comp);
        DestroyImmediate(comp.MyGameObject);
        this.ResetPosition();
    }
    #endregion

    #region protected methods

    protected void AddCompEvent(ComponentBase comp)
    {
        if (this.ClickType != PackageItemClickType.CantClick)
        {
            UIEventListener listener = UIEventListener.Get(comp.MyGameObject);
            listener.onClick += this.InPackageItemClick;
            listener.onPress += this.InPackageItemPress;
        }
    }

    /// <summary>
    /// 点击item
    /// </summary>
    /// <param name="go"></param>
    protected void InPackageItemClick(GameObject go)
    {
        ComponentBase comp = go.GetComponent<ComponentBase>();
        if (this.ClickType == PackageItemClickType.CantClick) return;
        if (this.ClickType == PackageItemClickType.OnceClick && this.backupSelectionItem == comp) return;
        this.ShowSelectionEffect(comp);
        this.ItemClick(comp);
    }

    /// <summary>
    /// 按下Item
    /// </summary>
    /// <param name="go"></param>
    /// <param name="isPress"></param>
    protected void InPackageItemPress(GameObject go, bool isPress)
    {
        ComponentBase comp = go.GetComponent<ComponentBase>();
        this.ItemPress(comp, isPress);
    }

    /// <summary>
    /// 开始更新
    /// </summary>
    protected void StartUpdatePackage()
    {
        this.IsUpdating = true;
    }

    /// <summary>
    /// 更新完成
    /// </summary>
    protected void FinishUpdatePackage()
    {
        this.AutoResetScrollView();
        this.IsUpdating = false;
        this.IsUpdated = true;
    }


    protected bool CheckIsDelay(int index)
    {
        return this.ImmediatelyShowCount > 0 && index >= this.ImmediatelyShowCount;
    }

    /// <summary>
    /// 显示组件
    /// </summary>
    /// <param name="containerPosition"></param>
    /// <returns></returns>
    protected IEnumerator ShowItemIterator(Vector3 containerPosition)
    {
        this.CurrentItemList.ForEach(a => a.Hide());
        foreach (ComponentBase comp in this.CurrentItemList)
        {
            comp.Show();
            yield return new WaitForSeconds(0.1f);
        }
    }
    #endregion

    #region private methods

    /// <summary>
    /// 显示选中特效
    /// </summary>
    /// <param name="trans"></param>
    private void ShowSelectionEffect(ComponentBase comp)
    {
        this.SelectionEffect.transform.parent = comp.MyTransform;
        this.SelectionEffect.transform.localPosition = Vector3.zero;
        this.SelectionEffect.transform.localScale = Vector3.one;
        this.SelectionEffect.SetActive(true);
    }

    private IEnumerator SelectionItemByIndexInspector(int index)
    {
        while (this.IsUpdating && index >= this.CurrentItemList.Count)
        {
            yield return null;
        }

        this.SelectionItem(index);
    }

    private bool SelectionItem(int index)
    {
        if (index>=0 && index < this.CurrentItemList.Count)
        {
            this.InPackageItemClick(this.CurrentItemList[index].gameObject);
            return true;
        }
        return false;
    }

    #endregion

    #region public methods

    /// <summary>
    /// 选中默认第一个
    /// </summary>
    public void SelectionFirstItem()
    {
        this.SelectionItemByIndex(0);
    }

    /// <summary>
    /// 通过Index选择
    /// </summary>
    /// <param name="index"></param>
    public void SelectionItemByIndex(int index)
    {
        if(index>=0)
            StartCoroutine(this.SelectionItemByIndexInspector(index));
    }

    /// <summary>
    /// 通过对象选择
    /// </summary>
    /// <param name="comp"></param>
    public void SelectionItem(ComponentBase comp)
    {
        if (this.CurrentItemList.Contains(comp))
        {
            this.InPackageItemClick(comp.MyGameObject);
        }
    }

    /// <summary>
    /// 自动重置ScrollView
    /// </summary>
    public void AutoResetScrollView()
    {
        if ( this.IsAutoResetScrollView)
        {
            this.ResetScrollView();
        }
    }

    /// <summary>
    /// 重置滑动位置
    /// </summary>
    public void ResetScrollView()
    {
        if (this.ScrollRect != null)
        {
            if (this.ScrollRect.horizontal)
            {
                this.ScrollRect.CalculateLayoutInputHorizontal();
            }
            else if (this.ScrollRect.vertical)
            {
                this.ScrollRect.CalculateLayoutInputVertical();
            }
        }
    }

    /// <summary>
    /// 重置布局中内容的位置
    /// </summary>
    public void ResetPosition()
    {
        if (this.LayoutGroup != null)
        {
            Type type = this.LayoutGroup.GetType();
            if (type.Equals(Type.GetType("HorizontalLayoutGroup")))
            {
                this.LayoutGroup.CalculateLayoutInputHorizontal();
            }
            else if (type.Equals(Type.GetType("VerticalLayoutGroup")))
            {
                this.LayoutGroup.CalculateLayoutInputVertical();
            }
            else if (type.Equals(Type.GetType("GridLayoutGroup")))
            {
                GridLayoutGroup gridLayout = this.LayoutGroup as GridLayoutGroup;
                if (gridLayout.startAxis == GridLayoutGroup.Axis.Horizontal)
                {
                    this.LayoutGroup.CalculateLayoutInputHorizontal();
                }
                else
                {
                    this.LayoutGroup.CalculateLayoutInputVertical();
                }
            }
        }
    }

    /// <summary>
    /// 显示选中特效
    /// </summary>
    public void ShowSelection(ComponentBase comp)
    {
        if (this.SelectionEffect != null)
        {
            this.SelectionEffect.transform.parent = comp.MyTransform;
            this.SelectionEffect.transform.localPosition = Vector3.zero;
            this.SelectionEffect.transform.localScale = Vector3.one;
            this.SelectionEffect.SetActive(true);
        }
    }

    /// <summary>
    /// 隐藏选中特效
    /// </summary>
    public void HideSelectionEffect()
    {
        if (this.SelectionEffect != null)
        {
            this.SelectionEffect.SetActive(false);
        }
    }

    /// <summary>
    /// 清空选中
    /// </summary>
    public void ClearSelection()
    {
        this.HideSelectionEffect();
        this.backupSelectionItem = null;
    }

    /// <summary>
    /// 获取Item数量
    /// </summary>
    /// <returns></returns>
    public int GetItemCount()
    {
        return this.CurrentItemList.Count;
    }

    /// <summary>
    /// 包裹是否为空
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
        return this.CurrentItemList.Count == 0;
    }

    /// <summary>
    /// 通过组件获取下标
    /// </summary>
    /// <param name="comp"></param>
    /// <returns></returns>
    public int GetItemIndex(ComponentBase comp)
    {
        if (this.CurrentItemList.Contains(comp))
        {
            return this.CurrentItemList.IndexOf(comp);
        }
        return 0;
    }

    /// <summary>
    /// 通过下标获取组件
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ComponentBase GetItemByIndex(int index)
    {
        if (0 <= index && index < this.CurrentItemList.Count)
        {
            return this.CurrentItemList[index];
        }
        return null;
    }

    /// <summary>
    /// 通过UID选中
    /// </summary>
    /// <param name="UID"></param>
    public void SelectionItemByUID(long UID)
    {
        StartCoroutine(this.SelectionItemIterator(UID));
    }

    private IEnumerator SelectionItemIterator(long UID)
    {
        while (this.IsUpdating)
        {
            yield return null;
        }
        ComponentBase item = this.CurrentItemList.FirstOrDefault(a=>((IUID)a).GetUID() == UID);
        if (item != null)
        {
            this.InPackageItemClick(item.MyGameObject);
        }
    }

    /// <summary>
    /// 通过ID选中
    /// </summary>
    /// <param name="ID"></param>
    public void SelectionItemByID(int ID)
    {
        StartCoroutine(this.SelectionItemIterator(ID));
    }

    private IEnumerator SelectionItemIterator(int ID)
    {
        while (this.IsUpdating)
        {
            yield return null;
        }
        ComponentBase item = this.CurrentItemList.FirstOrDefault(a => ((IID)a).GetID() == ID);
        if (item != null)
        {
            this.InPackageItemClick(item.MyGameObject);
        }
    }

    public ComponentBase GetItemByID(int id)
    {
        foreach (ComponentBase item in this.CurrentItemList)
        {
            if (((IID)item).GetID() == id)
            {
                return item;
            }
        }
        return null;
    }
    #endregion
}
