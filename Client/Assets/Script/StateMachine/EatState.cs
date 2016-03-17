public class EatState:StateBase
{
    public EatState(BevBase bev): base(bev) 
    {
    }
    #region override methods
    public override void Execute()
    {
        if (base.frameCounter >= base.intervalFrame)
        {
            base.frameCounter = 0;
            if (base.bev.HasFood())
            {
                base.bev.Eat();
            }
            else if (base.bev.HasPosTarget())
            {
                this.bev.Move();
            }
            else
            {
                this.bev.Idle();
            }
        }
        else
        {
            base.frameCounter++;
        }
    }
    #endregion
}
