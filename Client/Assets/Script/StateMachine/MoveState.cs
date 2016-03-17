
public class MoveState:StateBase
{
    public MoveState(BevBase bev): base(bev) 
    {
    }
    #region override methods
    public override void Execute()
    {
        if (base.frameCounter >= base.intervalFrame)
        {
            base.frameCounter = 0;
            if (base.bev.IsArrived())
            {
                this.bev.StateMachine.ChangeState(BevStateType.Idle);
            }
            else
            {
                if (base.bev.HasFood())
                {
                    this.bev.StateMachine.ChangeState(BevStateType.Eat);
                }
                else
                {
                    this.bev.Move();
                }
            }
        }
        else
        {
            base.frameCounter++;
        }
    }
    #endregion
}
