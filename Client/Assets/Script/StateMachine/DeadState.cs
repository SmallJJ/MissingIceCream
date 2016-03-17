
public class DeadState:StateBase
{
    public DeadState(BevBase bev): base(bev) 
    {
    }
    #region override methods
    public override void Execute()
    {
        if (base.frameCounter >= base.intervalFrame)
        {
            base.frameCounter = 0;
            if (base.bev.IsDead)
            {
                base.bev.Dead();
            }
        }
        else
        {
            base.frameCounter++;
        }
    }
    #endregion
}
