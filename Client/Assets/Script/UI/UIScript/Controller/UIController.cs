using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour 
{
    public static UIController Instance { get; private set; }

    private Dictionary<UIPanelType, PanelBase> m_OpenedPanelDic = new Dictionary<UIPanelType, PanelBase>();

    #region monoBehaviour methods
    void Awake()
    {
        Instance = this;
    }
    #endregion

    #region public methods

    #region OpenPanel
    public void OpenPanel(UIPanelType type,PanelParamBase param)
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
            panel.MyTransform.localPosition = Vector3.zero;
            this.m_OpenedPanelDic.Add(type, panel);
        }
        panel.Open(param,PanelEffectType.Open);
    }
    #endregion

    #endregion

}
