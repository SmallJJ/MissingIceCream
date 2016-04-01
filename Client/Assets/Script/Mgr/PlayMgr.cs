public class PlayMgr:MonoBehaviourSingleton<PlayMgr>
{
    #region MonoBehaviour methods

    void Start()
    {
        EasyTouch.instance.enabled = false;
        UIController.Instance.OpenPanel(UIPanelType.StartGamePanel,null, PanelEffectType.None);
    }

    protected override void Awake()
    {
        
    }

    #endregion
}
