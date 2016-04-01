using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviourSingleton<UIController>
{
    public Canvas UICanvas;
    private Dictionary<UIPanelType, PanelBase> m_OpenedPanelDic = new Dictionary<UIPanelType, PanelBase>();
    private List<UIPanelType> m_OpenedPanelOder = new List<UIPanelType>(); //用于不是一线型面板记录管理
    private PanelBackgroundComponent m_PanelBackgroundComp;
    private Queue<TipComponent> m_TipCompPool = new Queue<TipComponent>();
    private DescriptionComponent m_DescriptionComp;

    #region MonoBehaviour methods
    protected override void Awake()
    {
    }
    #endregion

    #region public methods

    public void OpenPanel(UIPanelType type, PanelParamBase param = null, PanelEffectType effectType = PanelEffectType.Open)
    {
        PanelBase panel = null;
        if (this.m_OpenedPanelDic.ContainsKey(type))
        {
            panel = this.m_OpenedPanelDic[type];
        }
        else
        {
            panel = GamePoolMgr.Instance.GetPanel(type);
            if (panel == null)
            {
                Debug.Log(type + " panel is null !");
                return;
            }
            panel.MyTransform.SetParent(this.UICanvas.transform);
            panel.MyTransform.localPosition = Vector3.zero;
            panel.MyTransform.localScale = Vector3.one;
            this.m_OpenedPanelDic.Add(type, panel);
        }
        if (panel.MyMaskType != MaskType.None)
        {
            this.GetPanelBgComp().Show(panel.MyTransform, type, panel.MyMaskType);
        }
        panel.MyTransform.SetAsLastSibling();
        panel.Open(type, param, effectType);
        if (this.m_OpenedPanelOder.Contains(type))
        {
            int index = this.m_OpenedPanelOder.IndexOf(type);
            if (index < this.m_OpenedPanelOder.Count - 1)
            {
                index++;
                this.m_OpenedPanelOder.RemoveRange(index, this.m_OpenedPanelOder.Count-index);
            }
        }
        else
        {
            this.m_OpenedPanelOder.Add(type);
        }
    }

    public void ClosePanel(UIPanelType type, PanelEffectType effectType= PanelEffectType.Close)
    {
        PanelBase panel = this.m_OpenedPanelDic[type];
        //最后一个打开的才有返回和进入别的面板的资格
        if (this.m_OpenedPanelOder[this.m_OpenedPanelOder.Count - 1] == type)
        {
            if (panel.EnterPanel == UIPanelType.None)
            {
                int index = this.m_OpenedPanelOder.IndexOf(type);
                if (index > 0)
                {
                    PanelBase backPanel = this.m_OpenedPanelDic[this.m_OpenedPanelOder[index - 1]];
                    //当前是二级面板,并且要返回的面板需要背景，就把背景还给要返回的面板
                    if (this.IsSecondPanel(type))
                    {
                        if (backPanel.MyMaskType != MaskType.None)
                        {
                            this.GetPanelBgComp().Show(backPanel.MyTransform, backPanel.ThisPanel, backPanel.MyMaskType);
                        }
                        this.m_OpenedPanelOder.Remove(type);
                    }
                    else
                    {
                        this.OpenPanel(backPanel.ThisPanel, backPanel.ThisPanelParam);
                    }
                }
            }
            else
            {
                this.OpenPanel(panel.EnterPanel, panel.EnterPanelParam);
            }
        }
        panel.Close(effectType);
    }

    // <summary>
    // 打开对话框界面
    // </summary>
    // <param name = "param" ></ param >
    public void ShowDialog(DialogParem param)
    {
        this.OpenPanel(UIPanelType.DialogPanel, param);
    }

    /// <summary>
    /// 提示框
    /// </summary>
    /// <param name="str"></param>
    /// <param name="param"></param>
    public void ShowTip(string str, params object[] param)
    {
        TipComponent tipComp = null;
        if (this.m_TipCompPool.Count > 0)
        {
            tipComp = this.m_TipCompPool.Dequeue();
        }
        else
        {
            tipComp = GamePoolMgr.Instance.LoadComponent<TipComponent>(PathConst.Component_Common + "TipComponent");
            tipComp.MyTransform.SetParent(this.UICanvas.transform);
            tipComp.MyTransform.localPosition = Vector3.zero;
            tipComp.MyTransform.localScale = Vector3.one;
            tipComp.MyGameObject.AddComponent<TipCompEffect>();
        }
        tipComp.transform.SetAsLastSibling();
        tipComp.UpdateInfo(str,param);
        tipComp.GetComponent<UIEffectBase>().Play();
    }

    public void TipCompPlayComplate(TipComponent tipComponent)
    {
        tipComponent.Clear();
        this.m_TipCompPool.Enqueue(tipComponent);
    }

    public void ShowDescription(string description)
    {
        if (this.m_DescriptionComp == null)
        {
            this.m_DescriptionComp = GamePoolMgr.Instance.LoadComponent<DescriptionComponent>(PathConst.Component_Common+ "DescriptionComponent");
            this.m_DescriptionComp.MyTransform.SetParent(this.UICanvas.transform);
            this.m_DescriptionComp.transform.localPosition = Vector3.zero;
            this.m_DescriptionComp.transform.localScale = Vector3.one;
        }
        this.m_DescriptionComp.MyTransform.SetAsLastSibling();
        this.m_DescriptionComp.Show();
        this.m_DescriptionComp.UpdateInfo(description);
    }

    public void HideDescription()
    {
        if (this.m_DescriptionComp != null)
        {
            this.m_DescriptionComp.Hide();
        }
    }
    #endregion

    #region private methods

    /// <summary>
    /// 面板背景组件
    /// </summary>
    /// <returns></returns>
    private PanelBackgroundComponent GetPanelBgComp()
    {
        if (this.m_PanelBackgroundComp==null)
        {
            this.m_PanelBackgroundComp = GamePoolMgr.Instance.LoadComponent < PanelBackgroundComponent>(PathConst.Component_Common + "PanelBackgroundComponent");
            this.m_PanelBackgroundComp.MyTransform.localPosition = Vector3.zero;
            this.m_PanelBackgroundComp.MyTransform.localScale = Vector3.one;
            this.m_PanelBackgroundComp.CloseEvent += (type) => this.ClosePanel(type);
        }
        return this.m_PanelBackgroundComp;
    }

    private bool IsSecondPanel(UIPanelType type)
    {
        switch (type)
        {
            case UIPanelType.DialogPanel:
            case UIPanelType.LevelSetingPanel:
            case UIPanelType.SaveLevelPanel:
            case UIPanelType.EditerHelpPanel:
                return true;
        }
        return false;
    }
    #endregion

}
