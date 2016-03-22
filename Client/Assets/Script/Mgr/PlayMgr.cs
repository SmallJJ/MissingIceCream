public class PlayMgr:MonoBehaviourSingleton<PlayMgr>
{
    #region MonoBehaviour methods

    void Start()
    {
        UIController.Instance.OpenPanel(UIPanelType.LevelEditerPanel);
    }

    protected override void Awake()
    {
        
    }

    #endregion
}
