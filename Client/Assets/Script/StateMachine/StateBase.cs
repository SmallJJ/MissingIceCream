public abstract class StateBase:IState
{
    // 行为和状态对应,这里保存一个行为
    protected BevBase bev { get; private set; }
    // 帧数计数器
    protected int frameCounter;
    //行为检测间隔帧数
    protected int intervalFrame = 3;
    public StateBase(BevBase bev)
    {
        this.bev = bev;
    }

    #region  IState methods
    public virtual void Enter()
    {
        this.frameCounter = 0;
    }

    public abstract void Execute();

    public virtual void Exit()
    {
    }
    #endregion
}
