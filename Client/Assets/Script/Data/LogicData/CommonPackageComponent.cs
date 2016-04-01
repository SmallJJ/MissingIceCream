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
            Transform itemParent = this.MyTransform;
            if (base.LayoutGroup != null)
            {
                itemParent = base.LayoutGroup.transform;
            }
            else if (base.ItemParent != null)
            {
                itemParent = base.ItemParent;
            }
            comp = TransUtils.InstantiateTransform(this.ItemPrefab.MyTransform, itemParent).GetComponent<ComponentBase>();
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
    /// <summary>
    /// 更新角色组件包裹
    /// </summary>
    /// <param name="iceCreamDatas"></param>
    public void UpdateRoleItems(RoleDataBase[] roleDatas)
    {
        if (roleDatas != null && roleDatas.Length > 0)
        {
            base.Clear();
            this.StartCoroutine(this.UpdateIceCreamPackageIterator(roleDatas));
        }
    }

    private IEnumerator UpdateIceCreamPackageIterator(RoleDataBase[] roleDatas)
    {
        this.StartUpdatePackage();
        for (int i = 0; i < roleDatas.Length; i++)
        {
            PropItemComponent propItem = this.InstantiateItem<PropItemComponent>(i);
            propItem.UpdateData(roleDatas[i]);
        }
        yield return null;
        this.FinishUpdatePackage();
    }


    /// <summary>
    /// 更新角色类型包裹
    /// </summary>
    /// <param name="roleTypeArray"></param>
    public void UpdateRoleType(Array roleTypeArray)
    {
        if (roleTypeArray != null && roleTypeArray.Length > 0)
        {
            base.Clear();
            this.StartCoroutine(this.UpdateRoleTypeIterator(roleTypeArray));
        }
    }

    private IEnumerator UpdateRoleTypeIterator(Array roleTypeArray)
    {
        this.StartUpdatePackage();
        for (int i = 0; i < roleTypeArray.Length; i++)
        {
            RoleTypeComponent roleTypeComp = this.InstantiateItem<RoleTypeComponent>(i);
            roleTypeComp.UpdateInfo((UIRoleType)roleTypeArray.GetValue(i));
            yield return null;
        }
        this.FinishUpdatePackage();
    }

    /// <summary>
    /// 更新关卡地图包裹
    /// </summary>
    public void UpdateLevelMapPackage(GridData[,] curLevelDatas, int gridWidth, int gridHeight)
    {
        if (curLevelDatas != null && curLevelDatas.Length > 0)
        {
            base.Clear();
            this.StartCoroutine(this.UpdateLevelMapPackageIterator(curLevelDatas, gridWidth, gridHeight));
        }
    }

    private IEnumerator UpdateLevelMapPackageIterator(GridData[,] curLevelDatas, int gridWidth, int gridHeight)
    {
        this.StartUpdatePackage();
        int index = 0;
        for (int row = 0; row < curLevelDatas.GetLength(0); row++)
        {
            for (int col = 0; col < curLevelDatas.GetLength(1); col++)
            {
                GridItemComponent comp = this.InstantiateItem<GridItemComponent>(index++);
                comp.UpdateInfo(new GridLogicData(curLevelDatas[row,col]));
                comp.MyTransform.localPosition = new Vector3(col * gridWidth, -row * gridHeight, 0);
            }
            yield return null;
        }
        this.FinishUpdatePackage();
    }

    /// <summary>
    /// 更新关卡地图包裹
    /// </summary>
    public void UpdateLevelMapPackage(bool[,] mazeArray, int gridWidth, int gridHeight)
    {
        if (mazeArray != null && mazeArray.Length > 0)
        {
            base.Clear();
            this.StartCoroutine(this.UpdateLevelMapPackageIterator(mazeArray, gridWidth, gridHeight));
        }
    }

    private IEnumerator UpdateLevelMapPackageIterator(bool[,] mazeArray, int gridWidth, int gridHeight)
    {
        this.StartUpdatePackage();
        GridData gridData = null;
        GridLogicData gridLogicData = null;
        int index = 0;
        for (int row = 0; row < mazeArray.GetLength(0); row++)
        {
            for (int col = 0; col < mazeArray.GetLength(1); col++)
            {
                GridItemComponent comp = this.InstantiateItem<GridItemComponent>(index++);
                RoadType type = mazeArray[row, col] ? RoadType.Wall : RoadType.Road;
                gridData = new GridData(type);
                gridData.SetPosition((byte)row, (byte)col);
                gridLogicData = new GridLogicData(gridData);
                comp.UpdateInfo(gridLogicData);
                comp.MyTransform.localPosition = new Vector3(col * gridWidth, -row * gridHeight, 0);
            }
            yield return null;
        }
        this.FinishUpdatePackage();
    }

    public void UpdateLevelListPackge(Dictionary<string, string> LevelNames, Action<string, LevelOperationType> operationEvent)
    {
        if (LevelNames != null && LevelNames.Count > 0)
        {
            base.Clear();
            this.StartCoroutine(this.UpdateLevelListPackgeIterator(LevelNames, operationEvent));
        }
    }

    private IEnumerator UpdateLevelListPackgeIterator(Dictionary<string, string> levelNames, Action<string, LevelOperationType> operationEvent)
    {
        this.StartUpdatePackage();
        int index = 0;
        foreach ( KeyValuePair<string,string> kv in levelNames)
        {
            LevelInfoItemComponent comp = this.InstantiateItem<LevelInfoItemComponent>(index++);
            comp.UpdateInfo(kv.Key,kv.Value, operationEvent);
            yield return null;
        }
        this.FinishUpdatePackage();
    }
    #endregion
}