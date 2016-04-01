using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelEditerMgr: MonoBehaviourSingleton<LevelEditerMgr>
{
    public CommonPackageComponent LevelPackage;
    public int GridWidth=100;
    public int GridHeight=100;
    public float PressTime = 0.4f;
    private MeshMgr m_MeshMgr;  //网格管理
    public Action<int> IceCreamCountChangeEvent;
    public Action<int> PropCountChangeEvent;
    public Action<int> MaterialCountChangeEvent;
    public Action<int> FreezerCountChangeEvent;
    public Action ClearPanelSelected;

    private int m_IceCreamCount;
    private int m_PropCount;
    private int m_MaterialCount;
    private int m_FreezerCount;

    private GridItemComponent m_MapSelectGridItemComp;
    private RoleDataBase m_PanelSelectData;
    private GridItemComponent m_PressGridItemComp;
    private bool m_IsPress;
    private int m_LevelRows;
    private int m_LevelCols;
    private GridData[,] m_CurLevelData;

   

    protected override void Awake()
    {
        base.Awake();
    }

    #region public methods
    public void CreateLevel( byte row,byte col)
    {
        this.ReSetData();
        row = (byte)(row / 2);
        col = (byte)(col / 2);
        this.m_MeshMgr = new MeshMgr(row, col);
        bool[,] mazeArray=this.m_MeshMgr.CreateMaze();
        this.LevelPackage.UpdateLevelMapPackage(mazeArray,this.GridWidth,this.GridHeight);
        this.m_LevelRows = row * 2 + 1;
        this.m_LevelCols = col * 2 + 1;
        this.InitCamera();
    }

    public void LoadLevel(string levelFileName)
    {
        this.ReSetData();
        this.m_CurLevelData = LevelDataMgr.Instance.LoadLevelData(levelFileName).GetLevelData();
        this.m_LevelRows = this.m_CurLevelData.GetLength(0);
        this.m_LevelCols = this.m_CurLevelData.GetLength(1);
        this.LevelPackage.UpdateLevelMapPackage(this.m_CurLevelData, this.GridWidth, this.GridHeight);
        this.InitCamera();
        this.DataTatol();
    }

    public void SaveLevel(string levelName)
    {
        List <ComponentBase> itemList=this.LevelPackage.CurrentItemList;
        int index = 0;
        LevelData leveldata = new LevelData(this.m_LevelRows, this.m_LevelCols);
        for (int row = 0; row < this.m_LevelRows; row++)
        {
            for (int col = 0; col < this.m_LevelCols; col++)
            {
                leveldata.SetLevelData(row, col, (itemList[index] as GridItemComponent).GetGridData());
                index++;
            }
        }
        this.m_CurLevelData = leveldata.GetLevelData();
        LevelDataMgr.Instance.SaveLevelData(leveldata, levelName);
    }

    /// <summary>
    /// 面板包裹选择
    /// </summary>
    /// <param name="roleData"></param>
    public void PanelPackageSelect(RoleDataBase roleData)
    {
        this.m_PanelSelectData = roleData;
        if (this.m_PanelSelectData!=null&& this.m_MapSelectGridItemComp != null)
        {
            //如果面板选择的数据不为null，并且地图中有选择grid，就将格子的数据改为面板选择的数据
            this.ChangeData(this.m_MapSelectGridItemComp, roleData.GridData);
        }
    }

    /// <summary>
    /// 判断门是否是固定门
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public bool IsFixedWallByPos(byte row, byte col)
    {
        if (row > 0 && row < this.m_LevelRows - 1
            && col > 0 && col < this.m_LevelCols - 1)
        {
            return false;
        }
        return true;
    }

    public void ClearMap()
    {
        this.LevelPackage.Clear();
    }
    #endregion

    #region private methods
    private void InitCamera()
    {
        this.AddEvent();
        this.LevelPackage.MyTransform.localPosition = new Vector3(-this.m_LevelCols * this.GridWidth / 2, this.m_LevelRows * this.GridHeight / 2, 0);
        CameraCtrl.Instance.SetCameraSize(this.m_LevelCols * this.GridWidth, this.m_LevelRows * this.GridHeight);
        EasyTouch.instance.enabled = true;
    }

    private void AddEvent()
    {
        this.LevelPackage.ItemClickEvent += (comp) =>
          {
              if (CameraCtrl.Instance.IsCanOperation())
              {
                  CameraCtrl.Instance.SetStatus(CameraCtrlType.Click);
                  GridItemComponent gridItemComp = (GridItemComponent)comp;
                  if (this.IsFixedWallByPos(gridItemComp.Row, gridItemComp.Col)
                  || this.m_MapSelectGridItemComp == gridItemComp)
                  {
                      this.m_MapSelectGridItemComp = null;
                      this.LevelPackage.ClearSelection();
                      return;
                  }
                this.m_MapSelectGridItemComp = gridItemComp;
                  LevelPackage.ShowSelectionEffect(gridItemComp);
                if (this.m_PanelSelectData != null)
                {
                    this.ChangeData(this.m_MapSelectGridItemComp, this.m_PanelSelectData.GridData);
                }
                  CameraCtrl.Instance.OperationEnd();
              }
          };

        this.LevelPackage.ItemPressEvent += (comp, isPress) =>
        {
            this.m_IsPress = isPress;
            if (isPress)
            {
                GridItemComponent gridItemComp= (GridItemComponent)comp;
                if (this.IsFixedWallByPos(gridItemComp.Row, gridItemComp.Col))
                {
                    this.LevelPackage.ClearSelection();
                    this.m_PressGridItemComp = null;
                    UIController.Instance.ShowTip("LevelEditerMgr.Tip.WallIsFixed");
                    return;
                }
                this.m_PressGridItemComp = gridItemComp;
                this.StartCoroutine("WaitPressIterator");
            }
            else
            {
                this.StopCoroutine("WaitPressIterator");
                CameraCtrl.Instance.OperationEnd();
            }
        };
    }

    private IEnumerator WaitPressIterator()
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup-startTime<this.PressTime)
        {
            if (CameraCtrl.Instance.IsCanOperation())
                yield return null;
            else
                yield break;
        }
        CameraCtrl.Instance.SetStatus(CameraCtrlType.LongPress);
        this.LevelPackage.ShowSelectionEffect(this.m_PressGridItemComp);
        while (this.m_IsPress)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(CameraCtrl.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
            if (raycastHit)
            {
                GridItemComponent gridItemComp=raycastHit.collider.gameObject.GetComponent<GridItemComponent>();
                if (gridItemComp!=null
                    &&gridItemComp != this.m_PressGridItemComp
                    &&!this.IsFixedWallByPos(gridItemComp.Row,gridItemComp.Col))
                {
                    this.ChangeData(gridItemComp, this.m_PressGridItemComp.GetGridData());
                    this.LevelPackage.ShowSelectionEffect(gridItemComp);
                    this.m_PressGridItemComp = gridItemComp;
                    this.m_MapSelectGridItemComp = gridItemComp;
                }
            }
            yield return null;
        }
    }

    /// <summary>
    /// 数据改变
    /// </summary>
    /// <param name="gridItemComp"></param>
    /// <param name="gridData"></param>
    private void ChangeData(GridItemComponent gridItemComp, GridData gridData)
    {
        if (gridData.RoleType.HasValue)
        {
            switch (gridData.RoleType.Value)
            {
                case RoleType.IceCream:
                    if (this.m_IceCreamCount >= UIConst.IceCreamCount)
                    {
                        if (this.ClearPanelSelected != null) this.ClearPanelSelected();
                        UIController.Instance.ShowTip("LevelEditerMgr.Tip.IceCreamCountLimit");
                        return;
                    }
                    this.m_IceCreamCount++;
                    break;
                case RoleType.Prop:
                    this.m_PropCount++;
                    break;
                case RoleType.Marterial:
                    MaterialType marterialType = gridData.MaterialType.Value;
                    if (marterialType == MaterialType.Freezer)
                    {
                        if (this.m_FreezerCount >= UIConst.FreezerCount)
                        {
                            if (this.ClearPanelSelected != null) this.ClearPanelSelected();
                            UIController.Instance.ShowTip("LevelEditerMgr.Tip.FreezerCountLimit");
                            return;
                        }
                        else
                        {
                            
                                this.m_FreezerCount++;
                        }
                    }
                    this.m_MaterialCount++;
                    break;
            }
        }
        if (gridItemComp.GetGridData().RoleType.HasValue)
        {
            RoleType roleType=gridItemComp.GetGridData().RoleType.Value;
            switch (roleType)
            {
                case RoleType.IceCream:
                    this.m_IceCreamCount--;
                    break;
                case RoleType.Prop:
                    this.m_PropCount--;
                    break;
                case RoleType.Marterial:
                    if (gridItemComp.GetGridData().MaterialType.Value== MaterialType.Freezer)
                    {
                            if (this.m_FreezerCount > 0)
                                this.m_FreezerCount--;
                    }
                    this.m_MaterialCount--;
                    break;
            }
        }
        gridItemComp.ChangeData(gridData);
        this.NoticePanelChangeData();
    }


    /// <summary>
    /// 数据统计，统计数量
    /// </summary>
    private void DataTatol()
    {
        RoleType? roleType = null;
        for (int row = 0; row < this.m_LevelRows; row++)
        {
            for (int col = 0; col < this.m_LevelCols; col++)
            {
                roleType = this.m_CurLevelData[row, col].RoleType;
                if (roleType.HasValue)
                {
                    switch (roleType.Value)
                    {
                        case RoleType.IceCream:
                            this.m_IceCreamCount++;
                            break;
                        case RoleType.Prop:
                            this.m_PropCount++;
                            break;
                        case RoleType.Marterial:
                            MaterialType marterialType = this.m_CurLevelData[row, col].MaterialType.Value;
                            if (marterialType == MaterialType.Freezer)
                            {
                                this.m_FreezerCount++;
                            }
                            this.m_MaterialCount++;
                            break;
                    }
                }
            }
        }
        this.NoticePanelChangeData();
    }

    /// <summary>
    /// 通知面板改变数据
    /// </summary>
    private void NoticePanelChangeData()
    {
        if(this.IceCreamCountChangeEvent!=null) this.IceCreamCountChangeEvent(this.m_IceCreamCount);
        if (this.PropCountChangeEvent != null) this.PropCountChangeEvent(this.m_PropCount);
        if (this.MaterialCountChangeEvent != null) this.MaterialCountChangeEvent(this.m_MaterialCount);
        if (this.FreezerCountChangeEvent != null) this.FreezerCountChangeEvent(this.m_FreezerCount);
    }


    /// <summary>
    /// 重置数据
    /// </summary>
    private void ReSetData()
    {
        this.m_IceCreamCount = 0;
        this.m_PropCount = 0;
        this.m_MaterialCount = 0;
        this.m_FreezerCount=0;
        this.m_MapSelectGridItemComp = null;
        this.m_PanelSelectData = null;
        this.m_PressGridItemComp= null;
        this.m_LevelRows = 0;
        this.m_LevelCols = 0;
        this.m_CurLevelData = null;
        this.m_IsPress = false;
    }
    #endregion
}
