using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// 一种通用的包裹结构
/// </summary>
public class CommonPackageComponent : PackageComponentBase
{
    public override event Action<ComponentBase> ItemClickEvent;
    public override event Action<ComponentBase, bool> ItemPressEvent;
    public ComponentBase ItemPrefab;

    private List<ComponentBase> m_TotalItemList = new List<ComponentBase>();

    #region override methods

    protected override void ItemClick(ComponentBase comp)
    {
        if (this.ItemClickEvent != null)
        {
            this.ItemClickEvent(comp);
        }
    }

    protected override void ItemPress(ComponentBase comp, bool isPress)
    {
        if (this.ItemPressEvent != null)
        {
            this.ItemPressEvent(comp, isPress);
        }
    }

    public override void Clear()
    {
        this.ResetScrollView();
        base.Clear();
        this.ClearReserved();
    }

    public override void Hide()
    {
        base.Hide();
        StopAllCoroutines();
    }

    public override void DeleteItem(ComponentBase item)
    {
        if (item != null)
        {
            ComponentBase itemComp = item.GetComponent<ComponentBase>();
            itemComp.Clear();
            itemComp.Hide();
            this.CurrentItemList.Remove(item);
            if (item == this.backupSelectionItem)
            {
                this.backupSelectionItem = null;
            }
            base.DeleteItem(item);
        }
    }

    #endregion

    #region private methods

    /// <summary>
    /// 使用泛型，使其更通用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="index"></param>
    /// <returns></returns>
    private T InstantiateItem<T>(int index) where T : ComponentBase
    {
        ComponentBase comp = null;
        if (this.m_TotalItemList.Count > index)
        {
            comp = this.m_TotalItemList[index];
            comp.Show();
        }
        else
        {
            comp = TransUtils.InstantiateTransform(this.ItemPrefab.MyTransform, base.LayoutGroup.transform).GetComponent<ComponentBase>();
            base.AddCompEvent(comp);
            this.m_TotalItemList.Add(comp);
        }
        comp.name = index.ToString("D4");
        //if (this.ImmediatelyShowCount > 0)
        //{
        //    if (index == this.ImmediatelyShowCount)
        //    {
        //        this.ResetPosition();
        //    }
        //    else if (index > this.ImmediatelyShowCount)
        //    {
        //        comp.MyTransform.localPosition = this.hidePosition;
        //    }
        //}
        this.CurrentItemList.Add(comp);
        return comp.GetComponent<T>();
    }

    /// <summary>
    /// 清空 
    /// 保留模式 暂时隐藏 减小开销
    /// </summary>
    private void ClearReserved()
    {
        foreach (ComponentBase item in this.CurrentItemList)
        {
            item.Clear();
            item.Hide();
        }
        this.CurrentItemList.Clear();
        this.ResetPosition();
    }

    #endregion

    #region public methods


    public void UpdateIceCreamPackage(IceCreamData[] iceCreamDatas)
    {
        if (iceCreamDatas != null && iceCreamDatas.Length > 0)
        {
            this.StartCoroutine(this.UpdateIceCreamPackageIterator(iceCreamDatas));
        }
    }

    private IEnumerator UpdateIceCreamPackageIterator(IceCreamData[] iceCreamDatas)
    {
        this.StartUpdatePackage();
        for (int i = 0; i < iceCreamDatas.Length; i++)
        {
            PropItemComponent propItem = this.InstantiateItem<PropItemComponent>(i);
            propItem.UpdateData(iceCreamDatas[i]);
        }
        yield return null;
        this.FinishUpdatePackage();
    }
    #endregion
}