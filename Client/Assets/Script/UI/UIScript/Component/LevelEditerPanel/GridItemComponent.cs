using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GridItemComponent : ComponentBase
{
    public Image BaseImage; //地基(路 or 墙)
    public Image RoleImage; //这块格子身上的东西(道具，物品，冰淇淋)

    public byte Row { get; private set; }
    public byte Col { get; private set; }
    private GridLogicData m_GridLogicData;

    #region private methods

    private void UpdateBaseImage(string icon)
    {
        this.BaseImage.sprite = GamePoolMgr.Instance.GetSprite(PathConst.Icon + icon);
    }

    private void UpdateRoleImage(string icon)
    {
        this.RoleImage.gameObject.SetActive(true);
        this.RoleImage.sprite = GamePoolMgr.Instance.GetSprite(PathConst.Icon + icon);
    }

    private void UpdateInfo()
    {
        this.UpdateBaseImage(this.m_GridLogicData.CurRoadData.Asset);
        if (this.m_GridLogicData.AttachRoleData != null)
        {
            this.UpdateRoleImage(this.m_GridLogicData.AttachRoleData.Asset);
        }
        else
        {
            this.RoleImage.gameObject.SetActive(false);
        }
    }
    #endregion

    #region public methods

    public void UpdateInfo(GridLogicData data)
    {
        this.m_GridLogicData = data;
        this.Row = data.GetGridData().Row;
        this.Col = data.GetGridData().Col;
        this.UpdateInfo();
    }

    public void ChangeData(GridData gridData)
    {
        this.m_GridLogicData.SetAttachRoleData(gridData);
        this.UpdateInfo();
    }

    public GridData GetGridData()
    {
        return this.m_GridLogicData.GetGridData();
    }
    #endregion
}
