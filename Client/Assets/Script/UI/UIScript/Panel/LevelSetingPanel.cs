using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class LevelSetingPanel : PanelBase
{
    public InputField InputCol;
    public InputField InputRow;
    public LocalLabelComponent ColDefualtLabel;
    public LocalLabelComponent RowDefualtLabel;
    public ButtonComponent EnsureBtn;

    private byte m_Col;
    private byte m_Row;

    #region override methods

    public override void Clear()
    {
        base.Clear();
        this.InputCol.text = string.Empty;
        this.InputRow.text = string.Empty;
    }
    public override void OpenComplete()
    {
        this.UpdateDefaultLabel();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        this.EnsureBtn.ClickEvent += () =>
        {
            bool notError = this.CheckError();
            if (notError)
            {
                this.EnterPanel = UIPanelType.LevelEditerPanel;
                this.EnterPanelParam = new LevelEditerParam() { Col = this.m_Col, Row = this.m_Row, OperationType = LevelOperationType.Create };
                UIController.Instance.ClosePanel(UIPanelType.LevelListPanel);
                this.CloseThisPanel();
            };
        };
    }

    public override void CloseComplete()
    {
        base.CloseComplete();
    }

    #endregion

    #region private methods

    private bool CheckError()
    {
        string col = this.InputCol.text;
        string row = this.InputRow.text;
        string content = string.Empty;
        if (col.Equals(string.Empty)||row.Equals(string.Empty))
        {
            content =LocalizationUtils.GetText("LevelSetingPanel.Tip.IsEmpty");
        }
        else
        {
            int colNum = int.Parse(col);
            int rowNum = int.Parse(row);
            if (rowNum < UIConst.MapRowMin || rowNum > UIConst.MapRowMax)
            {
                content = LocalizationUtils.GetText("LevelSetingPanel.Tip.RowLimit", UIConst.MapRowMin,UIConst.MapRowMax);
            }
            else if (colNum < UIConst.MapColMin || colNum > UIConst.MapColMax)
            {
                content = LocalizationUtils.GetText("LevelSetingPanel.Tip.ColLimit", UIConst.MapColMin,UIConst.MapColMax);
            }
            else
            {
                this.m_Col = (byte)colNum;
                this.m_Row = (byte)rowNum;
            }
        }
        if (content.Equals(string.Empty))
            return true;
        else
        {
            UIController.Instance.ShowDialog(new DialogParem() { Content = content, Type = DialogType.Ok });
            return false;
        }
        
    }

    private void UpdateDefaultLabel()
    {
        this.ColDefualtLabel.UpdateLabel("LevelSetingPanel.Input.DefaultLabel", UIConst.MapColMin,UIConst.MapColMax);
        this.RowDefualtLabel.UpdateLabel("LevelSetingPanel.Input.DefaultLabel", UIConst.MapRowMin,UIConst.MapRowMax);
    }
    #endregion

}
