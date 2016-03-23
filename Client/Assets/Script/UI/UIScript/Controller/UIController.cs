using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIController :MonoBehaviourSingleton<UIController> 
{
    public Canvas UICanvas;
    private Dictionary<UIPanelType, PanelBase> m_OpenedPanelDic = new Dictionary<UIPanelType, PanelBase>();

    #region monoBehaviour methods
    protected override void Awake()
    {
    }
    #endregion

    #region public methods

    #region OpenPanel
    public void OpenPanel(UIPanelType type,PanelParamBase param=null)
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
                Debug.Log(type+" panel is null !");
                return;
            }
            panel.MyTransform.SetParent(this.UICanvas.transform);
            panel.MyTransform.localPosition = Vector3.zero;
            panel.MyTransform.localScale = Vector3.one;
            this.m_OpenedPanelDic.Add(type, panel);
        }
        panel.Open(param,PanelEffectType.Open);
    }
    #endregion

    #endregion

}
